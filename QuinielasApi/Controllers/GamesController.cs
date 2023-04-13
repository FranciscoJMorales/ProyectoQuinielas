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
                }).OrderBy(g => g.GameDate)
                .ToListAsync();
            return pool;
        }

        [Route("create")]
        [HttpPost]
        public async Task<Result> Create(NewGame newGame)
        {
            var game = Mapper.ToDbModel(newGame);
            game.Id = 0;
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

        [Route("update/{id}")]
        [HttpPut]
        public async Task<Result> Update(int id, NewGame newGame)
        {
            var game = await _context.Games.FindAsync(id);
            game!.Team1 = newGame.Team1;
            game.Team2 = newGame.Team2;
            game.GameDate = newGame.GameDate;
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Game {newGame.Team1} - {newGame.Team2} updated");
            return new Result
            {
                Alert = new AlertInfo
                {
                    Alert = "Partido actualizado",
                    AlertIcon = "success",
                    AlertMessage = $"Has actualizado el partido {newGame.Team1} - {newGame.Team2} correctamente",
                    RedirectUrl = $"/games/{game.PoolId}"
                }
            };
        }

        [Route("setScore/{id}")]
        [HttpPut]
        public async Task<Result> SetScore(int id, GameScore score)
        {
            var game = await _context.Games.FindAsync(id);
            game!.Team1Score = score.Team1Score;
            game.Team2Score = score.Team2Score;
            var predictions = await _context.Predictions
                .Where(p => p.GameId == game.Id)
                .ToListAsync();
            foreach (var prediction in predictions)
            {
                if (prediction.Team1Score == game.Team1Score && prediction.Team2Score == game.Team2Score)
                    prediction.Score = 5;
                else if (prediction.Team1Score > prediction.Team2Score && game.Team1Score > game.Team2Score ||
                    prediction.Team1Score < prediction.Team2Score && game.Team1Score < game.Team2Score ||
                    prediction.Team1Score == prediction.Team2Score && game.Team1Score == game.Team2Score)
                    prediction.Score = 2;
                else
                    prediction.Score = 0;
            }
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Score set: {game.Team1} {score.Team1Score} - {score.Team2Score} {game.Team2}");
            return new Result
            {
                Alert = new AlertInfo
                {
                    Alert = "Marcador actualizado",
                    AlertIcon = "success",
                    AlertMessage = $"El marcador del partido {game.Team1} - {game.Team2} se ha colocado correctamente",
                    RedirectUrl = $"/games/{game.PoolId}"
                }
            };
        }
    }
}
