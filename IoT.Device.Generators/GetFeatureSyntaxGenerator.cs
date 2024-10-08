using System.Text;

namespace IoT.Device.Generators;

internal record struct ConditionData(string FieldName, IReadOnlyCollection<string> FeatureTypes);

internal static class GetFeatureSyntaxGenerator
{
    public static string GenerateAugmentation(string className, string namespaceName,
        Dictionary<string, ConditionData> model, bool invokeBaseImpl,
        CancellationToken cancellationToken)
    {
        var sb = new StringBuilder();
        sb.Append("""
        //------------------------------------------------------------------------------
        // <auto-generated>
        //      This code was generated by a tool.
        //      
        """);
        sb.Append(ProductInfo.Product);
        sb.Append(" v");
        sb.Append(ProductInfo.Version);
        sb.Append(" (");
        sb.Append(ProductInfo.Copyright);
        sb.Append("""
        )
        //
        //      Changes to this file may cause incorrect behavior and will be lost
        //      if the code is regenerated.
        // </auto-generated>
        //------------------------------------------------------------------------------

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
        foreach (var pair in model)
        {
            sb.Append("""
                    private 
            """);
            sb.Append(pair.Key);
            sb.Append(' ');
            sb.Append(pair.Value.FieldName);
            sb.AppendLine(";");
        }

        cancellationToken.ThrowIfCancellationRequested();

        sb.Append("""

                public override T GetFeature<T>()
                {
                    var type = typeof(T);


        """);

        foreach (var pair in model)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var data = pair.Value;

            sb.Append("""
                        if(
            """);
            var first = true;

            foreach (var featureType in data.FeatureTypes)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (!first)
                {
                    sb.Append("""
                     ||
                                    
                    """);
                }

                sb.Append("type == typeof(");
                sb.Append(featureType);
                sb.Append(')');

                first = false;
            }

            sb.Append("""
            )
                        {
                            return (
            """);
            sb.Append(data.FieldName);
            sb.Append("""
             ??= new(this)) as T;
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