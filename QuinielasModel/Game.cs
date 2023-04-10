using System.ComponentModel;

namespace QuinielasModel;

public class Game : Result
{
    public int Id { get; set; }

    public int PoolId { get; set; }

    [DisplayName("Quiniela")]
    public string PoolName { get; set; } = null!;

    [DisplayName("Quiniela")]
    public string Team1 { get; set; } = null!;

    [DisplayName("Quiniela")]
    public string Team2 { get; set; } = null!;

    [DisplayName("Quiniela")]
    public int? Team1Score { get; set; }

    [DisplayName("Quiniela")]
    public int? Team2Score { get; set; }

    [DisplayName("Quiniela")]
    public DateTime GameDate { get; set; }

    public bool? Active { get; set; }
}
