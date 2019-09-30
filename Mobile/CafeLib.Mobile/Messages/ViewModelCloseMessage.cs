using CafeLib.Core.Eventing;

namespace CafeLib.Mobile.Messages
{
    public class ViewModelCloseMessage : EventMessage
    {
        public ViewModelCloseMessage(object sender)
            : base(sender)
        {
        }
    }
}