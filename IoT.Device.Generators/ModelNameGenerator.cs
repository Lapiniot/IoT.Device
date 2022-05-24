using System.Text;
using IoT.Device.Generators.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Parser = IoT.Device.Generators.ExportAttributeSyntaxParser;
using Generator = IoT.Device.Generators.ModelNameSyntaxGenerator;
using static Microsoft.CodeAnalysis.TypedConstantKind;
using static Microsoft.CodeAnalysis.SpecialType;

namespace IoT.Device.Generators;

#pragma warning disable RS2008

[Generator]
public class ModelNameGenerator : IIncrementalGenerator
{
    private static readonly DiagnosticDescriptor NoPartialModifier = new("MNGEN001",
        "Generation warning",
        "Class is marked for export and has abstract property 'ModelName' that can be generated from the relevant ExportAttribute.ModelName, but class declaration has no 'partial' modifier keyword, so it cannot be augmented by the generator",
        nameof(ModelNameGenerator),
        DiagnosticSeverity.Warning,
        true);

    private static readonly DiagnosticDescriptor AbstractClassNotSupported = new("MNGEN002",
        "Generation warning",
        "Using ExportAttribute with abstract classes is meaningless, consider using it with purpose for concrete final classes",
        nameof(ModelNameGenerator),
        DiagnosticSeverity.Warning,
        true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var targets = context.SyntaxProvider.CreateSyntaxProvider(
                static (syntaxNode, _) => Parser.IsSyntaxTargetForGeneration(syntaxNode),
                static (context, ct) => Parser.GetSemanticTargetForGeneration((ClassDeclarationSyntax)context.Node, context.SemanticModel, ct))
            .Where(static target => target is not null);

        var combined = context.CompilationProvider.Combine(targets.Collect());

        context.RegisterSourceOutput(combined, static (context, source) =>
        {
            var (compilation, targets) = source;
            var cancellationToken = context.CancellationToken;

            foreach (var target in targets)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (target is null ||
                    compilation.GetSemanticModel(target.SyntaxTree) is not { } model ||
                    model.GetDeclaredSymbol(target, cancellationToken) is not INamedTypeSymbol implType)
                {
                    continue;
                }

                if (!implType.GetBaseTypes().Any(bt => bt.GetMembers("ModelName").Any(p => p is IPropertySymbol
                    {
                        IsAbstract: true,
                        Type.SpecialType: System_String
                    })) || implType.GetMembers("ModelName").Any(p => p is IPropertySymbol { IsOverride: true }))
                {
                    continue;
                }

                if (!target.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)))
                {
                    context.ReportDiagnostic(Diagnostic.Create(NoPartialModifier, target.GetLocation()));
                    return;
                }

                if (implType.IsAbstract)
                {
                    context.ReportDiagnostic(Diagnostic.Create(AbstractClassNotSupported, target.GetLocation()));
                    return;
                }

                if (!Parser.TryGetExportAttribute(implType, out var attribute, out var targetType, cancellationToken))
                    continue;

                var modelName = attribute switch
                {
                    // ModelName named argument specified explicitely, take it
                    {
                        NamedArguments: [{ Key: "ModelName", Value: { Kind: Primitive, Type.SpecialType: System_String, Value: string value } }]
                    } => value,
                    // Attribute constructor has second string type argument, take it
                    {
                        ConstructorArguments: [_, { Kind: Primitive, Type.SpecialType: System_String, Value: string value }, ..]
                    } => value,
                    // Attribute constructor has only first argument of type string which is also modelId by convension, use it
                    {
                        ConstructorArguments: [{ Kind: Primitive, Type.SpecialType: System_String, Value: string value }, ..]
                    } => value,
                    // Fallback to a very unlikely situation
                    _ => "unknown"
                };

                var code = Generator.GenerateAugmentationClass(implType.Name, implType.ContainingNamespace.ToDisplayString(), modelName);
                context.AddSource($"{implType.ToDisplayString()}.g.cs", SourceText.From(code.ToFullString(), Encoding.UTF8));
            }
        });
    }
}