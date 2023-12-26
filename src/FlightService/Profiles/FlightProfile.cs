using AutoMapper;
using FlightService.Dtos;
using FlightService.Entities;

namespace FlightService.Profiles
{
    public class FlightProfile : Profile
    {
        public FlightProfile()
        {
            CreateMap<Airport, AirportDto>();
            CreateMap<AirportDto, Airport>();
            CreateMap<Flight, FlightDto>();
            CreateMap<FlightDto, Flight>();
        }
    }
}
