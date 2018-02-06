using Ops.Infra.EventStore;

namespace Ops.Models.events
{
    public class GMApprovedSchedule : Events
    {
        public string GMId { get; set; }
        public string LocationId { get; set; }
        public string EOW { get; set; }
    }
}
