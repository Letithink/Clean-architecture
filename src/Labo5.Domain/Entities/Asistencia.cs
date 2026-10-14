namespace Labo5.Domain.Entities;

public partial class Asistencia
{
    public ulong IdAsistencia { get; set; }

    public int? IdEstudiante { get; set; }

    public int? IdCurso { get; set; }

    public DateOnly? Fecha { get; set; }

    public string? Estado { get; set; }
}
