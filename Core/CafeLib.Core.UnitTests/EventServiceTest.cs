using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CafeLib.Core.Eventing;
using CafeLib.Core.IoC;
using CafeLib.Core.UnitTests.EventHosts;
using Xunit;

namespace CafeLib.Core.UnitTests
{
    public class EventServiceTest
    {
        protected IServiceResolver Resolver;

        private int _commonEventMessageVisits;
        private readonly FooHost _fooHost;
        private readonly BarHost _barHost;

        public EventServiceTest()
        {
            Resolver = IocFactory.CreateRegistry()
                .AddEventService()
                .GetResolver();

            _fooHost = new FooHost(Resolver);
            _barHost = new BarHost(Resolver);
        }

        [Fact]
        public void CommonEventMessageTest()
        {
            var eventService = Resolver.Resolve<IEventService>();
            eventService.Publish(new CommonEventMessage(this));
            Assert.Equal(2, _commonEventMessageVisits);
        }

        private void AddCommonVisits()
        {
            _commonEventMessageVisits += 1;
        }

        public class CommonEventMessage : EventMessage
        {
            private readonly EventServiceTest _test;

            public CommonEventMessage(EventServiceTest test)
            {
                _test = test;
            }

            public void Visited()
            {
                _test.AddCommonVisits();
            }
        }
    }
}
