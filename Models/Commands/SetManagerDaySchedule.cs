using Ops.Infra;

namespace Ops.Models.commands
{
    public class SetManagerDaySchedule : Commands
    {
        public string ManagerId { get; set; }
        public int LocationId { get; set; }
        public string ShiftCode { get; set; }
        public string EOW { get; set; }
        public string Day { get; set; }
    }
}
