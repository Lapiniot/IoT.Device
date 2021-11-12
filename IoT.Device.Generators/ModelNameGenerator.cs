using System.Text;
using IoT.Device.Generators.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using Parser = IoT.Device.Generators.ExportAttributeSyntaxParser;
using Generator = IoT.Device.Generators.ModelNameSyntaxGenerator;

namespace IoT.Device.Generators;

#pragma warning disable RS2008

[Generator]
public class ModelNameGenerator : IIncrementalGenerator
{
    private static readonly DiagnosticDescriptor NoPartialModifier = new("MNGEN001",
        title: "Generation warning",
        messageFormat: "Class is marked for export and has abstract property 'ModelName' that can be generated from the relevant ExportAttribute.ModelName, but class declaration has no 'partial' modifier keyword, so it cannot be augmented by the generator",
        category: nameof(ModelNameGenerator),
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor AbstractClassNotSupported = new("MNGEN002",
        title: "Generation warning",
        messageFormat: "Using ExportAttribute with abstract classes is meaningless, consider using it with purpose for concrete final classes",
        category: nameof(ModelNameGenerator),
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var exportDescriptors = context.SyntaxProvider.CreateSyntaxProvider(
            static (node, _) => Parser.IsSuitableCandidate(node),
            static (context, ct) => Parser.Parse((AttributeSyntax)context.Node, context.SemanticModel, ct))
            .Where(descriptor => descriptor is not null);

        context.RegisterSourceOutput(exportDescriptors, static (context, source) =>
        {
            var (_, type, model) = source!;

            if(type.GetBaseTypes().Any(bt => bt.GetMembers("ModelName").Any(p => p is IPropertySymbol
                {
                    IsAbstract: true,
                    Type: { Name: "String", ContainingNamespace: { Name: "System", ContainingNamespace.IsGlobalNamespace: true } }
                })) &&
                !type.GetMembers("ModelName").Any(p => p is IPropertySymbol { IsOverride: true }))
            {
                if(type is not { DeclaringSyntaxReferences: { Length: > 0 } dsr } || dsr[0].GetSyntax(context.CancellationToken) is not ClassDeclarationSyntax cds)
                {
                    // Type has no declaration parts in the current context
                    return;
                }

                if(!cds.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)))
                {
                    context.ReportDiagnostic(Diagnostic.Create(NoPartialModifier, cds.GetLocation()));
                    return;
                }

                if(type.IsAbstract)
                {
                    context.ReportDiagnostic(Diagnostic.Create(AbstractClassNotSupported, cds.GetLocation()));
                    return;
                }

                var code = Generator.GenerateAugmentationClass(type, model);

                context.AddSource($"{type.ToDisplayString()}.g.cs", SourceText.From(code.ToFullString(), Encoding.UTF8));
            }
        });
    }
}