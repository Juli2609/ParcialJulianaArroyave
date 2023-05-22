using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ParcialJulianaArroyave.DAL.Entities;
using WebPages.Models;

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
            try
            {
                var url = _configuration["Api:TicketsUrl"];
                var json = await _httpClient.CreateClient().GetStringAsync(url);
                List<Ticket> tickets = JsonConvert.DeserializeObject<List<Ticket>>(json);
                return View(tickets);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }
        //ValidateTicket----------------------------------------------------
        [HttpGet]
        public IActionResult ValidateTicket()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidateTicket(Guid? id)
        {
            try
            ''{
                if (id == null)
                {
                    ViewData["Message"] = "Boleta no válida";
                    return View();
                }

                var url = String.Format("{0}/{1}", _configuration["Api:TicketsUrl"], id);
                var json = await _httpClient.CreateClient().GetStringAsync(url);
                var ticket = JsonConvert.DeserializeObject<Ticket>(json);

                if (ticket == null)
                {
                    ViewData["Message"] = "Boleta no válida";
                }
                else if (ticket.IsUsed == true)
                {
                    ViewData["Message"] = "Boleta ya usada";
                    ViewData["UseDate"] = ticket.UseDate;
                    ViewData["EntranceGate"] = ticket.EntranceGate;
                }
                else
                {
                    ViewData["Message"] = "Boleta válida, puede ingresar al concierto";
                    ticket.UseDate = DateTime.Now; // Actualiza la fecha de uso a la fecha actual
                    ticket.IsUsed = true; // Marca la boleta como usada

                    var updateUrl = String.Format("{0}/{1}", _configuration["Api:TicketsEditUrl"], id);
                    var response = await _httpClient.CreateClient().PutAsJsonAsync(updateUrl, ticket);

                    if (!response.IsSuccessStatusCode)
                    {
                        // Manejar el error en caso de que no se pueda actualizar la boleta
                        ViewData["Message"] = "Error al actualizar la boleta";
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }




        //-----------------------------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            try
            {
                return View(await GetTicketsById(id));
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, Ticket ticket)
        {
            try
            {
                var url = String.Format("{0}/{1}", _configuration["Api:TicketsEditUrl"], id);
                await _httpClient.CreateClient().PutAsJsonAsync(url, ticket);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            return View(await GetTicketsById(id));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var url = String.Format("{0}/{1}", _configuration["Api:TicketsDeleteUrl"], id);
                await _httpClient.CreateClient().DeleteAsync(url);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            try
            {
                return View(await GetTicketsById(id));
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        private async Task<Ticket> GetTicketsById(Guid? id)
        {
            var url = String.Format("{0}/{1}", _configuration["Api:TicketsUrl"], id);
            return JsonConvert.DeserializeObject<Ticket>(await _httpClient.CreateClient().GetStringAsync(url));
        }
    }
}
    