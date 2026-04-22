using Src.Modules.Ticket.Models;

namespace Src.Modules.Ticket.Repository
{
    public interface ITicketRepository
    {
        Task Create(string Titulo, string Descricao, PrioridadeTicket Prioridade, int IdUsuario, int? IdCategoria);

        Task<List<TicketModel>> GetAll();

        Task<TicketModel?> GetById(int id);

        Task Update(int Id, string? Titulo, string? Descricao, PrioridadeTicket? Prioridade);

        Task UpdateStatus(int idTicket, StatusTicket newStatus);

        Task Delete(int id);

        Task Assign(int ticketId, int atendenteId);
    }
}