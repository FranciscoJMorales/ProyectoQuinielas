﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuinielasWeb.Models;
using QuinielasWeb.Models.DTO;
using QuinielasWeb.Services;
using System.Collections;

namespace QuinielasWeb.Controllers
{
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
            var result = await _poolsService.Join(new QuinielasModel.DTO.UserPool { PoolId = id, UserId = (int)userid!, Password = password });
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
        public async Task<IActionResult> Create(QuinielasModel.Pool pool)
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


        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var result = await _poolsService.Leave(new QuinielasModel.DTO.UserPool { PoolId = id, UserId = (int)userid! });
            return new JsonResult(result.Alert!);
        }

        [HttpGet]
        public async Task<IActionResult> Quiniela(int id)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            ViewBag.User = HttpContext.Session.GetString("username");
            var pool = await _poolsService.GetPool(id);
            if (pool.AdminId == userid)
                pool.IsAdmin = true;
            if (pool.Users!.Find(u => u.Id == userid) != null)
                pool.IsParticipant = true;
            return View(pool);
        }

    }
}
