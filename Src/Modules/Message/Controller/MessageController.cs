using Microsoft.AspNetCore.Mvc;
using Src.Modules.Message.Service;
using Src.Modules.Message.Models;

namespace Src.Modules.Message.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _service;

        public MessageController(MessageService service)
        {
            _service = service;
        }


        [HttpPost]
        public async Task<IActionResult> Send([FromBody] MessageModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Texto))
                return BadRequest("Texto é obrigatório");

            var message = await _service.SendMessage(
                model.Texto,
                model.IdUsuario,
                model.IdTicket
            );

            return Ok(message);
        }


        [HttpGet("ticket/{idTicket}")]
        public async Task<IActionResult> GetByTicket(int idTicket)
        {
            var messages = await _service.GetMessagesByTicket(idTicket);

            return Ok(messages);
        }
    }
}