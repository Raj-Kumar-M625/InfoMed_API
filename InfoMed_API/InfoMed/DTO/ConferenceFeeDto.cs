namespace InfoMed.DTO
{
    public class ConferenceFeeDto
    {
        public int IdConferenceFee { get; set; }
        public int IdEvent { get; set; }
        public int IdEventVersion { get; set; }
        public string FeeName { get; set; }
        public string FeeDetailText { get; set; }
        public int MinimumPeopleCount { get; set; }
        public decimal Amount { get; set; }
        public int DayCount { get; set; }
        public DateTime ApplicableStartDate { get; set; }
        public DateTime ApplicableEndDate { get; set; }
        public int OrderNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
