using QuinielasModel;
using QuinielasModel.DTO.Games;

namespace QuinielasWeb.Services
{
    public class GamesService : ApiService
    {
        private string Url => baseUrl + "games";

        public async Task<NewGame?> GetGame(int id)
        {
            return await Get<NewGame?>($"{Url}/game/{id}");
        }

        public async Task<PoolGames?> GetPoolGames(int poolid)
        {
            return await Get<PoolGames?>($"{Url}/{poolid}");
        }

        public async Task<Result> Create(NewGame game)
        {
            return await Post<Result>($"{Url}/create", game);
        }

        public async Task<Result> Update(NewGame game)
        {
            return await Put<Result>($"{Url}/update/{game.Id}", game);
        }

        public async Task<Result> SetScore(GameScore score)
        {
            return await Put<Result>($"{Url}/setScore/{score.Id}", score);
        }
    }
}
