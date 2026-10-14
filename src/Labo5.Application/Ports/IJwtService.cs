using Labo5.Domain.Entities;

namespace Labo5.Application.Ports;

public interface IJwtService
{
    string GenerateToken(Usuario user);
}
