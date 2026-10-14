using Labo5.Application.DTOs;
using Labo5.Application.Ports;
using Labo5.Domain.Entities;
using Labo5.Domain.Ports;

namespace Labo5.Application.UseCases.Auth;

public class LoginUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public LoginUseCase(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<string?> ExecuteAsync(LoginModel login)
    {
        var user = await _unitOfWork.Repository<Usuario>()
            .GetFirstOrDefaultAsync(u => u.Usuario1 == login.Username);

        if (user == null || !_passwordHasher.VerifyPassword(login.Password, user.PasswordHash))
            return null;

        return _jwtService.GenerateToken(user);
    }
}
