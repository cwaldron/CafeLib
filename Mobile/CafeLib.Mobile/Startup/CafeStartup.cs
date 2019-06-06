using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CafeLib.Core.Extensions;
using CafeLib.Core.IoC;
using CafeLib.Mobile.Services;
using Xamarin.Forms;
// ReSharper disable PublicConstructorInAbstractClass
// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Startup
{
    /// <summary>
    /// Base class of CafeLib mobile application.
    /// </summary>
    public class CafeStartup<T> where T : CafeApplication
    {
        private readonly IPlatformApplication _platformApplication;
        private readonly MobileServices _mobileService;
        private readonly T _application;

        public CafeStartup(IPlatformApplication platformApplication)
        {
            _platformApplication = platformApplication ?? throw new ArgumentNullException(nameof(platformApplication));
            _mobileService = new MobileServices();
            _application = typeof(T).CreateInstance<T>(_mobileService);
        }

        public T Configure()
        {
            _platformApplication.Configure(_mobileService);
            _application.Configure();
            return _application;
        }
    }
}
