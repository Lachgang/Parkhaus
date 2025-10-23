namespace ParkhausUI.Models
{
    public class TicketAutomat(Parkhaus parkhaus)
    {
        public List<Tickets> TicketsListe { get; set; } = new List<Tickets>();
        public Tickets ErstelleTicket()
        {
            Tickets neuesTicket = new Tickets
            {
                TicketID = Guid.NewGuid().ToString(),
                Einfahrtszeit = DateTime.Now,
                Bezahlt = false
            };
            TicketsListe.Add(neuesTicket);
            return neuesTicket;
        }

        public void LöscheTicket(Tickets ticket) { 
            TicketsListe.Remove(ticket);
        }
        public Tickets? FindeTicketNachID(string ticketID)
        {
            return TicketsListe.FirstOrDefault(t => t.TicketID == ticketID);
        }

        public void BezahleTicket(Tickets ticket)
        {
            ticket.Bezahlt = true;
        }

        public float BerechneGebühr(Tickets ticket, DateTime Zeit)
        {
            return (float)(Zeit - ticket.Einfahrtszeit).TotalMinutes * parkhaus.PreisproMinute;
        }
    }

}
