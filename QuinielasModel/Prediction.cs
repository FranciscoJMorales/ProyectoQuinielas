namespace QuinielasModel;

public class Prediction
{
    public int GameId { get; set; }

    public int UserId { get; set; }

    public int? Team1Score { get; set; }

    public int? Team2Score { get; set; }

    public int? Score { get; set; }

    public bool? Active { get; set; }

    public Game Game { get; set; } = null!;

    public User User { get; set; } = null!;
}
