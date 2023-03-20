using Microsoft.AspNetCore.Mvc;
using QuinielasWeb.Models;
using QuinielasWeb.Services;
using QuinielasWeb.Utils;
using System.Diagnostics;
using System.Xml.Linq;
using QuinielasModel.DTO;

namespace QuinielasWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QuinielasContext _context;
        private readonly AuthService _authService;

        public HomeController(ILogger<HomeController> logger, QuinielasContext context, AuthService authService)
        {
            _context = context;
            _logger = logger;
            _authService = authService;
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
        public async Task<IActionResult> Login(string userid, string password)
        {
            var user = await _authService.Login(new UserAuth { UserEmail = userid, Password = password });
            if (user.HasError)
            {
                ViewBag.Alert = user.Alert.Alert;
                ViewBag.AlertIcon = user.Alert.AlertIcon;
                ViewBag.AlertMessage = user.Alert.AlertMessage;
                return View();
            }
            HttpContext.Session.SetInt32("userid", user.Id);
            HttpContext.Session.SetString("username", user.Username);
            return RedirectToAction("dashboard");
            //var user = _context.Users
            //    .Where(u => (u.Username == userid || u.Email == userid) && (bool)u.Active!)
            //    .FirstOrDefault();
            //if (user == null)
            //{
            //    ViewBag.Alert = "Login incorrecto";
            //    ViewBag.AlertIcon = "error";
            //    ViewBag.AlertMessage = "El usuario no existe";
            //    return View();
            //}
            //if (Encryption.ComparePasswords(user.Password, password))
            //{
            //    _logger.LogInformation($"{user.Username} logged in succesfully!");
            //    HttpContext.Session.SetInt32("userid", user.Id);
            //    return RedirectToAction("dashboard");
            //}
            //ViewBag.Alert = "Login incorrecto";
            //ViewBag.AlertIcon = "error";
            //ViewBag.AlertMessage = "La contrasena no coincide";
            //return View();
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
            if (!password.Equals(password2))
                return RedirectToAction("register");
            var usernameExists = _context.Users
                .Where(u => u.Username == username && (bool)u.Active!)
                .FirstOrDefault();
            if (usernameExists != null)
            {
                ViewBag.Alert = "Error al registrar";
                ViewBag.AlertIcon = "error";
                ViewBag.AlertMessage = "El nombre de usuario ya existe";
                return View();
            }
            var emailExists = _context.Users
                .Where(u => u.Email == email && (bool)u.Active!)
                .FirstOrDefault();
            if (emailExists != null)
            {
                ViewBag.Alert = "Error al registrar";
                ViewBag.AlertIcon = "error";
                ViewBag.AlertMessage = "El correo ya existe";
                return View();
            }
            User user = new User { Username = username, Email = email, Password = Encryption.EncryptPassword(password) };
            _context.Users.Add(user);
            _context.SaveChanges();
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
            var user = _context.Users.Find(userid);
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