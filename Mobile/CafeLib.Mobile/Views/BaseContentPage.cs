using CafeLib.Core.IoC;
using CafeLib.Mobile.Effects;
using CafeLib.Mobile.Extensions;
using CafeLib.Mobile.ViewModels;
using Xamarin.Forms;

// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Views
{
    public abstract class BaseContentPage : ContentPage, IPageBase
    {
        /// <summary>
        /// The viewmodel bound to the page.
        /// </summary>
        protected IServiceResolver Resolver => Application.Current.Resolve<IServiceResolver>();

        /// <summary>
        /// Navigation ownership.
        /// </summary>
        public INavigableOwner Owner { get; internal set; }

        /// <summary>
        /// BaseContextPage constructor.
        /// </summary>
        // ReSharper disable once PublicConstructorInAbstractClass
        public BaseContentPage()
        {
            var lifecycleEffect = new ViewLifecycleEffect();
            lifecycleEffect.Loaded += (s, e) => OnLoad();
            lifecycleEffect.Unloaded += (s, e) => OnUnload();
            Effects.Add(lifecycleEffect);
        }

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
        /// Dispose base content page.
        /// </summary>
        /// <param name="disposing">indicates whether the page is disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            BindingContext = null;
            Content = null;
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
