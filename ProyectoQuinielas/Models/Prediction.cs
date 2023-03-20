using System;
using System.Collections.Generic;

namespace QuinielasWeb.Models;

public partial class Prediction
{
    public int GameId { get; set; }

    public int UserId { get; set; }

    public int? Team1Score { get; set; }

    public int? Team2Score { get; set; }

    public int? Score { get; set; }

    public bool? Active { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
