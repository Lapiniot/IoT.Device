using System.Text.Json;
using static System.Globalization.CultureInfo;
using static System.Globalization.NumberStyles;

namespace IoT.Device.Lumi
{
    public abstract class LumiMotionSensor : LumiSubDeviceWithStatus
    {
        private int noMotionSeconds;

        protected LumiMotionSensor(string sid, int id) :
            base(sid, id, "nomotion") {}

        public int NoMotionSeconds
        {
            get => noMotionSeconds;
            private set => Set(ref noMotionSeconds, value);
        }

        protected internal override void OnStateChanged(JsonElement state)
        {
            var prevStatus = Status;

            base.OnStateChanged(state);

            if(state.TryGetProperty("status", out var value) && value.ValueKind == JsonValueKind.String)
            {
                NoMotionSeconds = 0;

                // Tricky part: motion sensors usually notify about motion fact
                // by sending {"status":"motion"} frequently several times in row.
                // Default "Status" property implementation triggers OnPropertyChanged event
                // only when underlying field value changes.
                // Here we handle status event in a slightly different way and trigger
                // OnPropertyChanged for every repeating "motion" status update disregard of
                // the previous state value.
                if(value.GetString() == prevStatus)
                {
                    OnPropertyChanged(nameof(Status));
                }
            }
            else if(state.TryGetProperty("no_motion", out value) && value.ValueKind == JsonValueKind.String &&
                    int.TryParse(value.GetString(), Any, InvariantCulture, out var intVal))
            {
                NoMotionSeconds = intVal;
                Status = "nomotion";
            }
        }
    }
}