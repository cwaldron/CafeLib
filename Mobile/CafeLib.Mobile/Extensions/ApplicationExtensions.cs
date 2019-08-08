﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CafeLib.Core.Eventing;
using CafeLib.Mobile.Messages;
using CafeLib.Mobile.Services;
using CafeLib.Mobile.Startup;
using CafeLib.Mobile.ViewModels;
using Xamarin.Forms;
// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Extensions
{
    public static class ApplicationExtensions
    {
        /// <summary>
        /// End the application.
        /// </summary>
        /// <param name="app">application</param>
        public static void EndApplication(this Application app)
        {
            var eventService = Application.Current.Resolve<IEventService>();
            eventService.Publish(new ApplicationExpiryMessage());
            eventService.Unsubscribe<ApplicationExpiryMessage>();
        }

        /// <summary>
        /// Get device service.
        /// </summary>
        /// <param name="app">application</param>
        /// <returns>device service</returns>
        public static IDeviceService GetDeviceService(this Application app)
            => app.Resolve<IDeviceService>();

        /// <summary>
        /// Get event service.
        /// </summary>
        /// <param name="app">application</param>
        /// <returns>device service</returns>
        public static IEventService GetEventService(this Application app)
            => app.Resolve<IEventService>();

        /// <summary>
        /// Get the application's resource.
        /// </summary>
        /// <typeparam name="T">resource type</typeparam>
        /// <param name="app">application</param>
        /// <param name="name">resource name</param>
        /// <returns></returns>
        public static T GetResource<T>(this Application app, string name)
            => (T)app.Resources[name];

        /// <summary>
        /// Get the application service resolver.
        /// </summary>
        /// <param name="app">application</param>
        /// <returns></returns>
        public static T Resolve<T> (this Application app) where T : class
            => (app as CafeApplication)?.Resolver.Resolve<T>();

        /// <summary>
        /// Resolve view model.
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <param name="app">application</param>
        /// <returns></returns>
        public static T ResolveViewModel<T>(this Application app) where T : BaseViewModel
            => app.Resolve<T>();

        /// <summary>
        /// Runs an action on the main thread.
        /// </summary>
        /// <param name="app">application</param>
        /// <param name="action">action</param>
        public static void RunOnMainThread(this Application app, Action action)
            => app.GetDeviceService().RunOnMainThread(action);

        /// <summary>
        /// Start application on view model.
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <param name="app">application</param>
        public static void StartOnViewModel<T>(this Application app) where T : BaseViewModel
        {
            var vm = app.ResolveViewModel<T>();
            app.MainPage = vm.AsNavigator();
            app.GetDeviceService().RunOnMainThread(async () => await vm.InitAsync());
        }

        /// <summary>
        /// Start application on view model.
        /// </summary>
        /// <typeparam name="T">view model type</typeparam>
        /// <typeparam name="TP">view model parameter type</typeparam>
        /// <param name="parameter">view model parameter</param>
        /// <param name="app">application</param>
        public static void StartOnViewModel<T, TP>(this Application app, TP parameter) where T : BaseViewModel<TP> where TP : class
        {
            var vm = app.ResolveViewModel<T>();
            app.MainPage = vm.AsNavigator();
            app.GetDeviceService().RunOnMainThread(async () => await vm.InitAsync(parameter));
        }

        /// <summary>
        /// Display an alert dialog.
        /// </summary>
        /// <param name="app">application</param>
        /// <param name="title">dialog title</param>
        /// <param name="message">message</param>
        /// <param name="ok"></param>
        public static void AlertDialog(this Application app, string title, string message, string ok = "OK")
            => (app as CafeApplication)?.DisplayAlert(title, message, ok);

        /// <summary>
        /// Display confirmation dialog
        /// </summary>
        /// <param name="app">application</param>
        /// <param name="title">dialog title</param>
        /// <param name="message">message</param>
        /// <param name="ok">OK</param>
        /// <param name="cancel">cancel</param>
        // ReSharper disable once MethodOverloadWithOptionalParameter
        public static Task<bool> ConfirmDialog(this Application app, string title, string message, string ok = "OK", string cancel = "Cancel")
            => (app as CafeApplication)?.DisplayConfirm(title, message, ok, cancel);

        /// <summary>
        /// Display option selection dialog.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="title">dialog title</param>
        /// <param name="cancel">cancel button display</param>
        /// <param name="delete">delete button display</param>
        /// <param name="options">option list</param>
        /// <returns></returns>
        public static Task<string> OptionsDialog(this Application app, string title, string cancel, string delete, IEnumerable<string> options)
            => (app as CafeApplication)?.DisplayOptions(title, cancel, delete, options);
    }
}
