using Microsoft.VisualBasic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API_Core_Project.Models
{
    public class BillModel:EntityBase
    {
        [Key]
        public int BillID { get; set; }
        public int PatientID { get; set; }

        [Required]
        public decimal ConsultingCharge { get; set; }

        public decimal InsuranceCoverage { get; set; }




        [Required]
        public decimal TotalCharge { get; set; }

        [Required]
        public DateTime DateOfVisit { get; set; }
    }
}
