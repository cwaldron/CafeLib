using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public abstract class CafeApplication : Application, IAlertService, IDisposable
    {
        protected IServiceRegistry Registry { get; }

        /// <summary>
        /// Default constructor used to suppress XAML warnings.
        /// </summary>
        public CafeApplication()
        {
            throw new Exception(@"Cannot instantiate cafe application via default constructor.");
        }

        /// <summary>
        /// Cafe mobile application constructor.
        /// </summary>
        /// <param name="serviceRegistry"></param>
        protected CafeApplication(IServiceRegistry serviceRegistry)
        {
            Registry = serviceRegistry ?? throw new ArgumentNullException(nameof(serviceRegistry));
        }

        public virtual void Configure()
        {
            Registry.AddScoped<IAlertService>(x => this);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Display an alert dialog.
        /// </summary>
        /// <param name="title">dialog title</param>
        /// <param name="message">dialog message</param>
        /// <param name="ok">accept button display</param>
        public void Alert(string title, string message, string ok = "OK")
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await MainPage.DisplayAlert(title, message, ok);
            });
        }

        /// <summary>
        /// Display confirmation dialog
        /// </summary>
        /// <param name="title">dialog title</param>
        /// <param name="message">dialog message</param>
        /// <param name="ok">accept button display</param>
        /// <param name="cancel">cancel button display</param>
        /// <returns></returns>
        public async Task<bool> Confirm(string title, string message, string ok = "OK", string cancel = "Cancel")
        {
            var completed = new TaskCompletionSource<bool>();

            Device.BeginInvokeOnMainThread(async () =>
            {
                var answer = await MainPage.DisplayAlert(title, message, ok, cancel);
                completed.SetResult(answer);
            });

            return await completed.Task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title">dialog title</param>
        /// <param name="cancel">cancel button display</param>
        /// <param name="delete">delete button display</param>
        /// <param name="options">enumerable list of option strings</param>
        /// <returns></returns>
        public async Task<string> SelectOption(string title, string cancel, string delete, IEnumerable<string> options)
        {
            var completed = new TaskCompletionSource<string>();

            Device.BeginInvokeOnMainThread(async () =>
            {
                var answer = await MainPage.DisplayActionSheet(title, cancel, delete, options.ToArray());
                completed.SetResult(answer);
            });

            return await completed.Task;
        }
    }
}
