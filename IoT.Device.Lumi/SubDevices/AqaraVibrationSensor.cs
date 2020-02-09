using System.Text.Json;
using IoT.Device.Metadata;
using static System.Text.Json.JsonValueKind;
using static IoT.Device.Metadata.ConnectivityTypes;
using static IoT.Device.Metadata.PowerSource;

namespace IoT.Device.Lumi.SubDevices
{
    [ModelID("DJT11LM")]
    [PowerSource(CR2032)]
    [ConnectivityType(ZigBee)]
    public class AqaraVibrationSensor : LumiSubDeviceWithStatus
    {
        private int bedActivity;
        private string coordinates;
        private int finalTiltAngle;

        internal AqaraVibrationSensor(string sid, int id) : base(sid, id) {}

        public override string Model => "vibration";

        public int FinalTiltAngle
        {
            get => finalTiltAngle;
            set => Set(ref finalTiltAngle, value);
        }

        public string Coordinates
        {
            get => coordinates;
            set => Set(ref coordinates, value);
        }

        public int BedActivity
        {
            get => bedActivity;
            set => Set(ref bedActivity, value);
        }

        protected internal override void OnStateChanged(JsonElement state)
        {
            base.OnStateChanged(state);

            if(state.TryGetProperty("final_tilt_angle", out var value) && value.ValueKind == Number)
            {
                FinalTiltAngle = value.GetInt32();
            }

            if(state.TryGetProperty("coordination", out value) && value.ValueKind == String)
            {
                Coordinates = value.GetString();
            }

            if(state.TryGetProperty("bed_activity", out value) && value.ValueKind == Number)
            {
                BedActivity = value.GetInt32();
            }
        }
    }
}