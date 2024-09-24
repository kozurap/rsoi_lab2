using Client.Infrostructure;
using Client.Models;
using Kernel.AbstractClasses;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class MeController : Controller
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

        public async Task<IActionResult> Get()
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
            var res = await Client.GetAsync<MeModel>(BuildUri("api/v1/me/get"), headers, null);
            return View("Profile", res);
        }
    }
}
