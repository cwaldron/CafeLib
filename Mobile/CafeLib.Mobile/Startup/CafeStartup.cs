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
        private readonly ICafeStartup _cafeApplication;
        private readonly CafeRegistry _cafeRegistry;
        private readonly T _application;

        public CafeStartup(ICafeStartup cafeApplication)
        {
            _cafeApplication = cafeApplication ?? throw new ArgumentNullException(nameof(cafeApplication));
            _cafeRegistry = new CafeRegistry();
            _application = typeof(T).CreateInstance<T>(_cafeRegistry);
        }

        public T Configure()
        {
            _cafeApplication.Configure(_cafeRegistry);
            _application.Configure();
            return _application;
        }
    }
}
