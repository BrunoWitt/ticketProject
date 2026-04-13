namespace ticketProject.src.Modules.Usuario.Repository
{
    public interface IUsuarioRepository
    {
        Task<Models.Usuario?> ValidadeUserLoginDB(string emailInput, string passwordHash);
    }
}