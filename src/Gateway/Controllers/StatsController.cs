using Gateway.Attributes;
using Gateway.Services;
using Kernel.AbstractClasses;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Gateway.Controllers
{
    [Route("api/v1/[controller]")]
    public class StatsController : Controller
    {
        private readonly StatsService _service;

        public StatsController()
        {
            _service = new StatsService();
        }

        [HttpGet]
        [Authorize("admin")]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            return Ok(await _service.Get());
        }
    }
}
