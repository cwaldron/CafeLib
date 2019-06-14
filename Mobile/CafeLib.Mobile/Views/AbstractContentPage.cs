using System;
using CafeLib.Core.IoC;
using CafeLib.Mobile.Extensions;
using CafeLib.Mobile.ViewModels;
using Xamarin.Forms;
// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Views
{
    public abstract class AbstractContentPage : ContentPage
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
        /// Set the binding context to the view model
        /// </summary>
        /// <typeparam name="T">viewmodel type</typeparam>
        /// <param name="viewModel">viewmodel instance</param>
        public void SetViewModel<T>(T viewModel) where T : AbstractViewModel
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
}
