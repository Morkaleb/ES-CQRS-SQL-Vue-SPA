using Microsoft.AspNetCore.Mvc;
using System;
using Ops.Domain;
using Ops.Infra.CommandToPublishEvent;
using Ops.Models.commands;
using Ops.Models.Commands;

namespace Ops.Controllers
{
    public class ManagerController : Controller
    {
        [HttpPost]
        [Route("api/[controller]/new")]
        public ActionResult CreateManager([FromBody]CreateManager createManager)
        {
            try
            {
                Aggregate managerAggreate = new ManagerAggregate();
                CommandHandler.ActivateCommand(createManager, managerAggreate);
                return Ok(createManager.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        [HttpPost]
        [Route("api/[controller]/setRole")]
        public ActionResult ChangeManagerRole([FromBody]ChangeManagerRole changeManagerRole)
        {
            try
            {
                Aggregate managerAggreate = new ManagerAggregate();
                CommandHandler.ActivateCommand(changeManagerRole, managerAggreate);
                return Ok(changeManagerRole.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        [HttpPost]
        [Route("api/[controller]/changeEmail")]
        public ActionResult SetEmail([FromBody]ChangeManagerEmail setManagerEmail)
        {
            try
            {
                Aggregate managerAggreate = new ManagerAggregate();
                CommandHandler.ActivateCommand(setManagerEmail, managerAggreate);
                return Ok(setManagerEmail.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
    }
}
