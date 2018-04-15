using IoT.Device.Lumi.Gateway;
using IoT.Device.Lumi.Gateway.SubDevices;

[assembly: SupportedSubDevice("switch", typeof(SwitchButtonSensor))]
[assembly: SupportedSubDevice("motion", typeof(MotionSensor))]
[assembly: SupportedSubDevice("magnet", typeof(WindowDoorSensor))]
[assembly: SupportedSubDevice("plug", typeof(SmartPlug))]
[assembly: SupportedSubDevice("smoke", typeof(HonneywellSmokeSensor))]
[assembly: SupportedSubDevice("weather.v1", typeof(AqaraWeatherSensor))]
[assembly: SupportedSubDevice("sensor_wleak.aq1", typeof(AqaraWaterLeakSensor))]