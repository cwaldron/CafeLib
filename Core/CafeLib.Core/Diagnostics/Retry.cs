using System;
using System.Collections.Generic;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace CafeLib.Core.Diagnostics
{
    public class Retry
    {
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
        public Retry(int retryLimit = 3, int retryInterval = 50)
        {
            Limit = retryLimit > 0 ? retryLimit : 1;
            Interval = retryInterval > 0 ? retryInterval : 50;
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
            await Do<object>(() =>
            {
                action();
                return null;
            });
        }

        /// <summary>
        /// Do retries on function if necessary.
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="action">function action</param>
        /// <returns>the action return result</returns>
        public async Task<T> Do<T>(Func<Task<T>> action)
        {
            var exceptions = new List<Exception>();

            for (var retry = 0; retry < Limit; retry++)
            {
                try
                {
                    if (retry > 0)
                        await Task.Delay(Interval);

                    return await action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            throw new AggregateException(exceptions);
        }

        #endregion
    }
}
