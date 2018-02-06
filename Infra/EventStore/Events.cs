using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ops.Infra.EventStore
{
    public class Events
    {
        public string StreamId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
