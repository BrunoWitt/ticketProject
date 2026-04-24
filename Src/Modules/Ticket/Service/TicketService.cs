using Src.Modules.Ticket.Repository;
using Src.Modules.Ticket.Models;
using Src.Modules.Ticket.DTOs;
using Src.Modules.User.Models;
using System.Numerics;

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
            var ticket = new TicketModel
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Prioridade = dto.Prioridade,
                IdUsuario = dto.IdUsuario,
                IdCategoria = dto.IdCategoria,
                Status = StatusTicket.aberto,
                DataHoraCriado = DateTime.UtcNow
            };

            await _repo.CreateAsync(ticket);
        }

        public async Task<List<TicketModel>> GetAll()
            => (await _repo.GetAllAsync()).ToList();

        public async Task<TicketModel?> GetById(int id)
            => await _repo.GetByIdAsync(id);

        public async Task Update(UpdateTicketDTO dto)
        {
            var ticket = await _repo.GetByIdAsync(dto.Id);

            if (ticket == null)
                throw new Exception("Ticket não encontrado");

            if (dto.Titulo != null)
                ticket.Titulo = dto.Titulo;

            if (dto.Descricao != null)
                ticket.Descricao = dto.Descricao;

            if (dto.Prioridade.HasValue)
                ticket.Prioridade = dto.Prioridade.Value;

            await _repo.UpdateAsync(ticket);
        }

        public async Task Delete(BigInteger id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task Assign(int idTicket, int idAtendente, int usuarioLogadoId, PerfilUsuario perfil)
        {
            if (perfil != PerfilUsuario.atendente)
                throw new Exception("Apenas atendentes podem assumir tickets");

            var ticket = await _repo.GetByIdAsync((int)(object)idTicket);

            if (ticket == null)
                throw new Exception("Ticket não encontrado");

            if (ticket.IdAtendente != null)
                throw new Exception("Ticket já possui um responsável");

            if (ticket.Status == StatusTicket.fechado)
                throw new Exception("Não é possível assumir um ticket fechado");

            ticket.IdAtendente = idAtendente;
            ticket.Status = StatusTicket.em_andamento;

            await _repo.UpdateAsync(ticket);
        }

        public async Task ChangeStatus(int idTicket, StatusTicket novoStatus, int userId, PerfilUsuario perfil)
        {
            var ticket = await _repo.GetByIdAsync((int)(object)idTicket);

            if (ticket == null)
                throw new Exception("Ticket não encontrado");

            if (perfil != PerfilUsuario.atendente)
                throw new Exception("Apenas atendentes podem alterar status");

            if (ticket.IdAtendente != userId)
                throw new Exception("Apenas o responsável pode alterar o status");

            if (ticket.Status == StatusTicket.fechado)
                throw new Exception("Ticket já está fechado");

            ticket.Status = novoStatus;

            await _repo.UpdateAsync(ticket);
        }

        public async Task Close(int idTicket, int userId, PerfilUsuario perfil)
        {
            var ticket = await _repo.GetByIdAsync((int)(object)idTicket);

            if (ticket == null)
                throw new Exception("Ticket não encontrado");

            if (perfil != PerfilUsuario.atendente)
                throw new Exception("Apenas atendentes podem fechar");

            if (ticket.IdAtendente != userId)
                throw new Exception("Apenas o responsável pode fechar");

            ticket.Status = StatusTicket.fechado;
            ticket.DataHoraFinalizado = DateTime.UtcNow;

            await _repo.UpdateAsync(ticket);
        }

        public async Task Reopen(int idTicket, int userId, PerfilUsuario perfil)
        {
            var ticket = await _repo.GetByIdAsync((int)(object)idTicket);

            if (ticket == null)
                throw new Exception("Ticket não encontrado");

            if (perfil != PerfilUsuario.usuario)
                throw new Exception("Apenas usuários podem reabrir");

            if (ticket.Status != StatusTicket.fechado)
                throw new Exception("Só é possível reabrir tickets fechados");

            if (ticket.IdUsuario != userId)
                throw new Exception("Você não é o dono do ticket");

            ticket.Status = StatusTicket.aberto;
            ticket.DataHoraFinalizado = null;

            await _repo.UpdateAsync(ticket);
        }

        public bool ItsLate(TicketModel ticket)
        {
            var limitHours = ticket.Prioridade switch
            {
                PrioridadeTicket.baixa => 48,
                PrioridadeTicket.media => 24,
                PrioridadeTicket.alta => 4,
                _ => 24
            };

            var deadline = ticket.DataHoraCriado.AddHours(limitHours);

            return DateTime.UtcNow > deadline && ticket.Status != StatusTicket.fechado;
        }

        public async Task<List<TicketResponseDTO>> GetAllWithSLA()
        {
            var tickets = await _repo.GetAllAsync();

            return tickets.Select(t => new TicketResponseDTO
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descricao = t.Descricao,
                Status = t.Status,
                Prioridade = t.Prioridade,
                Atrasado = ItsLate(t)
            }).ToList();
        }
    }
}