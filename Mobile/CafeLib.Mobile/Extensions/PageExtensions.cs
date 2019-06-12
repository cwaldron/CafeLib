using CafeLib.Mobile.ViewModels;
using Xamarin.Forms;

namespace CafeLib.Mobile.Extensions
{
    public static class PageExtensions
    {
        /// <summary>
        /// Get the view model bound to the page.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <returns></returns>
        public static T GetViewModel<T>(this Page page) where T : BaseViewModel
        {
            return (T) page.BindingContext;
        }

        /// <summary>
        /// Set the view model to the binding context of the page.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="viewModel"></param>
        public static void SetViewModel<T>(this Page page, T viewModel) where T : BaseViewModel
        {
            if (page.BindingContext == viewModel) return;
            page.BindingContext = null;
            page.BindingContext = viewModel;
        }

        /// <summary>
        /// Invoke the page viewmodel back command.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPage"></typeparam>
        /// <param name="page"></param>
        //public static void InvokeBackCommand<T, TPage>(this TPage page) where T : BaseViewModel<TPage> where TPage : Page
        //{
        //    var viewModel = page.GetViewModel<T, TPage>();
        //    var viewModelType = viewModel.GetType();
        //    var propInfo = viewModelType.GetTypeInfo().GetDeclaredProperty("BackCommand");
        //    var command = (ICommand)propInfo?.GetValue(viewModel);
        //    command?.Execute(null);
        //}
    }
}
