using Ops.Infra;

namespace Ops.Models.commands
{
    public class RequestChangeManagerSchedule : Commands
    {
        public string RequestId { get; set; }
        public string ManagerId { get; set; }
        public string ManagerFromId { get; set; }
        public string EOW { get; set; }
        public string ShiftCode { get; set; }
        public string Reason { get; set; }
        public string ShiftDate { get; set; }
        public string LocationId { get; set; }
        public string GMEmailAddress { get; set; }
        public string ManagerEmailAddress { get; set; }
        public string ManagerToName { get; set; }
        public string ManagerFromName { get; set; }
        public string RequestingManagerRole { get; set; }
    }
}
