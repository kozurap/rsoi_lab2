namespace Gateway.Dtos
{
    public class FlightDto
    {
        public int Id { get; set; }
        public string Flightnumber { get; set; } = null!;
        public DateTime Datetime { get; set; }
        public int? Fromairportid { get; set; }
        public int? Toairportid { get; set; }
        public int Price { get; set; }

        public virtual AirportDto? Fromairport { get; set; }
        public virtual AirportDto? Toairport { get; set; }
    }
}
