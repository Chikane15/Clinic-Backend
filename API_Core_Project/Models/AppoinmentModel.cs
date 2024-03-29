using System.ComponentModel.DataAnnotations;

namespace API_Core_Project.Models
{
    public class AppoinmentModel:EntityBase
    {
        [Key]
        public int AppoinmentId { get; set; }

        public int PatientId { get; set; }

        public DateOnly date { get; set; }

        public string? timeSlot { get; set; }

        public int DoctorId { get; set; }
    }
}
