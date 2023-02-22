using System;
using System.Collections.Generic;

namespace ProyectoQuinielas.Models;

public partial class Game
{
    public int Id { get; set; }

    public int PoolId { get; set; }

    public string Team1 { get; set; } = null!;

    public string Team2 { get; set; } = null!;

    public int? Team1Score { get; set; }

    public int? Team2Score { get; set; }

    public DateTime GameDate { get; set; }

    public virtual Pool Pool { get; set; } = null!;

    public virtual ICollection<Prediction> Predictions { get; } = new List<Prediction>();
}
