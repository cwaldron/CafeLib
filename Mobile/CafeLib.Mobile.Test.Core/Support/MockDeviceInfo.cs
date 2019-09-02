using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace CafeLib.Mobile.Test.Core.Support
{
    public class MockDeviceInfo : DeviceInfo
    {
        public MockDeviceInfo()
        {
            CurrentOrientation = DeviceOrientation.Portrait;
            PixelScreenSize = new Size(360.0, 760.0);
            ScalingFactor = 1.0;
        }

        public override Size PixelScreenSize { get; }
        public override Size ScaledScreenSize => new Size(PixelScreenSize.Width / ScalingFactor, PixelScreenSize.Height / ScalingFactor);
        public override double ScalingFactor { get; }
    }
}
