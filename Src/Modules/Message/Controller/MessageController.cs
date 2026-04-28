using Microsoft.AspNetCore.Mvc;
using Src.Modules.Message.Service;
using Src.Modules.Message.Models;
using Src.Shared.Base;

namespace Src.Modules.Message.Controller
{
    [ApiController]
    [Route("message")]
    public class MessageController 
        : BaseController<MessageModel, CreateMessageDTO, UpdateMessageDTO>
    {
        private readonly MessageService _service;

        public MessageController(MessageService service) : base(service)
        {
            _service = service;
        }


        [HttpGet("ticket/{idTicket}")]
        public async Task<IActionResult> GetByTicket(long idTicket)
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
        public async Task<IActionResult> Download(long id)
        {
            var anexo = await _service.GetAnexo(id);

            return File(anexo.Arquivo, anexo.Tipo);
        }
    }
}