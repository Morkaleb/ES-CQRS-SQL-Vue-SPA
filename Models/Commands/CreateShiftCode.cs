using Ops.Infra;

namespace Ops.Models.commands
{  
    public class CreateShiftCode : Ops.Infra.Commands
    {
        public int ShiftStatus { get; set; }
       
        public string ShiftType { get; set; }
        public int DaysOwed { get; set; }
    }
}
