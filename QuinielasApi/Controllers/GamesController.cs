using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuinielasApi.Models;
using QuinielasApi.Utils;
using QuinielasModel;
using QuinielasModel.DTO.Games;
using QuinielasModel.DTO.Pools;

namespace QuinielasApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly ILogger<GamesController> _logger;
        private readonly QuinielasContext _context;

        public GamesController(ILogger<GamesController> logger, QuinielasContext context)
        {
            _context = context;
            _logger = logger;
        }

        [Route("game/{id}")]
        [HttpGet]
        public async Task<NewGame?> GetGame(int id)
        {
            var game = await _context.Games
                .Include(g => g.Pool)
                .Where(g => g.Id == id && (bool)g.Active!)
                .Select(g => new NewGame
                {
                    Id = g.Id,
                    PoolId = g.PoolId,
                    PoolName = g.Pool.Name,
                    Team1 = g.Team1,
                    Team2 = g.Team2,
                    GameDate = g.GameDate
                }).FirstOrDefaultAsync();
            return game;
        }

        [Route("{poolid}")]
        [HttpGet]
        public async Task<PoolGames?> GetPoolGames(int poolid)
        {
            var pool = await _context.Pools
                .Where(p => p.Id == poolid && (bool)p.Active!)
                .Select(p => new PoolGames
                {
                    PoolId = p.Id,
                    AdminId = p.AdminId,
                    PoolName = p.Name,
                }).FirstOrDefaultAsync();
            if (pool == null)
                return null;
            pool.Games = await _context.Games
                .Where(g => g.PoolId == poolid)
                .Select(g => new QuinielasModel.Game
                {
                    PoolId = g.PoolId,
                    Active = g.Active,
                    GameDate = g.GameDate,
                    Id = g.Id,
                    PoolName = g.Pool.Name,
                    Team1 = g.Team1,
                    Team1Score = g.Team1Score,
                    Team2 = g.Team2,
                    Team2Score = g.Team2Score
                }).ToListAsync();
            return pool;
        }

        [Route("{create}")]
        [HttpPost]
        public async Task<Result> Create(NewGame newGame)
        {
            var game = Mapper.ToDbModel(newGame);
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Game {newGame.Team1} - {newGame.Team2} created in pool {newGame.PoolName}");
            return new Result
            {
                Alert = new AlertInfo
                {
                    Alert = "Partido creado",
                    AlertIcon = "success",
                    AlertMessage = $"Has creado el partido {newGame.Team1} - {newGame.Team2} correctamente",
                    RedirectUrl = $"/games/{newGame.PoolId}"
                }
            };
        }
    }
}
