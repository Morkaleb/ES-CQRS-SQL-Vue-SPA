using Ops.Infra;

namespace Ops.Models.commands
{
    public class CreateManager : Commands
    {
        public string Name { get; set; }
        public int Role { get; set; }
        public double VacationRate { get; set; }
        public double VacationDaysOwed { get; set; }
        public string EmailAddress { get; set; }
        public int LocationId { get; set; }
    }
}
