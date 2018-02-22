using Microsoft.AspNetCore.Mvc;
using System;
using Ops.Domain;
using Ops.Infra.CommandToPublishEvent;
using Ops.Models.commands;
using Ops.Models.events;

namespace Ops.Controllers
{
    public class RestaurantController : Controller
    {
        [HttpPost]
        [Route("api/[controller]/create")]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurant restaurant)
        {
            try
            {
                Aggregate restaurantAggreate = new RestaurantAggregate();
                CommandHandler.ActivateCommand(restaurant, restaurantAggreate);
                return Ok(restaurant.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }



        [HttpPost]
        [Route("api/[controller]/shiftrequirements")]
        public ActionResult SetShiftRequirements([FromBody] SetDailyShiftTypeRequirements scheduledailyshift)
        {
            try
            {
                Aggregate restaurantAggreate = new RestaurantAggregate();
                CommandHandler.ActivateCommand(scheduledailyshift, restaurantAggreate);
                return Ok(scheduledailyshift.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
    }
}
