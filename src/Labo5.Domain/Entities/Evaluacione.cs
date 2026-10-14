namespace Labo5.Domain.Entities;

public partial class Evaluacione
{
    public ulong IdEvaluacion { get; set; }

    public int? IdEstudiante { get; set; }

    public int? IdCurso { get; set; }

    public decimal? Calificacion { get; set; }

    public DateOnly? Fecha { get; set; }
}
