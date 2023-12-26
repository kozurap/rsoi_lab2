using Gateway.Dtos;
using Gateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PrivilegesController : ControllerBase
    {
        private readonly PrivilegeService _privilegeService;

        public PrivilegesController()
        {
            _privilegeService = new PrivilegeService();
        }

        [HttpGet]
        public async Task<ActionResult<UserPrivilegeDto>> Get([FromHeader(Name = HeaderConstant.UserName)] string userName)
        {
            return Ok(await _privilegeService.GetUserPrivilegeDto(userName));
        }
    }
}
