using System;
using System.Collections.Generic;

namespace ProyectoQuinielas.Models;

public partial class Pool
{
    public int Id { get; set; }

    public int AdminId { get; set; }

    public string Name { get; set; } = null!;

    public bool Public { get; set; }

    public string? Password { get; set; }

    public int UsersLimit { get; set; }

    public bool? Active { get; set; }

    public virtual User Admin { get; set; } = null!;

    public virtual ICollection<Game> Games { get; } = new List<Game>();

    public virtual ICollection<User> Users { get; } = new List<User>();
}
