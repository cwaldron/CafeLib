using System;
// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Support
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class TransientAttribute : Attribute
    {
        public bool IsTransient { get; }

        /// <summary>
        /// Page resolver constructor
        /// </summary>
        public TransientAttribute(bool transient = true)
        {
            IsTransient = transient;
        }
    }
}
