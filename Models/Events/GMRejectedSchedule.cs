using Ops.Infra.EventStore;

namespace Ops.Models.events
{
    public class GMRejectedSchedule : Ops.Infra.EventStore.Events
    {
        public string LocationId { get; set; }
        public string EOW { get; set; }
    }
}
