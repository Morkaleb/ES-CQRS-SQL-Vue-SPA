using Ops.Infra.EventStore;

namespace Ops.Models.events
{
    public class ShiftCodeCreated : Events
    {
        public string ShiftCode { get; set; }
        public string ShiftType { get; set; }
        public int DaysOwed { get; set; }
    }
}
