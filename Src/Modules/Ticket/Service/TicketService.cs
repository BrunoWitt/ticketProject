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


        public async Task ChangeStatus(int idTicket, StatusTicket novoStatus, int userId, PerfilUsuario perfil)
        {
            var ticket = await _repo.GetById(idTicket);

            if (ticket == null)
                throw new Exception("Ticket não encontrado");

            if (perfil != PerfilUsuario.atendente)
                throw new Exception("Apenas atendentes podem alterar status");

            if (ticket.IdAtendente != userId)
                throw new Exception("Apenas o responsável pode alterar o status");

            if (ticket.Status == StatusTicket.fechado)
                throw new Exception("Ticket já está fechado");

            await _repo.UpdateStatus(idTicket, novoStatus);
        }


        public async Task Close(int idTicket, int userId, PerfilUsuario perfil)
        {
            var ticket = await _repo.GetById(idTicket);

            if (ticket == null)
                throw new Exception("Ticket não encontrado");

            if (perfil != PerfilUsuario.atendente)
                throw new Exception("Apenas atendentes podem fechar");

            if (ticket.IdAtendente != userId)
                throw new Exception("Apenas o responsável pode fechar");

            await _repo.UpdateStatus(idTicket, StatusTicket.fechado);
        }


        public async Task Reopen(int idTicket, int userId, PerfilUsuario perfil)
        {
            var ticket = await _repo.GetById(idTicket);

            if (ticket == null)
                throw new Exception("Ticket não encontrado");

            if (perfil != PerfilUsuario.usuario)
                throw new Exception("Apenas usuários podem reabrir");

            if (ticket.Status != StatusTicket.fechado)
                throw new Exception("Só é possível reabrir tickets fechados");

            if (ticket.IdUsuario != userId)
                throw new Exception("Você não é o dono do ticket");

            await _repo.UpdateStatus(idTicket, StatusTicket.aberto);
        }
    }
}
