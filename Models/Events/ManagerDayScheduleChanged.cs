using Ops.Infra.EventStore;

namespace Ops.Models.events
{
    public class ManagerDayScheduleChanged : Events
    {
        public string Reason { get; set; }
        public string RequestId { get; set; }
        public string ManagerId { get; set; }
        public string ShiftCode { get; set; }
        public string ShiftDate { get; set; }
        public string LocationId { get; set; }
        public string ManagerToName { get; set; }
        public string EOW { get; set; }

    }
}
