using System.Collections.Generic;
using Ops.Infra;
using Ops.ReadModels;

namespace Ops.Models.commands
{
    public class GMApproveSchedule : Ops.Infra.Commands
    {
        public string RestaurantId { get; set; }
        public string GMId { get; set; }
        public string EOW { get; set; }
        public List<ManagerTableData> Managers { get; set; }
    }
}
