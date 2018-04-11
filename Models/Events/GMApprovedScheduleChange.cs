using Ops.Infra.EventStore;

namespace Ops.Models.events
{
    public class GMApprovedScheduleChange : Ops.Infra.EventStore.Events
    {
        public string ManagerId { get; set; }
        public string GMId { get; set; }
        public string RequestId { get; set; }
    }
}
