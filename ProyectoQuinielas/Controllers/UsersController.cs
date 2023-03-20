using Microsoft.AspNetCore.Mvc;
using QuinielasWeb.Models.DTO;
using QuinielasWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using QuinielasWeb.Utils;

namespace QuinielasWeb.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly QuinielasContext _context;

        public UsersController(ILogger<UsersController> logger, QuinielasContext context)
        {
            _context = context;
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
                return RedirectToAction("login", "Home");
            var user = _context.Users.Find(userid);
            ViewBag.User = user!.Username;
            return View(user);
        }

        [HttpGet]
        public IActionResult Update()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var user = _context.Users.Find(userid);
            ViewBag.User = user!.Username;
            return View(user);
        }

        [HttpPost]
        public IActionResult Update(User user)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var userExists = _context.Users
                .Where(u => (u.Username == user.Username || u.Email == user.Email) && u.Id != userid)
                .FirstOrDefault();
            if (userExists != null)
                return RedirectToAction("update");
            var currentUser = _context.Users.Find(userid);
            currentUser!.Username = user.Username;
            currentUser!.Email = user.Email;
            _context.SaveChanges();
            return RedirectToAction("profile");
        }

        [Route("/users/change_password")]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var user = _context.Users.Find(userid);
            ViewBag.User = user!.Username;
            return View();
        }

        [Route("/users/change_password")]
        [HttpPost]
        public IActionResult ChangePassword(string old_password, string password, string password2)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            if (!password.Equals(password2))
                return RedirectToAction("change_password");
            var user = _context.Users.Find(userid);
            if (Encryption.ComparePasswords(user!.Password, old_password))
            {
                user.Password = Encryption.EncryptPassword(password);
                _context.SaveChanges();
                _logger.LogInformation($"{user.Username} updated password");
                return RedirectToAction("profile");
            }
            return RedirectToAction("change_password");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var user = _context.Users.Find(id);
            user!.Active = false;
            _context.SaveChanges();
            _logger.LogInformation($"User {user.Username} deleted");
            HttpContext.Session.Clear();
            return new JsonResult(true);
        }
    }
}
