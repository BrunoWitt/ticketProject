using Src.Modules.User.Models;
using Src.Shared.Interfaces;

namespace Src.Modules.User.Repository
{
    public interface IUserRepository : IBaseRepository<UsuarioModel>
    {
        Task<UsuarioModel?> GetByEmail(string email);
    }
}