﻿using System;
using System.Collections.Generic;
using System.Text;
using CafeLib.Core.Eventing;
using CafeLib.Core.IoC;
using CafeLib.Mobile.Services;
using Xamarin.Forms;
// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Extensions
{
    public static class EventServiceExtensions
    {
        /// <summary>
        /// Subscribe to event message with action to run on the main thread
        /// </summary>
        /// <typeparam name="T">event message type</typeparam>
        /// <param name="eventService"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static Guid SubscribeOnMainThread<T>(this IEventService eventService, Action<T> action) where T : IEventMessage
        {
            return eventService.Subscribe<T>(x => Application.Current.Resolve<IDeviceService>().RunOnMainThread(() => action(x)));
        }
    }
}