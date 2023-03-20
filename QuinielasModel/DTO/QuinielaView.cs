namespace QuinielasModel.DTO;

public class QuinielaView : Result
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Privada { get; set; }

    public int Participantes { get; set; }

    public int Límite { get; set; }

    public string Administrador { get; set; } = null!;

}
