namespace Gateway.Dtos
{
    public class GetTicketDto
    {
        public string? FromAirport { get; set; }
        public string? ToAirport { get; set; }
        public DateTime? Date { get; set; }
        public int Id { get; set; }
        public Guid Ticketuid { get; set; }
        public string Username { get; set; }
        public string Flightnumber { get; set; }
        public int Price { get; set; }
        public string Status { get; set; }
    }
}
