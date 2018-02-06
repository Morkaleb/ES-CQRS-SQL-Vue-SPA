using Ops.Infra.EventStore;

namespace Ops.Models.events
{
    public class ManagerEmailChanged : Events
    {
        public string ManagerId { get; set; }
        public string EmailAddress { get; set; }
    }
}
