using System.ComponentModel.DataAnnotations;

namespace API_Core_Project.Models
{
    public class VisitModel:EntityBase
    {
        [Key]
        public int VId { get; set; }

        public int PId { get; set; }

        public DateOnly DateofVisit { get; set; }

        public int DoctorId { get; set; }

        public TimeOnly TimeSlot { get; set; }

        public int BillId { get; set; }

        public int PriId { get; set; }

        public int ReportID { get; set; }
    }
}
