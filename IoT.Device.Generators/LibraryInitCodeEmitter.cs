using System.Collections.Immutable;
using System.Text;

namespace IoT.Device.Generators;

internal static class LibraryInitCodeEmitter
{
    public static string Emit(string namespaceName, string className, string initMethodName, ImmutableArray<ExportContext> exports)
    {
        var sb = new StringBuilder();
        CodeEmitHelper.AppendFileHeader(sb);
        sb.Append("""

        #pragma warning disable CS1591


        """);

        if (!string.IsNullOrWhiteSpace(namespaceName))
        {
            sb.Append($$"""
        namespace {{namespaceName}};


        """);
        }

        CodeEmitHelper.AppendGeneratedCodeAttribute(sb);
        sb.Append("""

        public static partial class 
        """);
        sb.AppendLine(className);
        sb.Append("""
        {
            public static void 
        """);
        sb.Append(initMethodName);
        sb.Append("""
        ()
            {

        """);

        foreach (var (targetType, implType, model) in exports)
        {
            sb.Append("""
                    global::IoT.Device.DeviceFactory<
            """);
            sb.Append(targetType);
            sb.Append(">.Register<");
            sb.Append(implType);
            sb.Append(@">(""");
            sb.Append(model);
            sb.Append("""
            ");

            """);
        }

        sb.Append("""


        """);
        sb.Append($$"""
                {{initMethodName}}Extra();
            }

            static partial void {{initMethodName}}Extra();
        }
        """);

        return sb.ToString();
    }
}