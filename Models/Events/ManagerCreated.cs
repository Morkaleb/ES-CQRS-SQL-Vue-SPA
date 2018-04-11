using Ops.Infra.EventStore;

namespace Ops.Models.events
{
    public class ManagerCreated : Ops.Infra.EventStore.Events
    {
        public string ManagerId { get; set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Role { get; set; }
        public double VacationRate { get; set; }
        public double VacationDaysOwed { get; set; }
        public string EmailAddress { get; set; }
        public int LocationId { get; set; }
    }
}
