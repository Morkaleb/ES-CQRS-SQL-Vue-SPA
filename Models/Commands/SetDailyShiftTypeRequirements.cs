using System.Collections.Generic;
using Ops.Infra;

namespace Ops.Models.commands
{
    public class SetDailyShiftTypeRequirements : Commands
    {
        public string GmId { get; set; }
        public List<string> Monday { get; set; }
        public List<string> Tuesday { get; set; }
        public List<string> Wednesday { get; set; }
        public List<string> Thursday { get; set; }
        public List<string> Friday { get; set; }
        public List<string> Saturday { get; set; }
        public List<string> Sunday { get; set; }
    }
}

