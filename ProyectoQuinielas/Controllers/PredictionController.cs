using Microsoft.AspNetCore.Mvc;

namespace ProyectoQuinielas.Controllers
{
    public class PredictionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
