using QuinielasModel;
using QuinielasModel.DTO.Games;

namespace QuinielasWeb.Services
{
    public class GamesService : ApiService
    {
        private string Url => baseUrl + "games";

        public async Task<PoolGames?> GetGames(int poolid)
        {
            return await Get<PoolGames?>($"{Url}/{poolid}");
        }

        public async Task<Result> Create(Game game)
        {
            return await Post<Result>($"{Url}/create", game);
        }

        public async Task<Result> Update(Game game)
        {
            return await Put<Result>($"{Url}/update/{game.Id}", game);
        }
    }
}
