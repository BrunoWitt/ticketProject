namespace ticketProject.src.Modules.Usuario.Repository
{
    public interface IUsuarioRepository
    {
        Task<Models.Usuario?> GetByEmail(string emailInput);
        Task CreateUser(string nome, string email, string senha, Models.PerfilUsuario perfil_usuario);
    }
}