using System.ComponentModel;

namespace QuinielasModel;

public class Game : Result
{
    public int Id { get; set; }

    public int PoolId { get; set; }

    [DisplayName("Quiniela")]
    public string PoolName { get; set; } = null!;

    [DisplayName("Equipo 1")]
    public string Team1 { get; set; } = null!;

    [DisplayName("Equipo 2")]
    public string Team2 { get; set; } = null!;

    [DisplayName("Punteo equipo 1")]
    public int? Team1Score { get; set; }

    [DisplayName("Punteo equipo 2")]
    public int? Team2Score { get; set; }

    [DisplayName("Fecha")]
    public DateTime GameDate { get; set; }

    public bool? Active { get; set; }
}
