using System.ComponentModel.DataAnnotations;

namespace InfoMed.Models
{
    public class ScheduleMaster
    {
        [Key]
        public int IdScheduleMaster { get; set; } 
        public int IdEvent { get; set; } 
        public int IdEventVersion { get; set; } 
        public DateTime ScheduleDate { get; set; }
        public string DayScheduleName { get; set; }
        public string? DayScheduleDetailsText { get; set; }
        public bool IsActive { get; set; }
    }
}
