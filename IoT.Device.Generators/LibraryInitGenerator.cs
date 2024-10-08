using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.SymbolDisplayFormat;
using Generator = IoT.Device.Generators.LibraryInitSyntaxGenerator;
using Parser = IoT.Device.Generators.ExportAttributeSyntaxParser;

#pragma warning disable RS2008 // Enable analyzer release tracking

namespace IoT.Device.Generators;

[Generator]
public class LibraryInitGenerator : IIncrementalGenerator
{
    private static readonly DiagnosticDescriptor NoDefNamespaceWarning = new("LIGEN001",
        title: "Generation warning",
        messageFormat: "Cannot get library's default namespace",
        category: "LibraryInitGenerator", DiagnosticSeverity.Warning, isEnabledByDefault: true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var targets = context.SyntaxProvider.CreateSyntaxProvider(
            static (syntaxNode, _) => Parser.IsSyntaxTargetForGeneration(syntaxNode),
            static (context, ct) => Parser.GetSemanticTargetForGeneration((ClassDeclarationSyntax)context.Node, context.SemanticModel, ct))
            .Where(static target => target is not null);

        var combined = context.CompilationProvider
            .Combine(context.AnalyzerConfigOptionsProvider)
            .Combine(targets.Collect());

        context.RegisterSourceOutput(combined, static (ctx, source) =>
        {
            var ((compilation, options), targets) = source;

            var assemblyName = compilation.AssemblyName;

            if (string.IsNullOrEmpty(assemblyName))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(NoDefNamespaceWarning, Location.None));
                return;
            }

            var config = options.GlobalOptions;
            const string prefix = "build_property.library_init_generator_";

            var code = Generator.GenerateLibInitClass(assemblyName!,
                config.TryGetValue($"{prefix}class_name", out var value) && value is not "" ? value : "Library",
                config.TryGetValue($"{prefix}init_method_name", out value) && value is not "" ? value : "Init",
                GetExportDescriptors(targets, compilation, ctx.CancellationToken));

            ctx.AddSource("LibraryInit.g.cs", SourceText.From(code, Encoding.UTF8));
        });
    }

    private static IEnumerable<(string, string, string)> GetExportDescriptors(ImmutableArray<ClassDeclarationSyntax?> targets,
        Compilation compilation, CancellationToken cancellationToken)
    {
        foreach (var target in targets)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (target is null ||
                compilation.GetSemanticModel(target.SyntaxTree) is not { } semanticModel ||
                semanticModel.GetDeclaredSymbol(target, cancellationToken) is not INamedTypeSymbol implType)
            {
                continue;
            }

            if (Parser.TryGetExportAttribute(implType, out var attribute, out var targetType, cancellationToken) &&
                attribute is
                {
                    ConstructorArguments: [
                    {
                        Kind: TypedConstantKind.Primitive,
                        Type.SpecialType: SpecialType.System_String,
                        Value: string modelId
                    }, ..]
                })
            {
                yield return new(targetType!.ToDisplayString(FullyQualifiedFormat), implType.ToDisplayString(FullyQualifiedFormat), modelId);
            }
        }
    }
}