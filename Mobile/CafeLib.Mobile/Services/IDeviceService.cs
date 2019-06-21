using System;

namespace CafeLib.Mobile.Services
{
    public interface IDeviceService
    {
        /// <summary>
        /// Check whether the current managed thread id is the main thread.
        /// </summary>
        /// <returns>true if running on the main thread; otherwise false.</returns>
        bool IsOnMainThread();

        /// <summary>
        /// Runs an action on the main thread.
        /// </summary>
        /// <param name="action">action</param>
        void RunOnMainThread(Action action);
    }
}
