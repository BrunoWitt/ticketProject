using Src.Modules.User.Models;

namespace Src.Modules.User.Repository
{
    public interface IUserRepository
    {
        Task Create (string nome, string email, string senhaHash, PerfilUsuario perfilUsuario);
        Task<List<UsuarioModel>> Read();
        Task Update(int id, string nome, string email, string senhaHash, PerfilUsuario perfilUsuario);
        Task Delete(int id);
        Task<UsuarioModel?> GetByEmail(string emailInput);
    }
}