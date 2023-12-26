using Gateway.Dtos;
using Gateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MeController : ControllerBase
    {
        private readonly TicketService _ticketService;

        public MeController()
        {
            _ticketService = new TicketService(new PrivilegeService(), new FlightService());
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> Get([FromHeader(Name = HeaderConstant.UserName)] string userName)
        {
            return Ok(await _ticketService.GetUserInfoAsync(userName));
        }
    }
}
