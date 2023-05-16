namespace Bank.Core.Database
{
    public class Kurz
    {
        public string? Zkratka { get; set; }
        public float? Hodnota { get; set; }

        public Kurz(string zkratka, float hodnota)
        {
            Zkratka = zkratka;
            Hodnota = hodnota;
        }
        public Kurz()
        {

        }

    }
}
