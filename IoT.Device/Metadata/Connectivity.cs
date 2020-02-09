using System;

namespace IoT.Device.Metadata
{
    [Flags]
    public enum ConnectivityTypes
    {
        Ethernet = 1,
        WiFi24 = 2,
        WiFi5 = 4,
        ZigBee = 8,
        ZWave = 16,
        Bluetooth = 32,
        RF433 = 64,
        WiFi = WiFi24 | WiFi5
    }
}