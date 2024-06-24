using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMed.Models
{
    public class RegistrationMembers
    {
        [Key]
        public int IdRegistrationMember { get; set; }
        [ForeignKey("Registration")]
        public int? IdRegistration { get; set; }
        public string MemberName { get; set; }      
        public string EmailID { get; set; }   
        public string? MobileNumber { get; set; }      
        public virtual Registrations Registration { get; set; }
    }
}
