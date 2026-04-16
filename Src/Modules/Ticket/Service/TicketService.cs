using Src.Modules.Ticket.Repository;
using Src.Modules.Ticket.Models;
using Src.Modules.Ticket.DTOs;

namespace Src.Modules.Ticket.Service
{
    public class TicketService
    {
        private readonly TicketRepository _repo;

        public TicketService(TicketRepository repo)
        {
            _repo = repo;
        }

        public async Task Create(CreateTicketDTO dto)
        {
            await _repo.Create(dto.Titulo, dto.Descricao, dto.Prioridade, dto.IdUsuario, dto.IdCategoria);
        }

        public async Task<List<TicketModel>> GetAll()
            => await _repo.GetAll();

        public async Task<TicketModel?> GetById(int id)
            => await _repo.GetById(id);

        public async Task Update(UpdateTicketDTO dto)
        {
            await _repo.Update(dto.Id, dto.Titulo, dto.Descricao, dto.Prioridade);
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task Assign(int idTicket, int idAtendente)
        {
            // regra: ao assumir → vira EM_ANDAMENTO
            await _repo.Assign(idTicket, idAtendente);
        }
    }
}
