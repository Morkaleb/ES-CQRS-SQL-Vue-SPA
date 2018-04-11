using Ops.Infra.EventStore;

namespace Ops.Models.events
{
    public class PayrollApprovalRequiredForShiftChange : Ops.Infra.EventStore.Events
    {
        public string Id { get; set; }
        public string RequestId { get; set; }
    }
}
