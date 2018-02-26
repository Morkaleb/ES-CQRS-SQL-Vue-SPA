using System;
using Ops.Infra;
using Ops.Infra.CommandToPublishEvent;
using Ops.Infra.EventStore;
using Ops.Models.commands;
using Ops.Models.events;

namespace Ops.Domain
{
    public class ShiftCodeAggregate : Aggregate
    {
        public override Events[] Execute(Commands cmd)
        {
            if (cmd is CreateShiftCode) { return _CreateShiftCode((CreateShiftCode)(cmd)); }
            return null;
        }
        private Events[] _CreateShiftCode(CreateShiftCode cmd)
        {
            if (string.IsNullOrEmpty(cmd.Id)) throw new Exception("ShiftCode is a required field");
            if (string.IsNullOrEmpty(cmd.ShiftType)) throw new Exception("ShiftType is a required field");
            ShiftCodeCreated created = new ShiftCodeCreated
            {
                ShiftCode = cmd.Id,
                ShiftType = cmd.ShiftType,
                ShiftStatus = cmd.ShiftStatus,
                DaysOwed = cmd.DaysOwed
            };
            return new Events[] { created };
        }

        public override void Hydrate(EventFromES evt)
        {
            //nothing to hydrate for now
        }
    }
}
