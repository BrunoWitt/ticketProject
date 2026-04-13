using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ticketProject.src.Modules.Mensagem.Services;
using ticketProject.src.Hubs;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IHubContext<MessageHub> _hub;

    public MessageController(IHubContext<MessageHub> hub)
    {
        _hub = hub;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] string message)
    {
        await _hub.Clients.All.SendAsync("ReceiveMessage", message);

        return Ok(new { Status = "Message sent to all clients" });
    }
}