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
            Application.Current.Resolve<IDeviceService>().RunOnMainThread(async () =>
            {
                viewModel.Initialize();
                await NavigateAsync(navigation, viewModel, animate);
            });
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
            viewModel.Initialize();
            await navigation.PushAsync(viewModel, animate);
        }

        /// <summary>
        /// Navigate to view model.
        /// </summary>
        /// <param name="navigation">navigation service</param>
        /// <param name="parameter">view model parameter</param>
        /// <param name="animate">transition animation flag</param>
        /// <returns></returns>
        public static void Navigate<T, TP>(this INavigationService navigation, TP parameter, bool animate = false) where T : BaseViewModel<TP> where TP : class
        {
            Navigate(navigation, Application.Current.Resolve<IPageService>().ResolveViewModel<T>(), parameter, animate);
        }

        /// <summary>
        /// Navigate to view model.
        /// </summary>
        /// <param name="navigation">navigation service</param>
        /// <param name="viewModel">view model</param>
        /// <param name="parameter">view model parameter</param>
        /// <param name="animate">transition animation flag</param>
        /// <returns></returns>
        public static void Navigate<T, TP>(this INavigationService navigation, T viewModel, TP parameter, bool animate = false) where T : BaseViewModel<TP> where TP : class
        {
            Application.Current.Resolve<IDeviceService>().RunOnMainThread(async () =>
            {
                viewModel.Initialize(parameter);
                await NavigateAsync<T, TP>(navigation, parameter, animate);
            });
        }

        /// <summary>
        /// Navigate to view model.
        /// </summary>
        /// <param name="navigation">navigation service</param>
        /// <param name="parameter">view model parameter</param>
        /// <param name="animate">transition animation flag</param>
        /// <returns></returns>
        public static async Task NavigateAsync<T, TP>(this INavigationService navigation, TP parameter, bool animate = false) where T : BaseViewModel<TP> where TP : class
        {
            await NavigateAsync(navigation, Application.Current.Resolve<IPageService>().ResolveViewModel<T>(), parameter, animate);
        }

        /// <summary>
        /// Navigate to view model.
        /// </summary>
        /// <param name="navigation">navigation service</param>
        /// <param name="viewModel">view model</param>
        /// <param name="parameter">view model parameter</param>
        /// <param name="animate">transition animation flag</param>
        /// <returns></returns>
        public static async Task NavigateAsync<T, TP>(this INavigationService navigation, T viewModel, TP parameter, bool animate = false) where T : BaseViewModel<TP> where TP : class
        {
            viewModel.Initialize(parameter);
            await navigation.PushAsync(viewModel, animate);
        }
    }
}
