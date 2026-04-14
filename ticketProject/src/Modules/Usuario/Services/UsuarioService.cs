using ticketProject.src.Modules.Usuario.Models;
using ticketProject.src.Modules.Usuario.Repository;
using ticketProject.src.Modules.Usuario.DTOs;
using Microsoft.AspNetCore.Identity;

namespace ticketProject.src.Modules.Usuario.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repo;
        private readonly PasswordHasher<Models.Usuario> _hasher;

        public UsuarioService(IUsuarioRepository repo)
        {
            _repo = repo;
            _hasher = new PasswordHasher<Models.Usuario>();
        }


        public async Task<Models.Usuario?> Login(string email, string senha)
        {
            var usuario = await _repo.GetByEmail(email);

            if (usuario == null)
                return null;

            var result = _hasher.VerifyHashedPassword(usuario, usuario.senha, senha);

            if (result == PasswordVerificationResult.Failed)
                return null;

            return usuario;
        }

        public async Task<Models.Usuario?> CreateUser(string nome, string email, string senha, Models.PerfilUsuario perfil_usuario)
        {
            try
            {
                await _repo.CreateUser(nome, email, senha, perfil_usuario);
                return await _repo.GetByEmail(email);
            }
            catch
            {
                return null;
            }
        }
    }
}