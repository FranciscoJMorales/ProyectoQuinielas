using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuinielasApi.Models;
using QuinielasApi.Utils;
using QuinielasModel;
using QuinielasModel.DTO.Predictions;

namespace QuinielasApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PredictionsController : ControllerBase
    {
        private readonly ILogger<PredictionsController> _logger;
        private readonly QuinielasContext _context;

        public PredictionsController(ILogger<PredictionsController> logger, QuinielasContext context)
        {
            _context = context;
            _logger = logger;
        }

        [Route("send")]
        [HttpPost]
        public async Task<Result> SendPrediction(NewPrediction newPrediction)
        {
            var prediction = await _context.Predictions
                .Where(p => p.GameId == newPrediction.GameId && p.UserId == newPrediction.UserId)
                .FirstOrDefaultAsync();
            if (prediction == null)
            {
                prediction = Mapper.ToDbModel(newPrediction);
                await _context.Predictions.AddAsync(prediction);
                await _context.SaveChangesAsync();
            }
            else
            {
                prediction.Team1Score = newPrediction.Team1Score;
                prediction.Team2Score = newPrediction.Team2Score;
                await _context.SaveChangesAsync();
            }
            _logger.LogInformation($"Prediction sent by user id: {prediction.UserId} in game id: {prediction.GameId}");
            return new Result
            {
                Alert = new AlertInfo
                {
                    Alert = "Predicción enviada",
                    AlertIcon = "success",
                    AlertMessage = "Tu predicción se ha guardado correctamente"
                }
            };
        }

        [Route("byGame/{gameid}")]
        [HttpGet]
        public async Task<IEnumerable<UserPrediction>> GetPredictionsByGame(int gameid)
        {
            var predictions = await _context.Predictions
                .Where(p => p.GameId == gameid)
                .Select(p => new UserPrediction
                {
                    GameId = p.GameId,
                    User = p.User.Username,
                    Score = p.Score,
                    Team1Score = (int)p.Team1Score!,
                    Team2Score = (int)p.Team2Score!
                }).ToListAsync();
            return predictions;
        }
    }
}
