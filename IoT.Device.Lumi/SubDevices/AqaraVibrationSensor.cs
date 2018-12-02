using System.Json;
using IoT.Device.Metadata;
using static IoT.Device.Metadata.Connectivity;
using static IoT.Device.Metadata.PowerSource;

namespace IoT.Device.Lumi.SubDevices
{
    [ModelID("DJT11LM")]
    [PowerSource(CR2032)]
    [Connectivity(ZigBee)]
    public class AqaraVibrationSensor : LumiSubDeviceWithStatus
    {
        private int bedActivity;
        private string coordinates;
        private int finalTiltAngle;

        private AqaraVibrationSensor(string sid, int id) : base(sid, id) {}

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

        protected internal override void OnStateChanged(JsonObject state)
        {
            base.OnStateChanged(state);

            if(state.TryGetValue("final_tilt_angle", out var value))
            {
                FinalTiltAngle = value;
            }

            if(state.TryGetValue("coordination", out value))
            {
                Coordinates = value;
            }

            if(state.TryGetValue("bed_activity", out value))
            {
                BedActivity = value;
            }
        }
    }
}