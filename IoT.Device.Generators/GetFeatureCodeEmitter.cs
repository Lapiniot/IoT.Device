using System.Collections.Immutable;
using System.Text;

namespace IoT.Device.Generators;

internal record struct ConditionData(string FieldName, IReadOnlyCollection<string> FeatureTypes);

internal static class GetFeatureCodeEmitter
{
    public static string Emit(string className, string namespaceName,
        ImmutableArray<FeatureContext> model, bool invokeBaseImpl,
        CancellationToken cancellationToken)
    {
        var sb = new StringBuilder();
        CodeEmitHelper.AppendFileHeader(sb);
        sb.Append("""

        #pragma warning disable CS1591

        namespace 
        """);

        cancellationToken.ThrowIfCancellationRequested();

        sb.AppendLine(namespaceName);
        sb.Append("""
        {
            public partial class 
        """);
        sb.AppendLine(className);
        sb.Append("""
            {

        """);

        for (int i = 0; i < model.Length; i++)
        {
            var (type, implType) = model[i];

            sb.Append("""
                    private 
            """);
            sb.Append(!string.IsNullOrEmpty(implType) ? implType : type);
            sb.Append(" feature");
            sb.Append(i);
            sb.AppendLine(";");
        }

        cancellationToken.ThrowIfCancellationRequested();

        sb.Append("""

                
        """);
        CodeEmitHelper.AppendGeneratedCodeAttribute(sb);
        sb.Append("""

                public override T GetFeature<T>()
                {
                    var type = typeof(T);


        """);

        for (int i = 0; i < model.Length; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var (type, implType) = model[i];

            if (implType is { Length: > 0 })
            {
                sb.Append($$"""
                            if(type == typeof({{implType}}) || 
                                type == typeof({{type}}))
                
                """);
            }
            else
            {
                sb.Append($$"""
                            if(type == typeof({{type}}))
                
                """);
            }

            sb.Append($$"""
                        {
                            return (feature{{i}} ??= new(this)) as T;
                        }


            """);
        }

        if (invokeBaseImpl)
        {
            sb.Append("""
                        return base.GetFeature<T>();

            """);
        }
        else
        {
            sb.Append("""
                        return null;

            """);
        }

        sb.Append("""
                }
            }
        }
        """);

        return sb.ToString();
    }
}