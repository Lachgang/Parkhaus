using Microsoft.AspNetCore.Mvc;
using ParkhausAPI.Context;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Deltas;
using ParkhausAPI.Models;

namespace ParkhausAPI.Controllers
{
    public class TicketsController(ParkhausContext context): ODataController
    {
        [EnableQuery]
        public IQueryable<Tickets> Get()
        {
            return context.Tickets;
        }
        [EnableQuery]
        public async Task<IActionResult> Get(int key, CancellationToken Token)

        {

            var ticket = await context.Tickets.FindAsync(key, Token);
            if (ticket == null)
            {
                return (IActionResult)NotFound();
            }
            return (IActionResult)Ok(ticket);
        }
        public async Task<IActionResult> Post([FromBody] Models.Tickets ticket, CancellationToken Token)
        {
            if (!ModelState.IsValid)
            {
                return (IActionResult)BadRequest(ModelState);
            }
            context.Tickets.Add(ticket);
            await context.SaveChangesAsync(Token);
            return (IActionResult)Created(ticket);
        }
        public async Task<IActionResult> Delete(string key, CancellationToken Token)
        {
            var ticket = await context.Tickets.FindAsync(key, Token);
            if (ticket == null)
            {
                return (IActionResult)NotFound();
            }
            context.Tickets.Remove(ticket);
            await context.SaveChangesAsync(Token);
            return new NoContentResult();
        }
        public async Task<IActionResult> Patch(string key, [FromBody] Tickets tickets, CancellationToken Token)
        {
            if (!ModelState.IsValid)
            {
                return (IActionResult)BadRequest(ModelState);
            }
            var ticket = await context.Tickets.FindAsync(key, Token);
            if (ticket == null)
            {
                return (IActionResult)NotFound();
            }
            ticket.Einfahrtszeit = tickets.Einfahrtszeit;
            ticket.Ausfahrtszeit = tickets.Ausfahrtszeit;
            ticket.Bezahlt = tickets.Bezahlt;

            await context.SaveChangesAsync(Token);
            return (IActionResult)Updated(ticket);
        }
    }

}
