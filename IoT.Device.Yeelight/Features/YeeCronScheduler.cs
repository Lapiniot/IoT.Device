﻿using System;

namespace IoT.Device.Yeelight.Features
{
    public class YeeCronScheduler : YeelightDeviceCapability
    {
        public YeeCronScheduler(YeelightDevice device) : base(device) { }

        public override string[] SupportedMethods => throw new NotImplementedException();

        public override string[] SupportedProperties => throw new NotImplementedException();
    }
}