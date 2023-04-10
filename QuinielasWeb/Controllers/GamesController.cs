using Microsoft.AspNetCore.Mvc;
using QuinielasWeb.Services;

namespace QuinielasWeb.Controllers
{
    public class GamesController : Controller
    {
        private readonly ILogger<GamesController> _logger;
        private readonly GamesService _gamesService;

        public GamesController(ILogger<GamesController> logger, GamesService gamesService)
        {
            _logger = logger;
            _gamesService = gamesService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            ViewBag.User = HttpContext.Session.GetString("username");
            var poolGames = await _gamesService.GetGames(id);
            if (poolGames == null)
                return NotFound();
            if (poolGames.AdminId != userid)
                return Unauthorized();
            return View(poolGames);
        }
    }
}
