using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Newtonsoft.Json;
using System.Text;
using QuinielasModel.DTO;
using QuinielasModel;

namespace QuinielasWeb.Services
{
    public class AuthService
    {
        private readonly HttpClient _client = new();
        private readonly HttpClientHandler _clientHandler = new();
        private readonly string url = "https://localhost:7242/auth/";

        public async Task<UserId> Login(UserAuth userCreds)
        {
            var json_ = JsonConvert.SerializeObject(userCreds);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var response = await _client.PostAsync(url + "login", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<UserId>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        public async Task<UserId> Register(User userCreds)
        {
            var json_ = JsonConvert.SerializeObject(userCreds);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var response = await _client.PostAsync(url + "register", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<UserId>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }
    }
}
