using System.ComponentModel.DataAnnotations;

namespace API_Core_Project.Models
{
    public class ReportModel:EntityBase
    {
        [Key]
        public int ReportID { get; set; }
        public int PatientID { get; set; }

        public int DId { get; set; }

        public string? Diagnosis { get; set; }
    }
}
