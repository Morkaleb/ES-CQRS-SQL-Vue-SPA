using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Ops.Domain;
using Ops.Infra.CommandToPublishEvent;
using Ops.Infra.ReadModels;
using Ops.Models.commands;
using Ops.ReadModels;

namespace Ops.Controllers
{
    public class ScheduleController : Controller
    {
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
    }
}
