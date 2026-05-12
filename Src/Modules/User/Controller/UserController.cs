using Microsoft.AspNetCore.Mvc;
using Src.Modules.User.Models;
using Src.Modules.User.DTOs;
using Src.Modules.User.Service;
using Src.Shared.Authentication;
using Src.Modules.RefreshToken.Service;
using Src.Shared.Base;

namespace Src.Modules.User.Controller
{
    [ApiController]
    [Route("user")]
    public class UsuarioController
        : BaseController<UsuarioModel, CreateUserDTO, UpdateUserDTO>
    {
        private readonly UserService _userService;
        private readonly AuthService _authService;
        private readonly RefreshTokenService _refreshService;

        public UsuarioController(
            UserService service,
            AuthService authService,
            RefreshTokenService refreshService
        ) : base(service)
        {
            _userService = service;
            _authService = authService;
            _refreshService = refreshService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO dto)
        {
            var usuario = await _userService.GetUser(dto.Email, dto.Senha);

            if (usuario == null)
                return Unauthorized("Email ou senha inválidos");

            var accessToken = await _authService.GenerateToken(usuario);
            var refreshToken = await _refreshService.Create(usuario.Id);

            return Ok(new
            {
                accessToken,
                refreshToken = refreshToken.Token,
                usuario
            });
        }
    }
}