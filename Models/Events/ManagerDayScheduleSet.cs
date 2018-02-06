using Ops.Infra.EventStore;

namespace Ops.Models.events
{
    public class ManagerDayScheduleSet : Events
    {
        public int LocationId { get; set; }
        public string ShiftCode { get; set; }
        public string ManagerId { get; set; }
        public string EOW { get; set; }
        public string Day { get; set; }
    }
}
