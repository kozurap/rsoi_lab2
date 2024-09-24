using AutoMapper;
using Kernel.AbstractClasses;
using Kernel.Extensions;
using Kernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.Json;
using TicketService.Dtos;
using TicketService.Entities;
using TicketService.Filters;
using static Confluent.Kafka.ConfigPropertyNames;

namespace TicketService.Controllers
{
    public class TicketsController : RestControllerBase<Ticket, TicketDto, TicketFilter>
    {
        public TicketsController(IMapper mapper, AppDbContext dbContext, LogsProducer producer) : base(mapper, dbContext, producer)
        {
        }
        protected override Expression<Func<Ticket, bool>> UidPredicate(Guid uId)
        => x => x.Ticketuid == uId;

        protected override void SetUid(Ticket entity, Guid uId)
            => entity.Ticketuid = uId;

        protected override Guid? GetUid(Ticket e) => e.Ticketuid;
        protected override IQueryable<Ticket> AttachFilterToQueryable(IQueryable<Ticket> q, TicketFilter f)
        {
            if (f != null && f.UserName != null && f.UserName != "")
            {
                return q.WhereNext(f.UserName, x => x.Username == f.UserName);
            }
            return q;
        }

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
