namespace ticketProject.src.Modules.Mensagem.DTOs
{
    public class MessageDTO
    {
        public required int IdUsuario{get;set;}
        public required int IdTicket{get;set;}
        public required string Texto{get;set;}
    }
}