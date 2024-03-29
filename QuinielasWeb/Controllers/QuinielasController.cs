﻿using Microsoft.AspNetCore.Mvc;
using QuinielasWeb.Services;
using QuinielasModel;
using QuinielasModel.DTO.Pools;
using QuinielasModel.DTO.Predictions;
using Microsoft.AspNetCore.Authorization;

namespace QuinielasWeb.Controllers
{
    [Authorize]
    public class QuinielasController : Controller
    {
        private readonly ILogger<QuinielasController> _logger;
        private readonly PoolsService _poolsService;

        public QuinielasController(ILogger<QuinielasController> logger, PoolsService poolsService)
        {
            _logger = logger;
            _poolsService = poolsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            ViewBag.User = HttpContext.Session.GetString("username");
            var pools = await _poolsService.GetOtherPools((int)userid);
            return View(pools);
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            ViewBag.User = HttpContext.Session.GetString("username");
            var pools = await _poolsService.GetMyPools((int)userid);
            return View(pools);
        }

        [HttpGet]
        public async Task<IActionResult> Join()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            ViewBag.User = HttpContext.Session.GetString("username");
            var pools = await _poolsService.GetNewPools((int)userid);
            return View(pools);
        }

        [HttpPost]
        public async Task<IActionResult> Join(int id, string? password)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var pools = await _poolsService.GetNewPools((int)userid);
            var result = await _poolsService.Join(new UserPool { PoolId = id, UserId = (int)userid!, Password = password });
            ViewBag.User = HttpContext.Session.GetString("username");
            ViewBag.Alert = result.Alert!.Alert;
            ViewBag.AlertIcon = result.Alert.AlertIcon;
            ViewBag.AlertMessage = result.Alert.AlertMessage;
            ViewBag.RedirectUrl = result.Alert.RedirectUrl;
            return View(pools);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            ViewBag.User = HttpContext.Session.GetString("username");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Pool pool)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            pool.AdminId = (int)userid;
            var result = await _poolsService.Create(pool);
            ViewBag.User = HttpContext.Session.GetString("username");
            ViewBag.Alert = result.Alert!.Alert;
            ViewBag.AlertIcon = result.Alert.AlertIcon;
            ViewBag.AlertMessage = result.Alert.AlertMessage;
            ViewBag.RedirectUrl = result.Alert.RedirectUrl;
            return View(pool);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            ViewBag.User = HttpContext.Session.GetString("username");
            var pool = await _poolsService.GetPoolUpdateInfo(id);
            return View(pool);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdatePool pool)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var result = await _poolsService.Edit(pool);
            ViewBag.User = HttpContext.Session.GetString("username");
            ViewBag.Alert = result.Alert!.Alert;
            ViewBag.AlertIcon = result.Alert.AlertIcon;
            ViewBag.AlertMessage = result.Alert.AlertMessage;
            ViewBag.RedirectUrl = result.Alert.RedirectUrl;
            return View(pool);
        }

        [HttpPost]
        public async Task<IActionResult> Public(int id)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var result = await _poolsService.MakePublic(id);
            return new JsonResult(result.Alert);
        }

        [HttpPost]
        public async Task<IActionResult> Private(int id, string password)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var result = await _poolsService.MakePrivate(id, password);
            ViewBag.User = HttpContext.Session.GetString("username");
            ViewBag.Alert = result.Alert!.Alert;
            ViewBag.AlertIcon = result.Alert.AlertIcon;
            ViewBag.AlertMessage = result.Alert.AlertMessage;
            ViewBag.RedirectUrl = result.Alert.RedirectUrl;
            var pool = await _poolsService.GetPool(id, (int)userid);
            if (pool == null)
                return NotFound();
            if (pool.IsAdmin || pool.IsParticipant)
                return View("Quiniela", pool);
            return Unauthorized();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(int id, string password)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var result = await _poolsService.ChangePassword(id, password);
            ViewBag.User = HttpContext.Session.GetString("username");
            ViewBag.Alert = result.Alert!.Alert;
            ViewBag.AlertIcon = result.Alert.AlertIcon;
            ViewBag.AlertMessage = result.Alert.AlertMessage;
            ViewBag.RedirectUrl = result.Alert.RedirectUrl;
            var pool = await _poolsService.GetPool(id, (int)userid);
            if (pool == null)
                return NotFound();
            if (pool.IsAdmin || pool.IsParticipant)
                return View("Quiniela", pool);
            return Unauthorized();
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var result = await _poolsService.Leave(new UserPool { PoolId = id, UserId = (int)userid! });
            return new JsonResult(result.Alert!);
        }

        [HttpGet]
        public async Task<IActionResult> Quiniela(int id)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            ViewBag.User = HttpContext.Session.GetString("username");
            var pool = await _poolsService.GetPool(id, (int)userid);
            if (pool == null)
                return NotFound();
            if (pool.IsAdmin || pool.IsParticipant)
                return View(pool);
            return Unauthorized();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var result = await _poolsService.DeletePool(id);
            return new JsonResult(result.Alert);
        }
    }
}
