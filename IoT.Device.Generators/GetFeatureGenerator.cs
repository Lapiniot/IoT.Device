using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.SymbolDisplayFormat;
using Parser = IoT.Device.Generators.SupportsFeatureSyntaxParser;
using Generator = IoT.Device.Generators.GetFeatureSyntaxGenerator;

#pragma warning disable RS2008 // Enable analyzer release tracking

namespace IoT.Device.Generators;

[Generator]
public class GetFeatureGenerator : IIncrementalGenerator
{
    private static readonly DiagnosticDescriptor NoPartialWarning = new("GFGEN001",
        "Generation warning",
        "Class is marked with 'SupportsFeatureAttribute', but declaration has no 'partial' modifier keyword, so it cannot be augmented by the generator",
        nameof(GetFeatureGenerator), DiagnosticSeverity.Warning, true);

    private static readonly DiagnosticDescriptor GetFeatureDefinedWarning = new("GFGEN003",
        "Generation warning",
        "Class is marked with 'SupportsFeatureAttribute', but declaration already has GetFeature<T>() method defined",
        nameof(GetFeatureGenerator), DiagnosticSeverity.Warning, true);

    private static readonly DiagnosticDescriptor CannotOverrideWarning = new("GFGEN004",
        "Generation warning",
        "Class is marked with 'SupportsFeatureAttribute', but abstract GetFeature<T>() method is sealed and cannot be overriden by code-gen",
        nameof(GetFeatureGenerator), DiagnosticSeverity.Warning, true);

    private static readonly SyntaxTargetOnlyComparer<ClassDeclarationSyntax> SyntaxTargetOnlyComparer = new();

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var targetsProvider = context.SyntaxProvider.CreateSyntaxProvider(
                static (syntaxNode, _) => Parser.IsSyntaxTargetForGeneration(syntaxNode),
                static (context, ct) => Parser.GetSemanticTargetForGeneration((ClassDeclarationSyntax)context.Node, context.SemanticModel, ct))
            .Where(symbol => symbol is not null);

        var combined = targetsProvider.Combine(context.CompilationProvider).WithComparer(SyntaxTargetOnlyComparer);

        context.RegisterSourceOutput(combined, (ctx, source) =>
        {
            var (target, compilation) = source;
            var cancellationToken = ctx.CancellationToken;
            var comparer = SymbolEqualityComparer.Default;

            var featureBaseType = compilation.GetTypeByMetadataName("IoT.Device.DeviceFeature`1");
            var supportsFeatureAttributeBaseType = compilation.GetTypeByMetadataName("IoT.Device.SupportsFeatureAttribute");
            var objectType = compilation.ObjectType;

            if (target is null ||
                compilation.GetSemanticModel(target.SyntaxTree) is not { } semanticModel ||
                semanticModel.GetDeclaredSymbol(target, cancellationToken) is not { } targetType)
            {
                return;
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
                return;
            }

            #endregion

            cancellationToken.ThrowIfCancellationRequested();

            #region Check target has no implicetely defined GetFeature<T>() method

            skipHere = false;
            foreach (var member in targetType.GetMembers("GetFeature"))
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
                return;
            }

            #endregion

            cancellationToken.ThrowIfCancellationRequested();

            #region Check GetFeature<T>() is available for override

            var type = targetType;
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
                return;
            }

            #endregion

            cancellationToken.ThrowIfCancellationRequested();

            #region Extract all SupportsFeatureAttribute<TFeature>/SupportsFeatureAttribute<TFeature,TImpl>

            var attributes = targetType.GetAttributes();
            var features = new Dictionary<string, ConditionData>(attributes.Length, StringComparer.Ordinal);
            var fields = new HashSet<string>(StringComparer.Ordinal);

            foreach (var attribute in attributes)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var attributeClass = attribute.AttributeClass;
                var constructedFrom = attributeClass!.ConstructedFrom;

                if (constructedFrom.BaseType?.Equals(supportsFeatureAttributeBaseType, comparer) == false)
                {
                    continue;
                }

                var implType = attributeClass.TypeArguments switch
                {
                [var value] => value,
                [_, var value] => value,
                    _ => throw new NotImplementedException()
                };

                var fullTypeName = implType.ToDisplayString(FullyQualifiedFormat);

                if (!features.TryGetValue(fullTypeName, out var data))
                {
                    var name = implType.Name;
                    name = $"{char.ToLowerInvariant(name[0])}{name.Substring(1)}Feature";

                    var fieldName = name;
                    var index = 1;
                    while (!fields.Add(fieldName)) fieldName = $"{name}{index++}";

                    data = new(fieldName, new HashSet<string>(StringComparer.Ordinal));
                    features.Add(fullTypeName, data);
                }

                var featureType = attributeClass.TypeArguments[0];
                var featureTypes = (HashSet<string>)data.FeatureTypes;

                        while (featureType is INamedTypeSymbol { BaseType.ConstructedFrom: { } cf } && cf.Equals(featureBaseType, comparer) == false)
                        {
                            cancellationToken.ThrowIfCancellationRequested();

                    featureTypes.Add(featureType.ToDisplayString(FullyQualifiedFormat));

                    foreach (var @interface in featureType.AllInterfaces)
                    {
                        featureTypes.Add(@interface.ToDisplayString(FullyQualifiedFormat));
                    }

                    featureType = featureType.BaseType;
                }
            }

            if (features.Count == 0)
                // No relevant SupportsFeatureAttribute has been found. Strange, but skip this code-gen target
                return;

            #endregion

            #region Detect whether we need to call base.GetDerived<T>() from our generated override

            var invokeBaseImpl = false;
            type = targetType;
            while (!(type = type.BaseType)!.Equals(objectType, comparer))
            {
                cancellationToken.ThrowIfCancellationRequested();

                foreach (var member in type.GetMembers("GetFeature"))
                {
                    if (member is not IMethodSymbol { IsGenericMethod: true, TypeArguments.Length: 1, Parameters.Length: 0, IsOverride: true })
                        continue;

                    invokeBaseImpl = true;
                    break;
                }

                if (invokeBaseImpl) break;

                cancellationToken.ThrowIfCancellationRequested();

                foreach (var attribute in type.GetAttributes())
                {
                    cancellationToken.ThrowIfCancellationRequested();

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

            cancellationToken.ThrowIfCancellationRequested();

            var code = Generator.GenerateAugmentation(targetType.Name, targetType.ContainingNamespace.ToDisplayString(), features, invokeBaseImpl, cancellationToken);
            ctx.AddSource($"{targetType.ToDisplayString()}.g.cs", SourceText.From(code, Encoding.UTF8));
        });
    }
}