using System;
using System.Threading.Tasks;
using System.Windows.Input;

// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Commands
{
    /// <summary>
    /// Xamarin command adapter.
    /// </summary>
    public class XamAsyncCommand : XamAsyncCommand<object>, IXamAsyncCommand
    {
        private static readonly object Parameter = new object();

        /// <summary>
        /// XamCommand constructor.
        /// </summary>
        /// <param name="action">The action to run when the command executes.</param>
        public XamAsyncCommand(Func<Task> action)
            : base(p => action())
        {
        }

        /// <summary>
        /// XamCommand constructor.
        /// </summary>
        /// <param name="action">The action to run when the command executes.</param>
        /// <param name="canExecute">The routine determining the execution state of the command.</param>
        public XamAsyncCommand(Func<Task> action, Func<bool> canExecute)
            : base(p => action(), p => canExecute())
        {
        }

        public Task ExecuteAsync()
        {
            return ExecuteAsync(Parameter);
        }

        public bool CanExecute()
        {
            return CanExecute(Parameter);
        }
    }

    /// <summary>
    /// Xamarin command generic adapter.
    /// </summary>
    public class XamAsyncCommand<T> : IXamAsyncCommand<T>
    {
        private bool _isBusy;
        private readonly Func<T, Task> _action;
        private readonly Func<T, bool> _canExecute;

        public XamAsyncCommand(Func<T, Task> action, Func<T, bool> canExecute = null)
        {
            _isBusy = false;
            _action = action;
            _canExecute = canExecute;
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((T) parameter);
        }

        void ICommand.Execute(object parameter)
        {
            ExecuteAsync((T) parameter).Wait();
        }

        public event EventHandler CanExecuteChanged;

        public void ChangeCanExecute()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task ExecuteAsync(T parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    _isBusy = true;
                    await _action(parameter);
                }
                finally
                {
                    _isBusy = false;
                }
            }

            ChangeCanExecute();
        }

        public bool CanExecute(T parameter)
        {
            return !_isBusy && (_canExecute?.Invoke(parameter) ?? true);
        }
    }
}
