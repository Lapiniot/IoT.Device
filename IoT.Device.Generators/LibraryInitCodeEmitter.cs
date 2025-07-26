using System.Text;

namespace IoT.Device.Generators;

internal static class LibraryInitCodeEmitter
{
    public static string Emit(string namespaceName, string className, string initMethodName,
        IEnumerable<(string TargetType, string ImplType, string Model)> exports)
    {
        var sb = new StringBuilder();
        CodeEmitHelper.AppendFileHeader(sb);
        sb.Append("""

        using IoT.Device;

        #pragma warning disable CS1591

        namespace 
        """);

        sb.AppendLine(namespaceName);
        sb.Append("""
        {
            
        """);
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
                            DeviceFactory<
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
        sb.Append(initMethodName);
        sb.Append("""
        Extra();
                }

                static partial void 
        """);
        sb.Append(initMethodName);
        sb.Append("""
        Extra();
            }
        }
        """);

        return sb.ToString();
    }
}