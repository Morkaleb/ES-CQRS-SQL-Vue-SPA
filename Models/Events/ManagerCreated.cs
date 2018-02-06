using Ops.Infra.EventStore;

namespace Ops.Models.events
{
    public class ManagerCreated : Events
    {
        public string ManagerId { get; set;}
        public string Name { get; set; }
        public int Role { get; set; }
        public double VacationRate { get; set; }
        public double VacationDaysOwed { get; set; }
        public string EmailAddress { get; set; }
        public int LocationId { get; set; }
    }
}
