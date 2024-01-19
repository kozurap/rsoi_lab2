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
        private bool isCircuitOpen = false;
        private DateTime circuitOpenTime = DateTime.MinValue;
        private int attemptCount = 0;
        private const int MaxAttempts = 2;
        private const int circuitBreakerTimeSpanMilliseconds = 10000;

        public MeController()
        {
            _ticketService = new TicketService(new PrivilegeService(), new FlightService());
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> Get([FromHeader(Name = HeaderConstant.UserName)] string userName)
        {
            try
            {
                CheckIfCircuitBreakerTimeStampIsComplete();
                if (isCircuitOpen == false)
                {
                    return Ok(await _ticketService.GetUserInfoAsync(userName));
                }
                return StatusCode(503, "Сервис недоступен");
            }
            catch (Exception ex)
            {

                //in case of exception, if the max attempt of failure is not yet reached, then increase the counter
                if (isCircuitOpen == false && attemptCount < MaxAttempts)
                {
                    attemptCount++;
                }
                //if the count of max attempt if reached, then open the circuits and retuen message that the service is not available
                if (attemptCount >= MaxAttempts)
                {
                    if (isCircuitOpen == false)
                    {
                        RecordCircuitBreakerStart();
                    }
                    return StatusCode(500, "Сервис недоступен");
                }
                return StatusCode(500, ex.Message);
            }
        }

        private void CheckIfCircuitBreakerTimeStampIsComplete()
        {
            if (isCircuitOpen == true && circuitOpenTime.AddMilliseconds(circuitBreakerTimeSpanMilliseconds) < DateTime.UtcNow)
            {
                RecordCircuitBreakerEnd();
            }
        }
        private void RecordCircuitBreakerEnd()
        {
            circuitOpenTime = DateTime.MinValue;
            isCircuitOpen = false;
            attemptCount = 0;
        }
        private void RecordCircuitBreakerStart()
        {
            circuitOpenTime = DateTime.UtcNow;
            isCircuitOpen = true;
        }
    }
}
