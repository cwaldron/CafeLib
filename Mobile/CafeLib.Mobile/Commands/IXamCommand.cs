using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace CafeLib.Mobile.Commands
{
    /// <summary>
    /// ICommand adapter interface.
    /// </summary>
    public interface IXamCommand : ICommand
    {
        void ChangeCanExecute();
    }
}