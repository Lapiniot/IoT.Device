using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeAdjustProperty : YeelightDeviceFeature
    {
        public static readonly Type Type = typeof(YeeAdjustProperty);

        private readonly string method;

        protected YeeAdjustProperty(YeelightDevice device, string adjustMethodName) : base(device)
        {
            method = adjustMethodName;
        }

        public YeeAdjustProperty(YeelightDevice device) : this(device, "set_adjust") { }

        public override IEnumerable<string> SupportedMethods => new[] {method};

        public override IEnumerable<string> SupportedProperties => Array.Empty<string>();

        /// <summary>
        /// This method is used to change brightness, CT or color of a smart LED without knowing the current value,
        /// it's mainly used by controllers.
        /// </summary>
        /// <param name="action">The direction of the adjustment</param>
        /// <param name="propName">
        /// The property to adjust.
        /// <remarks>
        /// The valid value can be:
        /// “bright":   adjust brightness,
        /// “ct":       adjust color temperature,
        /// “color":    adjust color.
        /// When <paramref name="propName" /> is “color", the <paramref name="action" /> can only
        /// be <value>AdjustDirection.Circle</value>, otherwise, it will be deemed as invalid request.)
        /// </remarks>
        /// </param>
        /// <param name="cancellationToken">Token for external cancellation.</param>
        /// <returns>Operation result ("ok" or error description)</returns>
        public Task SetAdjustAsync(AdjustDirection action, string propName, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync(method, new object[] {action.ToString().ToLowerInvariant(), propName}, cancellationToken);
        }
    }
}