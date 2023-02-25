using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoQuinielas.Models;
using ProyectoQuinielas.Models.DTO;
using ProyectoQuinielas.Queries;
using ProyectoQuinielas.Utils;
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
            IEnumerable pools = context.Pools.Include(p => p.Users).Where(p => p.Users.Contains(user)).Select(p => new QuinielaView { Id = p.Id, Participantes = p.Users.Count, Privada = p.Private, Administrador = p.Admin.Username, Límite = p.UsersLimit, Nombre = p.Name });
            return View(pools);
        }

        [HttpGet]
        public IActionResult Mine()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login");
            QuinielasContext context = new QuinielasContext();
            var user = context.Users.Find(userid);
            ViewBag.User = user!.Username;
            IEnumerable pools = context.Pools.Include(p => p.Users).Where(p => p.AdminId == userid).Select(p => new QuinielaView { Id = p.Id, Participantes = p.Users.Count, Privada = p.Private, Administrador = p.Admin.Username, Límite = p.UsersLimit, Nombre = p.Name });
            return View(pools);
        }

        [HttpGet]
        public IActionResult Join()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login");
            QuinielasContext context = new QuinielasContext();
            var user = context.Users.Find(userid);
            ViewBag.User = user!.Username;
            IEnumerable pools = context.Pools.Include(p => p.Users).Where(p => p.Users.Count < p.UsersLimit && !p.Users.Contains(user)).Select(p => new QuinielaView { Id = p.Id, Participantes = p.Users.Count, Privada = p.Private, Administrador = p.Admin.Username, Límite = p.UsersLimit, Nombre = p.Name });
            return View(pools);
        }

        [HttpPost]
        public IActionResult Join(int id, string? password)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login");
            QuinielasContext context = new QuinielasContext();
            var user = context.Users.Find(userid);
            var pool = context.Pools.Find(id);
            if (pool!.Private)
            {
                if (pool!.Password!.Equals(password))
                {
                    pool!.Users.Add(user!);
                    context.SaveChanges();
                    _logger.LogInformation($"User Id: {user.Id} joined Pool Id: {pool.Id}");
                    return RedirectToAction("");
                }
                else
                    return RedirectToAction("join");
            }
            else
            {
                pool!.Users.Add(user!);
                context.SaveChanges();
                _logger.LogInformation($"User Id: {user.Id} joined Pool Id: {pool.Id}");
            }
            return RedirectToAction("");
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Pool pool)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login");
            pool.AdminId = (int)userid;
            if (!pool.Private)
                pool.Password = null;
            else
            {
                if (string.IsNullOrEmpty(pool.Password))
                    return RedirectToAction("create");
            }
            QuinielasContext context = new QuinielasContext();
            context.Pools.Add(pool);
            context.SaveChanges();
            _logger.LogInformation($"Pool Id: {pool.Id} created");
            return RedirectToAction("mine");
        }
    }
}
