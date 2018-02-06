using Microsoft.AspNetCore.Mvc;

namespace Ops.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
