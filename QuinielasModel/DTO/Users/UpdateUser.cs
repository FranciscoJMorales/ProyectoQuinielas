namespace QuinielasModel.DTO.Users;

public class UpdateUser
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
}
