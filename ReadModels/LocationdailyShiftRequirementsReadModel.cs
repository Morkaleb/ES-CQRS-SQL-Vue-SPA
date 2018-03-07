using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Ops.Infra.EventStore;
using Ops.Infra.ReadModels;

namespace Ops.ReadModels
{
    public class LocationDailyShiftRequirementsReadModel : ReadModel
    {
        public LocationDailyShiftRequirementsReadModel() { }
        public override dynamic EventPublish(EventFromES anEvent)
        {
            var readmodelCollection = Book.book;
            var data = anEvent.Data;
            switch (anEvent.EventType)
            {
                case "DailyShiftTypeRequirementsSet":
                    var id = data["Id"].Value;
                    if (readmodelCollection["LocationDailyShiftRequirements"].FindIndex(l => l.Id == id) == -1)
                    {
                        LocationDailyShiftRequirementsData locationShiftRequirements = new LocationDailyShiftRequirementsData
                        {
                            Id = anEvent.Id.Split('.')[1],
                            Monday = StringListBuilder(data["Monday"]),
                            Tuesday = StringListBuilder(data["Tuesday"]),
                            Wednesday = StringListBuilder(data["Wednesday"]),
                            Thursday = StringListBuilder(data["Thursday"]),
                            Friday = StringListBuilder(data["Friday"]),
                            Saturday = StringListBuilder(data["Saturday"]),
                            Sunday = StringListBuilder(data["Sunday"])
                        };
                        readmodelCollection["LocationDailyShiftRequirements"].Add(locationShiftRequirements);
                        return locationShiftRequirements;
                    } else
                    {
                        int index = readmodelCollection["LocationDailyShiftRequirements"].FindIndex(l => l.Id == id);
                        LocationDailyShiftRequirementsData locationShiftRequirements =
                            (LocationDailyShiftRequirementsData)readmodelCollection["LocationDailyShiftRequirements"][index];
                        locationShiftRequirements.Monday = StringListBuilder(data["Monday"]);
                        locationShiftRequirements.Tuesday = StringListBuilder(data["Tuesday"]);
                        locationShiftRequirements.Wednesday = StringListBuilder(data["Wednesday"]);
                        locationShiftRequirements.Thursday = StringListBuilder(data["Thursday"]);
                        locationShiftRequirements.Friday = StringListBuilder(data["Friday"]);
                        locationShiftRequirements.Saturday = StringListBuilder(data["Saturday"]);
                        locationShiftRequirements.Sunday = StringListBuilder(data["Sunday"]);
                        return locationShiftRequirements;
                    }
                    
            }return null;
        }

        private List<string> StringListBuilder(JArray array)
        {
            List<string> list = new List<string>();
            try
            {
                foreach (var item in array)
                {
                    list.Add(item.ToString());
                }
                return list;
            } catch (Exception e)
            {
                Console.Write(e);
                return null;
            }
        }
    }

    public class LocationDailyShiftRequirementsData : ReadModelData
    {
        public List<string> Monday { get; set; }
        public List<string> Tuesday { get; set; }
        public List<string> Wednesday { get; set; }
        public List<string> Thursday { get; set; }
        public List<string> Friday { get; set; }
        public List<string> Saturday { get; set; }
        public List<string> Sunday { get; set; }
    }

    
   
}
