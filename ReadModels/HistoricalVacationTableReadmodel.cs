using System.Collections.Generic;
using Ops.Infra.EventStore;
using Ops.Infra.ReadModels;

namespace Ops.ReadModels
{
    public class HistoricalVacationTableReadmodel : ReadModel
    {
        public HistoricalVacationTableReadmodel() {}
        public override dynamic EventPublish(EventFromES anEvent)
        {
            var readModelCollection = Book.book;

            var data = anEvent.Data;
            switch (anEvent.EventType)
            {
                case "ManagerCreated":
                    HistoricalVacationTableData historicalVacationData = new HistoricalVacationTableData
                    {
                        Id = data["ManagerId"],
                        ManagerName = data["Name"],
                        LastUpdated = anEvent.TimeStamp,
                        VacationOwed = 0,
                        locationId = data["LocationId"]
                    };

                    readModelCollection["HistoricalVacationTable"].Add(historicalVacationData);
                    Book.book = readModelCollection;
                    return historicalVacationData;

                case "ManagerDayScheduleSet":
                    string Id = data["ManagerId"];
                    string shiftCode = data["ShiftCode"];
                    HistoricalVacationTableData history = (HistoricalVacationTableData)readModelCollection["HistoricalVacationTable"].Find(m => m.Id == Id);
                    ShiftStatusTableData status = getShift(readModelCollection["ShiftStatusTable"], shiftCode);
                    history.StatHolidaysOwed = history.StatHolidaysOwed + status.DaysToOwe;
                    Book.book = readModelCollection;
                    return history;

            }
            return null;
        }

        private ShiftStatusTableData getShift(List<ReadModelData> list, string shiftCode)
        {
           foreach(ShiftStatusTableData code in list)
           {
                if (code.StatusId == shiftCode) return code;
           } return null;
        }
    }

    public class HistoricalVacationTableData : ReadModelData
    {
        public string LastUpdated { get; set; }
        public string ManagerName { get; set; }
        public double VacationOwed { get; set; }
        public int StatHolidaysOwed { get; set; }
        public string locationId { get; set; }
    }
}
