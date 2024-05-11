namespace InfoMed.Models
{
    public class Event:BaseEntity
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int DaysOfEvent { get; set; }
        public string Venue { get; set; }
        public string WebPageName { get; set; }
    }
}
