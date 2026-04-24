using Src.Modules.Ticket.Models;
using Src.Shared.Interfaces;

namespace Src.Modules.Ticket.Repository
{
    public interface ITicketRepository : IBaseRepository<TicketModel>
    {
        Task UpdateStatus(int idTicket, StatusTicket newStatus);

        Task Assign(int ticketId, int atendenteId);
    }
}