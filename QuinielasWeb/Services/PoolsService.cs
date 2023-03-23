using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using QuinielasModel;
using QuinielasModel.DTO;
using System.Text;

namespace QuinielasWeb.Services
{
    public class PoolsService : ApiService
    {
        private string Url => baseUrl + "pools";

        public async Task<QuinielaFull?> GetPool(int id)
        {
            return await Get<QuinielaFull?>($"{Url}/{id}");
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

        public async Task<Result> Create(Pool pool)
        {
            return await Post<Result>($"{Url}/create", pool);
        }

        public async Task<Result> Join(UserPool info)
        {
            return await Post<Result>($"{Url}/join", info);
        }

        public async Task<Result> Leave(UserPool info)
        {
            return await Post<Result>($"{Url}/leave", info);
        }
    }
}
