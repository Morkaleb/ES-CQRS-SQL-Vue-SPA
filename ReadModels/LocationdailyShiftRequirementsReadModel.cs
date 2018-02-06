using System.Collections.Generic;
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
                    string id = data["Id"];
                    string dayOfTheWeek = data["DayOfTheWeek"];                    
                    List<string> codes = new List<string>();
                    foreach(string code in data["ShiftTypes"])
                    {
                        codes.Add(code);
                    }                    
                    int locationIndax = readmodelCollection["LocationDailyShiftRequirements"].FindIndex(r => r.Id == id);
                    if(locationIndax == -1)
                    {
                        LocationDailyShiftRequirementsData requirements = new LocationDailyShiftRequirementsData
                        {
                            Id = data["Id"],
                            DaysOfTheWeek = new List<Day>()
                        };
                        Day thisDay = new Day
                        {
                            DayOfTheWeek = dayOfTheWeek,
                            ShiftCodes = codes
                        };
                        requirements.DaysOfTheWeek.Add(thisDay);
                        readmodelCollection["LocationDailyShiftRequirements"].Add(requirements);
                    }
                    else
                    {
                        dayOfTheWeek = data["DayOfTheWeek"];
                        LocationDailyShiftRequirementsData locationRequirements = (LocationDailyShiftRequirementsData)readmodelCollection["LocationDailyShiftRequirements"][locationIndax];
                        int dayIndex = locationRequirements.DaysOfTheWeek.FindIndex(d => d.DayOfTheWeek == dayOfTheWeek);
                        if(dayIndex == -1)
                        {
                            Day today = new Day
                            {
                                DayOfTheWeek = dayOfTheWeek,
                                ShiftCodes = codes
                            };
                            locationRequirements.DaysOfTheWeek.Add(today);
                        }
                        else
                        {
                            locationRequirements.DaysOfTheWeek[dayIndex].ShiftCodes = codes;
                        }

                    }
                    return readmodelCollection;
                    
            }return null;
        }
    }

    public class LocationDailyShiftRequirementsData : ReadModelData
    {
       public List<Day> DaysOfTheWeek { get; set; }
    }
    public class Day
    {
        public string DayOfTheWeek { get; set; }
        public List<string> ShiftCodes { get; set; }
    }
}
