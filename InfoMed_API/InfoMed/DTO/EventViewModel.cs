namespace InfoMed.DTO
{
    public class EventViewModel
    {
        public EventVersionDto EventVersion { get; set; }
        public PaymentDetailsDto? PaymentDetails { get; set; }
        public List<TextContentAreasDto> TextContentAreas { get; set; }
        public List<SponsersDto> Sponsers { get; set; }
        public List<SpeakersDto> Speakers { get; set; }
        public List<ScheduleMasterDto> ScheduleMaster { get; set; }
        public List<ConferenceFeeDto> ConferenceFee { get; set; }
        public List<LastYearMemoryDto> LastYearMemory { get; set; }
    }
}
