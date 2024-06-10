namespace InfoMed.DTO
{
    public class SpeakersDto
    {
        public int IdSpeaker { get; set; }
        public int IdEvent { get; set; }
        public int IdEventVersion { get; set; }
        public string SpeakerName { get; set; }
        public string AboutSpeaker { get; set; }       
        public string SpeakerImage { get; set; }
        public string SpeakerDesignation { get; set; }
        public int OrderNumber { get; set; }
        public bool Status { get; set; }

    }
}
