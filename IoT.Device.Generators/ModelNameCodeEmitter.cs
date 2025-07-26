using System.Text;

namespace IoT.Device.Generators;

internal static class ModelNameCodeEmitter
{
    public static string Emit(string className, string namespaceName, string model)
    {
        var sb = new StringBuilder();
        CodeEmitHelper.AppendFileHeader(sb);
        sb.Append("""

        #pragma warning disable CS1591

        namespace 
        """);
        sb.AppendLine(namespaceName);
        sb.Append("""
        {
            public partial class 
        """);
        sb.AppendLine(className);
        sb.Append("""
            {
                
        """);
        CodeEmitHelper.AppendGeneratedCodeAttribute(sb);
        sb.Append("""

                public override string ModelName => "
        """);
        sb.Append(model);
        sb.Append("""
        ";
            }
        }
        """);

        return sb.ToString();
    }
}