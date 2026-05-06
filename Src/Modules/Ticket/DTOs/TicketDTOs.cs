using Src.Modules.Ticket.Models;
using Src.Modules.User.Models;

namespace Src.Modules.Ticket.DTOs;

public class CreateTicketDTO
{
    public required string Titulo { get; set; }
    public required string Descricao { get; set; }
    public required PrioridadeTicket Prioridade { get; set; }
    public required int IdUsuario { get; set; }
    public int? IdCategoria { get; set; }
}

public class UpdateTicketDTO
{
    public required int Id { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public PrioridadeTicket? Prioridade { get; set; }
}

public class AssignTicketDTO
{
    public required int IdTicket { get; set; }
    public required int IdAtendente { get; set; }
}

public class TicketResponseDTO
{
    public long Id { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public StatusTicket Status { get; set; }
    public PrioridadeTicket Prioridade { get; set; }
    public long? IdCategoria {get;set;}
    public long? IdAtendente {get;set;}
    public DateTimeOffset DataHoraCriado {get;set;}
    public DateTimeOffset? DataHoraFinalizado {get;set;}
    public bool Atrasado { get; set; }
}


public class PaginacaoDTO
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public string? OrderBy { get; set; }
    public string? OrderDir { get; set; }
}