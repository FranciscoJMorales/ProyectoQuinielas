using QuinielasModel;

namespace QuinielasWeb.Services
{
    public class PredictionsService : ApiService
    {
        private string Url => baseUrl + "predictions";

        public async Task<Result> SendPrediction(Prediction prediction)
        {
            return await Post<Result>($"{Url}/create", prediction);
        }

        public async Task<Result> UpdatePrediction(Prediction prediction)
        {
            return await Put<Result>($"{Url}/update", prediction);
        }
    }
}
