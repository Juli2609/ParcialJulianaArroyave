using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ParcialJulianaArroyave.DAL.Entities;

namespace WebPages.Controllers
{
    public class TicketsController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;

        public TicketsController(IHttpClientFactory httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var url = "https://localhost:7274/Tickets/Get";
            var json = await _httpClient.CreateClient().GetStringAsync(url);
            List<Ticket> categories = JsonConvert.DeserializeObject<List<Ticket>>(json);
            return View(categories);
        }
    }
}
