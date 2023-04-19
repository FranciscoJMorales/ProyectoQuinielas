using QuinielasModel;
using QuinielasModel.DTO.Users;
using QuinielasModel.DTO;

namespace QuinielasWeb.Services
{
    public class UsersService : ApiService
    {
        public UsersService(IHttpContextAccessor accessor) : base(accessor) { }

        private string Url => baseUrl + "users";

        public async Task<User> GetUser(int id)
        {
            return await Get<User>($"{Url}/{id}");
        }

        public async Task<Report?> GetUserReport(int id)
        {
            return await Get<Report?>($"{Url}/report/{id}");
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
