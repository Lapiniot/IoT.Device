using System.Text;
using IoT.Device.Generators.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

using SG = IoT.Device.Generators.GetFeatureSyntaxGenerator;

namespace IoT.Device.Generators;

// TODO: Filter off inaccessible feature types (abstract classes, missing suitable constructor etc.)
// TODO: Emit warnings about wrongly supplied feature types
[Generator]
public class GetFeatureGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        if(context.SyntaxContextReceiver is FilterSupportsFeatureAttributesSyntaxContextReceiver sr)
        {
            foreach(var symbol in sr.Candidates)
            {
                var attributes = symbol.GetSupportsFeatureAttributes();

                if(attributes.Length > 0)
                {
                    context.AddSource($"{symbol.Name}.GetFeature.Generated.cs",
                        SourceText.From(SG.GenerateAugmentation(symbol, attributes).ToFullString(), encoding: Encoding.UTF8));
                }
            }
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new FilterSupportsFeatureAttributesSyntaxContextReceiver());
    }
}