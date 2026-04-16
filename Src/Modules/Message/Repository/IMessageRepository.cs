using Src.Modules.Message.Models;

namespace Src.Modules.Message.Repository
{
    public interface IMessageRepository
    {
        Task Create(MessageModel message);

        Task<List<MessageModel>> GetByTicket(int ticketId);

        Task Delete(long id); 
    }
}