using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ops.Infra.EventStore
{
    public class PersistentSubscriptionClient
    {
        private IEventStoreConnection _conn;

        private const int DEFAULTPORT = 1113;
        private static readonly UserCredentials User = new UserCredentials("admin", "changeit");
        private EventStoreSubscription _subscription;

        public void Start()
        {
            //uncommet to enable verbose logging in client.
            var settings = ConnectionSettings.Create(); //.EnableVerboseLogging().UseConsoleLogger();
            settings.SetHeartbeatTimeout(new TimeSpan(0, 0, 1));

            _conn = EventStoreConnection.Create(settings, new IPEndPoint(IPAddress.Loopback, DEFAULTPORT));
            CreateSubscription();
            _conn.ConnectAsync().Wait();
            ConnectToSubscription();
        }


        private async void ConnectToSubscription()
        {
            try
            {
                _subscription = await _conn.SubscribeToAllAsync(false, EventAppeared, SubscriptionDropped, User);
            }
            catch
            {
                _conn.ConnectAsync().Wait();
                _subscription = await _conn.SubscribeToAllAsync(false, EventAppeared, SubscriptionDropped, User);
            }
        }

        private void SubscriptionDropped(EventStoreSubscription eventStorePersistentSubscriptionBase,
            SubscriptionDropReason subscriptionDropReason, Exception ex)
        {
            ConnectToSubscription();
        }

        private static Task EventAppeared(EventStoreSubscription eventStorePersistentSubscriptionBase,
            ResolvedEvent resolvedEvent)
        {
            var data = Encoding.ASCII.GetString(resolvedEvent.Event.Data);
            if (resolvedEvent.Event.EventType[0] != '$')
            {
                return EventStoreInterface.publishToReadModel(resolvedEvent);
            }
            else return Task.FromResult(0);

        }
        private void CreateSubscription()
        {
            PersistentSubscriptionSettings settings = PersistentSubscriptionSettings.Create()
                .DoNotResolveLinkTos()
                .StartFromCurrent();

            try
            {
                _conn.SubscribeToAllAsync(false, EventAppeared, SubscriptionDropped, User).Wait();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException.GetType() != typeof(InvalidOperationException)
                    && ex.InnerException?.Message != $"Subscription group on stream already exists")
                {
                    throw;
                }
            }
        }
    }
}
