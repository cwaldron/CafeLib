using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CafeLib.Core.IoC;
using CafeLib.Mobile.Extensions;
using CafeLib.Mobile.Services;
using CafeLib.Mobile.Views;
using Xamarin.Forms;
// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.ViewModels
{
    public abstract class BaseViewModel : ObservableBase
    {
        /// <summary>
        /// ViewModelBase constructor
        /// </summary>
        /// <param name="resolver"></param>
        protected BaseViewModel(IServiceResolver resolver)
        {
            Resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            AppearingCommand = new Command(() => {});
            DisappearingCommand = new Command(() => { });
            BackButtonPressed = () => false;
        }

        /// <summary>
        /// Initialize the view model.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Service resolver.
        /// </summary>
        protected IServiceResolver Resolver { get; }

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
        /// Resolve the associated page.
        /// </summary>
        protected BaseContentPage Page => PageService.ResolvePage(this);

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
        /// Appearing command.
        /// </summary>
        public ICommand AppearingCommand { get; protected set; }

        /// <summary>
        /// Disappearing command.
        /// </summary>
        public ICommand DisappearingCommand { get; protected set; }


        public Func<bool> BackButtonPressed { get; protected set; }

        /// <summary>
        /// Resolve the associated page.
        /// </summary>

        /// <summary>
        /// Resolves viewmodel to is associated view.
        /// </summary>
        /// <returns>bounded page</returns>
        internal Page ResolvePage() => PageService.ResolvePage(this);

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

    public class BaseViewModel<TParameter> : BaseViewModel
    {
        /// <summary>
        /// ViewModelBase constructor
        /// </summary>
        /// <param name="resolver"></param>
        public BaseViewModel(IServiceResolver resolver)
            : base(resolver)
        {
        }

        /// <summary>
        /// Initialize and pass parameter to the view model.
        /// </summary>
        /// <param name="parameter">parameter passed to view model</param>
        public virtual void Initialize(TParameter parameter)
        {
        }
    }
}
