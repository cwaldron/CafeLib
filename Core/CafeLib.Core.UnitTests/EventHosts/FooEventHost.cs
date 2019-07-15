using CafeLib.Core.IoC;
using System;
using System.Collections.Generic;
using System.Text;

namespace CafeLib.Core.UnitTests.EventHosts
{
    public class FooEventHost
    {
        public FooEventHost(IServiceResolver resolver)
        {
            var eventService = resolver.Resolve<IEventService>();
            eventService.Subscribe<EventServiceTest.CommonEventMessage>(x => x.Visited());
        }
    }
}
