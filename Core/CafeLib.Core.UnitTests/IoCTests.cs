using CafeLib.Core.IoC;
using CafeLib.Core.UnitTests.Services;
using Microsoft.Extensions.Logging;
using Xunit;

namespace CafeLib.Core.UnitTests
{
    public class IoCTests
    {
        [Fact]
        public void IocTest()
        {
            IDisposableService disposableService;
            using (var resolver = IocFactory.CreateRegistry()
                .AddLogging(builder => builder.AddConsole().AddDebug())
                .AddSingleton<IFooService, FooService>()
                .AddSingleton<IBarService, BarService>()
                .AddSingleton<IDisposableService, DisposableService>()
                .GetResolver())
            {
                //do the actual work here
                var bar = resolver.Resolve<IBarService>();
                bar.DoSomeRealWork();
                disposableService = resolver.Resolve<IDisposableService>();
            }

            Assert.True(disposableService.IsDisposed);
        }

        [Fact]
        public void ServiceProviderTest()
        {
            var resolver = IocFactory.CreateRegistry()
                .AddPropertyService()
                .AddSingleton<ITestService>(x => new TestService())
                .GetResolver();

            var propertyService = resolver.Resolve<IPropertyService>();
            Assert.NotNull(propertyService);

            propertyService.SetProperty("name", "Kilroy");
            Assert.Equal("Kilroy", propertyService.GetProperty<string>("name"));

            Assert.True(propertyService.HasProperty("name"));

            Assert.True(propertyService.RemoveProperty("name"));

            Assert.False(propertyService.HasProperty("name"));

            var testService = resolver.Resolve<ITestService>();
            Assert.Equal("Kilroy is here!", testService.Test());
        }
    }
}
