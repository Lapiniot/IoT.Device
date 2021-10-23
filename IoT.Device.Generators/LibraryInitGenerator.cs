using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

using SG = IoT.Device.Generators.LibraryInitSyntaxGenerator;

namespace IoT.Device.Generators;

[Generator]
public class LibraryInitGenerator : ISourceGenerator
{
#pragma warning disable RS2008
    private static readonly DiagnosticDescriptor NoDefNamespaceWarning = new("LIGEN001",
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
            context.ReportDiagnostic(Diagnostic.Create(NoDefNamespaceWarning, Location.None));
            return;
        }

        var body = context.SyntaxContextReceiver is FilterExportAttributesSyntaxContextReceiver sr
            ? SG.GenerateExportStatements(sr.Exports)
            : null;

        var code = SG.GenerateLibInitClass(assemblyName, "Library", "Init", body);

        context.AddSource("LibraryInit.Generated.cs", SourceText.From(code.ToFullString(), Encoding.UTF8));
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new FilterExportAttributesSyntaxContextReceiver());
    }
}