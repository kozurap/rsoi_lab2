namespace Gateway.Dtos
{
    public class UserDto
    {
        public PrivilegeDto? Privilege { get; set; }
        public List<GetTicketDto> Tickets { get; set; }
    }
}
