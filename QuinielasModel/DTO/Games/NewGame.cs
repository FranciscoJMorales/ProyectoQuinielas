using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuinielasModel.DTO.Games
{
    public class NewGame : Result
    {
        public int Id { get; set; }

        public int PoolId { get; set; }

        [DisplayName("Quiniela")]
        public string PoolName { get; set; } = null!;

        [DisplayName("Equipo 1")]
        public string Team1 { get; set; } = null!;

        [DisplayName("Equipo 2")]
        public string Team2 { get; set; } = null!;

        [DisplayName("Fecha")]
        public DateTime GameDate { get; set; }

        [DisplayName("Fecha límite para predicciones")]
        public DateTime Deadline { get; set; }
    }
}
