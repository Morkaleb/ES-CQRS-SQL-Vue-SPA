using Ops.Infra;
using Ops.ReadModels;

namespace Ops.Models.commands
{
    public class PayrollApproveScheduleChange : Ops.Infra.Commands
    {
        public ChangeRequestsTableData Request { get; set; }
    }
}
