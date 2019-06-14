using System;
using System.Windows.Input;
using CafeLib.Core.IoC;
using CafeLib.Mobile.Views;
using Xamarin.Forms;
// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.ViewModels
{
    public abstract class BaseViewModel : AbstractViewModel
    {
        /// <summary>
        /// ViewModelBase constructor
        /// </summary>
        /// <param name="resolver"></param>
        protected BaseViewModel(IServiceResolver resolver)
            : base(resolver)
        {
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
        protected sealed override AbstractContentPage Page => PageService.ResolvePage(this);

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
