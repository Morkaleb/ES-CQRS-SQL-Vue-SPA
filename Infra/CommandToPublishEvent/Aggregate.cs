using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ops.Infra.EventStore;

namespace Ops.Infra.CommandToPublishEvent
{
    public abstract class Aggregate
    { 
        public abstract void Hydrate(EventFromES evt);
        public abstract Events[] Execute(Commands cmd);
    }
}
