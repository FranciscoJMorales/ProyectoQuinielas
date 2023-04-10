using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuinielasModel.DTO.Games
{
    public class PoolGames : Result
    {
        public int PoolId { get; set; }

        public string PoolName { get; set; } = null!;

        public IEnumerable<Game>? Games { get; set; }

        public int AdminId { get; set; }
    }
}
