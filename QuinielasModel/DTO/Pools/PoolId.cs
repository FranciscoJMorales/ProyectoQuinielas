using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuinielasModel.DTO.Pools
{
    public class PoolId : Result
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public string Name { get; set; } = null!;
    }
}
