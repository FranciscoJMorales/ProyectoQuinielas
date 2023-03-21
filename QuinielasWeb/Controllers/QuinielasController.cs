﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuinielasWeb.Models;
using QuinielasWeb.Models.DTO;
using System.Collections;

namespace QuinielasWeb.Controllers
{
    public class QuinielasController : Controller
    {
        private readonly ILogger<QuinielasController> _logger;
        private readonly QuinielasContext _context;

        public QuinielasController(ILogger<QuinielasController> logger, QuinielasContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var user = _context.Users.Find(userid);
            ViewBag.User = user!.Username;
            IEnumerable pools = _context.Pools.Include(p => p.Users).Where(p => p.Users.Contains(user)).Select(p => new QuinielaView { Id = p.Id, Participantes = p.Users.Count, Privada = p.Private, Administrador = p.Admin.Username, Límite = p.UsersLimit, Nombre = p.Name });
            return View(pools);
        }

        [HttpGet]
        public IActionResult Mine()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var user = _context.Users.Find(userid);
            ViewBag.User = user!.Username;
            IEnumerable pools = _context.Pools.Include(p => p.Users).Where(p => p.AdminId == userid).Select(p => new QuinielaView { Id = p.Id, Participantes = p.Users.Count, Privada = p.Private, Administrador = p.Admin.Username, Límite = p.UsersLimit, Nombre = p.Name });
            return View(pools);
        }

        [HttpGet]
        public IActionResult Join()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var user = _context.Users.Find(userid);
            ViewBag.User = user!.Username;
            IEnumerable pools = _context.Pools.Include(p => p.Users).Where(p => p.Users.Count < p.UsersLimit && !p.Users.Contains(user)).Select(p => new QuinielaView { Id = p.Id, Participantes = p.Users.Count, Privada = p.Private, Administrador = p.Admin.Username, Límite = p.UsersLimit, Nombre = p.Name });
            return View(pools);
        }

        [HttpPost]
        public IActionResult Join(int id, string? password)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var user = _context.Users.Find(userid);
            var pool = _context.Pools.Find(id);
            if (pool!.Private)
            {
                if (pool!.Password!.Equals(password))
                {
                    pool!.Users.Add(user!);
                    _context.SaveChanges();
                    _logger.LogInformation($"User Id: {user!.Id} joined Pool Id: {pool.Id}");
                    return RedirectToAction("");
                }
                else
                    return RedirectToAction("join");
            }
            else
            {
                pool!.Users.Add(user!);
                _context.SaveChanges();
                _logger.LogInformation($"User Id: {user!.Id} joined Pool Id: {pool.Id}");
            }
            return RedirectToAction("");
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Pool pool)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            pool.AdminId = (int)userid;
            if (!pool.Private)
                pool.Password = null;
            else
            {
                if (string.IsNullOrEmpty(pool.Password))
                    return RedirectToAction("create");
            }
            _context.Pools.Add(pool);
            _context.SaveChanges();
            _logger.LogInformation($"Pool Id: {pool.Id} created");
            return RedirectToAction("mine");
        }


        [HttpPost]
        public IActionResult Leave(int id)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var user = _context.Users.Where(u => u.Id == userid)
                         .Include(u => u.PoolsNavigation)
                         .SingleOrDefault();
            var pool = _context.Pools.Find(id);
            user!.PoolsNavigation.Remove(pool!);
            pool!.Users.Remove(user);
            //_context.Pools.Add(pool);
            _context.SaveChanges();
            _logger.LogInformation($"User {user.Username} left pool {pool.Name}");
            return new JsonResult(true);
        }

        [HttpGet]
        public IActionResult Quiniela(int id)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var user = _context.Users.Find(userid);
            ViewBag.User = user!.Username;
            var poolInfo = _context.Pools.Find(id);
            var pool = _context.Pools
                .Include(p => p.Users)
                .Where(p => p.Id == id)
                .Select(p => new QuinielaFull {
                    Id = p.Id,
                    Participantes = p.Users.Count,
                    Privada = p.Private,
                    AdminId = p.AdminId,
                    Administrador = p.Admin.Username,
                    Users = p.Users.ToList(),
                    Límite = p.UsersLimit,
                    Nombre = p.Name
                }).First();
            pool.UsersScore = _context.Users
                .Include(u => u.Predictions)
                .Include(u => u.PoolsNavigation)
                .Where(u => u.PoolsNavigation.Contains(poolInfo))
                .Select(u => new UserScore {
                    Usuario = u.Username,
                    Puntuación = (int)u.Predictions.Sum(p => p.Score)
                }).ToList();
            if (pool.AdminId == userid)
                pool.IsAdmin = true;
            if (pool.Users.Contains(user))
                pool.IsParticipant = true;
            return View(pool);
        }

    }
}