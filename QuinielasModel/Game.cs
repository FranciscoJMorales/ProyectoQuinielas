namespace QuinielasModel;

public class Game : Result
{
    public int Id { get; set; }

    public int PoolId { get; set; }

    public string Team1 { get; set; } = null!;

    public string Team2 { get; set; } = null!;

    public int? Team1Score { get; set; }

    public int? Team2Score { get; set; }

    public DateTime GameDate { get; set; }

    public bool? Active { get; set; }
}
