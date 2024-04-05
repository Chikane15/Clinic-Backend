using Microsoft.VisualBasic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API_Core_Project.Models
{
    public class VisitModel:EntityBase
    {
        [Key]
        public int VId { get; set; }

        public int PId { get; set; }

        public DateTime DateofVisit { get; set; }

        public int DoctorId { get; set; }


        public int BillId { get; set; }


        public int ReportID { get; set; }
    }
}
