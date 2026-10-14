namespace Labo5.Application.DTOs;

public class RegisterModel
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "Estudiante";
}
