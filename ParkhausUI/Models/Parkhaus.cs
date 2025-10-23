namespace ParkhausUI.Models
{
    public class Parkhaus
    {
        public Parkplatz[] Parkplaetze;
        public float PreisproMinute;
        public TicketAutomat TicketAutomat;

       
        public Parkhaus(int parkplatzAnzahl, float preisproMinute)
        {
            Parkplaetze = Enumerable.Range(0, parkplatzAnzahl)
                                   .Select(_ => new Parkplatz())
                                   .ToArray();
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
