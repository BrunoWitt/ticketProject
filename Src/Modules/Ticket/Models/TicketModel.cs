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
    media,
    alta
}


public class TicketModel : BaseModel
{
    public string? Titulo {get;set;}
    public string? Descricao {get;set;}
    public StatusTicket Status {get; set;}
    public PrioridadeTicket Prioridade {get;set;}
    public DateTimeOffset? DataHoraFinalizado {get;set;}
    public long? IdUsuario {get;set;}
    public long? IdAtendente {get;set;}
    public long? IdCategoria {get;set;}
}


public class PageResult<T>
{
    public List<T> Data { get; set; } = new();
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}