using IoT.Device.Lumi;
using IoT.Device.Lumi.SubDevices;

[assembly: ExportSubDevice("switch", typeof(SwitchButtonSensor))]
[assembly: ExportSubDevice("motion", typeof(MotionSensor))]
[assembly: ExportSubDevice("magnet", typeof(WindowDoorSensor))]
[assembly: ExportSubDevice("plug", typeof(SmartPlug))]
[assembly: ExportSubDevice("smoke", typeof(HonneywellSmokeSensor))]
[assembly: ExportSubDevice("weather.v1", typeof(AqaraWeatherSensor))]
[assembly: ExportSubDevice("sensor_wleak.aq1", typeof(AqaraWaterLeakSensor))]