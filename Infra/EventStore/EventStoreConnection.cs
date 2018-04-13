using EventStore.ClientAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ops.Infra.EventStore
{
    public class EventStoreConnection
    {
        static string connectionString = "ConnectTo=tcp://admin:changeit@localhost:1113; HeartBeatTimeout=500";
        public static IEventStoreConnection connection = global::EventStore.ClientAPI.EventStoreConnection.Create(connectionString);
        public static PersistentSubscriptionClient subscription = new PersistentSubscriptionClient();

        public static void StartConnection()
        {
            connection.ConnectAsync();
            subscription.Start();
        }

        public static void PublishToEventStore(Events evt)
        {
            var body = JsonConvert.SerializeObject(evt);
            var metaData = JsonConvert.SerializeObject(evt.TimeStamp);
            string eventType = evt.GetType().ToString().Split('.').Last();
            var eventToPublish = new EventData(Guid.NewGuid(), eventType, true,
                                                 Encoding.UTF8.GetBytes(body),
                                                 Encoding.UTF8.GetBytes(metaData));
            connection.AppendToStreamAsync(evt.StreamId, ExpectedVersion.Any, eventToPublish);
        }

        public static void ReadSavedEvents()
        {
            var allEvents = new List<ResolvedEvent>();
            var nonStatEvents = new List<ResolvedEvent>();
            AllEventsSlice currentSlice;
            var nextSliceStart = Position.Start;
            do
            {
                currentSlice =
                 connection.ReadAllEventsForwardAsync(nextSliceStart, 200, false).Result;

                nextSliceStart = currentSlice.NextPosition;

                allEvents.AddRange(currentSlice.Events);


            } while (!currentSlice.IsEndOfStream);

            foreach (var resolvedEvent in allEvents)
            {
                if (resolvedEvent.OriginalStreamId != "user-admin" &&
                    resolvedEvent.OriginalStreamId[0] != '$')
                {
                    nonStatEvents.Add(resolvedEvent);
                }

            }

            distrbuteSavedEvents(nonStatEvents);
        }

        private static void distrbuteSavedEvents(List<ResolvedEvent> nonStatEvents)
        {
            List<object> aslk = new List<object>();
            foreach (var savedEvent in nonStatEvents)
            {
                string StringyfiedBytes = System.Text.Encoding.UTF8.GetString(savedEvent.Event.Data);
                try
                {
                    var eventFromES = NormalizeESEvent(savedEvent);
                    EventDistributor.Publish(eventFromES);
                }
                catch (Exception e)
                {
                    Console.Write(e);
                }
            }
        }

        public static List<EventFromES> HydrateFromES(string streamId)
        {
            var eventStream = connection.ReadStreamEventsForwardAsync(streamId, 0, 256, false).Result.Events;
            List<EventFromES> theWetList = new List<EventFromES>();
            foreach (var eS_Event in eventStream)
            {
                EventFromES normalized = NormalizeESEvent(eS_Event);
                theWetList.Add(normalized);
            }
            return theWetList;
        }

        private static EventFromES NormalizeESEvent(ResolvedEvent eS_Event)
        {
            string eventData = System.Text.Encoding.UTF8.GetString(eS_Event.Event.Data);
            dynamic body = JsonConvert.DeserializeObject<dynamic>(eventData);
            EventFromES fromES = new EventFromES
            {
                StreamId = eS_Event.OriginalStreamId,
                Id = eS_Event.OriginalStreamId,
                Data = body,
                EventType = eS_Event.Event.EventType,
                TimeStamp = eS_Event.Event.Created.ToString()
            };
            return fromES;
        }

        public static Task publishToReadModel(ResolvedEvent eS_Event)
        {
            EventFromES normal = NormalizeESEvent(eS_Event);
            EventDistributor.Publish(normal);
            return Task.FromResult(0);
        }
    }

    public class EventFromES
    {
        public string Id { get; set; }
        public string TimeStamp { get; set; }
        public string StreamId { get; set; }
        public string EventType { get; set; }
        public dynamic Data { get; set; }
    }
}
