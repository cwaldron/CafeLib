using System.Threading.Tasks;

namespace CafeLib.Mobile.Commands
{
    public interface IXamAsyncCommand : IXamCommand
    {
        Task ExecuteAsync();
        bool CanExecute();
    }

    public interface IXamAsyncCommand<in T> : IXamCommand
    {
        Task ExecuteAsync(T parameter);

        bool CanExecute(T parameter);
    }
}
