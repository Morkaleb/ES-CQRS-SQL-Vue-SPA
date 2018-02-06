using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ops.Infra.EventStore;

namespace Ops.Infra.CommandToPublishEvent
{
    public class CommandHandler
    {
        public static void ActivateCommand(Commands cmd, Aggregate aggregate)
        {
            string[] aggregateType = aggregate.GetType().ToString().Split('.');
            string aggregateId = aggregateType[2] + "." + cmd.Id;
            List<EventFromES> eventStream = new List<EventFromES>();
            try
            {
                eventStream = EventStoreInterface.HydrateFromES(aggregateId);
            }
            catch (Exception e) { Console.Write(e); }
            if (eventStream.Count > 0)
            {
                foreach (var evt in eventStream)
                {
                    aggregate.Hydrate(evt);
                }

            }
            Events[] toComplete = aggregate.Execute(cmd);
            foreach (var evt in toComplete)
            {
                evt.StreamId = aggregateId;
                evt.TimeStamp = DateTime.Now;
                EventStoreInterface.PublishToEventStore(evt);

            }
        }
    }
}

