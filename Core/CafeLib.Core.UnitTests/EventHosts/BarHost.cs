using CafeLib.Core.IoC;

namespace CafeLib.Core.UnitTests.EventHosts
{
    public class BarHost
    {
        public BarHost(IServiceResolver resolver)
        {
            var eventService = resolver.Resolve<IEventService>();
            eventService.Subscribe<EventServiceTest.CommonEventMessage>(x => x.Visited());
        }
    }
}
