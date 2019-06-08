using System.Threading.Tasks;
using CafeLib.Mobile.ViewModels;
using Xamarin.Forms;

namespace CafeLib.Mobile.Services
{
    public interface INavigationService
    {
        /// <summary>
        /// Navigation page.
        /// </summary>
        Page NavigationPage { get; }

        /// <summary>
        /// Insert view model before the current view model.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="currentViewModel"></param>
        /// <returns></returns>
        Task InsertBeforeAsync<T1, T2>(T1 viewModel, T2 currentViewModel) where T1 : BaseViewModel where T2 : BaseViewModel;

        /// <summary>
        /// Navigate to pushed view model.
        /// </summary>
        /// <param name="viewModel">view model</param>
        /// <param name="animate">transition animation flag</param>
        /// <returns></returns>
        Task PushAsync<T>(T viewModel, bool animate = false) where T : BaseViewModel;

        /// <summary>
        /// Navigate back to popped view model
        /// </summary>
        /// <param name="animate">transition animation flag</param>
        /// <returns>page associated with view model</returns>
        Task<T> PopAsync<T>(bool animate = false) where T : BaseViewModel;

        /// <summary>
        /// Set the application navigator.
        /// </summary>
        /// <param name="page"></param>
        /// <returns>previous navigator</returns>
        Page SetNavigationPage(Page page);
    }
}
