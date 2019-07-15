using CafeLib.Core.IoC;
using System;
using System.Collections.Generic;
using System.Text;

namespace CafeLib.Core.UnitTests.EventHosts
{
    public class FooHost
    {
        public FooHost(IServiceResolver resolver)
        {
            var eventService = resolver.Resolve<IEventService>();
            eventService.Subscribe<EventServiceTest.CommonEventMessage>(x => x.Visited());
        }
    }
}
