using Microsoft.AspNetCore.Mvc;
using QuinielasWeb.Services;
using QuinielasModel.DTO.Users;
using QuinielasModel.DTO.Pools;

namespace QuinielasWeb.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly UsersService _usersService;
        private readonly PoolsService _poolsService;

        public UsersController(ILogger<UsersController> logger, UsersService usersService, PoolsService poolsService)
        {
            _logger = logger;
            _usersService = usersService;
            _poolsService = poolsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("profile");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var token = HttpContext.Session.GetString("token");
            var user = await _usersService.GetUser((int)userid);
            ViewBag.User = HttpContext.Session.GetString("username");
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var user = await _usersService.GetUser((int)userid);
            ViewBag.User = HttpContext.Session.GetString("username");
            return View(new UpdateUser { Id = user.Id, Username = user.Username, Email = user.Email });
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateUser user)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            user.Id = (int)userid;
            var result = await _usersService.Update(user);
            ViewBag.User = HttpContext.Session.GetString("username");
            ViewBag.Alert = result.Alert!.Alert;
            ViewBag.AlertIcon = result.Alert.AlertIcon;
            ViewBag.AlertMessage = result.Alert.AlertMessage;
            ViewBag.RedirectUrl = result.Alert.RedirectUrl;
            if (!result.HasError)
                HttpContext.Session.SetString("username", user.Username);
            return View(user);
        }

        [Route("/users/change_password")]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            ViewBag.User = HttpContext.Session.GetString("username");
            return View();
        }

        [Route("/users/change_password")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string old_password, string password, string password2)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            ViewBag.User = HttpContext.Session.GetString("username");
            if (!password.Equals(password2))
            {
                ViewBag.Alert = "Error al actualizar contrasena";
                ViewBag.AlertIcon = "error";
                ViewBag.AlertMessage = "Las contrasenas no coinciden";
                return View();
            }
            var result = await _usersService.ChangePassword((int)userid, new UpdatePassword { OldPassword = old_password, NewPassword = password });
            ViewBag.Alert = result.Alert!.Alert;
            ViewBag.AlertIcon = result.Alert.AlertIcon;
            ViewBag.AlertMessage = result.Alert.AlertMessage;
            ViewBag.RedirectUrl = result.Alert.RedirectUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var result = await _usersService.DeleteUser(id);
            HttpContext.Session.Clear();
            return new JsonResult(result.Alert);
        }

        [HttpGet]
        public async Task<IActionResult> Pool(int id)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            ViewBag.User = HttpContext.Session.GetString("username");
            var poolGames = await _poolsService.GetPoolUsers(id);
            if (poolGames == null)
                return NotFound();
            if (poolGames.AdminId != userid)
                return Unauthorized();
            return View(poolGames);
        }

        [HttpPost]
        public async Task<IActionResult> Invite(InviteUser invitation)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var result = await _poolsService.Invite(invitation);
            ViewBag.User = HttpContext.Session.GetString("username");
            ViewBag.Alert = result.Alert!.Alert;
            ViewBag.AlertIcon = result.Alert.AlertIcon;
            ViewBag.AlertMessage = result.Alert.AlertMessage;
            ViewBag.RedirectUrl = result.Alert.RedirectUrl;
            var poolGames = await _poolsService.GetPoolUsers(invitation.PoolId);
            if (poolGames == null)
                return NotFound();
            if (poolGames.AdminId != userid)
                return Unauthorized();
            return View("Pool", poolGames);
        }

        [HttpPost]
        [Route("/users/remove/{poolid}/{id}")]
        public async Task<IActionResult> Remove(int poolid, int id)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var result = await _poolsService.Remove(poolid, id);
            return new JsonResult(result.Alert);
        }
    }
}
