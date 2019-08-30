using Xamarin.Forms.Internals;

namespace CafeLib.Mobile.Test.Core.Support
{
    internal class MockTicker : Ticker
    {
        private bool _enabled;

        protected override void EnableTimer()
        {
            _enabled = true;

            while (_enabled)
            {
                SendSignals(16);
            }
        }

        protected override void DisableTimer()
        {
            _enabled = false;
        }
    }
}