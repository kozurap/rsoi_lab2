namespace Client.Models
{
    public class MeModel
    {
        public PrivilegeDto? Privilege { get; set; }
        public List<GetTicketDto> Tickets { get; set; }
    }

    public class PrivilegeDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int? Balance { get; set; }
    }

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
