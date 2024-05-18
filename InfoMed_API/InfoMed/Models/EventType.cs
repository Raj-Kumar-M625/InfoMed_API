using System.ComponentModel.DataAnnotations;

namespace InfoMed.Models
{
    public class EventType
    {
        [Key]
        public int IdEventType { get; set; }
        public string Name { get; set; }
    }
}
