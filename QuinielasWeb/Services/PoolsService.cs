using Newtonsoft.Json;
using QuinielasModel;
using QuinielasModel.DTO;
using System.Text;

namespace QuinielasWeb.Services
{
    public class PoolsService : ApiService
    {
        public async Task<QuinielaFull> GetPool(int id)
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var response = await _client.GetAsync(url + "pools/" + id);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<QuinielaFull>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        public async Task<IEnumerable<QuinielaView>> GetOtherPools(int userid)
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var response = await _client.GetAsync(url + "pools/other/" + userid);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<IEnumerable<QuinielaView>>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        public async Task<IEnumerable<QuinielaView>> GetMyPools(int userid)
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var response = await _client.GetAsync(url + "pools/mine/" + userid);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<IEnumerable<QuinielaView>>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        public async Task<IEnumerable<QuinielaView>> GetNewPools(int userid)
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var response = await _client.GetAsync(url + "pools/join/" + userid);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<IEnumerable<QuinielaView>>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        public async Task<Result> Create(Pool pool)
        {
            var json_ = JsonConvert.SerializeObject(pool);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var response = await _client.PostAsync(url + "pools/create", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<Result>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        public async Task<Result> Join(UserPool info)
        {
            var json_ = JsonConvert.SerializeObject(info);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var response = await _client.PostAsync(url + "pools/join", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<Result>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        public async Task<Result> Leave(UserPool info)
        {
            var json_ = JsonConvert.SerializeObject(info);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var response = await _client.PostAsync(url + "pools/leave", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<Result>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }
    }
}
