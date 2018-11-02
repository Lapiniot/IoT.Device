using IoT.Device.Lumi;
using IoT.Device.Lumi.SubDevices;

[assembly: ExportSubDevice("switch", typeof(ButtonSwitchV2))]
[assembly: ExportSubDevice("motion", typeof(MotionSensorV2))]
[assembly: ExportSubDevice("magnet", typeof(DoorWindowSensor))]
[assembly: ExportSubDevice("plug", typeof(SmartPowerPlug))]
[assembly: ExportSubDevice("smoke", typeof(HonneywellFireSmokeSensor))]
[assembly: ExportSubDevice("weather.v1", typeof(AqaraWeatherSensor))]
[assembly: ExportSubDevice("sensor_wleak.aq1", typeof(AqaraWaterLeakSensor))]
[assembly: ExportSubDevice("sensor_cube.aqgl01", typeof(AqaraCubeController))]
[assembly: ExportSubDevice("sensor_magnet.aq2", typeof(AqaraDoorWindowSensor))]
[assembly: ExportSubDevice("sensor_motion.aq2", typeof(AqaraMotionSensor))]
[assembly: ExportSubDevice("vibration", typeof(AqaraVibrationSensor))]