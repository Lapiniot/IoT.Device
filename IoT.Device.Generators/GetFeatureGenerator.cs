using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Parser = IoT.Device.Generators.SupportsFeatureSyntaxParser;
using Generator = IoT.Device.Generators.GetFeatureSyntaxGenerator;

namespace IoT.Device.Generators;

// TODO: Filter off inaccessible feature types (abstract classes, missing suitable constructor etc.)
[Generator]
public class GetFeatureGenerator : IIncrementalGenerator
{
    private static readonly DiagnosticDescriptor NoPartialWarning = new("GFGEN001",
        "Generation warning.",
        "Class is marked with 'SupportsFeatureAttribute', but declaration has no 'partial' modifier keyword, so it cannot be augmented by the generator.",
        nameof(GetFeatureGenerator), DiagnosticSeverity.Warning, true);

    private static readonly DiagnosticDescriptor GetFeatureDefinedWarning = new("GFGEN003",
        "Generation warning.",
        "Class is marked with 'SupportsFeatureAttribute', but declaration already has GetFeature<T>() method defined.",
        nameof(GetFeatureGenerator), DiagnosticSeverity.Warning, true);

    private static readonly DiagnosticDescriptor CannotOverrideWarning = new("GFGEN004",
        "Generation warning.",
        "Class is marked with 'SupportsFeatureAttribute', but abstract GetFeature<T>() method is sealed and cannot be overriden by code-gen.",
        nameof(GetFeatureGenerator), DiagnosticSeverity.Warning, true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var targetsProvider = context.SyntaxProvider.CreateSyntaxProvider(
                static (syntaxNode, _) => Parser.IsSyntaxTargetForGeneration(syntaxNode),
                static (context, ct) => Parser.GetSemanticTargetForGeneration((ClassDeclarationSyntax)context.Node, context.SemanticModel, ct))
            .Where(symbol => symbol is not null);

        var combined = context.CompilationProvider.Combine(targetsProvider.Collect());

        context.RegisterSourceOutput(combined, (ctx, source) =>
        {
            var (compilation, targets) = source;

            var comparer = SymbolEqualityComparer.Default;

            var supportsFeatureAttributeBaseType = compilation.GetTypeByMetadataName("IoT.Device.SupportsFeatureAttribute");
            var objectType = compilation.ObjectType;

            foreach (var target in targets)
            {
                if (target is null ||
                    compilation.GetSemanticModel(target.SyntaxTree) is not { } model ||
                    model.GetDeclaredSymbol(target, ctx.CancellationToken) is not { } symbol)
                {
                    continue;
                }

                #region Check target class is partial

                var skipHere = true;

                foreach (var modifier in target.Modifiers)
                {
                    if (!modifier.IsKind(SyntaxKind.PartialKeyword)) continue;
                    skipHere = false;
                    break;
                }

                if (skipHere)
                {
                    // Class is not partial, thus no chance to extend via code generation
                    ctx.ReportDiagnostic(Diagnostic.Create(NoPartialWarning, target.GetLocation()));
                    continue;
                }

                #endregion

                #region Check target has no implicetely defined GetFeature<T>() method

                skipHere = false;
                foreach (var member in symbol.GetMembers("GetFeature"))
                {
                    if (member is not IMethodSymbol { IsGenericMethod: true, TypeArguments.Length: 1, Parameters.Length: 0 })
                        continue;

                    skipHere = true;
                    break;
                }

                if (skipHere)
                {
                    // Class already has GetFeature method explicitly defined by override
                    ctx.ReportDiagnostic(Diagnostic.Create(GetFeatureDefinedWarning, target.GetLocation()));
                    continue;
                }

                #endregion

                #region Check GetFeature<T>() is available for override

                var type = symbol;
                skipHere = false;
                while (!(type = type.BaseType)!.Equals(objectType, comparer))
                {
                    var stopHere = false;
                    foreach (var member in type.GetMembers("GetFeature"))
                    {
                        if (member is not IMethodSymbol { IsGenericMethod: true, TypeArguments.Length: 1, Parameters.Length: 0, IsOverride: true, IsSealed: true })
                            continue;

                        skipHere = stopHere = true;
                        break;
                    }

                    if (stopHere) break;
                }

                if (skipHere)
                {
                    // Class has GetFeature<T>() overriden and sealed
                    ctx.ReportDiagnostic(Diagnostic.Create(CannotOverrideWarning, target.GetLocation()));
                    continue;
                }

                #endregion

                #region Extract all SupportsFeatureAttribute<TFeature>/SupportsFeatureAttribute<TFeature,TImpl>

                var attributes = symbol.GetAttributes();
                var list = new List<INamedTypeSymbol>(attributes.Length);

                foreach (var attribute in attributes)
                {
                    var attributeClass = attribute.AttributeClass;
                    var constructedFrom = attributeClass!.ConstructedFrom;
                    if (constructedFrom.BaseType?.Equals(supportsFeatureAttributeBaseType, comparer) == true)
                    {
                        list.Add(attributeClass);
                    }
                }

                if (list.Count <= 0)
                    // No relevant SupportsFeatureAttribute has been found. Strange, but skip this code-gen target
                    continue;

                #endregion

                #region Detect whether we need to call base.GetDerived<T>() from our generated override

                var invokeBaseImpl = false;
                type = symbol;
                while (!(type = type.BaseType)!.Equals(objectType, comparer))
                {
                    foreach (var member in type.GetMembers("GetFeature"))
                    {
                        if (member is not IMethodSymbol { IsGenericMethod: true, TypeArguments.Length: 1, Parameters.Length: 0, IsOverride: true })
                            continue;

                        invokeBaseImpl = true;
                        break;
                    }

                    if (invokeBaseImpl) break;

                    foreach (var attribute in type.GetAttributes())
                    {
                        var constructedFrom = attribute.AttributeClass!.ConstructedFrom;

                        if (constructedFrom.BaseType?.Equals(supportsFeatureAttributeBaseType, comparer) == false)
                        {
                            continue;
                        }

                        invokeBaseImpl = true;
                        break;
                    }

                    if (invokeBaseImpl) break;
                }

                #endregion

                var code = Generator.GenerateAugmentation(symbol.Name, symbol.ContainingNamespace.ToDisplayString(), list, invokeBaseImpl).ToFullString();
                ctx.AddSource($"{symbol.Name}.GetFeature.g.cs", SourceText.From(code, Encoding.UTF8));
            }
        });
    }
}