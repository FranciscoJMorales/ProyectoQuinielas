using QuinielasModel.DTO.Pools;
using QuinielasModel.DTO.Users;

namespace QuinielasModel.DTO;

public class Report : Result
{
    public int UserId { get; set; }
    public int OwnedPools { get; set; }
    public int ParticipantPools { get; set; }
    public int PredictionsSent { get; set; }
    public int TotalScore { get; set; }
    public List<PoolScore>? Scores { get; set; }
}
