using System;

// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AnimateNavigationAttribute : Attribute
    {
        /// <summary>
        /// Navigation animation attribute constructor.
        /// </summary>
        /// <param name="animate"></param>
        public AnimateNavigationAttribute(bool animate = true)
        {
            Push = animate;
            Pop = animate;
        }

        /// <summary>
        /// Animate during push navigation.
        /// </summary>
        public bool Push { get; set; }

        /// <summary>
        /// Animate during pop animation.
        /// </summary>
        public bool Pop { get; set; } 
    }
}