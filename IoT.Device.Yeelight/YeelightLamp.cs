using System.Json;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IoT.Protocol.Interfaces;

namespace IoT.Device.Yeelight
{
    public abstract class YeelightLamp : YeelightDevice
    {
        protected YeelightLamp(IConnectedEndpoint<JsonObject, JsonValue> endpoint) : base(endpoint) { }

        public async Task<SwitchState> GetPowerStateAsync(CancellationToken cancellationToken = default)
        {
            return (await GetPropertiesAsync(cancellationToken, "power").ConfigureAwait(false))[0]
                .ToEnumValue<SwitchState>();
        }

        public Task<JsonValue> SetPowerStateAsync(SwitchState state = SwitchState.On, CancellationToken cancellationToken = default)
        {
            return InvokeAsync("set_power", new JsonArray { state.ToJsonValue() }, cancellationToken);
        }

        public async Task<uint> GetBrightnessAsync(CancellationToken cancellationToken = default)
        {
            return (await GetPropertiesAsync(cancellationToken, "bright").ConfigureAwait(false))[0];
        }

        public Task<JsonValue> SetBrightnessAsync(uint value, CancellationToken cancellationToken = default)
        {
            return InvokeAsync("set_bright", new JsonArray(value), cancellationToken);
        }

        public Task<JsonValue> ToggleAsync(CancellationToken cancellationToken = default)
        {
            return InvokeAsync("toggle", EmptyArgs, cancellationToken);
        }

        public Task<JsonValue> SetPowerStateAsync(SwitchState state = SwitchState.On, Effect effect = Effect.Smooth,
            uint durationMilliseconds = 500, CancellationToken cancellationToken = default)
        {
            var args = new JsonArray { state.ToJsonValue(), effect.ToJsonValue(), durationMilliseconds };

            return InvokeAsync("set_power", args, cancellationToken);
        }

        public async Task<(string Mac, int Pid, int EventId, string BeaconKey)[]> MiBandGetConnectedAsync(CancellationToken cancellationToken = default)
        {
            var args = new JsonObject { { "table", "evtRuleTbl" } };

            var records = (JsonArray)await InvokeAsync("ble_dbg_tbl_dump", args, cancellationToken).ConfigureAwait(false);

            return records.Select(r => ((string)r["mac"], (int)r["pid"],
                (int)r["evtid"], (string)r["beaconkey"])).ToArray();
        }

        #region Power state defaults management

        public async Task<SwitchState> GetAutoSaveSettingsAsync(CancellationToken cancellationToken = default)
        {
            return (SwitchState)(int)(await GetPropertiesAsync(cancellationToken, "save_state").ConfigureAwait(false))[0];
        }

        public Task<JsonValue> SetAutoSaveSettingsAsync(SwitchState state = SwitchState.On, CancellationToken cancellationToken = default)
        {
            //    cfg_save_state???
            return InvokeAsync("set_ps", new JsonArray { "cfg_save_state", ((int)state).ToString() }, cancellationToken);
        }

        public async Task<JsonValue> GetPdoStatusAsync(CancellationToken cancellationToken = default)
        {
            return (await GetPropertiesAsync(cancellationToken, "pdo_status").ConfigureAwait(false))[0];
        }

        public Task<JsonValue> SetDefaultsAsync(CancellationToken cancellationToken = default)
        {
            return InvokeAsync("set_default", EmptyArgs, cancellationToken);
        }

        #endregion

        #region Lamp display name management

        public Task<JsonValue> SetNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return InvokeAsync("set_name", new JsonArray { name }, cancellationToken);
        }

        public async Task<string> GetNameAsync(CancellationToken cancellationToken = default)
        {
            return (await GetPropertiesAsync(cancellationToken, "name").ConfigureAwait(false))[0];
        }

        #endregion

        #region LAN control mode

        public async Task<SwitchState> GetLANControlModeAsync(CancellationToken cancellationToken = default)
        {
            return (SwitchState)(int)(await GetPropertiesAsync(cancellationToken, "lan_ctrl").ConfigureAwait(false))[0];
        }

        public Task<JsonValue> SetLANControlModeAsync(SwitchState state = SwitchState.On, CancellationToken cancellationToken = default)
        {
            //    cfg_lan_ctrl???
            return InvokeAsync("set_ps", new JsonArray { "cfg_lan_ctrl", ((int)state).ToString() }, cancellationToken);
        }

        #endregion

        #region Time scheduller options

        public async Task<JsonArray> CronGetAsync(uint type, CancellationToken cancellationToken = default)
        {
            return (JsonArray)await InvokeAsync("cron_get", new JsonArray { type }, cancellationToken).ConfigureAwait(false);
        }

        public Task<JsonValue> CronAddAsync(uint type, uint delay, CancellationToken cancellationToken = default)
        {
            return InvokeAsync("cron_add", new JsonArray { type, delay }, cancellationToken);
        }

        public Task<JsonValue> CronDelAsync(uint type, CancellationToken cancellationToken = default)
        {
            return InvokeAsync("cron_del", new JsonArray { type }, cancellationToken);
        }

        public async Task<uint> GetDelayOffAsync(CancellationToken cancellationToken = default)
        {
            return (await GetPropertiesAsync(cancellationToken, "delayoff").ConfigureAwait(false))[0];
        }

        #endregion
    }
}