using CafeLib.Mobile.ViewModels;
using CafeLib.Mobile.Views;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global

namespace CafeLib.Mobile.Services
{
    public interface IPageService
    {
        /// <summary>
        /// Resolve viewmodel type to is associated view.
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <returns>page instance that corresponds to the view model type</returns>
        BaseContentPage ResolvePage<T>() where T : BaseViewModel;

        /// <summary>
        /// Resolve viewmodel type to is associated view.
        /// </summary>
        /// <returns>page instance that corresponds to the view model</returns>
        BaseContentPage ResolvePage(BaseViewModel viewModel);

        /// <summary>
        /// Resolve view model
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <returns>view model instance</returns>
        T ResolveViewModel<T>() where T : BaseViewModel;
    }
}
