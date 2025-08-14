using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

#pragma warning disable RS2008 // Enable analyzer release tracking

namespace IoT.Device.Generators;

[Generator]
public class LibraryInitGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var configOptionsProvider = context.AnalyzerConfigOptionsProvider.Select(static (cfg, _) => GetConfigOptions(cfg));

        var targets = context.SyntaxProvider.CreateSyntaxProvider<ExportContext>(
            static (node, _) => node is ClassDeclarationSyntax { AttributeLists.Count: > 0 },
            static (context, ct) =>
                context.SemanticModel.GetDeclaredSymbol(context.Node, ct) is INamedTypeSymbol implType &&
                ExportAttributeSyntaxHelper.TryGetExportAttribute(implType, out var targetType, out string? model, ct)
                    ? new(
                        targetType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                        implType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                        model)
                    : default)
            .Where(m => m is { ImplType: not null });

        var combined = configOptionsProvider.Combine(targets.Collect());

        context.RegisterSourceOutput(combined, static (ctx, source) =>
        {
            var ((namespaceName, className, methodName), targets) = source;
            var code = LibraryInitCodeEmitter.Emit(namespaceName, className, methodName, targets);
            ctx.AddSource("LibraryInit.g.cs", SourceText.From(code, Encoding.UTF8));
        });
    }

    private static (string NamespaceName, string ClassName, string MethodName) GetConfigOptions(
        AnalyzerConfigOptionsProvider configProvider)
    {
        const string prefix = "build_property.LibraryInitGenerator";
        var global = configProvider.GlobalOptions;
        return (
            NamespaceName: global.TryGetValue($"{prefix}NamespaceName", out var value) && value is { Length: > 0 }
                ? value
                : global.TryGetValue($"build_property.RootNamespace", out value)
                    ? value
                    : "",
            ClassName: global.TryGetValue($"{prefix}ClassName", out value) && value is { Length: > 0 }
                ? value
                : "Library",
            MethodName: global.TryGetValue($"{prefix}InitMethodName", out value) && value is { Length: > 0 }
                ? value :
                "Init"
        );
    }
}