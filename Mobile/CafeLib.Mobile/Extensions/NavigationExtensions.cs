﻿using System.Threading.Tasks;
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
            navigation.Navigate(Application.Current.Resolve<IPageService>().ResolveViewModel<T>(), animate);
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
                await navigation.NavigateAsync<T, TP>(parameter, animate);
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
        /// Close the view model
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <param name="navigation">navigation service</param>
        /// <param name="animate">transition animation flag</param>
        public static void Close<T>(this INavigationService navigation, bool animate = false) where T : BaseViewModel
        {
            Application.Current.Resolve<IDeviceService>().RunOnMainThread(async () => await CloseAsync<T>(navigation, animate));
        }

        /// <summary>
        /// Close the view model
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <param name="navigation">navigation service</param>
        /// <param name="animate">transition animation flag</param>
        /// <returns></returns>
        public static async Task CloseAsync<T>(this INavigationService navigation, bool animate = false) where T : BaseViewModel
        {
            await navigation.PopAsync<T>(animate);
        }

        /// <summary>
        /// Close the modal view model
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <param name="navigation">navigation service</param>
        /// <param name="animate">transition animation flag</param>
        public static void CloseModal<T>(this INavigationService navigation, bool animate = false) where T : BaseViewModel
        {
            Application.Current.Resolve<IDeviceService>().RunOnMainThread(async () => await CloseModalAsync<T>(navigation, animate));
        }

        /// <summary>
        /// Close the modal view model
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <param name="navigation">navigation service</param>
        /// <param name="animate">transition animation flag</param>
        /// <returns></returns>
        public static async Task CloseModalAsync<T>(this INavigationService navigation, bool animate = false) where T : BaseViewModel
        {
            await navigation.PopModalAsync<T>(animate);
        }
    }
}
