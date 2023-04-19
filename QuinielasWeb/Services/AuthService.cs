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
        public AuthService(IHttpContextAccessor accessor) : base(accessor) { }

        private string Url => baseUrl + "auth";

        public async Task<UserToken> Login(UserAuth userCreds)
        {
            return await PostNoAuth<UserToken>($"{Url}/login", userCreds);
        }

        public async Task<UserToken> Register(UserRegister userInfo)
        {
            return await PostNoAuth<UserToken>($"{Url}/register", userInfo);
        }
    }
}
