using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InfoMed.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUser { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(20)]
        public string MobileNumber { get; set; }

        [Required]
        [StringLength(10)]
        public string Role { get; set; }

        [Required]
        public bool Status { get; set; }

        [NotMapped]
        public string Password { get; set; }
    }
}
