namespace Labo5.Domain.Entities;

public partial class Profesore
{
    public ulong IdProfesor { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Especialidad { get; set; }

    public string? Correo { get; set; }
}
