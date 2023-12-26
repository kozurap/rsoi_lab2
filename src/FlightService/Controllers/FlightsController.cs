using AutoMapper;
using FlightService.Dtos;
using FlightService.Entities;
using FlightService.Filters;
using System.Linq.Expressions;

namespace FlightService.Controllers
{
    public class FlightsController : RestControllerBase<Flight, FlightDto, FlightFilter>
    {
        public FlightsController(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
        {
        }
        protected override void MapDtoToEntity(Flight flight, FlightDto flightDto)
        {
            flight.Id = flightDto.Id;
            flight.Flightnumber = flightDto.Flightnumber;
            flight.Toairport = Mapper.Map<Airport>(flightDto.Toairport);
            flight.Fromairport = Mapper.Map<Airport>(flightDto.Fromairport);
            flight.Datetime = flightDto.Datetime;
        }
    }
}
