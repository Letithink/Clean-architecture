namespace Labo5.Domain.Entities;

public partial class Materia
{
    public ulong IdMateria { get; set; }

    public int? IdCurso { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }
}
