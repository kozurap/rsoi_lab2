namespace Client.Models
{
    public class TicketPurchaseDto
    {
        public string Flightnumber { get; set; }
        public int Price { get; set; }
        public bool Paidfrombalance { get; set; }
    }
}
