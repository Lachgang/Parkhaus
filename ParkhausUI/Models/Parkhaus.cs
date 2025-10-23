namespace ParkhausUI.Models
{
    public class Parkhaus
    {
        public Parkplatz[] Parkplaetze;
        public float PreisproMinute;
        public TicketAutomat TicketAutomat;

        public Parkhaus(int parkplatzAnzahl, float preisproMinute)
        {
            Parkplaetze = new Parkplatz[parkplatzAnzahl];
            PreisproMinute = preisproMinute;
            TicketAutomat = new TicketAutomat(this);
        }
        public int VerfügbareParkplätze()
        {
            return Parkplaetze.Count(p => !p.Belegt);
        }
        public int TotalParkplätze()
        {
            return Parkplaetze.Length;
        }
    }
}
