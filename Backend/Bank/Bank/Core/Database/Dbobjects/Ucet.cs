using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Core.Database
{

    [Table("Ucet")]
    public class Ucet
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public string? Mena { get; set; }
        public string? Count { get; set; }
        
        public Ucet() { }

        public Ucet(int? uid, string mena, string count )
        {
            UserId = uid;
            Mena = mena;
            Count = count;
        }


    }
}
