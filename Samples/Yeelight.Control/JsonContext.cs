using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Yeelight.Control;

[JsonSourceGenerationOptions(UnknownTypeHandling = JsonUnknownTypeHandling.JsonNode, Converters = [typeof(ObjectConverter)])]
[JsonSerializable(typeof(object))]
[JsonSerializable(typeof(JsonElement))]
[JsonSerializable(typeof(JsonNode))]
internal sealed partial class JsonContext : JsonSerializerContext
{
    private sealed class ObjectConverter : JsonConverter<object>
    {
        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            do
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.StartObject: return ReadObject(ref reader);
                    case JsonTokenType.StartArray: return ReadArray(ref reader);
                    case JsonTokenType.String: return reader.GetString();
                    case JsonTokenType.Number: return reader.GetInt64();
                    case JsonTokenType.Null: return null;
                    case JsonTokenType.False: return false;
                    case JsonTokenType.True: return true;
                    default: ThrowInvalidTokenType(reader.TokenType); break;
                }
            } while (reader.Read());

            return null;
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options) =>
            throw new NotSupportedException();

        private static object?[] ReadArray(ref Utf8JsonReader reader)
        {
            var list = new List<object?>();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.String: list.Add(reader.GetString()); break;
                    case JsonTokenType.Number: list.Add(ReadNumber(ref reader)); break;
                    case JsonTokenType.Null: list.Add(null); break;
                    case JsonTokenType.False: list.Add(false); break;
                    case JsonTokenType.True: list.Add(true); break;
                    case JsonTokenType.StartArray: list.Add(ReadArray(ref reader)); break;
                    case JsonTokenType.StartObject: list.Add(ReadObject(ref reader)); break;
                    case JsonTokenType.EndArray: return [.. list];
                    default: ThrowInvalidTokenType(reader.TokenType); break;
                }
            }

            return [.. list];
        }

        private static Dictionary<string, object?> ReadObject(ref Utf8JsonReader reader)
        {
            var bag = new Dictionary<string, object?>();
            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var name = reader.GetString()!;
                    if (reader.Read())
                    {
                        switch (reader.TokenType)
                        {
                            case JsonTokenType.String: bag[name] = reader.GetString(); break;
                            case JsonTokenType.Number: bag[name] = ReadNumber(ref reader); break;
                            case JsonTokenType.Null: bag[name] = null; break;
                            case JsonTokenType.False: bag[name] = false; break;
                            case JsonTokenType.True: bag[name] = true; break;
                            case JsonTokenType.StartArray: bag[name] = ReadArray(ref reader); break;
                            case JsonTokenType.StartObject: bag[name] = ReadObject(ref reader); break;
                            default: ThrowInvalidTokenType(reader.TokenType); break;
                        }
                    }
                }
                else
                {
                    ThrowInvalidTokenType(reader.TokenType);
                }
            }

            return bag;
        }

        private static object? ReadNumber(ref Utf8JsonReader reader) =>
            reader.TryGetInt64(out var longValue) ? longValue : reader.TryGetDouble(out var doubleValue) ? doubleValue : null;

        [DoesNotReturn]
        private static void ThrowInvalidTokenType(JsonTokenType tokenType) =>
            throw new InvalidOperationException($"Unexpected {nameof(JsonTokenType)}: {tokenType}.");
    }
}