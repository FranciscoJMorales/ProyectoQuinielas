using QuinielasModel;
using QuinielasModel.DTO.Predictions;

namespace QuinielasWeb.Services
{
    public class PredictionsService : ApiService
    {
        private string Url => baseUrl + "predictions";

        public async Task<Result> SendPrediction(NewPrediction prediction)
        {
            return await Post<Result>($"{Url}/send", prediction);
        }
    }
}
