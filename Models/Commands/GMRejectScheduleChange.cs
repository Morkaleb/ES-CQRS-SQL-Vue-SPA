using Ops.Infra;
using Ops.ReadModels;

namespace Ops.Models.commands
{
    public class GMRejectScheduleChange : Ops.Infra.Commands
    {
        public string GMId { get; set; }
        public string RequestId { get; set; }
        public string Reason { get; set; }
        public ManagerTableData ManagerFrom { get; set; }
        public ManagerTableData GM { get; set; }
        public ManagerTableData ManagerTo { get; set; }
        public ChangeRequestsTableData OrigionalRequest { get; set; }
    }
}
