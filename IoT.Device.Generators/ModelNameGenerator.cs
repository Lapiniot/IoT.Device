using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using SourceContext = (string NamespaceName, string TypeName, string ModelName,
    Microsoft.CodeAnalysis.Accessibility Accessibility,
    IoT.Device.Generators.DiagnosticContext? Diagnostic);

#pragma warning disable RS2008 // Enable analyzer release tracking

namespace IoT.Device.Generators;

[Generator]
public class ModelNameGenerator : IIncrementalGenerator
{
    private static readonly SymbolDisplayFormat? OmitGlobalFormat = SymbolDisplayFormat.FullyQualifiedFormat.
        WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted);

    private static readonly DiagnosticDescriptor NoPartialWarning = new("MNGEN001",
        "Generation warning",
        "Class is marked for export and has abstract property 'ModelName' that can be generated from the relevant ExportAttribute.ModelName, " +
        "but class declaration has no 'partial' modifier keyword, so it cannot be augmented by the generator",
        nameof(ModelNameGenerator), DiagnosticSeverity.Warning, true);

    private static readonly DiagnosticDescriptor AbstractClassNotSupportedWarning = new("MNGEN002",
        "Generation warning",
        "Abstract class is marked with ExportAttribute which makes no sense at all, consider using it with purpose for concrete final classes",
        nameof(ModelNameGenerator), DiagnosticSeverity.Warning, true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var targets = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: static (syntaxNode, _) => syntaxNode is ClassDeclarationSyntax { AttributeLists.Count: > 0 },
            transform: GetSourceGenerationContext)
            .Where(static m => m is { TypeName: not null } or { Diagnostic: not null });

        context.RegisterSourceOutput(targets, static (context, source) =>
        {
            var (namespaceName, typeName, modelName, accessibility, diagnostic) = source;

            if (diagnostic is { } value)
            {
                context.ReportDiagnostic(value.ToDiagnostic());
                return;
            }

            var code = ModelNameCodeEmitter.Emit(typeName, namespaceName, modelName, accessibility);
            var fileName = !string.IsNullOrEmpty(namespaceName)
                ? namespaceName + "." + typeName
                : typeName;
            context.AddSource($"{fileName}.g.cs", SourceText.From(code, Encoding.UTF8));
        });
    }

    private static SourceContext GetSourceGenerationContext(GeneratorSyntaxContext context, CancellationToken token)
    {
        if (context.SemanticModel.GetDeclaredSymbol(context.Node, token) is INamedTypeSymbol implType)
        {
            if (!ExportAttributeSyntaxHelper.TryGetExportAttribute(implType, out _, out string? model, token))
            {
                goto Skip;
            }

            foreach (var member in implType.GetMembers("ModelName"))
            {
                if (member is IPropertySymbol)
                {
                    // Class already contains ModelName property defined, skip generation
                    goto Skip;
                }
            }

            token.ThrowIfCancellationRequested();

            var type = implType;
            while ((type = type!.BaseType) is { })
            {
                foreach (var member in type.GetMembers("ModelName"))
                {
                    token.ThrowIfCancellationRequested();

                    if (member is IPropertySymbol
                        {
                            IsAbstract: var isAbstract, IsVirtual: var isVirtual, IsSealed: var isSealed,
                            IsReadOnly: true, Type.SpecialType: SpecialType.System_String,
                            GetMethod.DeclaredAccessibility: Accessibility.Public
                        })
                    {
                        if (isSealed)
                        {
                            // One of the type's ancestors has sealed ModelName property,
                            // thus there is no way to define new override, just skip such type
                            goto Skip;
                        }

                        if (isAbstract || isVirtual)
                        {
                            // Found type's ancestor which defines abstract or virtual ModelName property,
                            // suitable for override, just stop scan and go to the next check
                            goto CheckType;
                        }
                    }
                }
            }

            // No suitable ModelName property found for override during ancestor types traversal loop
            goto Skip;

        CheckType:
            token.ThrowIfCancellationRequested();

            var target = (ClassDeclarationSyntax)context.Node;

            if (implType.IsAbstract)
            {
                return default(SourceContext) with
                {
                    Diagnostic = new(AbstractClassNotSupportedWarning, LocationContext.Create(target.GetLocation()))
                };
            }

            foreach (var item in target.Modifiers)
            {
                if (item.IsKind(SyntaxKind.PartialKeyword))
                {
                    goto Success;
                }
            }

            return default(SourceContext) with
            {
                Diagnostic = new(NoPartialWarning, LocationContext.Create(target.GetLocation()))
            }
        ;

        Success:
            return (implType.ContainingNamespace.ToDisplayString(OmitGlobalFormat),
                implType.Name, model, implType.DeclaredAccessibility, Diagnostic: null);
        }

    Skip:
        return default;
    }
}