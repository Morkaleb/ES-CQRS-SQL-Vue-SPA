using Ops.Infra.EventStore;

namespace Ops.Models.events
{
    public class PayRollApprovedScheduleChange : Events
    {
        public string Id { get; set; }
        public string RequestId { get; set; }
    }
}
