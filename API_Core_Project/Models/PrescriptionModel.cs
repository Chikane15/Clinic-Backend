using System.ComponentModel.DataAnnotations;

namespace API_Core_Project.Models
{
    public class PrescriptionModel:EntityBase
    {
        [Key]
        public int PriId { get; set; }

        public string? Medicine { get; set; }

        public int PatientId{ get; set; }
    }
}
