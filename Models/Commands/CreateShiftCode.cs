using Ops.Infra;

namespace Ops.Models.commands
{  
    public class CreateShiftCode : Commands
    {
       
        public string ShiftType { get; set; }
        public int DaysOwed { get; set; }
    }
}
