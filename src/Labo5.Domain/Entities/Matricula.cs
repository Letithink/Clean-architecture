namespace Labo5.Domain.Entities;

public partial class Matricula
{
    public ulong IdMatricula { get; set; }

    public int? IdEstudiante { get; set; }

    public int? IdCurso { get; set; }

    public string? Semestre { get; set; }
}
