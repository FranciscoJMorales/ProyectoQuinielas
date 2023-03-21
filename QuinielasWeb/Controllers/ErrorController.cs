using Microsoft.AspNetCore.Mvc;

namespace QuinielasWeb.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error")]
        public IActionResult Index()
        {
            return View("Error");
        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    return View("NotFound");
                default:
                    return View("Error");
            }
        }
    }
}
