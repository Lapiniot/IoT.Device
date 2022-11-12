using System.Text.Json;
using System.Text.Json.Serialization;

namespace IoT.Device.Lumi;

[JsonSerializable(typeof(JsonElement))]
internal sealed partial class JsonContext : JsonSerializerContext
{ }