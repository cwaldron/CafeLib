using CafeLib.Mobile.Effects;
using CafeLib.Mobile.ViewModels;
using Xamarin.Forms;

// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Views
{
    public abstract class BaseMasterDetailPage : MasterDetailPage, IPageBase, ISoftNavigationPage
    {
        /// <summary>
        /// BaseMasterDetailPage constructor.
        /// </summary>
        // ReSharper disable once PublicConstructorInAbstractClass
        public BaseMasterDetailPage()
        {
            var lifecycleEffect = new ViewLifecycleEffect();
            lifecycleEffect.Loaded += (s, e) => OnLoad();
            lifecycleEffect.Unloaded += (s, e) => OnUnload();
            Effects.Add(lifecycleEffect);
        }

        /// <summary>
        /// Navigation ownership.
        /// </summary>
        public INavigableOwner Owner { get; internal set; }

        /// <summary>
        /// Get the view model bound to the page.
        /// </summary>
        /// <typeparam name="TViewModel">view model type</typeparam>
        public TViewModel GetViewModel<TViewModel>() where TViewModel : BaseViewModel
        {
            return (TViewModel)(BindingContext as BaseViewModel);
        }

        /// <summary>
        /// Set the binding context to the view model
        /// </summary>
        /// <typeparam name="TViewModel">view model type</typeparam>
        /// <param name="viewModel">viewmodel instance</param>
        public void SetViewModel<TViewModel>(TViewModel viewModel) where TViewModel : BaseViewModel
        {
            BindingContext = viewModel;
        }

        /// <summary>
        /// Process OnAppearing lifecycle event.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetViewModel<BaseViewModel>()?.AppearingCommand.Execute(null);
        }

        /// <summary>
        /// Process OnDisappearing lifecycle event.
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GetViewModel<BaseViewModel>()?.DisappearingCommand.Execute(null);
        }

        /// <summary>
        /// Process OnLoad lifecycle event.
        /// </summary>
        protected virtual void OnLoad()
        {
            GetViewModel<BaseViewModel>()?.LoadCommand.Execute(null);
        }

        /// <summary>
        /// Process OnUnload lifecycle event.
        /// </summary>
        protected virtual void OnUnload()
        {
            GetViewModel<BaseViewModel>()?.UnloadCommand.Execute(null);
        }

        /// <summary>
        /// Process hardware back button press event.
        /// </summary>
        /// <returns>true: ignore behavior; false: default behavior</returns>
        protected override bool OnBackButtonPressed()
        {
            return GetViewModel<BaseViewModel>() != null && GetViewModel<BaseViewModel>().BackButtonPressed.Execute(NavigationSource.Hardware);
        }

        /// <summary>
        /// Process software back button press event.
        /// </summary>
        /// <returns>true: ignore behavior; false: default behavior</returns>
        public bool OnSoftBackButtonPressed()
        {
            return GetViewModel<BaseViewModel>() != null && GetViewModel<BaseViewModel>().BackButtonPressed.Execute(NavigationSource.Software);
        }
    }
}