using System.ComponentModel.DataAnnotations;

namespace InfoMed.Models
{
    public class PaymentDetails
    {
        [Key]
        public int IdPaymentArea { get; set; }
        public int? IdEvent { get; set; }
        public int IdEventVersion { get; set; }
        public string PaymentTextContent { get; set; }
        public string? QRCodeImage { get; set; }
    }
}
