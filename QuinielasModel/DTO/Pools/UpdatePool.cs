using System.ComponentModel;

namespace QuinielasModel.DTO.Pools;

public class UpdatePool
{
    public int Id { get; set; }

    [DisplayName("Nombre de la quiniela")]
    public string Name { get; set; } = null!;

    [DisplayName("Límite de usuarios")]
    public int UsersLimit { get; set; }
}
