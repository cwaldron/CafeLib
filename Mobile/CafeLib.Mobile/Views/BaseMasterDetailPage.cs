using System;
using CafeLib.Mobile.ViewModels;
using Xamarin.Forms;

// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Views
{
    public abstract class BaseMasterDetailPage : MasterDetailPage, IPageBase, ISoftNavigationPage, IDisposable
    {
        private bool _disposed;

        /// <summary>
        /// Navigation ownership.
        /// </summary>
        public INavigableOwner Owner { get; internal set; }

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

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            Dispose(!_disposed);
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose base master detail page.
        /// </summary>
        /// <param name="disposing">indicate whether the queue is disposing</param>
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}