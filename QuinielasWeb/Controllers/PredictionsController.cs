using Microsoft.AspNetCore.Mvc;
using QuinielasModel.DTO.Predictions;
using QuinielasWeb.Services;

namespace QuinielasWeb.Controllers
{
    public class PredictionsController : Controller
    {
        private readonly ILogger<PredictionsController> _logger;
        private readonly PredictionsService _predictionsService;

        public PredictionsController(ILogger<PredictionsController> logger, PredictionsService predictionsService)
        {
            _logger = logger;
            _predictionsService = predictionsService;
        }

        [HttpPost]
        public async Task<IActionResult> Send(NewPrediction prediction)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
                return RedirectToAction("login", "Home");
            prediction.UserId = (int)userid;
            var result = await _predictionsService.SendPrediction(prediction);
            return new JsonResult(result.Alert!);
        }
    }
}
