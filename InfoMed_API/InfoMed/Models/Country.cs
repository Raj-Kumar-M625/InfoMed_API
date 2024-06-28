using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMed.Models
{
    [Table("Countries")]
    public class Country
    {
        public string CountryName { get; set; }
        public string ISDCode { get; set; }
        public string CountryCode { get; set; }
    }
}
