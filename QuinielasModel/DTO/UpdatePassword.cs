namespace QuinielasModel.DTO;

public class UpdatePassword
{
    public string OldPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}
