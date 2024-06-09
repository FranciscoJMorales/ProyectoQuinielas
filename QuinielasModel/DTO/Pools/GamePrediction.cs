namespace QuinielasModel.DTO.Pools;

public class GamePrediction
{
    public int Id { get; set; }
    public DateTime GameDate { get; set; }
    public DateTime Deadline { get; set; }
    public string Team1 { get; set; } = null!;
    public string Team2 { get; set; } = null!;
    public int? Team1Score { get; set; }
    public int? Team2Score { get; set; }
    public int? Team1Prediction { get; set; }
    public int? Team2Prediction { get; set; }
    public int? Score { get; set; }
    public bool Available { get; set; }
}
