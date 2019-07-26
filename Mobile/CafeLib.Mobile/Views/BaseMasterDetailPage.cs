using CafeLib.Mobile.ViewModels;
using Xamarin.Forms;

// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Views
{
    public abstract class BaseMasterDetailPage : MasterDetailPage, IPageBase, ISoftNavigationPage
    {
        public TViewModel GetViewModel<TViewModel>() where TViewModel : BaseViewModel
        {
            return (TViewModel)BindingContext;
        }

        public void SetViewModel<TViewModel>(TViewModel viewModel) where TViewModel : BaseViewModel
        {
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetViewModel<BaseViewModel>().AppearingCommand.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GetViewModel<BaseViewModel>().DisappearingCommand.Execute(null);
        }

        protected override bool OnBackButtonPressed()
        {
            return GetViewModel<BaseViewModel>().BackButtonPressed.Execute(NavigationSource.Hardware);
        }


        public bool OnSoftBackButtonPressed()
        {
            return GetViewModel<BaseViewModel>().BackButtonPressed.Execute(NavigationSource.Software);
        }
    }
}