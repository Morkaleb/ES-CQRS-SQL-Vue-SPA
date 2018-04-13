using System;
using Ops.Infra;
using Ops.Infra.CommandToPublishEvent;
using Ops.Infra.EventStore;
using Ops.Models.commands;
using Ops.Models.events;

namespace Ops.Domain
{
    public class ExampleAggregate : Aggregate
    {
        string _Id;        

        public override Events[] Execute(Commands cmd)
        {
            if (cmd is ExampleCommand) { return _exampleCommand((ExampleCommand)cmd); }
            return null;
        }

        private Events[] _exampleCommand(ExampleCommand cmd)
        {
            //we can check the _Id field to ensure that this command hasn't already been completed.
            //hydrate is called below by the EventStore Interfacing methods in infra.
            //This way, we can control the events to make sure that the data is as expected

            if ( !string.IsNullOrEmpty(_Id) ) { throw new Exception("This example event has already been created"); }
            if (string.IsNullOrEmpty(cmd.Id)) throw new Exception("Id is a required field");

            //check to see if all required fields are included
            //todo, build helper function to siplify required parameter checking
            //if all fields are correctly filled out, return an array of events
            return new Events[] { new ExampleEvent { Id = cmd.Id, Name = cmd.Name } };
        }

        public override void Hydrate(EventFromES evt)
        {
            if (evt.EventType == "ExampleEvent") { _onExampleEvent(evt); }
        }

        private void _onExampleEvent(EventFromES evt)
        {
            _Id = evt.Data["Id"];
        }      

    }
}
