using QuinielasApi.Models;

namespace QuinielasApi.Utils;

public static class Mapper
{
    public static QuinielasModel.User ToModel(User user)
    {
        return new QuinielasModel.User
        {
            Id = user.Id,
            Active = user.Active,
            Email = user.Email,
            Password = user.Password,
            Username = user.Username
        };
    }

    public static Pool ToDbModel(QuinielasModel.Pool pool)
    {
        return new Pool
        {
            Id = pool.Id,
            Active = pool.Active,
            AdminId = pool.AdminId,
            Name = pool.Name,
            Password = pool.Password,
            Private = pool.Private,
            UsersLimit = pool.UsersLimit
        };
    }
}
