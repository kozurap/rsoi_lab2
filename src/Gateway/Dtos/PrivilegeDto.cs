namespace Gateway.Dtos
{
    public class PrivilegeDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int? Balance { get; set; }

        public virtual ICollection<PrivilegeHistoryDto> PrivilegeHistories { get; set; }
    }

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

    public class UserPrivilegeDto
    {
        public string Status { get; set; }
        public int Balance { get; set; }
        public List<UserPrivilegeHistoryDto> History { get; set; }
    }

    public class UserPrivilegeHistoryDto
    {
        public DateTime Datetime { get; set; }
        public int BalanceDiff { get; set; }
        public string OperationType { get; set; }
        public Guid TicketUid { get; set; }
    }
}
