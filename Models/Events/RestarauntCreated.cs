using Ops.Infra.EventStore;

namespace Ops.Models.events
{
    public class RestarauntCreated : Events
    {
        public string Id { get; set; }
        public string StoreId { get; set; }
        public string LocationId { get; set; }
        //This is a placeholder for an external event from legacy integration and/or future Event Sourced API's
    }
}
