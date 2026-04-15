using Microsoft.AspNetCore.Mvc;
using Src.Modules.User.Models;
using Src.Modules.User.DTOs;
using Src.Modules.User.Service;
using Src.Shared.Authentication;

namespace Src.Modules.User.Controller
{
    [ApiController]
    [Route("/user")]
    public class UsuarioController : ControllerBase
    {
        private readonly UserService _service;
        private readonly AuthService _authService;

        public UsuarioController(UserService service, AuthService authService)
        {
            _service = service;
            _authService = authService;
        }


        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO dto)
        {
            var usuario = await _service.getUser(dto.Email, dto.Senha);

            if (usuario == null)
                return Unauthorized("Email ou senha inválidos");

            var token = await _authService.GenerateToken(usuario);

            return Ok(new
            {
                token,
                usuario
            });
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO dto)
        {
            await _service.CreateUser(dto.Nome, dto.Email, dto.Senha, dto.PerfilUsuario);

            return Ok("Usuário criado com sucesso");
        }


        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO dto)
        {
            await _service.UpdateUser(dto.Id, dto.Nome, dto.Email, dto.Senha, dto.PerfilUsuario);

            return Ok("Usuário atualizado com sucesso");
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _service.DeleteUser(id);

            return NoContent();
        }
    }
}