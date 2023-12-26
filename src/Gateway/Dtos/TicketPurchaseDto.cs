namespace Gateway.Dtos
{
    public class TicketPurchaseDto
    {
        public Guid TicketUid { get; set; }
        public string UserName { get; set; }
        public string FlightNumber { get; set; }
        public int Price { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public int PaidByMoney { get; set; }
        public int PaidByBonuses { get; set; }
        public PrivilegeTicketPurchaseDto Privilege { get; set; }
    }

    public class PrivilegeTicketPurchaseDto
    {
        public int Balance { get; set; }
        public string Status { get; set; }
    }
}
