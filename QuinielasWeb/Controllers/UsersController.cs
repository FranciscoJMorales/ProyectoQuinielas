﻿using Microsoft.AspNetCore.Mvc;
using QuinielasWeb.Services;
using QuinielasModel.DTO.Users;

namespace QuinielasWeb.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly UsersService _usersService;

        public UsersController(ILogger<UsersController> logger, UsersService usersService)
        {
            _logger = logger;
            _usersService = usersService;
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
    }
}
