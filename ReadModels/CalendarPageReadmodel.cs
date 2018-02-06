using System;
using System.Collections.Generic;
using Ops.Infra.EventStore;
using Ops.Infra.ReadModels;

namespace Ops.ReadModels
{
    public class CalendarPageReadmodel : ReadModel
    {
        public CalendarPageReadmodel() { }
        public override dynamic EventPublish(EventFromES anEvent)
        {
            var readModelCollection = Book.book;
            var data = anEvent.Data;
            switch (anEvent.EventType)
            {
                case "ManagerDayScheduleSet":
                    string locationId = data["LocationId"];
                    string day = data["Day"];
                    int locationIndex = findLocationDay(locationId, day, readModelCollection["CalendarPage"]);
                    string managerId = data["ManagerId"];
                    string name = findManager(readModelCollection["ManagerTable"], managerId);
                    string shiftCode = data["ShiftCode"];
                    if (locationIndex != -1)
                    {
                        CalendarPageData page = (CalendarPageData)readModelCollection["CalendarPage"][locationIndex];
                        int shiftIndex = page.Schedule.FindIndex(shift => shift.ShiftCode == shiftCode);
                        if (shiftIndex == -1)
                        {
                            page.Schedule.Add(new CalendarSchedule
                            {
                                ShiftCode = data["ShiftCode"],
                                ManagerId = data["ManagerId"],
                                ManagerName = name
                            });
                        }
                        else if(shiftIndex != -1)
                        {
                            page.Schedule[shiftIndex].ManagerName = name;
                            page.Schedule[shiftIndex].ManagerId = data["ManagerId"];
                            page.Schedule[shiftIndex].ShiftCode = data["ShiftCode"];
                        };
                    }
                    else
                    {
                        CalendarPageData calendarPage = new CalendarPageData
                        {
                            Id = Guid.NewGuid().ToString(),
                            LocationId = data["LocationId"],
                            Shiftdate = data["Day"],
                            EOW = data["EOW"],
                            Schedule = new List<CalendarSchedule>(),
                            Approved = false
                           
                        };
                        calendarPage.Schedule.Add(new CalendarSchedule
                        {
                            ShiftCode = data["ShiftCode"],
                            ManagerId = data["ManagerId"],
                            ManagerName = name
                        });
                        readModelCollection["CalendarPage"].Add(calendarPage);
                    }
                    return readModelCollection;

                case "ManagerDayScheduleChangeRequested":
                    locationId = data["LocationId"];
                    string shiftDate = data["ShiftDate"];
                    string code = data["ShiftCode"];
                    managerId = data["ManagerId"];
                    name = findManager(readModelCollection["ManagerTable"], managerId);
                    locationIndex = findLocationDay(locationId, shiftDate , readModelCollection["CalendarPage"]);
                    var dayPage = readModelCollection["CalendarPage"][locationIndex];
                    CalendarPageData aDay = (CalendarPageData)dayPage;
                    int ashiftIndex = aDay.Schedule.FindIndex(d => d.ShiftCode == code);
                    if(ashiftIndex != -1){
                        if(managerId == "Cancel Shift")
                        {
                            aDay.Schedule[ashiftIndex].ManagerName = "Cancelled" + " (pending)";
                            aDay.Schedule[ashiftIndex].ManagerId = "0";
                            return readModelCollection;
                        }
                        aDay.Schedule[ashiftIndex].ManagerId = data["ManagerId"];
                        aDay.Schedule[ashiftIndex].ManagerName = name + " (pending)";
                    }
                    return readModelCollection;

                case "PayrollApprovalRequiredForShiftChange":
                    string id = data["RequestId"];
                    int requestIndex = readModelCollection["ChangeRequestsTable"].FindIndex(r => r.Id == id);
                    ChangeRequestsTableData request = (ChangeRequestsTableData)readModelCollection["ChangeRequestsTable"][requestIndex];
                    int locationDayIndex = findLocationDay(request.LocationId, request.ShiftDate, readModelCollection["CalendarPage"]);
                    aDay = (CalendarPageData)readModelCollection["CalendarPage"][locationDayIndex];
                    aDay.Schedule.Find(d => d.ShiftCode == request.ShiftCode).ManagerName = request.ToName + " (Payroll Pending)";
                    return readModelCollection;

                case "PayRollApprovedScheduleChange":
                    id = data["RequestId"];
                    requestIndex = readModelCollection["ChangeRequestsTable"].FindIndex(r => r.Id == id);
                    request = (ChangeRequestsTableData)readModelCollection["ChangeRequestsTable"][requestIndex];
                    locationDayIndex = findLocationDay(request.LocationId, request.ShiftDate, readModelCollection["CalendarPage"]);
                    aDay = (CalendarPageData)readModelCollection["CalendarPage"][locationDayIndex];
                    aDay.Schedule.Find(d => d.ShiftCode == request.ShiftCode).ManagerName = request.ToName;
                    return readModelCollection;

                case "GMRejectedScheduleChange":
                    id = data["RequestId"];
                    requestIndex = readModelCollection["ChangeRequestsTable"].FindIndex(r => r.Id == id);
                    request = (ChangeRequestsTableData)readModelCollection["ChangeRequestsTable"][requestIndex];
                    locationDayIndex = findLocationDay(request.LocationId, request.ShiftDate, readModelCollection["CalendarPage"]);
                    aDay = (CalendarPageData)readModelCollection["CalendarPage"][locationDayIndex];
                    aDay.Schedule.Find(d => d.ShiftCode == request.ShiftCode).ManagerName = data["OrigionalManagerName"];
                    return readModelCollection;

                case "GMApprovedSchedule":
                    id = data["RestaurantId"];
                    string eow = data["EOW"];
                    List<CalendarPageData> days = scheduleReturn(Book.book["CalendarPage"], id, eow);
                    foreach(CalendarPageData thisDay in days)
                    {
                        thisDay.Approved = true;
                    }
                    return readModelCollection;

                case "ManagerDayScheduleChanged":
                    locationId = data["LocationId"];
                    shiftDate = data["ShiftDate"];
                    code = data["ShiftCode"];
                    managerId = data["ManagerId"];
                    name = findManager(readModelCollection["ManagerTable"], managerId);
                    locationIndex = findLocationDay(locationId, shiftDate, readModelCollection["CalendarPage"]);
                    dayPage = readModelCollection["CalendarPage"][locationIndex];
                     aDay = (CalendarPageData)dayPage;
                    ashiftIndex = aDay.Schedule.FindIndex(d => d.ShiftCode == code);
                    if (ashiftIndex != -1)
                    {
                        if (managerId == "Cancel Shift")
                        {
                            aDay.Schedule[ashiftIndex].ManagerName = "Cancelled";
                            aDay.Schedule[ashiftIndex].ManagerId = "0";
                            return readModelCollection;
                        }
                        aDay.Schedule[ashiftIndex].ManagerId = data["ManagerId"];
                        aDay.Schedule[ashiftIndex].ManagerName = name;
                    }
                    return readModelCollection;

                case "GMApprovedScheduleChange":
                    id = data["RequestId"];
                    ChangeRequestsTableData thisRequest = (ChangeRequestsTableData)Book.book["ChangeRequestsTable"].Find(r => r.Id == id);
                    locationIndex = findLocationDay(thisRequest.LocationId, thisRequest.ShiftDate, readModelCollection["CalendarPage"]);
                    dayPage = readModelCollection["CalendarPage"][locationIndex];
                    aDay = (CalendarPageData)dayPage;
                    ashiftIndex = aDay.Schedule.FindIndex(d => d.ShiftCode == thisRequest.ShiftCode);
                    aDay.Schedule[ashiftIndex].ManagerId = data["ManagerId"];
                    aDay.Schedule[ashiftIndex].ManagerName = thisRequest.ToName;
                    return readModelCollection;

            }
            return null;
        }

        int findLocationDay(string locationId, string shiftDate, List<ReadModelData> list)
        {
            string dayId = "";
            foreach (CalendarPageData day in list)
            {
                if (day.Shiftdate == shiftDate && day.LocationId == locationId)
                {
                    dayId = day.Id;
                }
            }
            var index = list.FindIndex(day => day.Id == dayId);
            return index;
        }

        private List<CalendarPageData> scheduleReturn(List<ReadModelData> list, string locationId, string eow)
        {
            List<CalendarPageData> returnable = new List<CalendarPageData>();
            foreach(ReadModelData day in list)
            {
                CalendarPageData thisDay = (CalendarPageData)day;
                if (thisDay.EOW == eow && thisDay.LocationId == locationId) returnable.Add(thisDay);
            }
            return returnable;
        }

        private string findManager(List<ReadModelData> list, string id)
        {
            string name = "";
            foreach (ManagerTableData manager in list)
            {
                if (manager.ManagerId == id) name = manager.ManagerName;
            }
            return name;
        }
    }

    public class CalendarPageData : ReadModelData
    {   
        public string LocationId { get; set; }
        public string Shiftdate { get; set; }
        public string EOW { get; set; }
        public bool Approved { get; set; }
        public List<CalendarSchedule> Schedule { get; set; }
    }

    public class CalendarSchedule
    {
        public string ShiftCode { get; set; }
        public string ManagerId { get; set; }
        public string ManagerName { get; set; }
    }
}
