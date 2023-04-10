using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Newtonsoft.Json;
using System.Text;
using QuinielasModel.DTO;
using QuinielasModel;
using Microsoft.AspNetCore.Authentication;
using System.Security.Policy;
using QuinielasModel.DTO.Auth;

namespace QuinielasWeb.Services
{
    public class AuthService : ApiService
    {
        private string Url => baseUrl + "auth";

        public async Task<UserId> Login(UserAuth userCreds)
        {
            return await Post<UserId>($"{Url}/login", userCreds);
        }

        public async Task<UserId> Register(UserRegister userInfo)
        {
            return await Post<UserId>($"{Url}/register", userInfo);
        }
    }
}
