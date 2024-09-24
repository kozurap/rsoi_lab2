using AuthorizationService.Models;
using AuthService.Services.Interfaces;
using Kernel.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/v1/user")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly LogsProducer _producer;

        public UserController(IAuthService authService, LogsProducer producer)
        {
            this.authService = authService;
            _producer = producer;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            await _producer.Produce("AuthServiceLogin");
            var result = await authService.Login(model.Login, model.Password, new List<string> {"user"});
            if(result == null)
            {
                return NotFound();
            }
            return Ok(new JwtToken() { Token = result });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] LoginModel model)
        {
            await _producer.Produce("AuthServiceRegister");
            var id = await authService.Register(model.Login, model.Password);
            return CreatedAtAction(nameof(Register), id);
        }
    }
}
