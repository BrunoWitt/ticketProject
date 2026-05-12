using Src.Modules.User.DTOs;
using Src.Modules.User.Models;
using Src.Modules.User.Repository;
using Microsoft.AspNetCore.Identity;
using Src.Shared.Base;

namespace Src.Modules.User.Service
{
    public class UserService
        : BaseService<UsuarioModel, CreateUserDTO, UpdateUserDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<UsuarioModel> _hasher;

        public UserService(IUserRepository userRepository)
            : base(userRepository)
        {
            _userRepository = userRepository;
            _hasher = new PasswordHasher<UsuarioModel>();
        }

        protected override UsuarioModel MapCreate(CreateUserDTO dto)
        {
            return new UsuarioModel
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = _hasher.HashPassword(new UsuarioModel(), dto.Senha),
                Perfil = dto.PerfilUsuario,
                DataHoraCriado = DateTimeOffset.UtcNow
            };
        }

        protected override UsuarioModel MapUpdate(UpdateUserDTO dto)
        {
            return new UsuarioModel
            {
                Id = dto.Id,
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = _hasher.HashPassword(new UsuarioModel(), dto.Senha),
                Perfil = dto.PerfilUsuario
            };
        }

        public async Task<UsuarioModel?> GetUser(string Email, string Senha)
        {
            var user = await _userRepository.GetByEmail(Email);

            if (user == null)
                return null;

            var result = _hasher.VerifyHashedPassword(
                user,
                user.Senha,
                Senha
            );

            if (result == PasswordVerificationResult.Failed)
                return null;

            return user;
        }
    }
}