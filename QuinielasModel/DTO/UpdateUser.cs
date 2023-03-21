namespace QuinielasModel.DTO;

public class UpdateUser
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
}
