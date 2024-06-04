namespace InfoMed.DTO
{
    public class LastYearMemoryDto
    {
        public int IdLastYearMemory { get; set; }
        public int? IdEvent { get; set; }
        public int IdEventVersion { get; set; }
        public string LastYearMemoryHeader { get; set; }
        public string LastYearMemoryText { get; set; }
        public bool Status { get; set; }
        public virtual LastYearMemoryDetailDto? LastYearMemoryDetail { get; set; }
    }
}
