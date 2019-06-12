﻿using System;
using CafeLib.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedMember.Global

namespace CafeLib.Core.IoC
{
    public class ServiceRegistry : IServiceRegistry, IServiceResolver
    {
        #region Private Variables

        private readonly ServiceCollection _serviceCollection;
        private IServiceProvider _serviceProvider;
        private bool _disposed;

        #endregion

        #region Constructors

        internal ServiceRegistry()
        {
            _serviceCollection = new ServiceCollection();
        }

        #endregion

        #region Properties

        public IServiceProvider ServiceProvider => _serviceProvider ?? (_serviceProvider = _serviceCollection.BuildServiceProvider());

        #endregion

        #region Methods

        /// <summary>
        /// Register logging service.
        /// </summary>
        /// <param name="configuration">configuration action</param>
        /// <returns>service registry</returns>
        public IServiceRegistry AddLogging(Action<ILoggingBuilder> configuration)
        {
            if (_serviceProvider != null) throw new InvalidOperationException(nameof(_serviceProvider));
            _serviceCollection.AddLogging(configuration);
            return this;
        }

        /// <summary>
        /// Register service of scoped lifetime.
        /// </summary>
        /// <typeparam name="TService">service type</typeparam>
        /// <returns>service registry</returns>
        public IServiceRegistry AddScoped<TService>() where TService : class
        {
            if (_serviceProvider != null) throw new InvalidOperationException(nameof(_serviceProvider));
            _serviceCollection.TryAddScoped<TService>();
            return this;
        }

        /// <summary>
        /// Register service of scoped lifetime.
        /// </summary>
        /// <typeparam name="TService">service type</typeparam>
        /// <typeparam name="TImpl">service implementation type</typeparam>
        /// <returns>service registry</returns>
        public IServiceRegistry AddScoped<TService, TImpl>() where TService : class where TImpl : class, TService
        {
            if (_serviceProvider != null) throw new InvalidOperationException(nameof(_serviceProvider));
            _serviceCollection.TryAddScoped<TService, TImpl>();
            return this;
        }

        /// <summary>
        /// Register service of scoped lifetime.
        /// </summary>
        /// <typeparam name="TService">service type</typeparam>
        /// <param name="factory">service factory</param>
        /// <returns>service registry</returns>
        public IServiceRegistry AddScoped<TService>(Func<IServiceProvider, TService> factory) where TService : class
        {
            if (_serviceProvider != null) throw new InvalidOperationException(nameof(_serviceProvider));
            _serviceCollection.TryAddScoped(factory.Invoke);
            return this;
        }

        /// <summary>
        /// Register service as a singleton.
        /// </summary>
        /// <typeparam name="TService">service type</typeparam>
        /// <returns>service registry</returns>
        public IServiceRegistry AddSingleton<TService>() where TService : class
        {
            if (_serviceProvider != null) throw new InvalidOperationException(nameof(_serviceProvider));
            _serviceCollection.TryAddSingleton<TService>();
            return this;
        }

        /// <summary>
        /// Register service as a singleton.
        /// </summary>
        /// <typeparam name="TService">service type</typeparam>
        /// <typeparam name="TImpl">service implementation type</typeparam>
        /// <returns>service registry</returns>
        public IServiceRegistry AddSingleton<TService, TImpl>() where TService : class where TImpl : class, TService
        {
            if (_serviceProvider != null) throw new InvalidOperationException(nameof(_serviceProvider));
            _serviceCollection.TryAddSingleton<TService, TImpl>();
            return this;
        }

        /// <summary>
        /// Register service as a singleton.
        /// </summary>
        /// <typeparam name="TService">service type</typeparam>
        /// <param name="factory">service factory</param>
        /// <returns>service registry</returns>
        public IServiceRegistry AddSingleton<TService>(Func<IServiceProvider, TService> factory) where TService : class
        {
            if (_serviceProvider != null) throw new InvalidOperationException(nameof(_serviceProvider));
            _serviceCollection.TryAddSingleton(factory.Invoke);
            return this;
        }

        /// <summary>
        /// Register service as transient.
        /// </summary>
        /// <typeparam name="TService">service type</typeparam>
        /// <returns>service registry</returns>
        public IServiceRegistry AddTransient<TService>() where TService : class
        {
            if (_serviceProvider != null) throw new InvalidOperationException(nameof(_serviceProvider));
            _serviceCollection.TryAddTransient<TService>();
            return this;
        }

        /// <summary>
        /// Register service as transient.
        /// </summary>
        /// <typeparam name="TService">service type</typeparam>
        /// <typeparam name="TImpl">service implementation type</typeparam>
        /// <returns></returns>
        public IServiceRegistry AddTransient<TService, TImpl>() where TService : class where TImpl : class, TService
        {
            if (_serviceProvider != null) throw new InvalidOperationException(nameof(_serviceProvider));
            _serviceCollection.TryAddTransient<TService, TImpl>();
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService">service type</typeparam>
        /// <param name="factory"></param>
        /// <returns></returns>
        public IServiceRegistry AddTransient<TService>(Func<IServiceProvider, TService> factory) where TService : class
        {
            if (_serviceProvider != null) throw new InvalidOperationException(nameof(_serviceProvider));
            _serviceCollection.TryAddTransient(factory.Invoke);
            return this;
        }

        public IServiceResolver GetResolver()
        {
            return this;
        }

        /// <summary>
        /// Resolve the service.
        /// </summary>
        /// <typeparam name="T">service type</typeparam>
        /// <returns>service instance</returns>
        public T Resolve<T>() where T : class
        {
            return ServiceProvider.GetService<T>();
        }

        /// <summary>
        /// Dispose service registry.
        /// </summary>
        public void Dispose()
        {
            Dispose(!_disposed);
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Helpers

        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            _serviceProvider?.GetServices<IServiceProvider>().ForEach(x => (x as IDisposable)?.Dispose());
        }

        #endregion
    }
}
