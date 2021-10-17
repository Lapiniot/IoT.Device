using IoT.Device.Lumi;
using IoT.Device.Lumi.SubDevices;

[assembly: ExportSubDevice<ButtonSwitchV2>("switch")]
[assembly: ExportSubDevice<MotionSensorV2>("motion")]
[assembly: ExportSubDevice<DoorWindowSensor>("magnet")]
[assembly: ExportSubDevice<SmartPowerPlug>("plug")]
[assembly: ExportSubDevice<HonneywellFireSmokeSensor>("smoke")]
[assembly: ExportSubDevice<AqaraWeatherSensor>("weather.v1")]
[assembly: ExportSubDevice<AqaraWaterLeakSensor>("sensor_wleak.aq1")]
[assembly: ExportSubDevice<AqaraCubeController>("sensor_cube.aqgl01")]
[assembly: ExportSubDevice<AqaraDoorWindowSensor>("sensor_magnet.aq2")]
[assembly: ExportSubDevice<AqaraMotionSensor>("sensor_motion.aq2")]
[assembly: ExportSubDevice<AqaraVibrationSensor>("vibration")]