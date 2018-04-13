using System;
using System.Collections.Generic;
using Ops.Infra.EventStore;
using Ops.Infra.ReadModels;

namespace Ops.ReadModels
{
    public class ExampleReadmodel : ReadModel
    {
        public ExampleReadmodel() { }
        public override dynamic EventPublish(EventFromES anEvent)
        {
            var readModelCollection = Book.book;
            var data = anEvent.Data;
            switch (anEvent.EventType)
            {
                case "ExampleEvent":
                    //Event Data that is not defined in the base class comes through the read model as a JObject, carried by a JArray
                    string Id = data["Id"];
                    string name = data["Name"];                   
                    return readModelCollection;                
            }
            return null;
        }

       
    }

    public class ExampleData : ReadModelData
    {
       //Naming conventions mean that all readmodelData classes that end in the word "Table" will be used to generate and populate SQL tables
       public string Name { get; set; }
    }
    
}
