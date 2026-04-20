using Src.Shared.Base;

namespace Src.Modules.Ticket.Models;

public enum StatusTicket
{
    aberto,
    em_andamento,
    resolvido,
    fechado
}


public enum PrioridadeTicket
{
    baixa,
    média,
    alta
}


public class TicketModel : BaseModel
{
    public string? Titulo {get;set;}
    public string? Descricao {get;set;}
    public StatusTicket Status {get; set;}
    public PrioridadeTicket Prioridade {get;set;}
    public DateTimeOffset DataHoraFinalizado {get;set;}
    public int? IdUsuario {get;set;}
    public int? IdAtendente {get;set;}
    public int? IdCategoria {get;set;}
}