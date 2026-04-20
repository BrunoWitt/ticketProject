using Microsoft.AspNetCore.Mvc;
using Src.Modules.Ticket.Service;
using Src.Modules.Ticket.DTOs;
using Src.Modules.Ticket.Models;
using Src.Modules.User.Models;
using System.Security.Claims;

namespace Src.Modules.Ticket.Controller
{
    [ApiController]
    [Route("ticket")]
    public class TicketController : ControllerBase
    {
        private readonly TicketService _service;

        public TicketController(TicketService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateTicketDTO dto)
        {
            await _service.Create(dto);
            return Ok();
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<TicketModel>>> GetAll()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketModel>> GetById(int id)
        {
            var ticket = await _service.GetById(id);

            if (ticket == null)
                return NotFound();

            return Ok(ticket);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateTicketDTO dto)
        {
            await _service.Update(dto);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return NoContent();
        }

        [HttpPost("assign")]
        public async Task<IActionResult> Assign([FromBody] AssignTicketDTO dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var roleClaim = User.FindFirst(ClaimTypes.Role);

            if (userIdClaim == null || roleClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);
            var perfil = Enum.Parse<PerfilUsuario>(roleClaim.Value);

            await _service.Assign(dto.IdTicket, dto.IdAtendente, userId, perfil);

            return Ok(new { message = "Ticket atribuído com sucesso" });
        }
    }
}