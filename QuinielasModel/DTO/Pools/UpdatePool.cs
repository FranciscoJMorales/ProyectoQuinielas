namespace QuinielasModel.DTO.Pools;

public class UpdatePool
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int UsersLimit { get; set; }
}
