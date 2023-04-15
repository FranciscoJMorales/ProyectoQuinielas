using QuinielasModel;
using QuinielasModel.DTO.Pools;
using QuinielasModel.DTO.Users;

namespace QuinielasWeb.Services
{
    public class PoolsService : ApiService
    {
        private string Url => baseUrl + "pools";

        public async Task<QuinielaFull?> GetPool(int id, int userid)
        {
            return await Get<QuinielaFull?>($"{Url}/{id}/{userid}");
        }

        public async Task<PoolId?> GetPoolId(int id)
        {
            return await Get<PoolId?>($"{Url}/id/{id}");
        }

        public async Task<bool?> IsAdmin(int id, int userId)
        {
            return await Get<bool?>($"{Url}/{id}/isAdmin/{userId}");
        }

        public async Task<IEnumerable<QuinielaView>> GetOtherPools(int userid)
        {
            return await Get<IEnumerable<QuinielaView>>($"{Url}/other/{userid}");
        }

        public async Task<IEnumerable<QuinielaView>> GetMyPools(int userid)
        {
            return await Get<IEnumerable<QuinielaView>>($"{Url}/mine/{userid}");
        }

        public async Task<IEnumerable<QuinielaView>> GetNewPools(int userid)
        {
            return await Get<IEnumerable<QuinielaView>>($"{Url}/join/{userid}");
        }

        public async Task<PoolUsers?> GetPoolUsers(int poolid)
        {
            return await Get<PoolUsers?>($"{Url}/users/{poolid}");
        }

        public async Task<Result> Create(Pool pool)
        {
            return await Post<Result>($"{Url}/create", pool);
        }

        public async Task<Result> Edit(UpdatePool pool)
        {
            return await Put<Result>($"{Url}/edit/{pool.Id}", pool);
        }

        public async Task<Result> MakePublic(int id)
        {
            return await Put<Result>($"{Url}/public/{id}", null);
        }

        public async Task<Result> MakePrivate(int id, string password)
        {
            return await Put<Result>($"{Url}/private/{id}", password);
        }

        public async Task<Result> Join(UserPool info)
        {
            return await Post<Result>($"{Url}/join", info);
        }

        public async Task<Result> Invite(InviteUser invitation)
        {
            return await Post<Result>($"{Url}/invite", invitation);
        }

        public async Task<Result> Leave(UserPool info)
        {
            return await Post<Result>($"{Url}/leave", info);
        }

        public async Task<Result> Remove(int poolid, int userid)
        {
            return await Post<Result>($"{Url}/remove/{poolid}/{userid}", null);
        }

        public async Task<Result> DeletePool(int id)
        {
            return await Delete<Result>($"{Url}/delete/{id}");
        }
    }
}
