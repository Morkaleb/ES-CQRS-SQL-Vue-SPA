using Ops.Infra;

namespace Ops.Models.commands
{
    public class GMRejectSchedule : Ops.Infra.Commands
    {
        public string LocationId { get; set; }
        public string EOW { get; set; }
    }
}
