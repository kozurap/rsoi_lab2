using Client.Infrostructure;
using Client.Models;
using Kernel.AbstractClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;

namespace Client.Controllers
{
    public class FlightController : Controller
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
        public async Task<IActionResult> GetFlights()
        {
            var token = HttpContext.Session.GetString("token");
            Dictionary<string,string>? headers = null;
            if(token != null)
            {
                headers = new Dictionary<string, string>()
                {
                    {"Authorization", token }
                };
            }
            var res = await Client.GetAsync<PaginationModel<FlightDto>>(BuildUri("api/v1/flights"), headers, new Dictionary<string, string>
            {
                { "page", "1" },
                { "size", "10" },
            });
            var tickets = await Client.GetAsync<PaginationModel<TicketDto>>(BuildUri("api/v1/tickets/GetAllTickets"), headers, null);
            var ticketList = new List<TicketDto>();
            if (tickets != null)
            {
                ticketList = tickets.Items.ToList();
            }
            var model = new FlightListModel();
            model.Flights = new List<FlightModel>();
            foreach(var e in res.Items)
            {
                var isEnabled = ticketList.Where(x=>x.Flightnumber == e.Flightnumber).Count() < 3;
                model.Flights.Add(new FlightModel()
                {
                    Id = e.Id,
                    Flightnumber = e.Flightnumber,
                    Datetime = e.Datetime,
                    Price = e.Price,
                    Fromairport = e.Fromairport.Name,
                    Toairport = e.Toairport.Name,
                    IsPurchasable = isEnabled
                });
            }
            return View("Flights", model);
        }

        [HttpPost]
        public async Task<IActionResult> BuyTicket(string flightNumber, int price, bool payByBalance)
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
            var dto = new TicketPurchaseDto()
            {
                Flightnumber = flightNumber,
                Price = price,
                Paidfrombalance = payByBalance
            };
            var res = await Client.PostAsync<PuchasedTicketDto, TicketPurchaseDto>(BuildUri("api/v1/tickets/PurchaseTicket"), dto, headers);
            return RedirectToAction("Index", "Home");
        }
    }
}
