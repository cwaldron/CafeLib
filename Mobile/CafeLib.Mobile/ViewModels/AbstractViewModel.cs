using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CafeLib.Core.IoC;
using CafeLib.Mobile.Extensions;
using CafeLib.Mobile.Services;
using CafeLib.Mobile.Views;
using Xamarin.Forms;
// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.ViewModels
{
    public abstract class AbstractViewModel : ObservableBase
    {
        /// <summary>
        /// BaseViewModel constructor.
        /// </summary>
        protected AbstractViewModel(IServiceResolver resolver)
        {
            Resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        /// <summary>
        /// Service resolver.
        /// </summary>
        protected IServiceResolver Resolver { get; }

        /// <summary>
        /// Resolve the associated page.
        /// </summary>
        protected abstract AbstractContentPage Page { get; }

        /// <summary>
        /// Resolve a view model via its type.
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <returns>view model instance</returns>
        protected T ResolveViewModel<T>() where T : AbstractViewModel => Resolver.Resolve<T>();

        /// <summary>
        /// Title.
        /// </summary>
        private string _title;
        public string Title
        {
            get => _title;
            set => SetValue(ref _title, value);
        }

        /// <summary>
        /// Page service.
        /// </summary>
        protected IPageService PageService => Resolver.Resolve<IPageService>();

        /// <summary>
        /// Navigation service.
        /// </summary>
        protected INavigationService NavigationService => Resolver.Resolve<INavigationService>();

        /// <summary>
        /// 
        /// </summary>
        protected IDeviceService DeviceService => Resolver.Resolve<IDeviceService>();

        /// <summary>
        /// Displays an alert on the page.
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="message">message</param>
        /// <param name="ok">OK</param>
        public void DisplayAlert(string title, string message, string ok = "OK")
        {
            DeviceService.RunOnMainThread(() =>
            {
                Application.Current.AlertDialog(title, message, ok);
            });
        }

        /// <summary>
        /// Displays an alert (simple question) on the page.
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="message">message</param>
        /// <param name="ok">OK</param>
        /// <param name="cancel">cancel</param>
        public async Task<bool> DisplayConfirm(string title, string message, string ok = "OK", string cancel = "Cancel")
        {
            var completed = new TaskCompletionSource<bool>();

            DeviceService.RunOnMainThread(async () =>
            {
                var answer = await Application.Current.ConfirmDialog(title, message, ok, cancel);
                completed.SetResult(answer);
            });

            return await completed.Task;
        }

        /// <summary>
        /// Displays an action sheet (list of buttons) on the page, asking for user input.
        /// </summary>
        /// <param name="title">dialog title</param>
        /// <param name="cancel">cancellation string</param>
        /// <param name="destroy">destroy string</param>
        /// <param name="options">option list</param>
        /// <returns></returns>
        public async Task<string> DisplayOptions(string title, string cancel, string destroy, IEnumerable<string> options)
        {
            var completed = new TaskCompletionSource<string>();

            DeviceService.RunOnMainThread(async () =>
            {
                var answer = await Application.Current.OptionsDialog(title, cancel, destroy, options.ToArray());
                completed.SetResult(answer);
            });

            return await completed.Task;
        }
    }
}
