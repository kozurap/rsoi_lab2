using Client.Infrostructure;
using Client.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class AuthorizationController : Controller
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
        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var token = await Client.PostAsync<JwtToken, LoginModel>(BuildUri("api/v1/auth/login"), model);
                HttpContext.Session.SetString("token", token.Token);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var token = await Client.PostAsync<string, RegisterModel>(BuildUri("api/v1/auth/register"), model);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}
