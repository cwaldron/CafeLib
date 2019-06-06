using System;
using CafeLib.Core.Extensions;
// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Startup
{
    /// <summary>
    /// Base class of CafeLib mobile application.
    /// </summary>
    public class CafeStartup<T> where T : CafeApplication
    {
        private readonly IPlatformApplication _platformApplication;
        private readonly CafeRegistry _cafeRegistry;
        private readonly T _application;

        public CafeStartup(IPlatformApplication platformApplication)
        {
            _platformApplication = platformApplication ?? throw new ArgumentNullException(nameof(platformApplication));
            _cafeRegistry = new CafeRegistry();
            _application = typeof(T).CreateInstance<T>(_cafeRegistry);
        }

        public T Configure()
        {
            _platformApplication.Configure(_cafeRegistry);
            _application.Configure();
            return _application;
        }
    }
}
