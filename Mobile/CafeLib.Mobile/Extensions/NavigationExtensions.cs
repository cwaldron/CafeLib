using System.Linq;
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
        /// Close the view model.
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <param name="navigator">navigation object</param>
        /// <param name="viewModel">view model</param>
        /// <param name="animate">transition animation flag</param>
        public static void Close<T>(this INavigation navigator, T viewModel, bool animate = false) where T : BaseViewModel
        {
            var page = viewModel.ResolvePage();
            var navigationType = navigator.GetNavigationType(page);
            if (navigationType == 0) return;

            Application.Current.Resolve<IDeviceService>().RunOnMainThread(async () =>
            {
                if (navigationType == 1)
                {
                    if (page != navigator.Peek()) navigator.BringToTop(page);
                    await navigator.PopAsync(animate);
                }
                else
                {
                    await page.Navigation.PopModalAsync(animate);
                }
            });
        }

        /// <summary>
        /// Close the view model.
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <typeparam name="TP">view model parameter type</typeparam>
        /// <param name="navigator">navigation object</param>
        /// <param name="viewModel">view model</param>
        /// <param name="parameter">view model parameter</param>
        /// <param name="animate">transition animation flag</param>
        public static void Close<T, TP>(this INavigation navigator, T viewModel, TP parameter, bool animate = false) where T : BaseViewModel where TP : class
        {
            var page = viewModel.ResolvePage();
            var navigationType = navigator.GetNavigationType(page);
            if (navigationType == 0) return;

            navigator.Close(viewModel, animate);

            var topPage = navigator.Peek();
            if (topPage == null) return;

            Application.Current.Resolve<IDeviceService>().RunOnMainThread(async () =>
            {
                var vm = topPage.GetViewModel<BaseViewModel<TP>>();
                await vm.InitAsync(parameter);
            });
        }

        /// <summary>
        /// Brings a page to the top of the navigation stack.
        /// </summary>
        /// <param name="navigator">navigation object</param>
        /// <param name="page">page to bring to the top of the navigation stack</param>
        /// <returns></returns>
        internal static void BringToTop(this INavigation navigator, Page page)
        {
            var topPage = navigator.NavigationStack.LastOrDefault();
            if (page == topPage) return;
            navigator.InsertPageBefore(topPage, page);
        }

        /// <summary>
        /// Determine the navigation stack containing the page.
        /// </summary>
        /// <param name="navigator">navigation object</param>
        /// <param name="page">page to locates</param>
        /// <returns>-1: modal stack, 1: navigation stack, 0: neither</returns>
        internal static int GetNavigationType(this INavigation navigator, Page page)
        {
            return navigator.NavigationStack.Contains(page)
                ? 1
                : navigator.ModalStack.Contains(page)
                    ? -1
                    : 0;
        }

        /// <summary>
        /// Returns the page at the top of the navigation stack.
        /// </summary>
        /// <param name="navigator">navigation object</param>
        /// <returns>page at top of navigation stack</returns>
        public static Page Peek(this INavigation navigator)
        {
            return navigator.NavigationStack.LastOrDefault();
        }

        /// <summary>
        /// Navigate to view model.
        /// </summary>
        /// <param name="navigation">navigation service</param>
        /// <param name="animate">transition animation flag</param>
        /// <returns></returns>
        public static void Navigate<T>(this INavigationService navigation, bool animate = false) where T : BaseViewModel
        {
            navigation.Navigate(Application.Current.Resolve<IPageService>().ResolveViewModel<T>(), animate);
        }

        /// <summary>
        /// Navigate to view model.
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <param name="navigation">navigation service</param>
        /// <param name="viewModel">view model</param>
        /// <param name="animate">transition animation flag</param>
        public static void Navigate<T>(this INavigationService navigation, T viewModel, bool animate = false) where T : BaseViewModel
        {
            Application.Current.Resolve<IDeviceService>().RunOnMainThread(async () =>
            {
                await navigation.NavigateAsync(viewModel, animate);
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
            await navigation.NavigateAsync(Application.Current.Resolve<IPageService>().ResolveViewModel<T>(), animate);
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
            await viewModel.InitAsync();
            await navigation.PushAsync(viewModel, animate);
        }

        /// <summary>
        /// Navigate to view model.
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <typeparam name="TP">view model parameter type</typeparam>
        /// <param name="navigation">navigation service</param>
        /// <param name="parameter">view model parameter</param>
        /// <param name="animate">transition animation flag</param>
        public static void Navigate<T, TP>(this INavigationService navigation, TP parameter, bool animate = false) where T : BaseViewModel<TP> where TP : class
        {
            navigation.Navigate(Application.Current.Resolve<IPageService>().ResolveViewModel<T>(), parameter, animate);
        }

        /// <summary>
        /// Navigate to view model.
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <typeparam name="TP">view model parameter type</typeparam>
        /// <param name="navigation">navigation service</param>
        /// <param name="viewModel">view model</param>
        /// <param name="parameter">view model parameter</param>
        /// <param name="animate">transition animation flag</param>
        /// <returns></returns>
        public static void Navigate<T, TP>(this INavigationService navigation, T viewModel, TP parameter, bool animate = false) where T : BaseViewModel<TP> where TP : class
        {
            Application.Current.Resolve<IDeviceService>().RunOnMainThread(async () =>
            {
                await navigation.NavigateAsync(viewModel, parameter, animate);
            });
        }

        /// <summary>
        /// Navigate to view model.
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <typeparam name="TP">view model parameter type</typeparam>
        /// <param name="navigation">navigation service</param>
        /// <param name="parameter">view model parameter</param>
        /// <param name="animate">transition animation flag</param>
        /// <returns></returns>
        public static async Task NavigateAsync<T, TP>(this INavigationService navigation, TP parameter, bool animate = false) where T : BaseViewModel<TP> where TP : class
        {
            await navigation.NavigateAsync(Application.Current.Resolve<IPageService>().ResolveViewModel<T>(), parameter, animate);
        }

        /// <summary>
        /// Navigate to view model.
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <typeparam name="TP">view model parameter type</typeparam>
        /// <param name="navigation">navigation service</param>
        /// <param name="viewModel">view model</param>
        /// <param name="parameter">view model parameter</param>
        /// <param name="animate">transition animation flag</param>
        /// <returns></returns>
        public static async Task NavigateAsync<T, TP>(this INavigationService navigation, T viewModel, TP parameter, bool animate = false) where T : BaseViewModel<TP> where TP : class
        {
            await viewModel.InitAsync(parameter);
            await navigation.PushAsync(viewModel, animate);
        }

        /// <summary>
        /// Navigate to modal view model.
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <param name="navigation">navigation service</param>
        /// <param name="viewModel">view model</param>
        /// <param name="animate">transition animation flag</param>
        public static void NavigateModal<T>(this INavigationService navigation, T viewModel, bool animate = false) where T : BaseViewModel
        {
            Application.Current.Resolve<IDeviceService>().RunOnMainThread(async () =>
            {
                await viewModel.InitAsync();
                await navigation.PushModalAsync(viewModel, animate);
            });
        }

        /// <summary>
        /// Navigate to modal view model.
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <typeparam name="TP">view model parameter type</typeparam>
        /// <param name="navigation">navigation service</param>
        /// <param name="viewModel">view model</param>
        /// <param name="parameter">view model parameter</param>
        /// <param name="animate">transition animation flag</param>
        /// <returns></returns>
        public static void NavigateModal<T, TP>(this INavigationService navigation, T viewModel, TP parameter, bool animate = false) where T : BaseViewModel<TP> where TP : class
        {
            Application.Current.Resolve<IDeviceService>().RunOnMainThread(async () =>
            {
                await viewModel.InitAsync(parameter);
                await navigation.PushModalAsync(viewModel, animate);
            });
        }
    }
}
