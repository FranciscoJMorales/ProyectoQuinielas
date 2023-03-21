using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Newtonsoft.Json;
using System.Text;
using QuinielasModel.DTO;
using QuinielasModel;

namespace QuinielasWeb.Services
{
    public class UsersService : ApiService
    {
        public async Task<User> GetUser(int id)
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var response = await _client.GetAsync(url + "users/" + id);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        public async Task<Result> Update(UpdateUser user)
        {
            var json_ = JsonConvert.SerializeObject(user);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var response = await _client.PostAsync(url + "users/update/" + user.Id, content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<Result>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }
        
        public async Task<Result> ChangePassword(int id, UpdatePassword password)
        {
            var json_ = JsonConvert.SerializeObject(password);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var response = await _client.PostAsync(url + "users/password/" + id, content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<Result>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        public async Task<Result> DeleteUser(int id)
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var response = await _client.PostAsync(url + "users/delete/" + id, null);
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
