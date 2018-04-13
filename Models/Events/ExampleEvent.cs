namespace Ops.Models.events
{
    public class ExampleEvent : Ops.Infra.EventStore.Events
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
