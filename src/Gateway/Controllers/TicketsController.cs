using Gateway.Dtos;
using Gateway.Enums;
using Gateway.Services;
using Kernel.AbstractClasses;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly TicketService _ticketService;
        private bool isCircuitOpen = false;
        private DateTime circuitOpenTime = DateTime.MinValue;
        private int attemptCount = 0;
        private const int MaxAttempts = 2;
        private const int circuitBreakerTimeSpanMilliseconds = 10000;
        private static Queue<QueueModel> queue = new Queue<QueueModel>();

        public TicketsController()
        {
            _ticketService = new TicketService(new PrivilegeService(), new FlightService());
        }

        [HttpGet]
        public async Task<ActionResult<PaginationModel<GetTicketDto>>> GetAll([FromQuery, Required] int page, [FromQuery, Required] int size, [FromHeader(Name = HeaderConstant.UserName)] string userName)
        {
            if (queue.Any())
            {
                while(queue.Count > 0)
                {
                    var item = queue.Dequeue();
                    if(item.Enum == QueueEnum.TicketServiceFallback)
                    {
                        await _ticketService.ReturnTicketsAsync(item.TicketUid, item.UserName, queue);
                    }
                    else if(item.Enum == QueueEnum.ReservationServiceFallback)
                    {
                        await _ticketService.UpdatePrivilegesAfterTicketDelete(item.UserName, item.Ticket);
                    }
                }
            }
            try
            {
                CheckIfCircuitBreakerTimeStampIsComplete();
                if (isCircuitOpen == false)
                {
                    return Ok(await _ticketService.GetAllTicketsAsync(page, size, userName));
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
                    return StatusCode(503, "Сервис недоступен");
                }
                return StatusCode(503, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<TicketPurchaseDto>> PurchaseTicket(BuyTicketDto model, [FromHeader(Name = HeaderConstant.UserName)] string userName)
        {
            return Ok(await _ticketService.BuyTicketsAsync(model.Flightnumber, model.Price, model.Paidfrombalance, userName));
        }

        [HttpGet("{ticketUid}")]
        public async Task<ActionResult<TicketDto>> GetByUid(string ticketUid, [FromHeader(Name = HeaderConstant.UserName)] string userName)
        {
            try
            {
                CheckIfCircuitBreakerTimeStampIsComplete();
                if (isCircuitOpen == false)
                {
                    return Ok(await _ticketService.GetTicketByUidAsync(ticketUid));
                }
                return StatusCode(500, "Сервис недоступен");
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
                    return StatusCode(500, "Сервис недоступен");
                }
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{ticketUid}")]
        public async Task<ActionResult<TicketDto>> CancelTicket(string ticketUid, [FromHeader(Name = HeaderConstant.UserName)] string userName)
        {
            await _ticketService.ReturnTicketsAsync(ticketUid, userName, queue);
            return NoContent();
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
