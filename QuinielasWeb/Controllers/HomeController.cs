using Microsoft.AspNetCore.Mvc;
using QuinielasWeb.Models;
using QuinielasWeb.Services;
using System.Diagnostics;
using QuinielasModel.DTO;

namespace QuinielasWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AuthService _authService;

        public HomeController(ILogger<HomeController> logger, AuthService authService)
        {
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
        public async Task<IActionResult> Login(UserAuth credentials)
        {
            var user = await _authService.Login(credentials);
            if (user.HasError)
            {
                ViewBag.Alert = user.Alert!.Alert;
                ViewBag.AlertIcon = user.Alert.AlertIcon;
                ViewBag.AlertMessage = user.Alert.AlertMessage;
                return View(credentials);
            }
            HttpContext.Session.SetInt32("userid", user.Id);
            HttpContext.Session.SetString("username", user.Username);
            return RedirectToAction("dashboard");
        }

        [Route("/register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Route("/register")]
        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password, string password2)
        {
            if (!password.Equals(password2))
            {
                ViewBag.Alert = "Error al registrar";
                ViewBag.AlertIcon = "error";
                ViewBag.AlertMessage = "Las contrasenas no coinciden";
                return View();
            }
            var user = await _authService.Register(new QuinielasModel.User { Username = username, Email = email, Password = password });
            if (user.HasError)
            {
                ViewBag.Alert = user.Alert!.Alert;
                ViewBag.AlertIcon = user.Alert.AlertIcon;
                ViewBag.AlertMessage = user.Alert.AlertMessage;
                return View();
            }
            HttpContext.Session.SetInt32("userid", user.Id);
            HttpContext.Session.SetString("username", user.Username);
            ViewBag.Alert = $"¡Bienvenido {user.Username}!";
            ViewBag.AlertIcon = "success";
            ViewBag.AlertMessage = "Te has registrado correctamente";
            ViewBag.RedirectUrl = "/dashboard";
            return View();
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
            ViewBag.User = HttpContext.Session.GetString("username");
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