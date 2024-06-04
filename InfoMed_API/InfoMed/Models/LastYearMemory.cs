using System.ComponentModel.DataAnnotations;

namespace InfoMed.Models
{
    public class LastYearMemory
    {
        [Key]        
        public int IdLastYearMemory { get; set; }
        public int? IdEvent { get; set; }  
        public int IdEventVersion { get; set; }       
        public string LastYearMemoryHeader { get; set; }  
        public string LastYearMemoryText { get; set; }
        public virtual LastYearMemoryDetail LastYearMemoryDetail { get; set; }
        public bool Status { get; set; }

    }
}
