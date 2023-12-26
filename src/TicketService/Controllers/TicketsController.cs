using AutoMapper;
using Kernel.Extensions;
using System.Linq.Expressions;
using TicketService.Dtos;
using TicketService.Entities;
using TicketService.Filters;

namespace TicketService.Controllers
{
    public class TicketsController : RestControllerBase<Ticket, TicketDto, TicketFilter>
    {
        public TicketsController(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
        {
        }
        protected override Expression<Func<Ticket, bool>> UidPredicate(Guid uId)
        => x => x.Ticketuid == uId;

        protected override void SetUid(Ticket entity, Guid uId)
            => entity.Ticketuid = uId;

        protected override Guid? GetUid(Ticket e) => e.Ticketuid;
        protected override IQueryable<Ticket> AttachFilterToQueryable(IQueryable<Ticket> q, TicketFilter f)
        => q.WhereNext(f.UserName, x => x.Username == f.UserName);

        protected override void MapDtoToEntity(Ticket e, TicketDto dto)
        {
            e.Flightnumber = dto.Flightnumber;
            e.Ticketuid = dto.Ticketuid;
            e.Price = dto.Price;
            e.Status = dto.Status;
            e.Username = dto.Username;
        }
    }
}
