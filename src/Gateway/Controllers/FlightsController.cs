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
        private bool isCircuitOpen;
        private DateTime circuitOpenTime;
        private int attemptCount;
        private const int MaxAttempts = 3;
        private const int circuitBreakerTimeSpanMilliseconds = 10000;
        public FlightsController()
        {
            _flightService = new FlightService();
            isCircuitOpen = false;
            circuitOpenTime = DateTime.MinValue;
            attemptCount = 0;
        }

        [HttpGet]
        public async Task<ActionResult<PaginationModel<FlightDto>>> GetAll([FromQuery, Required] int page, [FromQuery, Required] int size)
        {
            
            try
            {
                CheckIfCircuitBreakerTimeStampIsComplete();
                if (isCircuitOpen == false)
                {
                    return Ok(await _flightService.GetAllAsync(page, size));
                }
                return StatusCode(500, "Сервис полетов недоступен");
            }
            catch (Exception ex)
            {

                //in case of exception, if the max attempt of failure is not yet reached, then increase the counter
                if (isCircuitOpen == false && attemptCount < MaxAttempts)
                {
                    attemptCount++;
                }
                //if the count of max attempt if reached, then open the circuits and retuen message that the service is not available
                if (attemptCount > MaxAttempts)
                {
                    if (isCircuitOpen == false)
                    {
                        RecordCircuitBreakerStart();
                    }
                    return StatusCode(500, "Сервис полетов недоступен");
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
