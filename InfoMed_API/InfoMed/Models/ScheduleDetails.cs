using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMed.Models
{
    public class ScheduleDetails
    {
        [Key]
        public int IdScheduleDetail { get; set; }  
        public int IdScheduleMaster { get; set; } 
        public DateTime StartTime { get; set; } 
        public DateTime EndTime { get; set; }
        public string? Topic { get; set; }
        public bool IsActive { get; set; }
    }
}
