using System.ComponentModel;

namespace QuinielasModel.DTO.Auth;

public class UserAuth
{
    [DisplayName("Correo o usuario")]
    public string UserEmail { get; set; } = null!;

    [DisplayName("Contraseña")]
    public string Password { get; set; } = null!;
}
