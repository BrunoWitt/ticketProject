using ticketProject.src.Models;

namespace ticketProject.src.Modules.Mensagem.Repository
{
    public interface IMessageRepository
    {
        Task SendMessage(Message mensagem);
        Task<List<Message>> ReadAllMessagesFromTicket(Ticket ticket);
    }
}