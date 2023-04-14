namespace QuinielasModel.DTO.Predictions;

public class UserPrediction
{
    public int GameId { get; set; }
    public string User { get; set; } = null!;
    public int Team1Score { get; set; }
    public int Team2Score { get; set; }
    public int? Score { get; set; }
}
