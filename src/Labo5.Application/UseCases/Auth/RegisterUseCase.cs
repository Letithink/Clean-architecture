using Labo5.Application.DTOs;
using Labo5.Application.Ports;
using Labo5.Domain.Entities;
using Labo5.Domain.Ports;

namespace Labo5.Application.UseCases.Auth;

public class RegisterUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUseCase(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> ExecuteAsync(RegisterModel model)
    {
        if (await _unitOfWork.Repository<Usuario>().ExistsAsync(u => u.Usuario1 == model.Username))
            return false;

        var user = new Usuario
        {
            Usuario1 = model.Username,
            PasswordHash = _passwordHasher.HashPassword(model.Password),
            Rol = model.Role
        };

        await _unitOfWork.Repository<Usuario>().InsertAsync(user);
        await _unitOfWork.Complete();
        return true;
    }
}
