using Microsoft.AspNetCore.Mvc;
using Src.Modules.Ticket.Service;
using Src.Modules.Ticket.DTOs;
using Src.Modules.Ticket.Models;

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
            await _service.Assign(dto.IdTicket, dto.IdAtendente);
            return Ok();
        }
    }
}