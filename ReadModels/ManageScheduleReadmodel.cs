using System;
using Ops.Infra.EventStore;
using Ops.Infra.ReadModels;

namespace Ops.ReadModels
{
    public class ManagerScheduleTableReadModel : ReadModel
    {
        public ManagerScheduleTableReadModel() { }
        public override dynamic EventPublish(EventFromES anEvent)
        {
            var readModelCollection = Book.book;            
            var data = anEvent.Data;
            switch (anEvent.EventType)
            {   
                case "ManagerDayScheduleSet":
                        string locationId = data["LocationId"];
                        ManagerScheduleTableData schedule = new ManagerScheduleTableData
                        {
                            Id = Guid.NewGuid().ToString(),
                            ShiftCode = data["ShiftCode"].Value,
                            ShiftStatus = data["ShiftStatus"],
                            ManagerId = data["ManagerId"],
                            ShiftDate = data["Day"],
                            LocationId = locationId,
                            EOW = data["EOW"]
                        };
                        readModelCollection["ManagerScheduleTable"].Add(schedule);
                        return schedule;                    
            }
            return null;
        }
        
    }
    
    public class ManagerScheduleTableData : ReadModelData
    {        
        public string ShiftCode { get; set; }
        public string ShiftStatus { get; set; }
        public string ManagerId { get; set; }
        public string LocationId { get; set; }
        public string ShiftDate { get; set; }
        public string EOW { get; set; }
    }
}
