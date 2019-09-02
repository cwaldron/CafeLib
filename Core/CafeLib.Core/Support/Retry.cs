using System;
using System.Collections.Generic;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace CafeLib.Core.Support
{
    public class Retry
    {
        #region Constants

        private const int DefaultLimit = 3;
        private const int DefaultInterval = 50; // milliseconds

        #endregion

        #region Automatic Properties.

        /// <summary>
        /// Retry attempts limit.
        /// </summary>
        public int Limit { get; }

        /// <summary>
        /// Retry interval between attempts.
        /// </summary>
        public int Interval { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Retry constructor.
        /// </summary>
        /// <param name="retryLimit"></param>
        /// <param name="retryInterval"></param>
        public Retry(int retryLimit = DefaultLimit, int retryInterval = DefaultInterval)
        {
            Limit = retryLimit > 0 ? retryLimit : 1;
            Interval = retryInterval > 0 ? retryInterval : DefaultInterval;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Do retries on action if necessary.
        /// </summary>
        /// <param name="action">action</param>
        /// <returns>awaitable task</returns>
        public async Task Do(Action action)
        {
            await Do(async () =>
            {
                action();
                return await Task.FromResult(0);
            });
        }

        /// <summary>
        /// Do retries on function if necessary.
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="function">retry function</param>
        /// <returns>the action return result</returns>
        public async Task<T> Do<T>(Func<Task<T>> function)
        {
            var exceptions = new List<Exception>();

            for (var retry = 0; retry < Limit; retry++)
            {
                try
                {
                    return await function();
                }
                catch (TaskCanceledException)
                {
                    return await Task.FromResult(default(T));
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    await Task.Delay(Interval);
                }
            }

            throw new AggregateException(exceptions);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Run retry action.
        /// </summary>
        /// <param name="action">retry action</param>
        /// <returns>asynchronous task</returns>
        public static Task Run(Action action)
        {
            return (new Retry()).Do(action);
        }

        /// <summary>
        /// Run retry function.
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="function">retry function</param>
        /// <returns>asynchronous task</returns>
        public static Task Run<T>(Func<Task<T>> function)
        {
            return (new Retry()).Do(function);
        }

        #endregion
    }
}
