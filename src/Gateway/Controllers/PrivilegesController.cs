using Gateway.Dtos;
using Gateway.Services;
using Kernel.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PrivilegesController : ControllerBase
    {
        private readonly PrivilegeService _privilegeService;
        private bool isCircuitOpen = false;
        private DateTime circuitOpenTime = DateTime.MinValue;
        private int attemptCount = 0;
        private const int MaxAttempts = 2;
        private const int circuitBreakerTimeSpanMilliseconds = 10000;

        public PrivilegesController()
        {
            _privilegeService = new PrivilegeService();
        }

        [HttpGet]
        public async Task<ActionResult<UserPrivilegeDto>> Get([FromHeader(Name = HeaderConstant.UserName)] string userName)
        {
            try
            {
                CheckIfCircuitBreakerTimeStampIsComplete();
                if (isCircuitOpen == false)
                {
                    return Ok(await _privilegeService.GetUserPrivilegeDto(userName));
                }
                throw new ServiceUnavaliableException("Сервис бонусов недоступен");
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
                    throw new ServiceUnavaliableException("Сервис бонусов недоступен");
                }
                throw new ServiceUnavaliableException("Сервис бонусов недоступен");
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
