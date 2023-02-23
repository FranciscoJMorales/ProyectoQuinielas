using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoQuinielas.Models;
using ProyectoQuinielas.Models.DTO;
using ProyectoQuinielas.Queries;
using System.Collections;

namespace ProyectoQuinielas.Controllers
{
    public class QuinielasController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public QuinielasController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login");
            QuinielasContext context = new QuinielasContext();
            var user = context.Users.Find(userid);
            ViewBag.User = user!.Username;
            IEnumerable pools = context.Pools.Include(p => p.Users).Select(p => new QuinielaView { Id = p.Id, Participantes = p.Users.Count, Privada = p.Public, Administrador = p.Admin.Username, Límite = p.UsersLimit, Nombre = p.Name });
            // var pools = context.Database.SqlQuery<QuinielaView>(qPools.GetAll(userid)).ToList();
            return View(pools);
        }
    }
}
