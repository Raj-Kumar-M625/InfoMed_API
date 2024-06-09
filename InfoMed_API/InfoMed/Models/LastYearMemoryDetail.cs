using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMed.Models
{
    public class LastYearMemoryDetail
    {
        [Key]
        public int IdLastYearMemoryDetail { get; set; }
        [ForeignKey("LastYearMemory")]
        public int IdLastYearMemory { get; set; }
        //public string MediaContent { get; set; }
        public string MediaType { get; set; }
        public string MediaShortDesc { get; set; }
        public int OrderNumber {get; set;}
        public string? MediaPath { get; set; }
        public virtual LastYearMemory LastYearMemory { get; set; }
    }
}
