using Microsoft.AspNetCore.SignalR;
using Src.Modules.Message.Service;

namespace Src.Modules.Message.Hub
{
    public class MessageHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly MessageService _service;

        public MessageHub(MessageService service)
        {
            _service = service;
        }


        public async Task JoinTicket(string ticketId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, ticketId);
        }


        public async Task SendMessage(string ticketId, string texto, int idUsuario)
        {
            var message = await _service.SendMessage(texto, idUsuario, int.Parse(ticketId));

            await Clients.Group(ticketId).SendAsync("ReceiveMessage", message);
        }
    }
}