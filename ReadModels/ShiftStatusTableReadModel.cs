using Ops.Infra.EventStore;
using Ops.Infra.ReadModels;
using System;

namespace Ops.ReadModels
{
    public class ShiftStatusTableReadModel : ReadModel
    {
        public ShiftStatusTableReadModel() { }
        public override dynamic EventPublish(EventFromES anEvent)
        {
            var readmodelCollection = Book.book;
            var data = anEvent.Data;
            switch (anEvent.EventType)
            {
                case "ShiftCodeCreated":
                    string owe = data["DaysOwed"];
                    int daysToOwe = Int32.Parse(owe);
                    ShiftStatusTableData shiftStatusData = new ShiftStatusTableData
                    {
                        Id = (readmodelCollection["ShiftStatusTable"].Count + 1).ToString(),
                        StatusId = data["ShiftCode"].Value,
                        Description = data["ShiftType"].Value,
                        DaysToOwe = daysToOwe,
                        ShiftStatus= data["ShiftStatus"]
                    };
                    readmodelCollection["ShiftStatusTable"].Add(shiftStatusData);
                    Book.book = readmodelCollection;
                    return shiftStatusData;
            }
            return null;
        }
    }

    public class ShiftStatusTableData : ReadModelData
    {
        public string StatusId { get; set; }
        public string Description { get; set; }
        public int DaysToOwe { get; set; }
        public string ShiftStatus { get; set; }
    }
}
