using System;
using System.Collections.Generic;
using CafeLib.Core.Eventing;
using CafeLib.Core.IoC;
using CafeLib.Mobile.Extensions;
using CafeLib.Mobile.Services;
using CafeLib.Mobile.Test.Core.Support;
using Microsoft.Extensions.Logging;
using Moq;
using Xamarin.Forms;
// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Test.Core
{
    public abstract class MobileUnitTest
    {
        protected List<Guid> Subscribers;

        protected Mock<INavigationService> NavigationService;
        protected Mock<IPageService> PageService;
        protected Mock<IDeviceService> DeviceService;

        protected IServiceRegistry Registry { get; private set; }

        protected IServiceResolver Resolver => Registry.GetResolver();

        protected MockApplication App { get; private set; }

        public void Initialize()
        {
            SetupTest();
            CreateApplication();
            InitTest();
        }

        public void CreateApplication()
        {
            Subscribers = new List<Guid>();
            Device.PlatformServices = new MockPlatformServices();
            Device.Info = new MockDeviceInfo();
            App = new MockApplication(Registry);
        }

        public void Terminate()
        {
            Subscribers.ForEach(x => Resolver.Resolve<IEventService>().Unsubscribe(x));
            Subscribers.Clear();
            Resolver.Dispose();
        }

        /// <summary>
        /// Publish an event message.
        /// </summary>
        /// <typeparam name="T">event message type</typeparam>
        /// <param name="message">event message</param>
        protected void PublishEvent<T>(T message) where T : IEventMessage
        {
            Resolver.Resolve<IEventService>().Publish(message);
        }

        /// <summary>
        /// Subscribe an action to an event message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        protected void SubscribeEvent<T>(Action<T> action) where T : IEventMessage
        {
            Subscribers.Add(Resolver.Resolve<IEventService>().SubscribeOnMainThread(action));
        }

        /// <summary>
        /// Test entry point.
        /// </summary>
        protected virtual void InitTest()
        {
        }

        /// <summary>
        /// Setup test.
        /// </summary>
        protected virtual void SetupTest()
        {
            // fake set up of the IoC
            SetupRegistry();

            // Setup for all tests.
            NavigationService = new Mock<INavigationService>();
            PageService = new Mock<IPageService>();

            Registry
                .AddSingleton<IDeviceService, MockDeviceService>()
                .AddSingleton(x => NavigationService.Object)
                .AddSingleton(x => PageService.Object);
        }

        /// <summary>
        /// Setup the service registry.
        /// </summary>
        private void SetupRegistry()
        {
            Registry = IocFactory.CreateRegistry();
            Registry
                .AddLogging(builder => builder.AddConsole().AddDebug())
                .AddEventService();
        }
    }
}