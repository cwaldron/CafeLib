using CafeLib.Core.Eventing;
using CafeLib.Mobile.ViewModels;

namespace CafeLib.Mobile.Messages
{
    public class ViewModelCloseMessage : EventMessage
    {
        public ViewModelCloseMessage(BaseViewModel sender)
            : base(sender)
        {
        }
    }
}