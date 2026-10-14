namespace Labo5.Domain.Entities;

public partial class Usuario
{
    public ulong IdUsuario { get; set; }

    public string Usuario1 { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public int? IdEstudiante { get; set; }

    public int? IdProfesor { get; set; }

    public DateTime? FechaCreacion { get; set; }
}
