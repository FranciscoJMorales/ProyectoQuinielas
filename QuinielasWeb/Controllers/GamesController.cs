using Microsoft.AspNetCore.Mvc;
using QuinielasModel.DTO.Games;
using QuinielasWeb.Services;

namespace QuinielasWeb.Controllers
{
    public class GamesController : Controller
    {
        private readonly ILogger<GamesController> _logger;
        private readonly GamesService _gamesService;
        private readonly PoolsService _poolsService;

        public GamesController(ILogger<GamesController> logger, GamesService gamesService, PoolsService poolsService)
        {
            _logger = logger;
            _gamesService = gamesService;
            _poolsService = poolsService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            ViewBag.User = HttpContext.Session.GetString("username");
            var poolGames = await _gamesService.GetPoolGames(id);
            if (poolGames == null)
                return NotFound();
            if (poolGames.AdminId != userid)
                return Unauthorized();
            return View(poolGames);
        }

        public async Task<IActionResult> Create(int id)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            ViewBag.User = HttpContext.Session.GetString("username");
            var isAdmin = await _poolsService.IsAdmin(id, (int)userid);
            if (isAdmin == null)
                return NotFound();
            if (!(bool)isAdmin)
                return Unauthorized();
            var newGame = new NewGame { PoolId = id };
            return View(newGame);
        }

        public async Task<IActionResult> Update(int id)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            ViewBag.User = HttpContext.Session.GetString("username");
            var game = await _gamesService.GetGame(id);
            var isAdmin = await _poolsService.IsAdmin(game!.PoolId, (int)userid);
            if (isAdmin == null)
                return NotFound();
            if (!(bool)isAdmin)
                return Unauthorized();
            return View(game);
        }
    }
}
