namespace Labo5.Domain.Entities;

public partial class Curso
{
    public ulong IdCurso { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int Creditos { get; set; }
}
