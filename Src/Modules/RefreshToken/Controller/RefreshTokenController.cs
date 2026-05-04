using Microsoft.AspNetCore.Mvc;
using Src.Modules.RefreshToken.Service;
using Src.Modules.User.Repository;
using Src.Shared.Authentication;

namespace Src.Modules.RefreshToken.Controller
{
    [ApiController]
    [Route("auth")]
    public class RefreshTokenController : ControllerBase
    {
        private readonly RefreshTokenService _refreshService;
        private readonly IUserRepository _userRepository;
        private readonly AuthService _authService;

        public RefreshTokenController(
            RefreshTokenService refreshService,
            IUserRepository userRepository,
            AuthService authService
        )
        {
            _refreshService = refreshService;
            _userRepository = userRepository;
            _authService = authService;
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDTO dto)
        {
            var refresh = await _refreshService.Validate(dto.RefreshToken);

            if (refresh == null)
                return Unauthorized("Refresh token inválido");

            var usuario = await _userRepository.GetByIdAsync((int)refresh.IdUsuario);

            if (usuario == null)
                return Unauthorized();

            var newAccessToken = await _authService.GenerateToken(usuario);
            var newRefreshToken = await _refreshService.Rotate(refresh);

            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken.Token
            });
        }
    }
}