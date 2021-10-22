using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace IoT.Device.Generators;

[Generator]
public partial class LibraryInitGenerator : ISourceGenerator
{
#pragma warning disable RS2008
    private static readonly DiagnosticDescriptor NoDefNamespaceWarningDescriptor = new("LIGEN001",
        title: "Generation warning",
        messageFormat: "Cannot get library's default namespace",
        category: "LibraryInitGenerator",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public void Execute(GeneratorExecutionContext context)
    {
        string? assemblyName = context.Compilation.AssemblyName;

        if(string.IsNullOrEmpty(assemblyName))
        {
            context.ReportDiagnostic(Diagnostic.Create(NoDefNamespaceWarningDescriptor, Location.None));
            return;
        }

        var code = CodeGenerator.GenerateLibInitClass(assemblyName, "Library", "Init");

        context.AddSource("LibraryInit.Generated.cs", SourceText.From(code.ToFullString(), Encoding.UTF8));
    }

    public void Initialize(GeneratorInitializationContext context)
    {
    }
}