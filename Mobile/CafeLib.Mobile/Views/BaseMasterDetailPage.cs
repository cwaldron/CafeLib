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

        public TViewModel GetViewModel<TViewModel>() where TViewModel : BaseViewModel
        {
            return (TViewModel)(BindingContext as BaseViewModel);
        }

        public void SetViewModel<TViewModel>(TViewModel viewModel) where TViewModel : BaseViewModel
        {
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetViewModel<BaseViewModel>()?.AppearingCommand.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GetViewModel<BaseViewModel>()?.DisappearingCommand.Execute(null);
        }

        protected virtual void OnLoad()
        {
            GetViewModel<BaseViewModel>()?.LoadCommand.Execute(null);
        }

        protected virtual void OnUnload()
        {
            GetViewModel<BaseViewModel>()?.UnloadCommand.Execute(null);
        }

        protected override bool OnBackButtonPressed()
        {
            return GetViewModel<BaseViewModel>() != null && GetViewModel<BaseViewModel>().BackButtonPressed.Execute(NavigationSource.Hardware);
        }

        public bool OnSoftBackButtonPressed()
        {
            return GetViewModel<BaseViewModel>() != null && GetViewModel<BaseViewModel>().BackButtonPressed.Execute(NavigationSource.Software);
        }
    }
}