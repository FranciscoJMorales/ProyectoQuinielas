using Microsoft.AspNetCore.Mvc;
using QuinielasWeb.Models;
using QuinielasWeb.Services;
using System.Diagnostics;
using QuinielasModel.DTO.Auth;

namespace QuinielasWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AuthService _authService;
        private readonly UsersService _usersService;

        public HomeController(ILogger<HomeController> logger, AuthService authService, UsersService usersService)
        {
            _logger = logger;
            _authService = authService;
            _usersService = usersService;
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
            HttpContext.Session.SetString("token", user.Token);
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
        public async Task<IActionResult> Register(UserRegister userInfo)
        {
            if (!userInfo.Password.Equals(userInfo.ConfirmPassword))
            {
                ViewBag.Alert = "Error al registrar";
                ViewBag.AlertIcon = "error";
                ViewBag.AlertMessage = "Las contrasenas no coinciden";
                return View(userInfo);
            }
            var user = await _authService.Register(userInfo);
            if (!user.HasError)
            {
                HttpContext.Session.SetInt32("userid", user.Id);
                HttpContext.Session.SetString("username", user.Username);
                HttpContext.Session.SetString("token", user.Token);
            }
            ViewBag.Alert = user.Alert!.Alert;
            ViewBag.AlertIcon = user.Alert.AlertIcon;
            ViewBag.AlertMessage = user.Alert.AlertMessage;
            ViewBag.RedirectUrl = user.Alert.RedirectUrl;
            return View(userInfo);
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
        public async Task<IActionResult> Dashboard()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            ViewBag.User = HttpContext.Session.GetString("username");
            var report = await _usersService.GetUserReport((int)userid);
            return View(report);
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