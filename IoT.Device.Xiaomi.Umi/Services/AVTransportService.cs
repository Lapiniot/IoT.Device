using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IoT.Protocol.Soap;
using IoT.Protocol.Upnp;

namespace IoT.Device.Xiaomi.Umi.Services
{
    public sealed class AVTransportService : SoapActionInvoker
    {
        internal AVTransportService(UmiSpeakerDevice parent) : base(parent.Endpoint,
            $"{parent.DeviceId}-MR/upnp.org-AVTransport-1/control", UpnpServices.AVTransport)
        {
        }

        public Task<IDictionary<string, string>> GetMediaInfoAsync(uint instanceID = 0, CancellationToken cancellationToken = default)
        {
            //UInt32 NrTracks, 
            //String MediaDuration, 
            //String CurrentURI, 
            //String CurrentURIMetaData, 
            //String NextURI, 
            //String NextURIMetaData, 
            //String PlayMedium, 
            //String RecordMedium, 
            //String WriteStatus
            return InvokeAsync("GetMediaInfo", cancellationToken, ("InstanceID", instanceID));
        }

        //void SetRecordQualityMode(UInt32 instanceID, String newRecordQualityMode);

        public Task<IDictionary<string, string>> GetPositionInfoAsync(uint instanceID = 0, CancellationToken cancellationToken = default)
        {
            //UInt32 Track, 
            //String TrackDuration, 
            //String TrackMetaData, 
            //String TrackURI, 
            //String RelTime, 
            //String AbsTime, 
            //Int32 RelCount, 
            //Int32 AbsCount
            return InvokeAsync("GetPositionInfo", cancellationToken, ("InstanceID", instanceID));
        }

        public Task<IDictionary<string, string>> GetTransportInfoAsync(uint instanceID = 0, CancellationToken cancellationToken = default)
        {
            //String CurrentTransportState, 
            //String CurrentTransportStatus, 
            //String CurrentSpeed
            return InvokeAsync("GetTransportInfo", cancellationToken, ("InstanceID", instanceID));
        }

        public Task SetAVTransportUriAsync(uint instanceID = 0, string currentURI = null, string currentURIMetaData = null, CancellationToken cancellationToken = default)
        {
            return InvokeAsync("SetAVTransportURI", cancellationToken,
                ("InstanceID", instanceID),
                ("CurrentURI", currentURI),
                ("CurrentURIMetaData", currentURIMetaData));
        }

        public Task SetNextAVTransportUriAsync(uint instanceID = 0, string nextURI = null, string nextURIMetaData = null, CancellationToken cancellationToken = default)
        {
            return InvokeAsync("SetNextAVTransportURI", cancellationToken,
                ("InstanceID", instanceID),
                ("NextURI", nextURI),
                ("NextURIMetaData", nextURIMetaData));
        }

        public Task StopAsync(uint instanceID = 0, CancellationToken cancellationToken = default)
        {
            return InvokeAsync("Stop", cancellationToken, ("InstanceID", instanceID));
        }

        public Task PlayAsync(uint instanceID = 0, string speed = "1", CancellationToken cancellationToken = default)
        {
            return InvokeAsync("Play", cancellationToken, ("InstanceID", instanceID), ("Speed", speed));
        }

        public Task PauseAsync(uint instanceID = 0, CancellationToken cancellationToken = default)
        {
            return InvokeAsync("Pause", cancellationToken, ("InstanceID", instanceID));
        }

        public Task NextAsync(uint instanceID = 0, CancellationToken cancellationToken = default)
        {
            return InvokeAsync("Next", cancellationToken, ("InstanceID", instanceID));
        }

        public Task PreviousAsync(uint instanceID = 0, CancellationToken cancellationToken = default)
        {
            return InvokeAsync("Previous", cancellationToken, ("InstanceID", instanceID));
        }

        public Task SeekAsync(uint instanceID = 0, string seekMode = null, string target = null, CancellationToken cancellationToken = default)
        {
            // seekMode:
            //TRACK_NR
            //ABS_TIME
            //REL_TIME
            //ABS_COUNT
            //REL_COUNT
            //CHANNEL_FREQ
            //TAPE-INDEX
            //FRAME
            return InvokeAsync("Seek", cancellationToken, ("InstanceID", instanceID), ("Unit", seekMode), ("Target", target));
        }

        public Task SetPlayModeAsync(uint instanceID = 0, string newPlayMode = "NORMAL", CancellationToken cancellationToken = default)
        {
            //newPlayMode:
            //NORMAL
            //SHUFFLE
            //REPEAT_SHUFFLE
            //REPEAT_TRACK
            //REPEAT_ONE
            //REPEAT_ALL
            //RANDOM
            //DIRECT_1
            //INTRO
            return InvokeAsync("SetPlayMode", cancellationToken, ("InstanceID", instanceID), ("NewPlayMode", newPlayMode));
        }

        public Task<IDictionary<string, string>> GetCurrentTransportActionsAsync(uint instanceID = 0, CancellationToken cancellationToken = default)
        {
            return InvokeAsync("GetCurrentTransportActions", cancellationToken, ("InstanceID", instanceID));
        }

        public Task<IDictionary<string, string>> GetTransportSettingsAsync(uint instanceID = 0, CancellationToken cancellationToken = default)
        {
            return InvokeAsync("GetTransportSettings", cancellationToken, ("InstanceID", instanceID));
        }

        public Task<IDictionary<string, string>> GetDeviceCapabilitiesAsync(uint instanceID = 0, CancellationToken cancellationToken = default)
        {
            return InvokeAsync("GetDeviceCapabilities", cancellationToken, ("InstanceID", instanceID));
        }

        //void Record(UInt32 instanceID);
    }
}