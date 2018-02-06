using System.Collections.Generic;
using Ops.Infra.EventStore;

namespace Ops.Models.events
{
    public class DailyShiftTypeRequirementsSet : Events
    {
        public string Id { get; set; }
        public string ManagerType { get; set; }
        public string GmId { get; set; }
        public string DayOfTheWeek { get; set; }

        public List<string> ShiftTypes { get; set; }
    }
}
