using Microsoft.AspNetCore.Mvc;

namespace QuinielasWeb.Controllers
{
    public class PredictionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
