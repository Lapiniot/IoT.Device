using System.Json;

namespace IoT.Device.Interfaces
{
    public interface IJsonCommunicationEndpoint : ICommunicationEndpoint<JsonObject, JsonValue>
    {
    }
}