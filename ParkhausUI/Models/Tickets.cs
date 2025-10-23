namespace ParkhausUI.Models
{
    public class Tickets
    {
        
        public string TicketID { get; set; } = string.Empty;

        public DateTime Einfahrtszeit { get; set; }

        public DateTime? Ausfahrtszeit { get; set; }

     
        public bool Bezahlt { get; set; } = false;
    }
}
