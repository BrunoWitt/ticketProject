using ticketProject.src.Modules.Usuario.Models;
using ticketProject.src.Modules.Usuario.Repository;

namespace ticketProject.src.Modules.Usuario.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repo;

        public UsuarioService(IUsuarioRepository repo)
        {
            _repo = repo;
        }


        public async Task<Models.Usuario?> Login(string email, string senha)
        {
            var passwordHash = senha;

            var usuario = await _repo.ValidadeUserLoginDB(email, passwordHash);

            if (usuario == null)
                return null;

            return usuario;
        }
    }
}