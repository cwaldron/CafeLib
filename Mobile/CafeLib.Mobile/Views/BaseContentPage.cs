using CafeLib.Mobile.ViewModels;
// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Views
{
    public abstract class BaseContentPage<T> : AbstractContentPage where T : BaseViewModel
    {
        /// <summary>
        /// The viewmodel bound to the page.
        /// </summary>
        public T ViewModel => BindingContext as T;

        /// <summary>
        /// Default constructor to allow creation of simple fakes for unit testing.
        /// This constructor should never be used in production code.
        /// </summary>
        protected BaseContentPage()
        {
            SetViewModel(ResolveViewModel());
        }

        /// <summary>
        /// Process background button press event.
        /// </summary>
        /// <returns></returns>
        protected override bool OnBackButtonPressed()
        {
            return ViewModel.OnBackButtonPressed();
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
