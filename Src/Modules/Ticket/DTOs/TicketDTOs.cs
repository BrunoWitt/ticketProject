using Src.Modules.Ticket.Models;

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