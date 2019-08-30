using System;
using System.Threading;
using System.Threading.Tasks;
using CafeLib.Mobile.Services;

// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Test.Core.Support
{
    internal class MockDeviceService : IDeviceService
    {
        public bool IsOnMainThread()
        {
            return true;
        }

        public void RunOnMainThread(Action action)
        {
            action();
        }

        public void RunOnWorkerThread(Action action, CancellationToken cancellationToken = default)
        {
            Task.Run(action, cancellationToken);
        }
    }
}