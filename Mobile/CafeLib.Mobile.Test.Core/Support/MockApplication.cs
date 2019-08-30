using CafeLib.Core.IoC;
using CafeLib.Mobile.Startup;

namespace CafeLib.Mobile.Test.Core.Support
{
    public class MockApplication : CafeApplication
    {
        public MockApplication(IServiceRegistry registry)
            : base(registry)
        {
        }
    }
}