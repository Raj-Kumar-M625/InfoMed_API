using System.ComponentModel.DataAnnotations;

namespace InfoMed.Models
{
    public class EventsMaster
    {
        [Key]
        public int IdEvent { get; set; }
        public string EventMasterName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
