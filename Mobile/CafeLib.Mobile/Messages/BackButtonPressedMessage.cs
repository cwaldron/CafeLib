using CafeLib.Core.Eventing;
using CafeLib.Mobile.ViewModels;

namespace CafeLib.Mobile.Messages
{
    public class BackButtonPressedMessage : EventMessage
    {
        public NavigationSource NavigationSource => (NavigationSource) Sender;

        public BackButtonPressedMessage(NavigationSource navigationSource)
            : base(navigationSource)
        {
        }
    }
}