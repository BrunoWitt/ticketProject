using Src.Modules.Message.Models;
using Src.Modules.Message.Repository;
using Src.Shared.Base;

namespace Src.Modules.Message.Service
{
    public class MessageService 
        : BaseService<MessageModel, CreateMessageDTO, UpdateMessageDTO>
    {
        private readonly IMessageRepository _messageRepo;

        public MessageService(IMessageRepository repo) : base(repo)
        {
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


        public async Task<MessageModel> SendMessage(string texto, long idUsuario, long idTicket)
        {
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