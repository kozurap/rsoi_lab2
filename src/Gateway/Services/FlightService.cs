using Gateway.Dtos;
using Kernel.AbstractClasses;

namespace Gateway.Services
{
    public class FlightService : ClientServiceBase
    {
        protected override string BaseUri => "http://flightservice:80";

        public FlightService() : base()
        {
        }

        public async Task<PaginationModel<FlightDto>?> GetAllAsync(int page, int size) => 
            await Client.GetAsync<PaginationModel<FlightDto>>(BuildUri("api/v1/flights"), null, new Dictionary<string, string>
        {
            { "page", page.ToString() },
            { "size", size.ToString() },
        });
    }
}
