using System;
using CafeLib.Core.IoC;
using Xunit;

namespace CafeLib.Core.UnitTests
{
    public class PropertyServiceTest
    {
        private readonly Guid _propertyGuid = Guid.NewGuid();
        protected IServiceResolver Resolver;

        public class TestObject
        {
            public int Value { get; set; }
        }

        public PropertyServiceTest()
        {
            Resolver = IocFactory.CreateRegistry()
                .AddPropertyService()
                .GetResolver();
        }

        [Fact]
        public void PropertyServiceSetPropertyGuidTest()
        {
            var propertyService = Resolver.Resolve<IPropertyService>();
            propertyService.SetProperty(_propertyGuid, 20);
            Assert.Equal(20, propertyService.GetProperty<int>(_propertyGuid));
        }

        [Fact]
        public void PropertyServiceSetPropertyStringTest()
        {
            var propertyService = Resolver.Resolve<IPropertyService>();
            propertyService.SetProperty("key", 20);
            Assert.Equal(20, propertyService.GetProperty<int>("key"));
        }

        [Fact]
        public void PropertyServiceSetPropertyTypeTest()
        {
            var propertyService = Resolver.Resolve<IPropertyService>();
            propertyService.SetProperty(new TestObject { Value = 20 });
            var testObject = propertyService.GetProperty<TestObject>();
            Assert.Equal(20, testObject.Value);
        }
    }
}