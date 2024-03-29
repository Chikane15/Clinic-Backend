using System.ComponentModel.DataAnnotations;

namespace API_Core_Project.Models
{
    public class DoctorImconeModel:EntityBase
    {

        [Key]
        public int DoctorId { get; set; }

        public int Salary { get; set; }
    }
}
