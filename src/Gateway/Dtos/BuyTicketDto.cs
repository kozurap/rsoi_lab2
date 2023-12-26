namespace Gateway.Dtos
{
    public class BuyTicketDto
    {
        public string Flightnumber { get; set; }
        public int Price { get; set; }
        public bool Paidfrombalance { get; set; }
    }
}
