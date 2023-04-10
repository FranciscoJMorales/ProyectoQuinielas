using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Newtonsoft.Json;
using System.Text;
using QuinielasModel;
using QuinielasModel.DTO.Users;

namespace QuinielasWeb.Services
{
    public class UsersService : ApiService
    {
        private string Url => baseUrl + "users";

        public async Task<User> GetUser(int id)
        {
            return await Get<User>($"{Url}/{id}");
        }

        public async Task<Result> Update(UpdateUser user)
        {
            return await Put<Result>($"{Url}/update/{user.Id}", user);
        }
        
        public async Task<Result> ChangePassword(int id, UpdatePassword password)
        {
            return await Put<Result>($"{Url}/password/{id}", password);
        }

        public async Task<Result> DeleteUser(int id)
        {
            return await Delete<Result>($"{Url}/delete/{id}");
        }
    }
}
