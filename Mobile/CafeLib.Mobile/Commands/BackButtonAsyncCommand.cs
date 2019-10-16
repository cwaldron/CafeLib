using System;
using System.Threading.Tasks;
using CafeLib.Mobile.ViewModels;

namespace CafeLib.Mobile.Commands
{
    public class BackButtonAsyncCommand : XamAsyncCommand<NavigationSource, bool>
    {
        /// <summary>
        /// Back button command constructor
        /// </summary>
        /// <param name="command">back button command</param>
        public BackButtonAsyncCommand(Action<NavigationSource> command)
            : base(p => { command(p); return Task.FromResult(true); })
        {
        }

        /// <summary>
        /// Back button command constructor
        /// </summary>
        /// <param name="command">back button command</param>
        /// <param name="canExecute">The routine determining the execution state of the command.</param>
        public BackButtonAsyncCommand(Action<NavigationSource> command, Action<NavigationSource> canExecute)
            : base(p => { command(p); return Task.FromResult(true); }, p => { canExecute(p); return true; })
        {
        }
    }
}
