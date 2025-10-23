using System.ComponentModel.DataAnnotations;

namespace ParkhausAPI.Models
{
    public class Tickets
    {
        [Key]
        public string TicketID { get; set; } = string.Empty;
        [Required]
        public DateTime Einfahrtszeit { get; set; }

        public DateTime? Ausfahrtszeit { get; set; }

        [Required]
        public bool Bezahlt { get; set; } = false;
    }
}
