using System.ComponentModel.DataAnnotations;

namespace API_Core_Project.Models
{
    public class DoctorModel : EntityBase
    {
        [Key]
        public int DoctorID { get; set; }

        [Required]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string? LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string? Speciality { get; set; }

        [Required]
        [StringLength(100)]
        public string? Email { get; set; }

        [Required]
        public int Salary { get; set; }
    }
}