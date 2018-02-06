using Microsoft.AspNetCore.Mvc;
using System;
using Ops.Domain;
using Ops.Infra.CommandToPublishEvent;
using Ops.Models.commands;

namespace Ops.Controllers
{
    public class ShiftCodeController : Controller
    {
        [HttpPost]
        [Route("api/[controller]/new")]
        public ActionResult SetShiftType([FromBody] CreateShiftCode shiftCode)
        {
            try
            {
                Aggregate managerAggreate = new ShiftCodeAggregate();
                CommandHandler.ActivateCommand(shiftCode, managerAggreate);
                return Ok(shiftCode.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
    }
}
