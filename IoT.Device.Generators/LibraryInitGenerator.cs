using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using Generator = IoT.Device.Generators.LibraryInitSyntaxGenerator;
using Parser = IoT.Device.Generators.ExportAttributeSyntaxParser;

namespace IoT.Device.Generators;

[Generator]
public class LibraryInitGenerator : IIncrementalGenerator
{
#pragma warning disable RS2008
    private static readonly DiagnosticDescriptor NoDefNamespaceWarning = new("LIGEN001",
        title: "Generation warning",
        messageFormat: "Cannot get library's default namespace",
        category: "LibraryInitGenerator",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var exportDescriptors = context.SyntaxProvider.CreateSyntaxProvider(
            static (node, _) => Parser.IsSuitableCandidate(node),
            static (context, ct) => Parser.Parse((AttributeSyntax)context.Node, context.SemanticModel, ct))
            .Where(s => s is not null);

        var combined = context.CompilationProvider.Combine(exportDescriptors.Collect());

        context.RegisterSourceOutput(combined, static (ctx, source) =>
        {
            var (compilation, descriptors) = source;

            var assemblyName = compilation.AssemblyName;

            if (string.IsNullOrEmpty(assemblyName))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(NoDefNamespaceWarning, Location.None));
                return;
            }

            var code = Generator.GenerateLibInitClass(assemblyName!, "Library", "Init", descriptors);

            ctx.AddSource("LibraryInit.g.cs", SourceText.From(code.ToFullString(), Encoding.UTF8));
        });
    }
}