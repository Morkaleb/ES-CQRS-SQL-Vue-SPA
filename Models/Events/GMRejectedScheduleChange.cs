using Ops.Infra.EventStore;

namespace Ops.Models.events
{
    public class GMRejectedScheduleChange : Ops.Infra.EventStore.Events
    {
        public string GMId { get; set; }
        public string RequestId { get; set; }
        public string Reason { get; set; }
        public string OrigionalManagerName { get; set; }
        public string OriginalManagerId { get; set; }
        
    }
}
