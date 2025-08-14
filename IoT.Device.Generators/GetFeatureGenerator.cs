using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using SourceGenerationContext =
(
    IoT.Device.Generators.FeatureSourceContext? Context,
    IoT.Device.Generators.DiagnosticContext? Diagnostic
);

#pragma warning disable RS2008 // Enable analyzer release tracking

namespace IoT.Device.Generators;

[Generator]
public class GetFeatureGenerator : IIncrementalGenerator
{
    private static readonly SymbolDisplayFormat? OmitGlobalFormat = SymbolDisplayFormat.FullyQualifiedFormat.
        WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted);

    private static readonly DiagnosticDescriptor NoPartialError = new("GFGEN001",
        "Generation error",
        "Class is marked with 'SupportsFeatureAttribute', but declaration has no 'partial' modifier keyword, so it cannot be augmented with generated code",
        nameof(GetFeatureGenerator), DiagnosticSeverity.Error, true);

    private static readonly DiagnosticDescriptor GetFeatureDefinedError = new("GFGEN003",
        "Generation error",
        "Class is marked with 'SupportsFeatureAttribute', but declaration already has GetFeature<T>() method defined",
        nameof(GetFeatureGenerator), DiagnosticSeverity.Error, true);

    private static readonly DiagnosticDescriptor CannotOverrideSealedError = new("GFGEN004",
        "Generation error",
        "Class is marked with 'SupportsFeatureAttribute', but abstract GetFeature<T>() method is sealed and cannot be overriden in the code generated",
        nameof(GetFeatureGenerator), DiagnosticSeverity.Error, true);

    private static readonly DiagnosticDescriptor NoAbstractGetFeatureDefinedError = new("GFGEN005",
        "Generation error",
        "Class is marked with 'SupportsFeatureAttribute', but doesn't inherit from the type with abstract or virtual GetFeature<T>() method defined",
        nameof(GetFeatureGenerator), DiagnosticSeverity.Error, true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var targetsProvider = context.SyntaxProvider.CreateSyntaxProvider(
                static (node, _) => node is ClassDeclarationSyntax { AttributeLists.Count: > 0 },
                static (context, ct) =>
                {
                    var node = (ClassDeclarationSyntax)context.Node;
                    var symbol = context.SemanticModel.GetDeclaredSymbol(node, ct);
                    if (symbol is INamedTypeSymbol typeSymbol)
                    {
                        var builder = ImmutableArray.CreateBuilder<FeatureContext>();
                        foreach (var attribute in typeSymbol.GetAttributes())
                        {
                            if (SupportsFeatureSyntaxHelper.IsFeatureAttribute(attribute) &&
                                SupportsFeatureSyntaxHelper.TryGetFeatureType(attribute,
                                    out var featureType, out var featureImplType))
                            {
                                builder.Add(new(
                                    featureType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                                    featureImplType?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)));
                            }
                        }

                        if (builder.Count == 0)
                        {
                            goto Skip;
                        }

                        foreach (var modifier in node.Modifiers)
                        {
                            if (modifier.IsKind(SyntaxKind.PartialKeyword))
                            {
                                goto CheckGetFeaturePresense;
                            }
                        }

                        // There was no 'partial' modifier specified, generate diagnostic and skip
                        return default(SourceGenerationContext) with
                        {
                            Diagnostic = new(NoPartialError, LocationContext.Create(node.GetLocation()))
                        };

                    CheckGetFeaturePresense:

                        foreach (var member in typeSymbol.GetMembers("GetFeature"))
                        {
                            if (member is IMethodSymbol { IsGenericMethod: true, TypeArguments.Length: 1, Parameters.Length: 0 })
                            {
                                return default(SourceGenerationContext) with
                                {
                                    Diagnostic = new(GetFeatureDefinedError, LocationContext.Create(node.GetLocation()))
                                };
                            }
                        }

                        bool shouldCallBaseImpl = false;
                        var type = typeSymbol;
                        while ((type = type!.BaseType) is not null)
                        {
                            foreach (var member in type.GetMembers("GetFeature"))
                            {
                                if (member is IMethodSymbol
                                    {
                                        IsGenericMethod: true,
                                        TypeArguments.Length: 1,
                                        Parameters.Length: 0,
                                        IsSealed: var isSealed,
                                        IsAbstract: var isAbstract,
                                        IsVirtual: var isVirtual,
                                        IsOverride: var isOverride
                                    })
                                {
                                    if (isSealed)
                                    {
                                        return default(SourceGenerationContext) with
                                        {
                                            Diagnostic = new(CannotOverrideSealedError, LocationContext.Create(node.GetLocation()))
                                        };
                                    }
                                    else if (isOverride || isAbstract || isVirtual)
                                    {
                                        shouldCallBaseImpl = !isAbstract;
                                        goto Success;
                                    }
                                }
                            }

                            foreach (var attribute in type.GetAttributes())
                            {
                                if (SupportsFeatureSyntaxHelper.IsFeatureAttribute(attribute))
                                {
                                    shouldCallBaseImpl = true;
                                    goto Success;
                                }
                            }
                        }

                        return default(SourceGenerationContext) with
                        {
                            Diagnostic = new(NoAbstractGetFeatureDefinedError, LocationContext.Create(node.GetLocation()))
                        };

                    Success:
                        return (new(typeSymbol.Name, typeSymbol.ContainingNamespace.ToDisplayString(OmitGlobalFormat),
                            shouldCallBaseImpl, builder.ToImmutable()), null);
                    }

                Skip:
                    return default;
                })
            .Where(ctx => ctx.Context is not null || ctx.Diagnostic is not null);

        var combined = targetsProvider.Combine(context.CompilationProvider);

        context.RegisterSourceOutput(combined, (ctx, source) =>
        {
            var ((sourceContext, diagnostic), _) = source;
            if (diagnostic is { } value)
            {
                ctx.ReportDiagnostic(value.ToDiagnostic());
                return;
            }

            var (typeName, namespaceName, shouldCallBaseImpl, features) = sourceContext!.Value;

            var code = GetFeatureCodeEmitter.Emit(typeName, namespaceName, features, shouldCallBaseImpl, ctx.CancellationToken);
            ctx.AddSource($"{typeName}.g.cs", SourceText.From(code, Encoding.UTF8));
        });
    }
}