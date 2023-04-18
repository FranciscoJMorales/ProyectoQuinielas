using QuinielasModel;
using QuinielasModel.DTO.Predictions;

namespace QuinielasWeb.Services
{
    public class PredictionsService : ApiService
    {
        public PredictionsService(IHttpContextAccessor accessor) : base(accessor) { }

        private string Url => baseUrl + "predictions";

        public async Task<IEnumerable<UserPrediction>?> GetPredictionsByGame(int gameid)
        {
            return await Get<IEnumerable<UserPrediction>?>($"{Url}/byGame/{gameid}");
        }

        public async Task<Result> SendPrediction(NewPrediction prediction)
        {
            return await Post<Result>($"{Url}/send", prediction);
        }
    }
}
