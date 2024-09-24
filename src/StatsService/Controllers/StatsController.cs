using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StatsService.Models;

namespace StatsService.Controllers
{
    [Route("api/v1/stats")]
    public class StatsController : Controller
    {
        private readonly LogsConsumer _consumer;
        private readonly AppDbContext _appDbContext;
        public StatsController(LogsConsumer consumer, AppDbContext context)
        {
            _consumer = consumer;
            _appDbContext = context;
        }
        [HttpGet]
        [Route("statistics")]
        public async Task<List<string>> Get([FromQuery] int count)
            {
            var result = new List<string>();
            while(true)
            {
                var x = _consumer.Consume();
                if(x == "plz stop")
                {
                    break;
                }
                result.Add(x);
            }
            var moreResults = _appDbContext.Set<Stat>().ToList();
            var entities = result.Select(x => new Stat() { Text = x }).ToArray();
            await _appDbContext.AddRangeAsync(entities);
            if (moreResults != null && moreResults.Any())
            {
                result.AddRange(moreResults.Select(x=>x.Text).ToList());
            }
            _appDbContext.SaveChanges();
            return result;
        }
    }
}
