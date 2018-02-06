using Ops.Infra;
using Ops.ReadModels;

namespace Ops.Models.commands
{
    public class PayrollApproveScheduleChange : Commands
    {
        public ChangeRequestsTableData Request { get; set; }
    }
}
