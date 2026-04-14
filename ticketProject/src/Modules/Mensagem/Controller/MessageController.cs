using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ticketProject.src.Modules.Mensagem.Services;
using ticketProject.src.Hubs;
using ticketProject.src.Modules.Mensagem.DTOs;
using ticketProject.src.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IHubContext<MessageHub> _hub;
    private readonly MessageService MessageService;

    public MessageController(IHubContext<MessageHub> hub, MessageService messageService)
    {
        _hub = hub;
        MessageService = messageService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] MessageDTO messageDTO)
    {
        var mensagem = new Message
        {
            id_usuario = messageDTO.IdUsuario,
            id_ticket = messageDTO.IdTicket,
            texto = messageDTO.Texto,
        };
        await MessageService.SendMessage(mensagem);
        await _hub.Clients.Group($"ticket-{messageDTO.IdTicket}").SendAsync("ReceiveMessage", messageDTO);

        return Ok(new { Status = "Message sent to all clients" });
    }


    [HttpGet("load/{ticketId}")]
    public async Task<IActionResult> LoadMessages(int ticketId, [FromServices] MessageService messageService)
    {
        var messages = await messageService.loadMessages(ticketId);
        return Ok(messages);
    }
}