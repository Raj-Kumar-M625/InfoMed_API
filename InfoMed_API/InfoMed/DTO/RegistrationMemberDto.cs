using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMed.DTO
{
    public class RegistrationMemberDto
    {
        [Key]
        public int IdRegistrationMember { get; set; }
        [ForeignKey("Registration")]
        public int? IdRegistration { get; set; }
        public string MemberName { get; set; }
        public string EmailID { get; set; }
        public string MobileNumber { get; set; }
    }
}
