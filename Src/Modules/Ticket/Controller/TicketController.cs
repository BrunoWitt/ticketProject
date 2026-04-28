using Microsoft.AspNetCore.Mvc;
using Src.Modules.Ticket.Service;
using Src.Modules.Ticket.DTOs;
using Src.Modules.Ticket.Models;
using Src.Modules.User.Models;
using Src.Shared.Base;
using System.Security.Claims;

namespace Src.Modules.Ticket.Controller
{
    [Route("ticket")]
    public class TicketController 
        : BaseController<TicketModel, CreateTicketDTO, UpdateTicketDTO>
    {
        private readonly TicketService _ticketService;

        public TicketController(TicketService service) : base(service)
        {
            _ticketService = service;
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAllCustom()
        {
            return Ok(await _ticketService.GetAllWithSLA());
        }


        [HttpPost("assign")]
        public async Task<IActionResult> Assign([FromBody] AssignTicketDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var perfil = Enum.Parse<PerfilUsuario>(User.FindFirst(ClaimTypes.Role)!.Value);

            await _ticketService.Assign(dto.IdTicket, dto.IdAtendente, userId, perfil);

            return Ok(new { message = "Ticket atribuído com sucesso" });
        }


        [HttpPut("status")]
        public async Task<IActionResult> ChangeStatus(int id, StatusTicket status)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var perfil = Enum.Parse<PerfilUsuario>(User.FindFirst(ClaimTypes.Role)!.Value);

            await _ticketService.ChangeStatus(id, status, userId, perfil);

            return Ok();
        }


        [HttpPost("close/{id}")]
        public async Task<IActionResult> Close(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var perfil = Enum.Parse<PerfilUsuario>(User.FindFirst(ClaimTypes.Role)!.Value);

            await _ticketService.Close(id, userId, perfil);

            return Ok(new { message = "Ticket fechado" });
        }


        [HttpPost("reopen/{id}")]
        public async Task<IActionResult> Reopen(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var perfil = Enum.Parse<PerfilUsuario>(User.FindFirst(ClaimTypes.Role)!.Value);

            await _ticketService.Reopen(id, userId, perfil);

            return Ok(new { message = "Ticket reaberto" });
        }
    }
}