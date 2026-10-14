using Labo5.Application.Ports;

namespace Labo5.Infrastructure.Services;

public class BCryptPasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);

    public bool VerifyPassword(string password, string passwordHash)
        => BCrypt.Net.BCrypt.Verify(password, passwordHash);
}
