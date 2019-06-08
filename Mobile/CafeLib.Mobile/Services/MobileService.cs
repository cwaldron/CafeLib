using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CafeLib.Core.Extensions;
using CafeLib.Mobile.Extensions;
using CafeLib.Mobile.Support;
using CafeLib.Mobile.ViewModels;
using CafeLib.Mobile.Views;
using Xamarin.Forms;

namespace CafeLib.Mobile.Services
{
    internal class MobileService : IPageService, IAlertService, INavigationService, IDeviceService
    {
        private readonly Assembly _appAssembly;
        private readonly Dictionary<Type, PageResolver> _pageResolvers;

        private const string ViewModelSuffix = "ViewModel";
        private const string PageSuffix = "Page";
        private const string DefaultAcceptText = "OK";
        private const string DefaultCancelText = "Cancel";

        public Page NavigationPage { get; private set; }

        /// <summary>
        /// Bootstrapper constructor
        /// </summary>
        internal MobileService()
        {
            _appAssembly = Application.Current.GetType().Assembly;
            _pageResolvers = new Dictionary<Type, PageResolver>();
            InitPageResolvers();
        }

        /// <summary>
        /// Resolve viewmodel type to is associated view.
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <returns>page instance that corresponds to the view model type</returns>
        public AbstractContentPage ResolvePage<T>() where T : BaseViewModel
        {
            return (AbstractContentPage)ResolvePage(typeof(T));
        }

        /// <summary>
        /// Resolve viewmodel type to is associated view.
        /// </summary>
        /// <param name="viewModel">view model</param>
        /// <returns>page instance that corresponds to the view model type</returns>
        public AbstractContentPage ResolvePage(BaseViewModel viewModel)
        {
            return (AbstractContentPage)ResolvePage(viewModel.GetType());
        }

        /// <summary>
        /// Display an alert dialog.
        /// </summary>
        /// <param name="title">dialog title</param>
        /// <param name="message">dialog message</param>
        /// <param name="ok">accept button display</param>
        public void DisplayAlert(string title, string message, string ok = DefaultAcceptText)
        {
            Application.Current.AlertDialog(title, message, ok);
        }

        /// <summary>
        /// Display confirmation dialog
        /// </summary>
        /// <param name="title">dialog title</param>
        /// <param name="message">dialog message</param>
        /// <param name="ok">accept button display</param>
        /// <param name="cancel">cancel button display</param>
        /// <returns>true for OK, false for cancel</returns>
        public async Task<bool> DisplayConfirm(string title, string message, string ok = DefaultAcceptText, string cancel = DefaultCancelText)
        {
            return await Application.Current.ConfirmDialog(title, message, ok, cancel);
        }

        /// <summary>
        /// Display option selection dialog.
        /// </summary>
        /// <param name="title">dialog title</param>
        /// <param name="cancel">cancel button display</param>
        /// <param name="delete">delete button display</param>
        /// <param name="options">enumerable list of option strings</param>
        /// <returns>the selected option string</returns>
        public async Task<string> DisplayOptions(string title, string cancel, string delete, IEnumerable<string> options)
        {
            return await Application.Current.OptionsDialog(title, cancel, delete, options);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="currentViewModel"></param>
        /// <returns></returns>
        public async Task InsertBeforeAsync<T1, T2>(T1 viewModel, T2 currentViewModel) where T1 : BaseViewModel where T2 : BaseViewModel
        {
            NavigationPage.Navigation.InsertPageBefore(viewModel.ResolvePage(), currentViewModel.ResolvePage());
            await Task.CompletedTask;
        }

        /// <summary>
        /// Asynchronously adds page to the top of the navigation stack.
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <param name="viewModel">view model</param>
        /// <param name="animate">optional animation</param>
        /// <returns></returns>
        public async Task PushAsync<T>(T viewModel, bool animate = false) where T : BaseViewModel
        {
            var page = viewModel.ResolvePage();
            page.SetViewModel(viewModel);
            await NavigationPage.Navigation.PushAsync(page, animate);
        }

        /// <summary>
        /// Asynchronously remove most recent page from the navigation stack.
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <param name="animate">optional animation</param>
        /// <returns>The page previously at top of the navigation stack</returns>
        public async Task<T> PopAsync<T>(bool animate = false) where T : BaseViewModel
        {
            var page = await NavigationPage.Navigation.PopAsync(animate);
            return page.GetViewModel<T>();
        }

        /// <summary>
        /// Set the application navigator.
        /// </summary>
        /// <param name="page"></param>
        /// <returns>previous navigator</returns>
        public Page SetNavigationPage(Page page)
        {
            var previousNavigator = NavigationPage;
            NavigationPage = new NavigationPage(page);
            return previousNavigator;
        }

        /// <summary>
        /// Run action on main UI thread.
        /// </summary>
        /// <param name="action">action</param>
        public void RunOnMainThread(Action action)
        {
            Device.BeginInvokeOnMainThread(action);
        }

        /// <summary>
        /// Set up page resolvers.
        /// </summary>
        private void InitPageResolvers()
        {
            var viewModelTypeInfos = _appAssembly
                .CreatableTypes()
                .Inherits<BaseViewModel>()
                .EndsWith(ViewModelSuffix);

            foreach (var viewModelTypeInfo in viewModelTypeInfos)
            {
                var pageType = FindPageType(viewModelTypeInfo);
                if (pageType == null) continue;
                _pageResolvers.Add(viewModelTypeInfo.AsType(), new PageResolver(pageType));
            }
        }

        /// <summary>
        /// Find corresponding page type for a view model type.
        /// </summary>
        /// <param name="viewModelTypeInfo">view model type</param>
        /// <returns>page type</returns>
        private Type FindPageType(MemberInfo viewModelTypeInfo)
        {
            var pageTypeInfo = _appAssembly
                .CreatableTypes()
                .Inherits<AbstractContentPage>()
                .SingleOrDefault(x => x.Name == PageNameFromViewModel(viewModelTypeInfo));

            return pageTypeInfo?.AsType();
        }

        /// <summary>
        /// Gets the name of a view based on the associated view model.
        /// </summary>
        private static string PageNameFromViewModel(MemberInfo viewModelTypeInfo)
        {
            var viewModelName = viewModelTypeInfo.Name;
            if (viewModelName.EndsWith(ViewModelSuffix))
            {
                return viewModelName.Replace(ViewModelSuffix, PageSuffix);
            }

            throw new FormatException($"{viewModelName} is treated as view model but does not end with '{ViewModelSuffix}'. Name should be '{viewModelName}{ViewModelSuffix}', expected corresponding view should be named '{viewModelName}{PageSuffix}'.");
        }

        /// <summary>
        /// Resolves page associasted with the viewmodel.
        /// </summary>
        /// <param name="viewModelType">view model type</param>
        /// <returns>bounded page</returns>
        private Page ResolvePage(Type viewModelType)
        {
            if (!viewModelType.GetTypeInfo().IsSubclassOf(typeof(BaseViewModel)))
            {
                throw new ArgumentException($"{nameof(viewModelType)} is not a type of BaseViewModel");
            }

            // Check resolver registration.
            CheckResolverRegistration(viewModelType);

            // Obtain page from resolver.
            return GetPageFromResolver(viewModelType);
        }

        /// <summary>
        /// Check resolver registration.
        /// </summary>
        /// <param name="viewModelType"></param>
        private void CheckResolverRegistration(Type viewModelType)
        {
            if (!_pageResolvers.ContainsKey(viewModelType))
            {
                _pageResolvers.Add(viewModelType, new PageResolver(FindPageType(viewModelType)));
            }
        }

        /// <summary>
        /// Get the page from the page resolver.
        /// </summary>
        /// <returns></returns>
        /// <param name="viewModelType"></param>
        /// <returns></returns>
        private Page GetPageFromResolver(Type viewModelType)
        {
            // Leave if no page resolver exist for this view model type.
            if (!_pageResolvers.ContainsKey(viewModelType)) return default;

            // Resolve the page.
            var pageResolver = _pageResolvers[viewModelType];
            return (Page)pageResolver.Resolve();
        }
    }
}
