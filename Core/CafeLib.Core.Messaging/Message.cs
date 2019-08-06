using System;
// ReSharper disable UnusedMember.Global

namespace CafeLib.Core.Messaging
{
    public abstract class Message : IMessage
    {
        public DateTime TimeStamp { get; }

        protected Message()
        {
            TimeStamp = DateTime.UtcNow;
        }
    }
}
