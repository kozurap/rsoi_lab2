using Microsoft.AspNetCore.Mvc;

namespace TicketService.Controllers
{
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
           => Ok();
    }
}
