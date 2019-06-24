using System;
using System.Windows.Input;
using Xamarin.Forms;
// ReSharper disable UnusedTypeParameter
// ReSharper disable UnusedMemberInSuper.Global

namespace CafeLib.Mobile.Commands
{
    /// <summary>
    /// ICommand adapter interface.
    /// </summary>
    public interface IXamCommand : ICommand
    {
        void ChangeCanExecute();
    }

    public interface IXamCommand<T> : IXamCommand
    {
    }
}