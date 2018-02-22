using System;
using Ops.Infra;
using Ops.Infra.CommandToPublishEvent;
using Ops.Infra.EventStore;
using Ops.Models.commands;
using Ops.Models.events;

namespace Ops.Domain
{
    public class RestaurantAggregate : Aggregate
    {
        string _Id;
        public override Events[] Execute(Commands cmd)
        {
            if (cmd is CreateRestaurant) { return _CreateRestaurant((CreateRestaurant)cmd); }
            if (cmd is SetDailyShiftTypeRequirements) { return _SetDailyShiftRequirements((SetDailyShiftTypeRequirements)cmd); }
            return null;
        }

        private Events[] _SetDailyShiftRequirements(SetDailyShiftTypeRequirements cmd)
        {
            if (string.IsNullOrEmpty(_Id)) throw new Exception("Restaurant not found");
            if (string.IsNullOrEmpty(cmd.Id)) throw new Exception("Restaurant Location required");
            DailyShiftTypeRequirementsSet shiftRequirements = new DailyShiftTypeRequirementsSet
            {
                Id = cmd.Id,
                Monday = cmd.Monday,
                Tuesday = cmd.Tuesday,
                Wednesday = cmd.Wednesday,
                Thursday = cmd.Thursday,
                Friday = cmd.Friday,
                Saturday = cmd.Saturday,
                Sunday = cmd.Sunday,
                GmId = cmd.GmId
            };
            return new Events[] { shiftRequirements };
        }

        private Events[] _CreateRestaurant(CreateRestaurant cmd)
        {
            if (string.IsNullOrEmpty(cmd.Id)) { throw new Exception("Id is a required field"); }
            RestarauntCreated restaurant = new RestarauntCreated
            {
                Id = cmd.Id,
                LocationId = cmd.Id,
                StoreId = cmd.Id
            };
            return new Events[] { restaurant };
        }

        public override void Hydrate(EventFromES evt)
        {
            _Id = evt.Id;
        }
    }
}
