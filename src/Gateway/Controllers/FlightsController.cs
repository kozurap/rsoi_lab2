using Gateway.Dtos;
using Gateway.Services;
using Kernel.AbstractClasses;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly FlightService _flightService;
        public FlightsController()
        {
            _flightService = new FlightService();
        }

        [HttpGet]
        public async Task<ActionResult<PaginationModel<FlightDto>>> GetAll([FromQuery, Required] int page, [FromQuery, Required] int size)
        {
            return Ok(await _flightService.GetAllAsync(page, size));
        }
    }
}
