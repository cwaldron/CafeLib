using System.Windows.Input;
using Xamarin.Forms;
// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.ViewModels
{
    public abstract class BaseMasterDetailViewModel<TObject> : BaseViewModel<TObject> where TObject : class
    {
        protected BaseMasterDetailViewModel()
        {
            PresentedCommand = new Command(() => { });
        }

        private bool _isPresented;
        public bool IsPresented
        {
            get => _isPresented;
            set
            {
                if (SetValue(ref _isPresented, value))
                    _presentedCommand.Execute(_isPresented);
            }
        }

        /// <summary>
        /// Appearing command.
        /// </summary>
        private ICommand _presentedCommand;
        public ICommand PresentedCommand
        {
            get => _presentedCommand;
            set => SetValue(ref _presentedCommand, value);
        }
    }

    public abstract class BaseMasterDetailViewModel : BaseMasterDetailViewModel<object>
    {
    }
}