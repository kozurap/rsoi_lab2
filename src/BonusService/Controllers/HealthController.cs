using Microsoft.AspNetCore.Mvc;

namespace PrivilegeService.Controllers
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
