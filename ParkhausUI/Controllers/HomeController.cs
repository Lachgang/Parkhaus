using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ParkhausUI.Models;

namespace ParkhausUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static Parkhaus parkhaus = new Parkhaus(50, 0.25f);
        private readonly string apiBaseUrl = "http://ParkhausAPI/Tickets";
        private static readonly HttpClient httpClient = new HttpClient();

        
        private static Dictionary<string, int> ticketParkplatzMap = new Dictionary<string, int>();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(parkhaus);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> ParkhausEinfahrt(int parkplatz)
        {
            if (parkhaus.VerfügbareParkplätze() < 1)
            {
                ViewBag.Message = "Keine Plätze frei! Scusi";
                return View("Index", parkhaus);
            }

            if (parkhaus.Parkplaetze[parkplatz - 1].Belegt)
            {
                ViewBag.Message = "Platz bereits belegt!";
                return View("Index", parkhaus);
            }

            parkhaus.Parkplaetze[parkplatz - 1].Belegt = true;

            var ticket = parkhaus.TicketAutomat.ErstelleTicket();

            
            ticketParkplatzMap[ticket.TicketID] = parkplatz;

            try
            {
                await speichern(ticket);
                ViewBag.Message = $"Willkommen! Ihr Ticket ID ist {ticket.TicketID}";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Fehler beim Speichern des Tickets: {ex.Message}";
                parkhaus.TicketAutomat.LöscheTicket(ticket);
                parkhaus.Parkplaetze[parkplatz - 1].Belegt = false;
                ticketParkplatzMap.Remove(ticket.TicketID);
            }
            return View("Index", parkhaus);
        }

        [HttpPost]
        public async Task<IActionResult> ParkhausAusfahrt(string ticketId)
        {
            var ticket = parkhaus.TicketAutomat.FindeTicketNachID(ticketId);
            if (ticket == null)
            {
                ViewBag.Message = "Ungültige Ticket ID!";
                return View("Index", parkhaus);
            }

            
            if (!ticketParkplatzMap.TryGetValue(ticketId, out int parkplatzNummer))
            {
                ViewBag.Message = "Parkplatz-Zuordnung nicht gefunden!";
                return View("Index", parkhaus);
            }

            ticket.Ausfahrtszeit = DateTime.Now;
            ticket.Bezahlt = true;

            try
            {
                await Aktualisieren(ticket);

                
                parkhaus.Parkplaetze[parkplatzNummer - 1].Belegt = false;

                parkhaus.TicketAutomat.LöscheTicket(ticket);
                ticketParkplatzMap.Remove(ticketId);

                ViewBag.Message = "Danke fürs Besuchen! Auf Wiedersehen!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Fehler beim Aktualisieren des Tickets: {ex.Message}";
            }
            return View("Index", parkhaus);
        }

        private async Task speichern(Tickets ticket)
        {
            if (ticket.Ausfahrtszeit == default(DateTime))
            {
                ticket.Ausfahrtszeit = null;
            }
            try
            {
                var json = JsonSerializer.Serialize(ticket);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiBaseUrl, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Speichern des Tickets: " + ex.Message);
            }
        }

        private async Task Aktualisieren(Tickets ticket)
        {
            try
            {
                var json = JsonSerializer.Serialize(ticket);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PatchAsync($"{apiBaseUrl}('{ticket.TicketID}')", content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Aktualisieren des Tickets: " + ex.Message);
            }
        }
    }
}
