using Src.Modules.Message.Models;
using Src.Shared.Interfaces;

namespace Src.Modules.Message.Repository
{
    public interface IMessageRepository : IBaseRepository<MessageModel>
    {
        Task<List<MessageModel>> GetByTicket(long ticketId);
        Task CreateAnexo(long idMensagem, byte[] arquivo, string tipo);
        Task<AnexoModel?> GetAnexo(long id);
    }
}