using System;
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
                        ManagerFirstName = data["FirstName"],
                        ManagerLastName = data["LastName"],
                        LastUpdated = anEvent.TimeStamp,
                        VacationOwed = data["VacationDaysOwed"],
                        LocationId = data["LocationId"],
                        VacationRate = data["VacationRate"]
                    };

                    readModelCollection["HistoricalVacationTable"].Add(historicalVacationData);
                    Book.book = readModelCollection;
                    return historicalVacationData;

                case "ManagerDayScheduleSet":
                    string Id = data["ManagerId"];
                    var IdArray = Id.Split(" ").Length -1;
                    if (Id.Split(" ")[IdArray] != "IC")
                    {
                        string shiftCode = data["ShiftCode"];
                        HistoricalVacationTableData history = (HistoricalVacationTableData)readModelCollection["HistoricalVacationTable"].Find(m => m.Id == Id);
                        ShiftStatusTableData status = getShift(readModelCollection["ShiftStatusTable"], shiftCode);
                        history.StatHolidaysOwed = history.StatHolidaysOwed + status.DaysToOwe;
                        Book.book = readModelCollection;
                        return history;
                    } return null;

                case "ManagerDayScheduleChanged":
                    {
                        string managerFromId = data["ManagerFromId"];
                        string ShiftCode = data["ShiftCode"];
                        if (ShiftCode.Split("(").Length > 1)
                        {
                            if(ShiftCode.Split("(")[1] == "Owed)")
                            {
                                var manager = (HistoricalVacationTableData)readModelCollection["HistoricalVacationTable"].Find(m => m.Id == managerFromId);
                                manager.StatHolidaysOwed--;
                                string managerToId = data["ManagerId"];
                                if(managerToId != "Cancel Shift")
                                {
                                    HistoricalVacationTableData managerTo =
                                        (HistoricalVacationTableData)readModelCollection["HistoricalVacationTable"].Find(m => m.Id == managerToId);
                                    managerTo.StatHolidaysOwed++;                                     
                                       
                                }
                            }
                        }
                        return null;
                    }

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
        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }
        public double VacationOwed { get; set; }
        public int StatHolidaysOwed { get; set; }
        public string LocationId { get; set; }
        public double VacationRate { get; set; }
    }
}
