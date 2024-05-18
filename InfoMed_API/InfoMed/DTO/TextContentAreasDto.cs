namespace InfoMed.DTO
{
    public class TextContentAreasDto
    {
        public int IdTextContentArea { get; set; }
        public int IdEvent { get; set; }
        public int IdEventVersion { get; set; }
        public string ContentHeader { get; set; }
        public string ContentText { get; set; }
        public int OrderNumber { get; set; }
        public bool Status { get; set; }
    }
}
