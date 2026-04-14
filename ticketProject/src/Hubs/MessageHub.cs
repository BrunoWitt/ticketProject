using Microsoft.AspNetCore.SignalR;

namespace ticketProject.src.Hubs
{
    public class MessageHub : Hub
    {
        public async Task JoinTicketGroup(int ticketId)
        {
            await Groups.AddToGroupAsync(
                Context.ConnectionId, 
                $"ticket-{ticketId}"
            );
        }

        public async Task LeaveTicketGroup(int ticketId)
        {
            await Groups.RemoveFromGroupAsync(
                Context.ConnectionId, 
                $"ticket-{ticketId}"
            );
        }
    }
}