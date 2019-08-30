using CafeLib.Core.IoC;
using CafeLib.Mobile.Services;
using CafeLib.Mobile.Test.Core.Support;
using Microsoft.Extensions.Logging;
using Moq;
using Xamarin.Forms;
// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Test.Core
{
    public abstract class MobileUnitTest
    {
        protected Mock<INavigationService> NavigationService;
        protected Mock<IPageService> PageService;
        protected Mock<IDeviceService> DeviceService;

        protected IServiceRegistry Registry { get; private set; }

        protected IServiceResolver Resolver => Registry.GetResolver();

        protected MockApplication App { get; private set; }

        public void Initialize()
        {
            SetupTest();
            InitTest();
        }

        public void CreateApplication()
        {
            Device.PlatformServices = new MockPlatformServices();
            App = new MockApplication(Registry);
        }

        protected virtual void InitTest()
        {
        }

        protected virtual void SetupTest()
        {
            // fake set up of the IoC
            SetupRegistry();

            // Setup for all tests.
            NavigationService = new Mock<INavigationService>();
            PageService = new Mock<IPageService>();

            Registry
                .AddSingleton<IDeviceService, MockDeviceService>()
                .AddSingleton(x => NavigationService.Object)
                .AddSingleton(x => PageService.Object);
        }

        private void SetupRegistry()
        {
            Registry = IocFactory.CreateRegistry();
            Registry
                .AddLogging(builder => builder.AddConsole().AddDebug())
                .AddEventService();
        }
    }
}