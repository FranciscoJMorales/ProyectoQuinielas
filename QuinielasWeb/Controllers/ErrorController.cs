using Microsoft.AspNetCore.Mvc;
using QuinielasWeb.Models;

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
            var userid = HttpContext.Session.GetInt32("userid");
            var error = new ErrorViewModel
            {
                StatusCode = statusCode,
                IsLoggedIn = (userid != null)
            };
            switch (statusCode)
            {
                case 401:
                    error.Status = "Unauthorized";
                    error.Message = "No puedes ver la página que solicitas";
                    break;
                case 404:
                    error.Status = "Not Found";
                    error.Message = "La página que buscas no existe";
                    break;
                default:
                    error.Status = "Error";
                    error.Message = "Ha ocurrido un error inesperado";
                    break;
            }
            return View("Error", error);
        }
    }
}
