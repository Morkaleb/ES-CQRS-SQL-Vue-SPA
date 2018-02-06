using Ops.Infra;

namespace Ops.Models.commands
{
    public class GMRejectSchedule : Commands
    {
        public string LocationId { get; set; }
        public string EOW { get; set; }
    }
}
