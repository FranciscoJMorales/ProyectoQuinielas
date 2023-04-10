namespace QuinielasModel.DTO.Users;

public class UpdatePassword
{
    public string OldPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}
