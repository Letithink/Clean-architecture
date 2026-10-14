using System.Security.Claims;
using Labo5.Application.DTOs;
using Labo5.Application.UseCases.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Labo5_LeticiaMaldonado.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LoginUseCase _loginUseCase;
    private readonly RegisterUseCase _registerUseCase;

    public AuthController(LoginUseCase loginUseCase, RegisterUseCase registerUseCase)
    {
        _loginUseCase = loginUseCase;
        _registerUseCase = registerUseCase;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginDto)
    {
        var token = await _loginUseCase.ExecuteAsync(loginDto);

        if (token == null)
        {
            return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos." });
        }

        return Ok(new
        {
            mensaje = "Inicio de sesión exitoso",
            token = token
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel registroDto)
    {
        bool creadoExitosamente = await _registerUseCase.ExecuteAsync(registroDto);

        if (!creadoExitosamente)
        {
            return BadRequest(new { mensaje = "El nombre de usuario ya se encuentra registrado." });
        }

        return Ok(new { mensaje = "Usuario creado exitosamente." });
    }

    [Authorize]
    [HttpGet("perfil")]
    public IActionResult Perfil()
    {
        var nombreUsuario = User.Identity?.Name;
        var rolUsuario = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            usuario = nombreUsuario,
            rol = rolUsuario
        });
    }
}
