using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

using SG = IoT.Device.Generators.GetFeatureSyntaxGenerator;

namespace IoT.Device.Generators;

// TODO: Filter off inaccessible feature types (abstract classes, missing suitable constructor etc.)
// TODO: Emit warnings about wrongly supplied feature types
// TODO: Add support for features described via interfaces
// TODO: Add support for SupportsFeatureAttribute<TFeature, TImpl>
[Generator]
public class GetFeatureGenerator : ISourceGenerator
{
#pragma warning disable RS2008
    private static readonly DiagnosticDescriptor GeneralWarning = new("GFGEN001",
        title: "Generation warning",
        messageFormat: "{0}",
        category: nameof(GetFeatureGenerator),
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public void Execute(GeneratorExecutionContext context)
    {
        if(context.SyntaxContextReceiver is FilterSupportsFeatureAttributesSyntaxContextReceiver sr)
        {
            foreach(var (Class, Attributes) in sr.Classes)
            {
                context.AddSource($"{Class.Name}.GetFeature.Generated.cs",
                    SourceText.From(SG.GenerateAugmentation(Class, Attributes).ToFullString(), encoding: Encoding.UTF8));
            }
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new FilterSupportsFeatureAttributesSyntaxContextReceiver());
    }
}