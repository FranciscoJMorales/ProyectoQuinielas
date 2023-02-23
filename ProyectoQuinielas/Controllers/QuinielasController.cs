using Microsoft.AspNetCore.Mvc;

namespace ProyectoQuinielas.Controllers
{
    public class QuinielasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
