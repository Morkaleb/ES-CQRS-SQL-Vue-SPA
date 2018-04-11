using Ops.Infra.EventStore;

namespace Ops.Models.events
{
    public class ShiftCodeCreated : Ops.Infra.EventStore.Events
    {
        public string ShiftCode { get; set; }
        public string ShiftType { get; set; }
        public int ShiftStatus { get; set; }
        public int DaysOwed { get; set; }
    }
}
