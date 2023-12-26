using AutoMapper;
using TicketService.Dtos;
using TicketService.Entities;

namespace TicketService.Profiles
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<Ticket, TicketDto>();
            CreateMap<TicketDto, Ticket>();
        }
    }
}
