using Microsoft.AspNetCore.Mvc;
using ProyectoQuinielas.Models;
using ProyectoQuinielas.Utils;
using System.Diagnostics;
using System.Xml.Linq;

namespace ProyectoQuinielas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("/")]
        public IActionResult Index()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            return RedirectToAction("dashboard");
        }

        [Route("/login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("/login")]
        [HttpPost]
        public IActionResult Login(string userid, string password, string rememberMe)
        {
            QuinielasContext context = new QuinielasContext();
            var user = context.Users
                .Where(u => (u.Username == userid || u.Email == userid) && (bool)u.Active!)
                .FirstOrDefault();
            if (user == null)
            {
                ViewBag.Alert = "Login incorrecto";
                ViewBag.AlertIcon = "error";
                ViewBag.AlertMessage = "El usuario no existe";
                return View();
            }
            if (Encryption.ComparePasswords(user.Password, password))
            {
                _logger.LogInformation($"{user.Username} logged in succesfully!");
                HttpContext.Session.SetInt32("userid", user.Id);
                return RedirectToAction("dashboard");
            }
            ViewBag.Alert = "Login incorrecto";
            ViewBag.AlertIcon = "error";
            ViewBag.AlertMessage = "La contrasena no coincide";
            return View();
        }

        [Route("/register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Route("/register")]
        [HttpPost]
        public IActionResult Register(string username, string email, string password, string password2)
        {
            QuinielasContext context = new QuinielasContext();
            if (!password.Equals(password2))
                return RedirectToAction("register");
            var userExists = context.Users
                .Where(u => u.Username == username || u.Email == email)
                .FirstOrDefault();
            if (userExists != null)
                return RedirectToAction("register");
            User user = new User { Username = username, Email = email, Password = Encryption.EncryptPassword(password) };
            context.Users.Add(user);
            context.SaveChanges();
            _logger.LogInformation($"{user.Username} registered succesfully!");
            HttpContext.Session.SetInt32("userid", user.Id);
            return RedirectToAction("dashboard");
        }

        [Route("/logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("login", "Home");
        }

        [Route("/dashboard")]
        [HttpGet]
        public IActionResult Dashboard()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            QuinielasContext context = new QuinielasContext();
            var user = context.Users.Find(userid);
            ViewBag.User = user!.Username;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}