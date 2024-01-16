using Gateway.Dtos;
using Gateway.Enums;

namespace Gateway
{
    public class QueueModel
    {
        public QueueEnum Enum { get; set; }
        public string? TicketUid { get; set; }
        public string? UserName { get; set; }
        public GetTicketDto? Ticket { get; set; }

    }
}
