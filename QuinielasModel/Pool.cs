using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QuinielasModel;

public class Pool : Result
{
    public int Id { get; set; }

    public int AdminId { get; set; }

    [Required]
    [DisplayName("Nombre de la quiniela")]
    public string Name { get; set; } = null!;

    [Required]
    [DisplayName("Privada")]
    public bool Private { get; set; }

    [DisplayName("Contraseña")]
    public string? Password { get; set; }

    [Required]
    [DisplayName("Límite de usuarios")]
    public int UsersLimit { get; set; }

    [Required]
    [DisplayName("Quiero participar en mi quiniela")]
    public bool Join { get; set; }

    public bool? Active { get; set; }
}
