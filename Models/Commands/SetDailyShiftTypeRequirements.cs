using System.Collections.Generic;
using Ops.Infra;

namespace Ops.Models.commands
{
    public class SetDailyShiftTypeRequirements : Commands
    {
        public string GmId { get; set; }
        public List<string> ShiftTypes { get; set; }
        public string DayOfTheWeek { get; set; }
    }
}
