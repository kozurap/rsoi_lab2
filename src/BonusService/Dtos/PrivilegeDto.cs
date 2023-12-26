namespace PrivilegeService.Dtos
{
    public class PrivilegeDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int? Balance { get; set; }

        public virtual ICollection<PrivilegeHistoryDto> PrivilegeHistories { get; set; }
    }
}
