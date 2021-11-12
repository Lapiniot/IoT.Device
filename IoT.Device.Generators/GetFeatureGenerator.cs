using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using IoT.Device.Generators.Helpers;

using Parser = IoT.Device.Generators.SupportsFeatureSyntaxParser;
using Generator = IoT.Device.Generators.GetFeatureSyntaxGenerator;

namespace IoT.Device.Generators;

// TODO: Filter off inaccessible feature types (abstract classes, missing suitable constructor etc.)
// TODO: Emit warnings about wrongly supplied feature types
[Generator]
public class GetFeatureGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var source = context.SyntaxProvider.CreateSyntaxProvider(
            static (node, _) => Parser.IsSuitableCandidate(node),
            static (context, ct) => Parser.Parse((ClassDeclarationSyntax)context.Node, context.SemanticModel, ct))
            .Where(symbol => symbol is not null);

        context.RegisterSourceOutput(source, (context, symbol) =>
        {
            var attributes = symbol!.GetSupportsFeatureAttributes();

            if(attributes.Length > 0)
            {
                context.AddSource($"{symbol!.Name}.GetFeature.g.cs",
                    SourceText.From(Generator.GenerateAugmentation(symbol, attributes).ToFullString(), encoding: Encoding.UTF8));
            }
        });
    }
}