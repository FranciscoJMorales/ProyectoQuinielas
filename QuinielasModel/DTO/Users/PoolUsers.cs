namespace QuinielasModel.DTO.Users;

public class PoolUsers
{
    public int PoolId { get; set; }

    public string PoolName { get; set; } = null!;

    public IEnumerable<UserId>? Users { get; set; }

    public int AdminId { get; set; }
}
