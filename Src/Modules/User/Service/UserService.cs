using Src.Modules.User.DTOs;
using Src.Modules.User.Models;
using Src.Modules.User.Repository;
using Microsoft.AspNetCore.Identity;

namespace Src.Modules.User.Service
{
    //Criar usuário
    //Login
    public class UserService{
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<UsuarioModel> _hasher;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _hasher = new PasswordHasher<UsuarioModel>();
        }


        public async Task<UsuarioModel?> GetUser(string Email, string Senha)
        ///
        /// Usado pelo controller para verificar a existencia do usuário e devolver o modelo de usuario para o controller | controller junta isso com o jwt
        /// 
        {
            var user = await _userRepository.GetByEmail(Email);

            if (user == null){
                return null;
            }

            var result = _hasher.VerifyHashedPassword(user, user.Senha, Senha); //user serve para ter um salt diferente para cada usuario, user.senha é a senha hasheada e loginDTO.senha é a senha provida para comparação
        
            if (result == PasswordVerificationResult.Failed){
                return null;
            }

            return user;
        }


        public async Task CreateUser(string nome, string email, string senha, PerfilUsuario perfil)
        {
            var senhaHash = _hasher.HashPassword(new UsuarioModel(), senha);

            var usuario = new UsuarioModel
            {
                Nome = nome,
                Email = email,
                Senha = senhaHash,
                Perfil = perfil,
                DataHoraCriado = DateTimeOffset.UtcNow
            };

            await _userRepository.CreateAsync(usuario);
        }


        public async Task UpdateUser(int id, string nome, string email, string senha, PerfilUsuario perfil)
        {
            var senhaHash = _hasher.HashPassword(new UsuarioModel(), senha);

            var usuario = new UsuarioModel
            {
                Id = id,
                Nome = nome,
                Email = email,
                Senha = senhaHash,
                Perfil = perfil
            };

            await _userRepository.UpdateAsync(usuario);
        }


        public async Task DeleteUser(int id)
        {
            await _userRepository.DeleteAsync(id);
        }
    }
}