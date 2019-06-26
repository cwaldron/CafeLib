using System;
using System.Collections.Concurrent;
using CafeLib.Core.Eventing;
using CafeLib.Core.Extensions;

namespace CafeLib.Core.IoC
{
    internal class EventService : IEventService
    {
        /// <summary>
        /// This map contains the event Message type key and a collection of subscribers associated with the message type.
        /// </summary>
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Guid, object>> _subscriptions;

        /// <summary>
        /// This map provides type lookup.
        /// </summary>
        private readonly ConcurrentDictionary<Guid, Type> _lookup;

        /// <summary>
        /// Synchronizes access to internal tables.
        /// </summary>
        private static readonly object Mutex = new object();

        /// <summary>
        /// EventBus constructor.
        /// </summary>
        public EventService()
        {
            _subscriptions = new ConcurrentDictionary<Type, ConcurrentDictionary<Guid, object>>();
            _lookup = new ConcurrentDictionary<Guid, Type>();
        }

        /// <summary>
        /// Subscribe the specified handler.
        /// </summary>
        /// <param name='action'>
        /// Event action.
        /// </param>
        /// <typeparam name='T'>
        /// Type of IEventMessage.
        /// </typeparam>
        public Guid Subscribe<T>(Action<T> action) where T : IEventMessage
        {
            lock (Mutex)
            {
                var subscribers = _subscriptions.GetOrAdd(typeof(T), new ConcurrentDictionary<Guid, object>());
                var key = Guid.NewGuid();
                subscribers.TryAdd(key, action);
                _lookup.TryAdd(key, typeof(T));
                return key;
            }
        }

        /// <summary>
        /// Publish the specified message.
        /// </summary>
        /// <param name='message'>
        /// Message.
        /// </param>
        /// <typeparam name='T'>
        /// Type of IEventMessage.
        /// </typeparam>
        public void Publish<T>(T message) where T : IEventMessage
        {
            ConcurrentDictionary<Guid, object> subscribers;
            lock (Mutex)
            {
                if (!_subscriptions.ContainsKey(typeof(T))) return;
                subscribers = _subscriptions[typeof(T)];
            }
            subscribers.ForEach(x => ((Action<T>)x.Value)?.Invoke(message));
        }

        /// <summary>
        /// Unsubscribe all specified handlers of type T.
        /// </summary>
        /// <typeparam name='T'>
        /// Type of IEventMessage.
        /// </typeparam>
        public void Unsubscribe<T>() where T : IEventMessage
        {
            lock (Mutex)
            {
                if (!_subscriptions.ContainsKey(typeof(T))) return;
                var subscribers = _subscriptions[typeof(T)];
                subscribers.ForEach(x =>
                {
                    subscribers.TryRemove(x.Key, out _);
                    _lookup.TryRemove(x.Key, out _);
                });
                _subscriptions.TryRemove(typeof(T), out _);
            }
        }

        /// <summary>
        /// Unsubscribe the specified handler of type T and Guid identifier.
        /// </summary>
        /// <param name="subscriberId">subscriber identifier</param>
        /// <typeparam name='T'>
        /// Type of IEventMessage.
        /// </typeparam>
        public void Unsubscribe<T>(Guid subscriberId) where T : IEventMessage
        {
            Unsubscribe(subscriberId);
        }

        /// <summary>
        /// Unsubscribe the specified handler using subscriber identifier.
        /// </summary>
        /// <param name="subscriberId">subscriber identifier</param>
        public void Unsubscribe(Guid subscriberId)
        {
            lock (Mutex)
            {
                if (!_lookup.ContainsKey(subscriberId)) return;
                var subscriberType = _lookup[subscriberId];

                var subscribers = _subscriptions[subscriberType];
                subscribers.TryRemove(subscriberId, out _);
                _lookup.TryRemove(subscriberId, out _);
                if (subscribers.Count == 0)
                {
                    _subscriptions.TryRemove(subscriberType, out _);
                }
            }
        }
    }
}
