using Microsoft.AspNetCore.Mvc;
using ProyectoQuinielas.Models.DTO;
using ProyectoQuinielas.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace ProyectoQuinielas.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public UsersController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("profile");
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login");
            QuinielasContext context = new QuinielasContext();
            var user = context.Users.Find(userid);
            ViewBag.User = user!.Username;
            IEnumerable pools = context.Pools.Include(p => p.Users).Where(p => p.AdminId == userid).Select(p => new QuinielaView { Id = p.Id, Participantes = p.Users.Count, Privada = p.Private, Administrador = p.Admin.Username, Límite = p.UsersLimit, Nombre = p.Name });
            return View(user);
        }
    }
}
