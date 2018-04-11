using Ops.Infra.EventStore;

namespace Ops.Models.events
{
    public class ManagerEmailChanged : Ops.Infra.EventStore.Events
    {
        public string ManagerId { get; set; }
        public string EmailAddress { get; set; }
    }
}
