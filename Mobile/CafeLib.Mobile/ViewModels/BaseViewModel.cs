using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CafeLib.Core.IoC;
using CafeLib.Mobile.Extensions;
using CafeLib.Mobile.Services;
using CafeLib.Mobile.Views;
using Xamarin.Forms;

namespace CafeLib.Mobile.ViewModels
{
    public abstract class BaseViewModel : ObservableBase
    {
        /// <summary>
        /// BaseViewModel constructor.
        /// </summary>
        protected BaseViewModel(IServiceResolver resolver)
        {
            Resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            OnBackButtonPressed = () => false;
        }

        /// <summary>
        /// Service resolver.
        /// </summary>
        protected IServiceResolver Resolver { get; }

        protected AbstractContentPage Page => PageService.ResolvePage(this);

        /// <summary>
        /// Back button pressed listener.
        /// </summary>
        public Func<bool> OnBackButtonPressed { get; protected set; }

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
        /// Establish viewmodel as the navigation page.
        /// </summary>
        /// <returns></returns>
        public Page AsNavigationPage()
        {
            NavigationService.SetNavigationPage(PageService.ResolvePage(this));
            return NavigationService.NavigationPage;
        }

        /// <summary>
        /// Resolves viewmodel to is associated view.
        /// </summary>
        /// <returns>bounded page</returns>
        public Page ResolvePage()
        {
            return PageService.ResolvePage(this);
        }

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
