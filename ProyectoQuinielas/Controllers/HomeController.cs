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
            return RedirectToAction("Login");
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
                .Where(u => u.Username == userid || u.Email == userid)
                .FirstOrDefault();
            if (user == null)
                return RedirectToAction("Login");
            if (Encryption.ComparePasswords(user.Password, password))
            {
                _logger.LogInformation($"{user.Username} logged in succesfully");
                return RedirectToAction("/dashboard/index");
            }
            return RedirectToAction("Login");
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
                return RedirectToAction("Register");
            var userExists = context.Users
                .Where(u => u.Username == username || u.Email == email)
                .FirstOrDefault();
            if (userExists != null)
                return RedirectToAction("Register");
            User user = new User { Username = username, Email = email, Password = Encryption.EncryptPassword(password), Active = 1 };
            context.Users.Add(user);
            context.SaveChanges();
            return RedirectToAction("Login");
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