using System;

namespace Ops.Infra.EventStore
{
    public class Events
    {
        public string StreamId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
