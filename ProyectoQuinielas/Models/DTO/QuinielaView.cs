using System;
using System.Collections.Generic;

namespace ProyectoQuinielas.Models.DTO;

public class QuinielaView
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public sbyte Privada { get; set; }

    public int Participantes { get; set; }

    public int Límite { get; set; }

    public string Administrador { get; set; } = null!;

}
