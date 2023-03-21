using Microsoft.AspNetCore.Mvc;

namespace QuinielasWeb.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
