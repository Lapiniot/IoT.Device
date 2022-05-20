using System.Text.Json;
using IoT.Device.Metadata;

using static System.Globalization.CultureInfo;
using static System.Globalization.NumberStyles;
using static IoT.Device.Metadata.ConnectivityTypes;
using static IoT.Device.Metadata.PowerSource;

namespace IoT.Device.Lumi.SubDevices;

[ExportSubDevice("weather.v1")]
[ModelID("WSDCGQ11LM")]
[PowerSource(CR2032)]
[ConnectivityType(ZigBee)]
public sealed partial class AqaraWeatherSensor : LumiSubDevice
{
    private decimal humidity;
    private decimal pressure;
    private decimal temperature;

    internal AqaraWeatherSensor(string sid, int id) : base(sid, id) { }

    public decimal Temperature
    {
        get => temperature;
        private set => Set(ref temperature, value);
    }

    public decimal Humidity
    {
        get => humidity;
        private set => Set(ref humidity, value);
    }

    public decimal Pressure
    {
        get => pressure;
        private set => Set(ref pressure, value);
    }

    protected internal override void OnStateChanged(JsonElement state)
    {
        base.OnStateChanged(state);

        if (state.TryGetProperty("temperature", out var value) && value.ValueKind == JsonValueKind.String &&
           int.TryParse(value.GetString(), Any, InvariantCulture, out var intVal)
        )
        {
            Temperature = new(intVal, 0, 0, false, 2);
        }

        if (state.TryGetProperty("humidity", out value) && value.ValueKind == JsonValueKind.String &&
           int.TryParse(value.GetString(), Any, InvariantCulture, out intVal))
        {
            Humidity = new(intVal, 0, 0, false, 2);
        }

        if (state.TryGetProperty("pressure", out value) && value.ValueKind == JsonValueKind.String &&
           int.TryParse(value.GetString(), Any, InvariantCulture, out intVal))
        {
            Pressure = new(intVal, 0, 0, false, 3);
        }
    }
}