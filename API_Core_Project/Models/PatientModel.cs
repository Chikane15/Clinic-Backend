using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace API_Core_Project.Models
{
    public class PatientModel:EntityBase
    {
        [Key]
        public int PatientID { get; set; }

        [Required]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string? LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly DOB { get; set; }

        [StringLength(15)]
        public string? Contact { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(100)]
        public string? Address { get; set; }

        [Required]
        public string? Gender { get; set; }

        [StringLength(10)]
        public string? BloodType { get; set; }

        [StringLength(15)]
        public string? EmergencyContact { get; set; }

        [StringLength(50)]
        public string? Insurance { get; set; }
    }
}
