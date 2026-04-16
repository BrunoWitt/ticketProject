using Src.Modules.Ticket.Models;

namespace Src.Modules.Ticket.Repository
{
    public interface ITicketRepository
    {
        Task Create(TicketModel ticket);

        Task<List<TicketModel>> GetAll();

        Task<TicketModel?> GetById(int id);

        Task Update(TicketModel ticket);

        Task Delete(int id);

        Task Assign(int ticketId, int atendenteId);
    }
}