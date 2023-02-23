using System;
using System.Collections.Generic;

namespace ProyectoQuinielas.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool? Active { get; set; }

    public virtual ICollection<Pool> Pools { get; } = new List<Pool>();

    public virtual ICollection<Prediction> Predictions { get; } = new List<Prediction>();

    public virtual ICollection<Pool> PoolsNavigation { get; } = new List<Pool>();
}
