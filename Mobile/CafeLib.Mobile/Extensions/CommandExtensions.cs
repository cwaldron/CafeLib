using System.Threading.Tasks;
using System.Windows.Input;
using CafeLib.Mobile.Commands;

namespace CafeLib.Mobile.Extensions
{
    public static class CommandExtensions
    {
        /// <summary>
        /// Execute command asynchronously.
        /// </summary>
        /// <param name="command">command</param>
        /// <returns>task</returns>
        public static Task ExecuteAsync(this ICommand command)
        {
            return ((XamAsyncCommand) command).ExecuteAsync();
        }

        /// <summary>
        /// Execute command asynchronously.
        /// </summary>
        /// <typeparam name="T">parameter type</typeparam>
        /// <param name="command">command</param>
        /// <param name="parameter">command parameter</param>
        /// <returns>task</returns>
        public static Task ExecuteAsync<T>(this ICommand command, T parameter)
        {
            return ((XamAsyncCommand<T>)command).ExecuteAsync(parameter);
        }
    }
}
