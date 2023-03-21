using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QuinielasWeb.Models;

public partial class User
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

    public virtual ICollection<Pool> Pools { get; } = new List<Pool>();

    public virtual ICollection<Prediction> Predictions { get; } = new List<Prediction>();

    public virtual ICollection<Pool> PoolsNavigation { get; } = new List<Pool>();
}
