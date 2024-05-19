using System.ComponentModel.DataAnnotations;

namespace InfoMed.Models
{
    public class SponserType
    {
        [Key]
        public int IdSponserType { get; set; }
        public string Name { get; set; }
    }
}
