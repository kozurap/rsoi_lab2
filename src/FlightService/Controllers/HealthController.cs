using Microsoft.AspNetCore.Mvc;

namespace FlightService.Controllers
{
    [ApiController]
    [Route("manage/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
       => Ok();
    }
}
