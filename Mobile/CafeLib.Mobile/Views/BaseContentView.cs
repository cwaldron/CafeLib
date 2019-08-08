using System;
using CafeLib.Mobile.Effects;
using CafeLib.Mobile.Extensions;
using CafeLib.Mobile.ViewModels;
using Xamarin.Forms;

// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Views
{
    public class BaseContentView : ContentView
    {
        public Action Appearing;
        public Action Disappearing;

        /// <summary>
        /// BaseContextView constructor.
        /// </summary>
        public BaseContentView()
        {
            var lifecycleEffect = new ViewLifecycleEffect();
            lifecycleEffect.Loaded += (s, e) => OnLoad();
            lifecycleEffect.Unloaded += (s, e) => OnUnload();
            Effects.Add(lifecycleEffect);
            Appearing = OnAppearing;
            Disappearing = OnDisappearing;
        }

        /// <summary>
        /// The viewmodel bound to the page.
        /// </summary>
        public BaseViewModel ViewModel
        {
            get => BindingContext as BaseViewModel;
            set => BindingContext = value;
        }

        protected virtual void OnAppearing()
        {
            if (ViewModel == null) return;
            ViewModel.IsVisible = true;
            ViewModel.AppearingCommand.Execute(null);
        }

        protected virtual void OnDisappearing()
        {
            if (ViewModel == null) return;
            ViewModel.DisappearingCommand.Execute(null);
            ViewModel.IsVisible = false;
        }

        protected virtual void OnLoad()
        {
        }

        protected virtual void OnUnload()
        {
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext is BaseViewModel vm)
            {
                Application.Current.RunOnMainThread(async () => await vm.InitAsync());
            }
        }
    }
}
