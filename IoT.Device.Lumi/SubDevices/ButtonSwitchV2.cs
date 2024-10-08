﻿using IoT.Device.Metadata;

using static IoT.Device.Metadata.PowerSource;
using static IoT.Device.Metadata.ConnectivityTypes;

namespace IoT.Device.Lumi.SubDevices;

[ExportSubDevice("switch")]
[ModelID("WXKG01LM")]
[PowerSource(CR2032)]
[ConnectivityType(ZigBee)]
public sealed partial class ButtonSwitchV2 : LumiSubDevice
{
    internal ButtonSwitchV2(string sid, int id) : base(sid, id) { }
}