using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.TypedConstantKind;
using static Microsoft.CodeAnalysis.SpecialType;
using Parser = IoT.Device.Generators.ExportAttributeSyntaxParser;
using Generator = IoT.Device.Generators.ModelNameSyntaxGenerator;

namespace IoT.Device.Generators;

#pragma warning disable RS2008

[Generator]
public class ModelNameGenerator : IIncrementalGenerator
{
    private static readonly DiagnosticDescriptor NoPartialWarning = new("MNGEN001",
        "Generation warning.",
        "Class is marked for export and has abstract property 'ModelName' that can be generated from the relevant ExportAttribute.ModelName, " +
        "but class declaration has no 'partial' modifier keyword, so it cannot be augmented by the generator.",
        nameof(ModelNameGenerator), DiagnosticSeverity.Warning, true);

    private static readonly DiagnosticDescriptor AbstractClassNotSupportedWarning = new("MNGEN002",
        "Generation warning.",
        "Using ExportAttribute with abstract classes is meaningless, consider using it with purpose for concrete final classes.",
        nameof(ModelNameGenerator), DiagnosticSeverity.Warning, true);

    private static readonly SyntaxTargetOnlyComparer<ClassDeclarationSyntax> SyntaxTargetOnlyComparer = new();

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var targets = context.SyntaxProvider.CreateSyntaxProvider(
                static (syntaxNode, _) => Parser.IsSyntaxTargetForGeneration(syntaxNode),
                static (context, ct) => Parser.GetSemanticTargetForGeneration((ClassDeclarationSyntax)context.Node, context.SemanticModel, ct))
            .Where(static target => target is not null);

        var combined = targets.Combine(context.CompilationProvider).WithComparer(SyntaxTargetOnlyComparer);

        context.RegisterSourceOutput(combined, static (context, source) =>
        {
            var (target, compilation) = source;
            var cancellationToken = context.CancellationToken;

            cancellationToken.ThrowIfCancellationRequested();

            if (target is null ||
                compilation.GetSemanticModel(target.SyntaxTree) is not { } model ||
                model.GetDeclaredSymbol(target, cancellationToken) is not { } implType)
            {
                return;
            }

            var shouldSkip = false;

            foreach (var member in implType.GetMembers("ModelName"))
            {
                if (member is IPropertySymbol)
                    // Class already contains ModelName property defined, skip generation
                    return;
            }

            shouldSkip = true;
            var type = implType;
            while ((type = type!.BaseType) is { })
            {
                cancellationToken.ThrowIfCancellationRequested();
                var shouldBreak = false;

                foreach (var member in type.GetMembers("ModelName"))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (member is IPropertySymbol
                        {
                            IsAbstract: var isAbstract, IsSealed: var isSealed, IsReadOnly: true,
                            Type.SpecialType: System_String,
                            GetMethod.DeclaredAccessibility: Accessibility.Public
                        })
                    {
                        if (isAbstract)
                        {
                            shouldSkip = false;
                            shouldBreak = true;
                            break;
                        }

                        if (isSealed)
                        {
                            shouldBreak = true;
                            break;
                        }
                    }
                }

                if (shouldBreak) break;
            }

            // None of class base types has readonly abstract property ModelName, which we can override
            if (shouldSkip) return;

            shouldSkip = true;

            foreach (var item in target.Modifiers)
            {
                if (item.IsKind(SyntaxKind.PartialKeyword))
                {
                    shouldSkip = false;
                    break;
                }
            }

            if (shouldSkip)
            {
                // Class is not partial, thus no chance to extend via code generation
                context.ReportDiagnostic(Diagnostic.Create(NoPartialWarning, target.GetLocation()));
                return;
            }

            if (implType.IsAbstract)
            {
                // Abstract classes are not supported too
                context.ReportDiagnostic(Diagnostic.Create(AbstractClassNotSupportedWarning, target.GetLocation()));
                return;
            }

            if (!Parser.TryGetExportAttribute(implType, out var attribute, out _, cancellationToken))
                // weird situation, should be impossible to get here, but just skip so far
                return;

            var modelName = attribute switch
            {
                // ModelName named argument specified explicitly, take it
                {
                    NamedArguments: [{ Key: "ModelName", Value: { Kind: Primitive, Type.SpecialType: System_String, Value: string value } }]
                } => value,
                // Attribute constructor has second string type argument, take it
                {
                    ConstructorArguments: [_, { Kind: Primitive, Type.SpecialType: System_String, Value: string value }, ..]
                } => value,
                // Attribute constructor has only first argument of type string which is also modelId by convention, use it
                {
                    ConstructorArguments: [{ Kind: Primitive, Type.SpecialType: System_String, Value: string value }, ..]
                } => value,
                // Fallback to a very unlikely situation
                _ => "unknown"
            };

            var code = Generator.GenerateAugmentationClass(implType.Name, implType.ContainingNamespace.ToDisplayString(), modelName);
            context.AddSource($"{implType.ToDisplayString()}.g.cs", SourceText.From(code, Encoding.UTF8));
        });
    }
}