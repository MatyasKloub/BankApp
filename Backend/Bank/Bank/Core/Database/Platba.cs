using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Core.Database
{
    [Table("Platba")]
    public class Platba
    {
        [Key]
        public int? Id { get; set; }
        public int from { get; set; }
        public string? Currency { get; set; }
        public string? Value { get; set; }
        public string? to { get; set; }


        public Platba()
        {
        }

        public Platba(int from, string currency, string value, string to="none")
        {
            this.from = from;
            Currency = currency;
            Value = value;
        }

    }
}
