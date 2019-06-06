using CafeLib.Core.IoC;

namespace CafeLib.Mobile.Startup
{
    /// <summary>
    /// Base class of the native application.
    /// </summary>
    public interface IPlatformApplication
    {
        void Configure(IServiceRegistry registry);
    }
}
