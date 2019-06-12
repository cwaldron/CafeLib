using System;
using CafeLib.Core.IoC;
using CafeLib.Mobile.Services;
using Microsoft.Extensions.Logging;
// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Startup
{
    internal sealed class CafeRegistry : IServiceRegistry, IServiceResolver
    {
        private readonly IServiceRegistry _serviceRegistry;

        /// <summary>
        /// ServiceProvider instance constructor.
        /// </summary>
        internal CafeRegistry()
        {
            var mobileService = new Lazy<MobileService>(() => new MobileService(GetResolver()));
            _serviceRegistry = IocFactory.CreateRegistry()
                .AddSingleton(x => mobileService.Value as IServiceResolver)
                .AddSingleton(x => mobileService.Value as IPageService)
                .AddSingleton(x => mobileService.Value as INavigationService)
                .AddSingleton(x => mobileService.Value as IDeviceService);
        }

        internal IPageService PageService => GetResolver().Resolve<IPageService>();

        internal INavigationService NavigationService => GetResolver().Resolve<INavigationService>();

        internal IDeviceService DeviceService => GetResolver().Resolve<IDeviceService>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration">configuration action</param>
        /// <returns></returns>
        public IServiceRegistry AddLogging(Action<ILoggingBuilder> configuration)
        {
            return _serviceRegistry.AddLogging(configuration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <returns></returns>
        public IServiceRegistry AddScoped<TService, TImpl>() where TService : class where TImpl : class, TService
        {
            return _serviceRegistry.AddScoped<TService, TImpl>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="factory"></param>
        /// <returns></returns>
        public IServiceRegistry AddScoped<TService>(Func<IServiceProvider, TService> factory) where TService : class
        {
            return _serviceRegistry.AddScoped(factory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <returns></returns>
        public IServiceRegistry AddSingleton<TService, TImpl>() where TService : class where TImpl : class, TService
        {
            return _serviceRegistry.AddSingleton<TService, TImpl>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="factory"></param>
        /// <returns></returns>
        public IServiceRegistry AddSingleton<TService>(Func<IServiceProvider, TService> factory) where TService : class
        {
            return _serviceRegistry.AddSingleton(factory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <returns></returns>
        public IServiceRegistry AddTransient<TService, TImpl>() where TService : class where TImpl : class, TService
        {
            return _serviceRegistry.AddTransient<TService, TImpl>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="factory"></param>
        /// <returns></returns>
        public IServiceRegistry AddTransient<TService>(Func<IServiceProvider, TService> factory) where TService : class
        {
            return _serviceRegistry.AddTransient(factory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IServiceResolver GetResolver()
        {
            return _serviceRegistry.GetResolver();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _serviceRegistry.Dispose();
        }

        /// <summary>
        /// Resolve the dependency.
        /// </summary>
        /// <typeparam name="T">dependency type</typeparam>
        /// <returns>instance of dependency type</returns>
        public T Resolve<T>() where T : class
        {
            return GetResolver().Resolve<T>();
        }
    }
}
