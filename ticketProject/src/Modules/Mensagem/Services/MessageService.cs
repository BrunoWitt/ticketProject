using ticketProject.src.Models;
using ticketProject.src.Modules.Mensagem.Repository;

namespace ticketProject.src.Modules.Mensagem.Services
{
    public class MessageService
    {
        private readonly IMessageRepository _repo;
        public MessageService(IMessageRepository repo)
        {
            _repo = repo;
        }

        
        public async Task<List<Message>> loadMessages(Ticket Ticket)
        {
            var messages = await _repo.ReadAllMessagesFromTicket(Ticket);

            return messages;
        }


        public async Task SendMessage(Message mensagem)
        {
            await _repo.SendMessage(mensagem);
        }
    }
}