namespace Gateway.Services
{
    public class StatsService : ClientServiceBase
    {
        protected override string BaseUri => "http://statsservice:8030";//8030

        public async Task<IEnumerable<string>> Get() =>
            await Client.GetAsync<List<string>>(BuildUri("api/v1/stats/statistics"), null, new Dictionary<string, string>()
            {
                { "count", "10" }
            });

    }
}
