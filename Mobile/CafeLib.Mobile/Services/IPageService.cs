using CafeLib.Mobile.ViewModels;
using CafeLib.Mobile.Views;

namespace CafeLib.Mobile.Services
{
    public interface IPageService
    {
        /// <summary>
        /// Resolve viewmodel type to is associated view.
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <returns>page instance that corresponds to the view model type</returns>
        AbstractContentPage ResolvePage<T>() where T : BaseViewModel;

        /// <summary>
        /// Resolve viewmodel type to is associated view.
        /// </summary>
        /// <returns>page instance that corresponds to the view model</returns>
        AbstractContentPage ResolvePage(BaseViewModel viewModel);
    }
}
