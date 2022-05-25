using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using IoT.Device.Generators.Helpers;

using Parser = IoT.Device.Generators.SupportsFeatureSyntaxParser;
using Generator = IoT.Device.Generators.GetFeatureSyntaxGenerator;
using Microsoft.CodeAnalysis.CSharp;

namespace IoT.Device.Generators;

// TODO: Filter off inaccessible feature types (abstract classes, missing suitable constructor etc.)
// TODO: Emit warnings about wrongly supplied feature types
[Generator]
public class GetFeatureGenerator : IIncrementalGenerator
{
    private static readonly DiagnosticDescriptor NoPartialModifier = new("GFGEN001",
        "Generation warning.",
        "Class is marked with 'SupportsFeatureAttribute', but declaration has no 'partial' modifier keyword, so it cannot be augmented by the generator.",
        nameof(GetFeatureGenerator), DiagnosticSeverity.Warning, true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var targets = context.SyntaxProvider.CreateSyntaxProvider(
            static (syntaxNode, _) => Parser.IsSyntaxTargetForGeneration(syntaxNode),
            static (context, ct) => Parser.GetSemanticTargetForGeneration((ClassDeclarationSyntax)context.Node, context.SemanticModel, ct))
            .Where(symbol => symbol is not null);

        var combined = context.CompilationProvider.Combine(targets.Collect());

        context.RegisterSourceOutput(combined, (ctx, source) =>
        {
            var (compilation, targets) = source;

            foreach (var target in targets)
            {
                if (target is null ||
                    compilation.GetSemanticModel(target.SyntaxTree) is not { } model ||
                    model.GetDeclaredSymbol(target, ctx.CancellationToken) is not INamedTypeSymbol symbol)
                {
                    continue;
                }

                var shouldSkip = true;

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
                    ctx.ReportDiagnostic(Diagnostic.Create(NoPartialModifier, target.GetLocation()));
                    continue;
                }

                var attributes = symbol.GetSupportsFeatureAttributes();

                if (attributes.Length > 0)
                {
                    ctx.AddSource($"{symbol!.Name}.GetFeature.g.cs",
                        SourceText.From(Generator.GenerateAugmentation(symbol, attributes).ToFullString(), encoding: Encoding.UTF8));
                }
            }
        });
    }
}