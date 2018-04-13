using Microsoft.AspNetCore.Mvc;
using System;
using Ops.Domain;
using Ops.Infra.CommandToPublishEvent;
using Ops.Models.commands;

namespace Ops.Controllers
{
    public class ExampleController : Controller
    {
        [HttpPost]
        [Route("api/[controller]/new")]
        public ActionResult Example([FromBody] ExampleCommand example)
        {
            try
            {
                //Commands inherit Id from their base class, this data is used to create the eventstream Id.
                //If that stream is derived from fields in the command or event other than a simple Id, this is the place to make assign it.
                Aggregate exampleAggreate = new ExampleAggregate();
                CommandHandler.ActivateCommand(example, exampleAggreate);
                return Ok(example.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
    }
}
