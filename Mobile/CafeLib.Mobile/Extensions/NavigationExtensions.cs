using System.Threading.Tasks;
using CafeLib.Mobile.Services;
using CafeLib.Mobile.ViewModels;
using Xamarin.Forms;
// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Extensions
{
    public static class NavigationExtensions
    {
        /// <summary>
        /// Navigate to view model.
        /// </summary>
        /// <param name="navigation">navigation service</param>
        /// <param name="animate">transition animation flag</param>
        /// <returns></returns>
        public static void Navigate<T>(this INavigationService navigation, bool animate = false) where T : BaseViewModel
        {
            Navigate(navigation, Application.Current.Resolve<IPageService>().ResolveViewModel<T>(), animate);
        }

        /// <summary>
        /// Navigate to view model.
        /// </summary>
        /// <param name="navigation">navigation service</param>
        /// <param name="viewModel">view model</param>
        /// <param name="animate">transition animation flag</param>
        /// <returns></returns>
        public static void Navigate<T>(this INavigationService navigation, T viewModel, bool animate = false) where T : BaseViewModel
        {
            Application.Current.Resolve<IDeviceService>().RunOnMainThread(async () => await NavigateAsync(navigation, viewModel, animate));
        }

        /// <summary>
        /// Navigate to view model.
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <param name="navigation">navigation service</param>
        /// <param name="animate">transition animation flag</param>
        /// <returns></returns>
        public static async Task NavigateAsync<T>(this INavigationService navigation, bool animate = false) where T : BaseViewModel
        {
            await NavigateAsync(navigation, Application.Current.Resolve<IPageService>().ResolveViewModel<T>(), animate);
        }

        /// <summary>
        /// Asynchronously adds page to the top of the navigation stack.
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <param name="navigation">navigation service</param>
        /// <param name="viewModel">view model</param>
        /// <param name="animate">optional animation</param>
        /// <returns></returns>
        public static async Task NavigateAsync<T>(this INavigationService navigation, T viewModel, bool animate = false) where T : BaseViewModel
        {
            await navigation.PushAsync(viewModel, animate);
        }
    }
}
