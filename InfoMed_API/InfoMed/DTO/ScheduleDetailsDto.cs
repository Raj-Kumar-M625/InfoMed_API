using InfoMed.Models;

namespace InfoMed.DTO
{
    public class ScheduleDetailsDto
    {
        public int IdScheduleDetail { get; set; }
        public int IdScheduleMaster { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Topic { get; set; }
    }
}
