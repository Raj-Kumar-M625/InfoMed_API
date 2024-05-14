namespace InfoMed.Models
{
    public class EventsMaster
    {
        public int IdEvent { get; set; }
        public string EventMasterName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
