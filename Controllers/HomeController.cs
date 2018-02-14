using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Ops.Controllers
{

    [EnableCors("MyPolicy")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
