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
        public async Task<IActionResult> Send([FromBody] MenssageDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Texto))
                return BadRequest("Texto é obrigatório");

            var message = await _service.SendMessage(
                dto.Texto,
                dto.IdUsuario,
                dto.IdTicket
            );

            return Ok(message);
        }


        [HttpGet("ticket/{idTicket}")]
        public async Task<IActionResult> GetByTicket(int idTicket)
        {
            var messages = await _service.GetMessagesByTicket(idTicket);

            return Ok(messages);
        }


        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] AnexoDTO dto)
        {
            if (dto.Arquivo == null || dto.Arquivo.Length == 0)
                return BadRequest("Arquivo inválido");

            using var memoryStream = new MemoryStream();
            await dto.Arquivo.CopyToAsync(memoryStream);

            var bytes = memoryStream.ToArray();

            await _service.Anexo(dto.IdMensagem, bytes, dto.Arquivo.ContentType);

            return Ok("Arquivo enviado com sucesso");
        }


        [HttpGet("anexo/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            var anexo = await _service.GetAnexo(id);

            return File(anexo.Arquivo, anexo.Tipo);
        }
    }
}