using Src.Modules.Ticket.Repository;
using Src.Modules.Ticket.Models;
using Src.Modules.Ticket.DTOs;
using Src.Modules.User.Models;
using Src.Shared.Base;
using System.Numerics;
using Src.Modules.Historico.Repository;
using Src.Modules.Historico.Service;
using Src.Modules.Historico.Models;

namespace Src.Modules.Ticket.Service
{
    public class TicketService 
        : BaseService<TicketModel, CreateTicketDTO, UpdateTicketDTO>
    {
        private readonly HistoricoService _historicoService;
        private readonly ITicketRepository _ticketRepository;

        public TicketService(
            ITicketRepository repo,
            HistoricoService historicoService
        ) : base(repo)
        {
            _ticketRepository = repo;
            _historicoService = historicoService;
        }
        
        protected override TicketModel MapCreate(CreateTicketDTO dto)
        {
            return new TicketModel
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Prioridade = dto.Prioridade,
                IdUsuario = dto.IdUsuario,
                IdCategoria = dto.IdCategoria,
                Status = StatusTicket.aberto,
                DataHoraCriado = DateTimeOffset.UtcNow
            };
        }

        protected override TicketModel MapUpdate(UpdateTicketDTO dto)
        {
            return new TicketModel
            {
                Id = dto.Id,
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Prioridade = dto.Prioridade ?? PrioridadeTicket.media
            };
        }


        public async Task Assign(long idTicket, long idAtendente, long usuarioLogadoId, PerfilUsuario perfil)
        {
            if (perfil != PerfilUsuario.atendente)
                throw new Exception("Apenas atendentes podem assumir tickets");

            var ticket = await _repo.GetByIdAsync((int)idTicket);

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


        public async Task ChangeStatus(long idTicket, StatusTicket novoStatus, long userId, PerfilUsuario perfil)
        {
            var ticket = await _repo.GetByIdAsync((int)idTicket);

            if (ticket == null)
                throw new Exception("Ticket não encontrado");

            if (perfil != PerfilUsuario.atendente)
                throw new Exception("Apenas atendentes podem alterar status");

            if (ticket.IdAtendente != userId)
                throw new Exception("Apenas o responsável pode alterar o status");

            if (ticket.Status == StatusTicket.fechado)
                throw new Exception("Ticket já está fechado");

            var statusAnterior = ticket.Status;

            ticket.Status = novoStatus;

            await _repo.UpdateAsync(ticket);

            await _historicoService.RegistrarMudanca(
                ticket.Id,
                statusAnterior,
                novoStatus
            );

            await _repo.UpdateAsync(ticket);
        }

        public async Task Close(long idTicket, long userId, PerfilUsuario perfil)
        {
            var ticket = await _repo.GetByIdAsync((int)idTicket);

            if (ticket == null)
                throw new Exception("Ticket não encontrado");

            if (perfil != PerfilUsuario.atendente)
                throw new Exception("Apenas atendentes podem fechar");

            if (ticket.IdAtendente != userId)
                throw new Exception("Apenas o responsável pode fechar");

            ticket.Status = StatusTicket.fechado;
            ticket.DataHoraFinalizado = DateTimeOffset.UtcNow;

            var statusAnterior = ticket.Status;

            ticket.Status = StatusTicket.fechado;
            ticket.DataHoraFinalizado = DateTime.UtcNow;

            await _repo.UpdateAsync(ticket);

            await _historicoService.RegistrarMudanca(
                ticket.Id,
                statusAnterior,
                StatusTicket.fechado
            );
        }


        public async Task Reopen(long idTicket, long userId, PerfilUsuario perfil)
        {
            var ticket = await _repo.GetByIdAsync((int)idTicket);

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

            var statusAnterior = ticket.Status;

            ticket.Status = StatusTicket.aberto;

            await _repo.UpdateAsync(ticket);

            await _historicoService.RegistrarMudanca(
                ticket.Id,
                statusAnterior,
                StatusTicket.aberto
            );
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

            return DateTimeOffset.UtcNow > deadline && ticket.Status != StatusTicket.fechado;
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
                IdCategoria = t.IdCategoria,
                IdAtendente = t.IdAtendente,
                DataHoraCriado = t.DataHoraCriado,
                DataHoraFinalizado = t.DataHoraFinalizado,
                Atrasado = ItsLate(t)
            }).ToList();
        }


        public async Task<PageResult<TicketResponseDTO>> GetPaged(PaginacaoDTO request)
        {
            var result = await _ticketRepository.GetPaged(request);

            foreach (var t in result.Data)
            {
                t.Atrasado = ItsLate(new TicketModel
                {
                    DataHoraCriado = t.DataHoraCriado,
                    Status = t.Status,
                    Prioridade = t.Prioridade
                });
            }

            return result;
        }
    }
}