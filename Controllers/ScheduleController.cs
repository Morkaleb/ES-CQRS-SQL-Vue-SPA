using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Ops.Domain;
using Ops.Infra.CommandToPublishEvent;
using Ops.Infra.ReadModels;
using Ops.Models.commands;
using Ops.ReadModels;
using System.Net.Http;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Threading.Tasks;

namespace Ops.Controllers
{
    public class ScheduleController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public ScheduleController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("api/[controller]/set")]
        public ActionResult SetManagerDaySchedule([FromBody] SetManagerDaySchedule setManagerDaySchedule)
        {
            try
            {
                setManagerDaySchedule.EOW = GetEOW(setManagerDaySchedule.Day);
                setManagerDaySchedule.Id = setManagerDaySchedule.LocationId + "." + setManagerDaySchedule.EOW;
                Aggregate managerAggreate = new ScheduleAggregate();
                CommandHandler.ActivateCommand(setManagerDaySchedule, managerAggreate);
                return Ok(setManagerDaySchedule.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        [HttpPost]
        [Route("api/[controller]/changeDay")]
        public ActionResult ChangeManagerDaySchedule([FromBody] RequestChangeManagerSchedule requestChangeManagerDaySchedule)
        {
            try
            {
                requestChangeManagerDaySchedule.EOW = GetEOW(requestChangeManagerDaySchedule.ShiftDate);
                requestChangeManagerDaySchedule.Id = requestChangeManagerDaySchedule.LocationId + "." + requestChangeManagerDaySchedule.EOW;
                requestChangeManagerDaySchedule.RequestId = Guid.NewGuid().ToString();
                Aggregate managerAggreate = new ScheduleAggregate();
                CommandHandler.ActivateCommand(requestChangeManagerDaySchedule, managerAggreate);
                return Ok(requestChangeManagerDaySchedule.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        [HttpPost]
        [Route("api/[controller]/GMApproveChange")]
        public ActionResult GmApproveChange([FromBody] GMApproveScheduleChange gMApproveScheduleChange)
        {
            try
            {
                gMApproveScheduleChange.Id = gMApproveScheduleChange.Request.LocationId + "." + gMApproveScheduleChange.Request.EOW;
                Aggregate managerAggreate = new ScheduleAggregate();
                CommandHandler.ActivateCommand(gMApproveScheduleChange, managerAggreate);
                return Ok(gMApproveScheduleChange.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        [HttpPost]
        [Route("api/[controller]/PayrollApproveChange")]
        public ActionResult PayRollApproveChange([FromBody] PayrollApproveScheduleChange payRollApproveScheduleChange)
        {
            try
            {
                payRollApproveScheduleChange.Id = payRollApproveScheduleChange.Request.FromId;
                Aggregate managerAggreate = new ScheduleAggregate();
                CommandHandler.ActivateCommand(payRollApproveScheduleChange, managerAggreate);
                return Ok(payRollApproveScheduleChange.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        [HttpPost]
        [Route("api/[controller]/GMRejectChange")]
        public ActionResult GmRejectChange([FromBody]GMRejectScheduleChange gMRejectScheduleChange)
        {
            try
            {
                gMRejectScheduleChange.GM = (ManagerTableData)Book.book["ManagerTable"].Find(gm => gm.Id == gMRejectScheduleChange.GMId);
                gMRejectScheduleChange.ManagerFrom = (ManagerTableData)Book.book["ManagerTable"].Find(gm => gm.Id == gMRejectScheduleChange.Id);
                gMRejectScheduleChange.OrigionalRequest = (ChangeRequestsTableData)Book.book["ChangeRequestsTable"].Find(r => r.Id == gMRejectScheduleChange.RequestId);
                gMRejectScheduleChange.ManagerTo = (ManagerTableData)Book.book["ManagerTable"].Find(m => m.Id == gMRejectScheduleChange.Id);
                Aggregate managerAggregate = new ScheduleAggregate();
                CommandHandler.ActivateCommand(gMRejectScheduleChange, managerAggregate);
                return Ok(gMRejectScheduleChange.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        [HttpPost]
        [Route("api/[controller]/payrollAcceptChange")]
        public ActionResult PayrollAcceptChange([FromBody] PayrollApproveScheduleChange payrollApproveScheduleChange)
        {
            try
            {
                payrollApproveScheduleChange.Id = payrollApproveScheduleChange.Request.FromId;
                Aggregate managerAggreate = new ScheduleAggregate();
                CommandHandler.ActivateCommand(payrollApproveScheduleChange, managerAggreate);
                return Ok(payrollApproveScheduleChange.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        [HttpPost]
        [Route("api/[controller]/approveSchedule")]
        public ActionResult ApproveSchedule([FromBody] GMApproveSchedule schedule)
        {
            List<ReadModelData> managers = Book.book["ManagerTable"];
            List<ManagerTableData> locationManagers = new List<ManagerTableData>();
            foreach (ManagerTableData manager in managers)
            {
                if (manager.LocationId.ToString() == (string)schedule.RestaurantId) locationManagers.Add(manager);
            }
            schedule.Id = schedule.RestaurantId + "." + schedule.EOW;
            try
            {
                schedule.Managers = locationManagers;
                Aggregate scheduleAggregate = new ScheduleAggregate();
                CommandHandler.ActivateCommand(schedule, scheduleAggregate);
                return Ok(schedule.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpGet]
        [Route("api/[controller]/download")]
        public ActionResult GetMonth()
        {
            var scheduledata = Book.book["WeeklyCalendarPage"];
            string queryString = Request.QueryString.ToString();
            string locationId = queryString.Split('&')[0].Trim('?').Trim('=').Split("=")[1];
            string startDate = queryString.Split('&')[1].Trim('?').Trim('=').Split("=")[1];
            string endDate = queryString.Split('&')[2].Trim('?').Trim('=').Split("=")[1];
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = locationId + "_" + startDate + "_" + endDate + ".xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            List<WeeklyCalendarData> filtered = new List<WeeklyCalendarData>();
            DateTime startTime = Convert.ToDateTime(startDate);
            DateTime endtime = Convert.ToDateTime(endDate);
            DateTime aTime = startTime.AddDays(7);
            string month = aTime.ToString("MMMMM");
            foreach (WeeklyCalendarData week in scheduledata)
            {
                var shiftDate = Convert.ToDateTime(week.EOW);
                int datecompare = DateTime.Compare(shiftDate, startTime);
                int dateCompare2 = DateTime.Compare(endtime, shiftDate);
                if (DateTime.Compare(shiftDate, startTime) >= 0 && DateTime.Compare(endtime, shiftDate) >= 0)
                {
                    filtered.Add(week);
                }
            }
            filtered = filtered.OrderBy(d => d.EOW).ToList();
            string result = WritepdfTable(sFileName, filtered, month);
            string fileName = startDate + "_" + endDate + "_" + locationId + "_Schedule.pdf";
            return Content(result,"text/html");
        }


        [HttpGet]
        [Route("api/[controller]/downloadPGI")]
        public FileResult GetDownload()
        {
            var scheduledata = Book.book["ManagerScheduleTable"];
            var managerData = Book.book["ManagerTable"];
            var queryString = Request.QueryString.ToString();
            var startDate = queryString.Split('&')[0].Trim('?').Trim('=').Split("=")[1];
            var endDate = queryString.Split('&')[1].Trim('?').Trim('=').Split("=")[1];
            DateTime t1 = Convert.ToDateTime(startDate);
            DateTime t2 = Convert.ToDateTime(endDate);
            List<CSVImportRow> collection = new List<CSVImportRow>();
            foreach (ManagerTableData manager in managerData)
            {
                CSVImportRow row = new CSVImportRow
                {
                    EmployeeId = Int32.Parse(manager.Id),
                    FirstName = manager.FirstName,
                    LastName = manager.LastName,
                    StatDays = 0,
                    Vacation = manager.VacationBalance
                };
                collection.Add(row);
            }
            foreach (ManagerScheduleTableData data in scheduledata)
            {
                var shiftDate = Convert.ToDateTime(data.ShiftDate);
                int datecompare = DateTime.Compare(shiftDate, t1);
                int dateCompare2 = DateTime.Compare(t2, shiftDate);
                if (DateTime.Compare(shiftDate, t1) >= 0 && DateTime.Compare(t2, shiftDate) >= 0)
                {
                    if (data.ShiftStatus == "3")
                    {
                        var row = collection.Find(r => r.EmployeeId.ToString() == data.ManagerId);
                        row.StatDays++;
                    }
                    if (data.ShiftStatus == "2")
                    {
                        var row = collection.Find(r => r.EmployeeId.ToString() == data.ManagerId);
                        row.StatDays--;
                    }
                    if (data.ShiftStatus == "1")
                    {
                        var row = collection.Find(r => r.EmployeeId.ToString() == data.ManagerId);
                        ManagerTableData manager = (ManagerTableData)managerData.Find(m => m.Id == data.ManagerId);
                        row.Vacation += manager.VacationRate / 5;
                    };
                }
            }
            string FileName = startDate + "_" + endDate;
            WriteCSV(collection, FileName);
            var result = System.IO.File.ReadAllBytes(@"C:\Working\Ops\" + FileName + ".csv");
            return File(result, "application/x-msdownload", FileName + ".csv");

        }


        private string GetEOW(string date)
        {
            string EOW;
            DateTime parsedDay = DateTime.Parse(date);
            int dayOfWeek = (int)parsedDay.DayOfWeek;
            if (dayOfWeek != 0)
            {
                EOW = parsedDay.AddDays(7 - dayOfWeek).Date.ToString("MM-dd-yyyy");
            }
            else
            {
                EOW = parsedDay.Date.ToString("MM-dd-yyyy");
            }
            return EOW;
        }

        public void WriteCSV<T>(IEnumerable<T> items, string fileName)
        {
            Type itemType = typeof(T);
            var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .OrderBy(p => p.Name);
            if (!System.IO.File.Exists(@"C:\Working\Ops\" + fileName + ".csv"))
            {
                using (var writer = new StreamWriter(@"C:\Working\Ops\" + fileName + ".csv"))
                {
                    writer.WriteLine(string.Join(", ", props.Select(p => p.Name)));

                    foreach (var item in items)
                    {
                        writer.WriteLine(string.Join(", ", props.Select(p => p.GetValue(item, null))));
                    }
                }
            }
            else
            {
                System.IO.File.Delete(@"C:\Working\Ops\" + fileName + ".csv");
                using (var writer = new StreamWriter(@"C:\Working\Ops\" + fileName + ".csv"))
                {
                    writer.WriteLine(string.Join(", ", props.Select(p => p.Name)));

                    foreach (var item in items)
                    {
                        writer.WriteLine(string.Join(", ", props.Select(p => p.GetValue(item, null))));
                    }
                }
            }
        }

        public string WritepdfTable(string fileName, List<WeeklyCalendarData> data, string month)
        {
            string table = "<!DOCTYPE html><html><body><table><caption>" + month + " Schedule</caption>";
            table += "<tr><th>Monday</th><th>Tuesday</th><th>Wednesday</th><th>Thursday</th><th>Friday</th><th>Saturday</th><th>Sunday</th>";
            foreach (WeeklyCalendarData week in data)
            {
                string weekRow = "<tr>";
                foreach(WeeklyCalendarDay day in week.Days)
                {
                    DateTime date;
                    DateTime.TryParse(day.Date,out date);
                    string dOFTheM = date.Day.ToString();
                    string dayCell = "<p>" + dOFTheM + "</p>";
                    foreach(Shift shift in day.Shifts)
                    {
                        dayCell += "" + shift.ManagerName + " " + shift.ShiftCode.Split(" ")[0]  + "<br/>";
                    }
                    dayCell = "<td>" + dayCell + "</td>";
                    weekRow += dayCell;
                }
                weekRow += "</tr>";
                table += weekRow;
            }
            table += "</table></body></html>";
            table += "<style>table, th, td {border:1px solid black;} td {padding:5px; margin:5px;} p{margin-left:45%;}</style>";
            return table;
        }
    }

    public class CSVImportRow
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Vacation { get; set; }
        public int StatDays { get; set; }
    }
}
