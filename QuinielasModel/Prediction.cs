namespace QuinielasModel;

public class Prediction : Result
{
    public int GameId { get; set; }

    public int UserId { get; set; }

    public int? Team1Score { get; set; }

    public int? Team2Score { get; set; }

    public int? Score { get; set; }

    public bool? Active { get; set; }
}
