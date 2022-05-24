using System.Collections.Immutable;
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
        var targets = context.SyntaxProvider.CreateSyntaxProvider(
            static (syntaxNode, _) => Parser.IsSyntaxTargetForGeneration(syntaxNode),
            static (context, ct) => Parser.GetSemanticTargetForGeneration((ClassDeclarationSyntax)context.Node, context.SemanticModel, ct))
            .Where(static target => target is not null);

        var combined = context.CompilationProvider.Combine(targets.Collect());

        context.RegisterSourceOutput(combined, static (ctx, source) =>
        {
            var (compilation, targets) = source;

            var assemblyName = compilation.AssemblyName;

            if (string.IsNullOrEmpty(assemblyName))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(NoDefNamespaceWarning, Location.None));
                return;
            }

            var code = Generator.GenerateLibInitClass(assemblyName!, "Library", "Init", GetExportDescriptors(targets, compilation, ctx.CancellationToken));

            ctx.AddSource("LibraryInit.g.cs", SourceText.From(code.ToFullString(), Encoding.UTF8));
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
                yield return new(targetType!.ToDisplayString(), implType.ToDisplayString(), modelId);
            }
        }
    }
}