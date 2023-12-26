namespace TicketService.Dtos
{
    public class TicketDto
    {
        public int Id { get; set; }
        public Guid Ticketuid { get; set; }
        public string Username { get; set; }
        public string Flightnumber { get; set; }
        public int Price { get; set; }
        public string Status { get; set; }
    }
}
