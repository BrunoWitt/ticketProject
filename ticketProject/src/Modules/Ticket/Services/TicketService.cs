using ticketProject.src.Models;
using ticketProject.src.Repositories;
using ticketProject.src.Services.Filter;
using ticketProject.src.Modules.Repository;

namespace ticketProject.src.Modules.TicketS.Services
{
    public class TicketService{
        private readonly ITicketRepository _repo;

        public TicketService(ITicketRepository repo)
        {
            _repo = repo;
        }

        public void CreateTicket(Ticket ticket)
        {
            _repo.CreateTicketDB(ticket);
        }


        public List<Ticket> GetAllTickets()
        {
            return _repo.ReadAllTicketsDB();
        }


        public List<Ticket> GetTicketDB(TicketFilter filter)
        {
            return _repo.GetTicketsDB(filter);
        }
    }
}