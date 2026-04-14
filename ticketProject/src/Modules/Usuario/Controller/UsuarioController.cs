using Microsoft.AspNetCore.Mvc;
using ticketProject.src.Modules.Usuario.Services;
using ticketProject.src.Modules.Usuario.DTOs;
using ticketProject.src.Shared.Auth;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ticketProject.src.Modules.Usuario.Repository;
using ticketProject.src.Modules.Usuario.Models;

[ApiController]
[Route("usuario")]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService _service;
    private readonly Auth _auth;

    public UsuarioController(UsuarioService service, Auth auth)
    {
        _service = service;
        _auth = auth;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var usuario = await _service.Login(dto.Email, dto.Senha); //Verificação se existe o usuário e se login é ok

        if (usuario == null)
            return Unauthorized("Email ou senha inválidos");

        var token = await _auth.GenerateToken(usuario);

        return Ok(new
        {
            token,
            usuario = new
            {
                usuario.id_usuario,
                usuario.nome,
                usuario.email,
                usuario.perfil_usuario,
                usuario.data_hora_criacao,
                usuario.data_hora_delecao
            }
        });
    }


    [HttpPost("createUser")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO dto)
    {
        var usuario = await _service.CreateUser(dto.Nome, dto.Email, dto.Senha, dto.PerfilUsuario);

        if (usuario == null)
            return BadRequest("Não foi possível criar o usuário");

        return Ok(new
        {
            usuario.id_usuario,
            usuario.nome,
            usuario.email,
            usuario.perfil_usuario,
            usuario.data_hora_criacao,
            usuario.data_hora_delecao
        });
    }
}