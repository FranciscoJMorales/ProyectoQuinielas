namespace QuinielasWeb.Models.DTO
{
    public class QuinielaFull
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public bool Privada { get; set; }

        public int Participantes { get; set; }

        public int Límite { get; set; }

        public int AdminId { get; set; }

        public string Administrador { get; set; } = null!;

        public List<User> Users { get; set; }

        public List<UserScore> UsersScore { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsParticipant { get; set; }

    }
}
