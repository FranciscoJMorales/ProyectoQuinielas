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
            return View(user);
        }

        [HttpGet]
        public IActionResult Update()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login");
            QuinielasContext context = new QuinielasContext();
            var user = context.Users.Find(userid);
            ViewBag.User = user!.Username;
            return View(user);
        }

        [HttpPut]
        public IActionResult Update(User user)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login");
            QuinielasContext context = new QuinielasContext();
            var userExists = context.Users
                .Where(u => u.Username == user.Username || u.Email == user.Email)
                .FirstOrDefault();
            if (userExists != null)
                return RedirectToAction("update");
            var currentUser = context.Users.Find(userid);
            currentUser!.Username = user.Username;
            currentUser!.Email = user.Email;
            context.SaveChanges();
            return RedirectToAction("profile");
        }

        [Route("change_password")]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login");
            return View();
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login");
            return View();
        }
    }
}
