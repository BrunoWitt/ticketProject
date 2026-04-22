using Src.Modules.Message.Models;
using Src.Modules.Message.Repository;

namespace Src.Modules.Message.Service
{
    public class MessageService
    {
        private readonly IMessageRepository _repo;

        public MessageService(IMessageRepository repo)
        {
            _repo = repo;
        }

        public async Task<MessageModel> SendMessage(string texto, int idUsuario, int idTicket)
        {
            var message = new MessageModel
            {
                Texto = texto,
                IdUsuario = idUsuario,
                IdTicket = idTicket
            };

            await _repo.Create(message);

            return message;
        }


        public async Task<List<MessageModel>> GetMessagesByTicket(int idTicket)
        {
            return await _repo.GetByTicket(idTicket);
        }


        public async Task Anexo(int idMensagem, byte[] arquivo, string tipo)
        {
            if (arquivo.Length > 5 * 1024 * 1024)
                throw new Exception ("Arquivo muito grande");

            await _repo.CreateAnexo(idMensagem, arquivo, tipo);
        }


        public async Task<AnexoModel> GetAnexo(int id)
        {
            var anexo = await _repo.GetAnexo(id);

            if (anexo == null)
                throw new Exception("Anexo não encontrado");

            return anexo;
        }
        
    }   
}