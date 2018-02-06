using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ops.Infra.EventStore;

namespace Ops.Infra.ReadModels
{
    public abstract class ReadModel
    {
        public abstract dynamic EventPublish(EventFromES anEvent);
    }

    public static class ReadModelRegistration
    {
        public static void Register()
        {
            EventDistributor.Publish(new EventFromES { });
        }
    }
}
