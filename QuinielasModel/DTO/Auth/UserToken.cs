namespace QuinielasModel.DTO.Auth;

public class UserToken : Result
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Token { get; set; } = null!;
}
