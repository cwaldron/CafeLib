using System;
using Microsoft.Extensions.Logging;
using Xunit;

namespace CafeLib.Core.UnitTests.Services
{
    public interface IDisposableService : IDisposable
    {
        void DoThing(int number);

        bool IsDisposed { get; }
    }

    public class DisposableService : IDisposableService
    {
        private readonly ILogger<FooService> _logger;
        private int _disposed;

        public DisposableService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FooService>();
        }

        public bool IsDisposed => _disposed > 0;

        public void DoThing(int number)
        {
            _logger.LogInformation($"Doing the thing {number}");
        }

        public void Dispose()
        {
            Dispose(!IsDisposed);
            _disposed++;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose service provider
        /// </summary>
        /// <param name="disposing"></param>
        protected void Dispose(bool disposing)
        {
            if (!disposing) return;
            Assert.True(!IsDisposed);
        }
    }
}