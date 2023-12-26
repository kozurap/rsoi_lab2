using Gateway.Dtos;
using Gateway.Services;
using Kernel.AbstractClasses;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly TicketService _ticketService;

        public TicketsController()
        {
            _ticketService = new TicketService(new PrivilegeService(), new FlightService());
        }

        [HttpGet]
        public async Task<ActionResult<PaginationModel<TicketDto>>> GetAll([FromQuery, Required] int page, [FromQuery, Required] int size, [FromHeader(Name = HeaderConstant.UserName)] string userName)
        {
            return Ok(await _ticketService.GetAllTicketsAsync(page, size, userName));
        }

        [HttpPost]
        public async Task<ActionResult<TicketPurchaseDto>> PurchaseTicket(BuyTicketDto model, [FromHeader(Name = HeaderConstant.UserName)] string userName)
        {
            return Ok(await _ticketService.BuyTicketsAsync(model.Flightnumber, model.Price, model.Paidfrombalance, userName));
        }

        [HttpGet("{ticketUid}")]
        public async Task<ActionResult<TicketDto>> GetByUid(string ticketUid, [FromHeader(Name = HeaderConstant.UserName)] string userName)
        {
            return Ok(await _ticketService.GetTicketByUidAsync(ticketUid));
        }

        [HttpDelete("{ticketUid}")]
        public async Task<ActionResult<TicketDto>> CancelTicket(string ticketUid, [FromHeader(Name = HeaderConstant.UserName)] string userName)
        {
            await _ticketService.ReturnTicketsAsync(ticketUid, userName);
            return NoContent();
        }
    }
}
