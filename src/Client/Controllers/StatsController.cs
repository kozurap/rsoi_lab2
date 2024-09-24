using Client.Infrostructure;
using Client.Models;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class StatsController : Controller
    {
        protected readonly HttpClientDecorator Client = new HttpClientDecorator();
        protected string BaseUri = "http://gateway:8080";//8080
        private string GetBaseUri() => BaseUri;

        protected string BuildUri(string stringToAppend)
        {
            var baseUri = GetBaseUri();
            baseUri = baseUri.Last() != '/' ? baseUri + '/' : baseUri;
            return baseUri + stringToAppend;
        }

        [HttpGet]
        public async Task<IActionResult> Stats()
        {
            var token = HttpContext.Session.GetString("token");
            Dictionary<string, string>? headers = null;
            if (token != null)
            {   
                headers = new Dictionary<string, string>()
                {
                    {"Authorization", token }
                };
            }
            var res = await Client.GetAsync<List<string>>(BuildUri("api/v1/stats"), headers, null);
            if(res == null)
            {
                res = new List<string>();
            }
            var model = new StatModel();
            var dict = new Dictionary<string, int>();
            foreach(var e in res)
            {
                if (!dict.ContainsKey(e))
                {
                    dict.Add(e, 1);
                }
                dict[e] += 1;
            }
            model.Stats = dict.Select(x => new SingleStatModel()
            {
                Text = x.Key,
                Count = x.Value
            }).ToList();
            return View("Stats", model);
        }
    }
}
