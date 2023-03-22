using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QuinielasModel;

public class User : Result
{
    public int Id { get; set; }

    [Required]
    [DisplayName("Nombre de usuario")]
    public string Username { get; set; } = null!;

    [Required]
    [DisplayName("Correo electrónico")]
    public string Email { get; set; } = null!;

    [Required]
    [DisplayName("Contraseña")]
    public string Password { get; set; } = null!;

    public bool? Active { get; set; }
}
