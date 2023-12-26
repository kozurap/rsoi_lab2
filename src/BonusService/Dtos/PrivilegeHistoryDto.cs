namespace PrivilegeService.Dtos
{
    public class PrivilegeHistoryDto
    {
        public int Id { get; set; }
        public int? PrivilegeId { get; set; }
        public Guid TicketUid { get; set; }
        public DateTime Datetime { get; set; }
        public int BalanceDiff { get; set; }
        public string OperationType { get; set; } = null!;

        public virtual PrivilegeDto? Privilege { get; set; }
    }
}
