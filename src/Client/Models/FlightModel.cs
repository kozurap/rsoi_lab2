namespace Client.Models
{
    public class FlightModel
    {
        public int Id { get; set; }
        public string Flightnumber { get; set; } = null!;
        public DateTime Datetime { get; set; }
        public string? Fromairport { get; set; }
        public string? Toairport { get; set; }
        public bool IsPurchasable { get; set; }
        public int Price { get; set; }
        public bool PayByBalance { get; set; }
    }
}
