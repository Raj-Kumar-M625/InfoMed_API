using InfoMed.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMed.Models
{
    public class Registrations
    {
        [Key]
        public int IdRegistration { get; set; }
        public int IdEvent { get; set; }       
        public string RegType { get; set; }
        public DateTime RegisteredDate { get; set; }
        public string CompanyName { get; set; }        
        public string Name { get; set; }        
        public string EmailID { get; set; }
        public string MobileNumber { get; set; }
        public string CountryName { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }        
        public string IdConferenceFee { get; set; }        
        public decimal? AmountToBePaid { get; set; }
        public int? NoOfPersons { get; set; }
    }
}
