using Microsoft.AspNetCore.Mvc;
using ticketProject.src.Modules.TicketS.Services;
using ticketProject.src.Models;
using ticketProject.src.Services.Filter;

[ApiController]
[Route("ticket")]

public class TicketController : ControllerBase
{
    private readonly TicketService _service;

    public TicketController(TicketService service)
    {
        _service = service;
    }

    [HttpGet("all")]
    public ActionResult<List<Ticket>> GetAllTickets()
    {
        var tickets = _service.GetAllTickets();
        return Ok(tickets);
    }


    [HttpGet("user/{userId}")]
    public ActionResult<List<Ticket>> GetTicketsByUserId(TicketFilter filter)
    {
        var tickets = _service.GetTicketDB(filter);
        return Ok(tickets);
    }

    
    [HttpPost("create")]
    public ActionResult CreateTicket(Ticket ticket)
    {
        _service.CreateTicket(ticket);
        return Ok();
    }
}