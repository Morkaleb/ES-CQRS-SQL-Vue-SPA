using System;
using System.Collections.Generic;
using Ops.Infra.EventStore;
using Ops.Infra.ReadModels;

namespace Ops.ReadModels
{
    public class ManagerTableReadModel : ReadModel
    {
        public ManagerTableReadModel() { }
        public override dynamic EventPublish(EventFromES anEvent)
        {
            var readmodelCollection = Book.book;
            var data = anEvent.Data;
            int index;
            switch (anEvent.EventType)
            {
                case "ManagerCreated":
                    ManagerTableData managerData = new ManagerTableData
                    {
                        Id = data["ManagerId"],
                        ManagerId = data["ManagerId"],
                        ManagerName = data["Name"],
                        ServiceYears = GetServiceYears(anEvent.TimeStamp),
                        LastUpdated = anEvent.TimeStamp,
                        VacationBalance = data["VacationDaysOwed"],
                        VacationRate = data["VacationRate"],
                        EmailAddress = data["EmailAddress"],
                        LocationId = data["LocationId"],
                        Role = data["Role"]
                    };
                    readmodelCollection["ManagerTable"].Add(managerData);
                    return managerData;

                case "ManagerEmailSet":
                    index = GetIndex(readmodelCollection["ManagerTable"], anEvent.Data["ManagerId"]);
                    if (index != -1)
                    {
                        ManagerTableData manager = (ManagerTableData)readmodelCollection["ManagerTable"][index];
                        manager.EmailAddress = data["EmailAddress"];
                        return manager;
                    }
                    break;

                case "ManagerWorkedAHoliday":
                    index = GetIndex(readmodelCollection["ManagerTable"], anEvent.Data["ManagerId"]);
                    if (index != -1)
                    {
                        ManagerTableData manager = (ManagerTableData)readmodelCollection["ManagerTable"][index];
                        manager.StatBalance = manager.StatBalance + 1;
                        return manager;
                    }
                    break;

                case "ManagerCancelledStatShift":
                    index = GetIndex(readmodelCollection["ManagerTable"], anEvent.Data["ManagerId"]);
                    if (index != -1)
                    {
                        ManagerTableData manager = (ManagerTableData)readmodelCollection["ManagerTable"][index];
                        manager.StatBalance = manager.StatBalance - 1;
                        return manager;
                    }
                    break;

            }
            return null;
        }

        private double GetServiceYears(string thetimeStamp)
        {
            if (!string.IsNullOrEmpty(thetimeStamp))
            {
                DateTime timeStamp = DateTime.Parse(thetimeStamp);
                double years = timeStamp.Year - DateTime.Now.Year;
                double daysOfYear = (DateTime.Now.DayOfYear - timeStamp.DayOfYear) / 365;
                double timeServed = years + daysOfYear;
                return timeServed;
            }
            return 0.0;
        }

        private int GetIndex(List<ReadModelData> managers, string managerId)
        {            
            int index = -1;
            foreach(ManagerTableData m in managers)
            {
                if(m.ManagerId == managerId)
                {
                    index = managers.FindIndex(ma => ma.Id == m.Id);
                }
            }
            return index;
        }
    }

    public class ManagerTableData : ReadModelData
    {
        public string ManagerId { get; set; }
        public string ManagerName { get; set; }
        public double ServiceYears { get; set; }
        public double VacationBalance { get; set; }
        public double StatBalance { get; set; }
        public double DaysOwed { get; set; }
        public string LastUpdated { get; set; }
        public string EmailAddress { get; set; }
        public double VacationRate { get; set; }
        public int LocationId { get; set; }
        public int Role { get; set; }
    }

}
