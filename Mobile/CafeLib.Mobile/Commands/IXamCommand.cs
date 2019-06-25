using System.Windows.Input;

namespace CafeLib.Mobile.Commands
{
    /// <summary>
    /// ICommand adapter interface.
    /// </summary>
    public interface IXamCommand : ICommand
    {
        void ChangeCanExecute();
    }

    public interface IXamCommand<in T> : IXamCommand
    {
    }

    public interface IXamCommand<in TParameter, out TResult> : IXamCommand<TParameter>
    {
        TResult Execute(TParameter parameter);
    }
}