namespace Gateway.Dtos
{
    public class UserDto
    {
        public PrivilegeDto Privilege { get; set; }
        public List<TicketDto> Tickets { get; set; }
    }
}
