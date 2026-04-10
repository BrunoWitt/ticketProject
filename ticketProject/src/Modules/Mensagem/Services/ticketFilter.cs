using ticketProject.src.Models;

namespace ticketProject.src.Services.Filter
{
    public class TicketFilter
    {
        public StatusTicket? Status {get; set;}
        public PrioridadeTicket? Prioridade { get; set;}
        public int? IdUsuario {get; set;}
        public int? IdResponsavel {get; set;}
        public int? IdCategoria {get; set;}
        public string? atribuido {get; set;}
    }
}