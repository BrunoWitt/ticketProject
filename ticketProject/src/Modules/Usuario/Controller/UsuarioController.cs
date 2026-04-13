using Microsoft.AspNetCore.Mvc;
using ticketProject.src.Modules.Usuario.Services;
using ticketProject.src.Modules.Usuario.DTOs;

[ApiController]
[Route("usuario")]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService _service;

    public UsuarioController(UsuarioService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var usuario = await _service.Login(dto.Email, dto.Senha);

        if (usuario == null)
            return Unauthorized("Email ou senha inválidos");

        return Ok(usuario);
    }
}