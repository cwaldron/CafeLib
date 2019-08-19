﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CafeLib.Core.Eventing;
using CafeLib.Core.IoC;
using CafeLib.Mobile.Commands;
using CafeLib.Mobile.Extensions;
using CafeLib.Mobile.Services;
using Xamarin.Forms;
// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.ViewModels
{
    public abstract class BaseViewModel : ObservableBase
    {
        private readonly List<Guid> _onAppearingSubscribers;
        private readonly List<Guid> _onLoadSubscribers;

        //private readonly List<Guid> _subscriberHandles;
        protected readonly Func<ICommand, Task> ExecuteCommand; 

        protected enum LifecycleState { Load, Appearing, Disappearing, Unload }

        /// <summary>
        /// BaseViewModel constructor.
        /// </summary>
        protected BaseViewModel()
        {
            _onAppearingSubscribers = new List<Guid>();
            _onLoadSubscribers = new List<Guid>();
            Resolver = Application.Current.Resolve<IServiceResolver>();
            AppearingCommand = new Command(() => { });
            DisappearingCommand = new Command(() => { });
            CloseCommand = new Command(() => Close());
            FocusCommand = new Command(() => { });
            LoadCommand = new Command(() => { });
            UnloadCommand = new Command(() => { });
            BackButtonPressed = new XamCommand<NavigationSource, bool>(x =>
            {
                Close();
                return true;
            });

            ExecuteCommand = async command =>
            {
                switch (command)
                {
                    case IXamAsyncCommand a:
                        await a.ExecuteAsync();
                        break;

                    default:
                        command?.Execute(null);
                        break;
                }
            };
        }

        /// <summary>
        /// Initialize the view model.
        /// </summary>
        public virtual async Task InitAsync()
        {
            await Task.CompletedTask;
        }

        protected LifecycleState Lifecycle { get; private set; }

        /// <summary>
        /// Service resolver.
        /// </summary>
        protected IServiceResolver Resolver { get; }

        /// <summary>
        /// Page service.
        /// </summary>
        protected IPageService PageService => Resolver.Resolve<IPageService>();

        /// <summary>
        /// Navigation service.
        /// </summary>
        protected INavigationService NavigationService => Resolver.Resolve<INavigationService>();

        /// <summary>
        /// Navigation Service
        /// </summary>
        protected IDeviceService DeviceService => Resolver.Resolve<IDeviceService>();

        /// <summary>
        /// Navigation Service
        /// </summary>
        protected IEventService EventService => Resolver.Resolve<IEventService>();

        /// <summary>
        /// Resolve the associated page.
        /// </summary>
        protected Page Page => PageService.ResolvePage(this);

        /// <summary>
        /// Appearing command.
        /// </summary>
        private ICommand _appearingCommand;
        public ICommand AppearingCommand
        {
            get => _appearingCommand;
            set
            {
                _appearingCommand = new XamAsyncCommand(async () =>
                {
                    Lifecycle = LifecycleState.Appearing;
                    IsVisible = true;
                    ReleaseSubscribers();
                    AddSubscribers();

                    try
                    {
                        IsEnabled = false;
                        await ExecuteCommand(value);
                    }
                    finally
                    {
                        IsEnabled = true;
                    }
                });
            }
        }

        /// <summary>
        /// Disappearing command.
        /// </summary>
        private ICommand _disappearingCommand;
        public ICommand DisappearingCommand
        {
            get => _disappearingCommand;
            set
            {
                _disappearingCommand = new XamAsyncCommand(async () =>
                {
                    try
                    {
                        Lifecycle = LifecycleState.Disappearing;
                        await ExecuteCommand(value);
                    }
                    finally
                    {
                        ReleaseSubscribers();
                        IsVisible = false;
                    }
                });
            }
        }

        /// <summary>
        /// Load command.
        /// </summary>
        private ICommand _loadCommand;
        public ICommand LoadCommand
        {
            get => _loadCommand;
            set
            {
                _loadCommand = new XamAsyncCommand(async () =>
                {
                    Lifecycle = LifecycleState.Load;
                    ReleaseSubscribers();
                    await ExecuteCommand(value);
                });
            }
        }

        /// <summary>
        /// Load command.
        /// </summary>
        private ICommand _unloadCommand;
        public ICommand UnloadCommand
        {
            get => _unloadCommand;
            set
            {
                _unloadCommand = new XamAsyncCommand(async () =>
                {
                    try
                    {
                        Lifecycle = LifecycleState.Disappearing;
                        await ExecuteCommand(value);
                    }
                    finally
                    {
                        ReleaseSubscribers();
                    }
                });
            }
        }

        /// <summary>
        /// Back button pressed handler.
        /// </summary>
        public IXamCommand<NavigationSource, bool> BackButtonPressed { get; set; }

        /// <summary>
        /// Close command.
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// Focus command.
        /// </summary>
        public ICommand FocusCommand { get; set; }

        /// <summary>
        /// Title.
        /// </summary>
        private string _title;
        public string Title
        {
            get => _title;
            set => SetValue(ref _title, value);
        }

        /// <summary>
        /// Determines whether input is permitted.
        /// </summary>
        private bool _isEnabled;

        public virtual bool IsEnabled
        {
            get => _isEnabled;
            set => SetValue(ref _isEnabled, value);
        }

        /// <summary>
        /// Determines visibility of the view model
        /// </summary>
        private bool _isVisible;
        public virtual bool IsVisible
        {
            get => _isVisible;
            set => SetValue(ref _isVisible, value);
        }

        /// <summary>
        /// Add event message subscribers.
        /// </summary>
        protected virtual void AddSubscribers()
        {
        }

        /// <summary>
        /// Release event message subscribers.
        /// </summary>
        protected virtual void ReleaseSubscribers()
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (Lifecycle)
            {
                case LifecycleState.Appearing:
                case LifecycleState.Disappearing:
                    _onAppearingSubscribers.ForEach(x => EventService.Unsubscribe(x));
                    _onAppearingSubscribers.Clear();
                    break;

                case LifecycleState.Load:
                case LifecycleState.Unload:
                    _onLoadSubscribers.ForEach(x => EventService.Unsubscribe(x));
                    _onLoadSubscribers.Clear();
                    break;
            }
        }

        /// <summary>
        /// Publish an event message.
        /// </summary>
        /// <typeparam name="T">event message type</typeparam>
        /// <param name="message">event message</param>
        protected void PublishEvent<T>(T message) where T : IEventMessage
        {
            EventService.Publish(message);
        }

        /// <summary>
        /// Subscribe an action to an event message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        protected void SubscribeEvent<T>(Action<T> action) where T : IEventMessage
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (Lifecycle)
            {
                case LifecycleState.Appearing:
                    _onAppearingSubscribers.Add(EventService.SubscribeOnMainThread(action));
                    break;

                case LifecycleState.Load:
                    _onLoadSubscribers.Add(EventService.SubscribeOnMainThread(action));
                    break;
            }
        }

        /// <summary>
        /// Resolves viewmodel to is associated page.
        /// </summary>
        /// <returns>bounded page</returns>
        internal Page ResolvePage() => PageService.ResolvePage(this);

        /// <summary>
        /// Establish view model as the application navigator.
        /// </summary>
        /// <returns></returns>
        public NavigationPage AsNavigator()
        {
            return NavigationService.SetNavigator(this);
        }

        /// <summary>
        /// Close the view model.
        /// </summary>
        /// <param name="animate"></param>
        public virtual void Close(bool animate = false)
        {
            Page.Navigation.Close(this, animate);
        }

        /// <summary>
        /// Displays an alert on the page.
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="message">message</param>
        /// <param name="ok">OK</param>
        public Task DisplayAlert(string title, string message, string ok = "OK")
        {
            return Application.Current.AlertDialog(title, message, ok);
        }

        /// <summary>
        /// Displays an alert (simple question) on the page.
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="message">message</param>
        /// <param name="ok">OK</param>
        /// <param name="cancel">cancel</param>
        public Task<bool> DisplayConfirm(string title, string message, string ok = "OK", string cancel = "Cancel")
        {
            return Application.Current.ConfirmDialog(title, message, ok, cancel);
        }

        /// <summary>
        /// Displays an action sheet (list of buttons) on the page, asking for user input.
        /// </summary>
        /// <param name="title">dialog title</param>
        /// <param name="cancel">cancellation string</param>
        /// <param name="destroy">destroy string</param>
        /// <param name="options">option list</param>
        /// <returns></returns>
        public Task<string> DisplayOptions(string title, string cancel, string destroy, IEnumerable<string> options)
        {
            return Application.Current.OptionsDialog(title, cancel, destroy, options);
        }
    }

    public class BaseViewModel<TParameter> : BaseViewModel where TParameter : class
    {
        /// <summary>
        /// Initialize and pass parameter to the view model.
        /// </summary>
        /// <param name="parameter">parameter passed to view model</param>
        public virtual async Task InitAsync(TParameter parameter)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Close the view model.
        /// </summary>
        /// <param name="parameter">parameter to forward</param>
        /// <param name="animate">animation flag</param>
        public virtual void Close(TParameter parameter, bool animate = false)
        {
            Page.Navigation.Close(this, parameter, animate);
        }
    }
}
