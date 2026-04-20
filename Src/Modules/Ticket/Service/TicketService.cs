using Src.Modules.Ticket.Repository;
using Src.Modules.Ticket.Models;
using Src.Modules.Ticket.DTOs;
using Src.Modules.User.Models;

namespace Src.Modules.Ticket.Service
{
    public class TicketService
    {
        private readonly ITicketRepository _repo;

        public TicketService(ITicketRepository repo)
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

        public async Task Assign(int idTicket, int idAtendente, int usuarioLogadoId, PerfilUsuario perfil)
        {
            if (perfil != PerfilUsuario.atendente)
                throw new Exception("Apenas atendentes podem assumir tickets");

            var ticket = await _repo.GetById(idTicket);

            if (ticket == null)
                throw new Exception("Ticket não encontrado");

            if (ticket.IdAtendente != null)
                throw new Exception("Ticket já possui um responsável");

            if (ticket.Status == StatusTicket.fechado)
                throw new Exception("Não é possível assumir um ticket fechado");

            await _repo.Assign(idTicket, idAtendente);
        }
    }
}
