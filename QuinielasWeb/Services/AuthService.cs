using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Newtonsoft.Json;
using System.Text;
using QuinielasModel.DTO;
using QuinielasModel;
using Microsoft.AspNetCore.Authentication;

namespace QuinielasWeb.Services
{
    public class AuthService : ApiService
    {
        public async Task<UserId> Login(UserAuth userCreds)
        {
            var json_ = JsonConvert.SerializeObject(userCreds);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var response = await _client.PostAsync(url + "auth/login", content);
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
            var response = await _client.PostAsync(url + "auth/register", content);
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
