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

        [HttpGet]
        [Route("/games/{poolid}")]
        public async Task<IActionResult> Index(int poolid)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            ViewBag.User = HttpContext.Session.GetString("username");
            var poolGames = await _gamesService.GetPoolGames(poolid);
            if (poolGames == null)
                return NotFound();
            if (poolGames.AdminId != userid)
                return Unauthorized();
            return View(poolGames);
        }

        [HttpGet]
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
            var pool = await _poolsService.GetPoolId(id);
            var now = DateTime.Now;
            var start = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
            var newGame = new NewGame { PoolId = id, PoolName = pool!.Name, GameDate = start };
            return View(newGame);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewGame game)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var result = await _gamesService.Create(game);
            ViewBag.User = HttpContext.Session.GetString("username");
            ViewBag.Alert = result.Alert!.Alert;
            ViewBag.AlertIcon = result.Alert.AlertIcon;
            ViewBag.AlertMessage = result.Alert.AlertMessage;
            ViewBag.RedirectUrl = result.Alert.RedirectUrl;
            return View(game);
        }

        [HttpGet]
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

        [HttpPost]
        public async Task<IActionResult> Update(NewGame game)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            var result = await _gamesService.Update(game);
            ViewBag.User = HttpContext.Session.GetString("username");
            ViewBag.Alert = result.Alert!.Alert;
            ViewBag.AlertIcon = result.Alert.AlertIcon;
            ViewBag.AlertMessage = result.Alert.AlertMessage;
            ViewBag.RedirectUrl = result.Alert.RedirectUrl;
            return View(game);
        }
    }
}
