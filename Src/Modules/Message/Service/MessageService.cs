using Src.Modules.Message.Models;
using Src.Modules.Message.Repository;
using Src.Shared.Base;
using Src.Modules.Ticket.Repository;
using Src.Modules.User.Repository;

using Src.Modules.User.Models;
using Src.Modules.Ticket.Models;

namespace Src.Modules.Message.Service
{
    public class MessageService 
        : BaseService<MessageModel, CreateMessageDTO, UpdateMessageDTO>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepo;

        public MessageService(
            IMessageRepository repo,
            ITicketRepository ticketRepository,
            IUserRepository userRepository
        ) : base(repo)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _messageRepo = repo;
        }


        protected override MessageModel MapCreate(CreateMessageDTO dto)
        {
            return new MessageModel
            {
                Texto = dto.Texto,
                IdUsuario = dto.IdUsuario,
                IdTicket = dto.IdTicket,
                DataHoraCriado = DateTimeOffset.UtcNow
            };
        }


        protected override MessageModel MapUpdate(UpdateMessageDTO dto)
        {
            return new MessageModel
            {
                Id = dto.Id,
                Texto = dto.Texto
            };
        }


        public async Task<MessageModel> SendMessage(
            string texto,
            long idUsuario,
            long idTicket
        )
        {
            var usuario = await _userRepository.GetByIdAsync((int)idUsuario);

            if (usuario == null)
                throw new Exception("Usuário não encontrado");

            var ticket = await _ticketRepository.GetByIdAsync((int)idTicket);

            if (ticket == null)
                throw new Exception("Ticket não encontrado");

            if (usuario.Perfil == PerfilUsuario.atendente)
            {
                ticket.Status = StatusTicket.aguardando;
            }
            else
            {
                ticket.Status = StatusTicket.em_andamento;
            }

            await _ticketRepository.UpdateAsync(ticket);

            var dto = new CreateMessageDTO
            {
                Texto = texto,
                IdUsuario = idUsuario,
                IdTicket = idTicket
            };

            return await Create(dto);
        }


        public async Task<List<MessageModel>> GetMessagesByTicket(long idTicket)
        {
            return await _messageRepo.GetByTicket(idTicket);
        }


        public async Task Anexo(long idMensagem, byte[] arquivo, string tipo)
        {
            if (arquivo.Length > 5 * 1024 * 1024)
                throw new Exception("Arquivo muito grande");

            await _messageRepo.CreateAnexo(idMensagem, arquivo, tipo);
        }


        public async Task<AnexoModel> GetAnexo(long id)
        {
            var anexo = await _messageRepo.GetAnexo(id);

            if (anexo == null)
                throw new Exception("Anexo não encontrado");

            return anexo;
        }
    }
}