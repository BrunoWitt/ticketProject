using ticketProject.src.Models;
using ticketProject.src.Services.Filter;

namespace ticketProject.src.Modules.Repository
{
    public interface ITicketRepository
    {
        void CreateTicketDB(Models.Ticket ticket);
        List<Models.Ticket> ReadAllTicketsDB();
        List<Models.Ticket> GetTicketsDB(TicketFilter filter);
        void UpdateStatusDB(int id_ticket, StatusTicket newStatus);
    }
}