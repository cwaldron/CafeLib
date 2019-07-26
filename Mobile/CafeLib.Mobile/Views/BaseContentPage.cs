using System;
using CafeLib.Core.IoC;
using CafeLib.Mobile.Extensions;
using CafeLib.Mobile.ViewModels;
using Xamarin.Forms;

// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Views
{
    public abstract class BaseContentPage : ContentPage, IPageBase, IDisposable
    {
        private bool _disposed;

        /// <summary>
        /// The viewmodel bound to the page.
        /// </summary>
        protected IServiceResolver Resolver => Application.Current.Resolve<IServiceResolver>();

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(!_disposed);
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Get the view model bound to the page.
        /// </summary>
        /// <typeparam name="TViewModel">view model type</typeparam>
        public TViewModel GetViewModel<TViewModel>() where TViewModel : BaseViewModel
        {
            return (TViewModel) BindingContext;
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
        /// Dispose concurrent queue.
        /// </summary>
        /// <param name="disposing">indicate whether the queue is disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            BindingContext = null;
            Content = null;
        }
    }

    public abstract class BaseContentPage<T> : BaseContentPage, ISoftNavigationPage where T : BaseViewModel
    {
        /// <summary>
        /// The viewmodel bound to the page.
        /// </summary>
        public T ViewModel => GetViewModel<T>();

        /// <summary>
        /// Default constructor to allow creation of simple fakes for unit testing.
        /// This constructor should never be used in production code.
        /// </summary>
        protected BaseContentPage()
        {
            SetViewModel(ResolveViewModel());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.IsVisible = true;
            ViewModel.AppearingCommand.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel.DisappearingCommand.Execute(null);
            ViewModel.IsVisible = false;
        }

        /// <summary>
        /// Process hardware back button press event.
        /// </summary>
        /// <returns>true: ignore behavior; false: default behavior</returns>
        protected override bool OnBackButtonPressed()
        {
            return ViewModel.BackButtonPressed.Execute(NavigationSource.Hardware);
        }

        /// <summary>
        /// Process software back button press event.
        /// </summary>
        /// <returns>true: ignore behavior; false: default behavior</returns>
        public virtual bool OnSoftBackButtonPressed()
        {
            return ViewModel.BackButtonPressed.Execute(NavigationSource.Software);
        }

        /// <summary>
        /// Resolve view model.
        /// </summary>
        /// <returns></returns>
        protected T ResolveViewModel()
        {
            return Resolver.Resolve<T>();
        }
    }
}
