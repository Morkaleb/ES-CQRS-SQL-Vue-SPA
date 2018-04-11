using Ops.Infra;
using Ops.ReadModels;

namespace Ops.Models.commands
{
    public class GMApproveScheduleChange : Ops.Infra.Commands
    {
        public string LocationId { get; set; }
        public string ManagerId { get; set; }
        public string EOW { get; set; }
        public string GMId { get; set; }
        public ChangeRequestsTableData Request { get; set; }
    }
}
