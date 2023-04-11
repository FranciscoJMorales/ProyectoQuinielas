using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuinielasModel.DTO.Games
{
    public class GameScore : Result
    {
        public int Id { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
    }
}
