using Gateway.Dtos;
using Gateway.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Gateway.Controllers
{
    [Route("api/v1/Auth")]
    public class AuthController : Controller
    {
        private readonly Services.AuthService _authService;
        public AuthController(Services.AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]AuthDto dto)
        {
            var res = await _authService.Login(dto);
            return Ok(res);
        }

        [HttpPost]
        [Route("register")]
        public async Task<Guid> Register([FromBody] AuthDto dto)
        {
            var res = await _authService.Register(dto);
            return res;
        }
    }
}
