using System.ComponentModel.DataAnnotations;

namespace Assignment_05_03.Models
{
    public class Logger:EntityBase
    {
        [Key]
        public Guid RequestID { get; set; }

        
        public DateOnly RequestDate { get; set; }

        [StringLength(1000)]
        public string? RequestedController { get; set; }

        
        public TimeOnly RequestTime { get; set; }

        [StringLength(1000)]
        public string? RequestAction { get; set; }

        [StringLength(9000)]
        public string RequestBody { get; set; }

        [StringLength(9000)]
        public string? RequestHeaders { get; set; }

        [StringLength(1000)]
        public string? RequestUrl { get; set; }
    }

    public class ErrorLogger : EntityBase
    {
        [Key]
        public Guid RequestID { get; set; }


        public DateOnly RequestDate { get; set; }

        [StringLength(1000)]
        public string? RequestedController { get; set; }


        public TimeOnly RequestTime { get; set; }

        [StringLength(1000)]
        public string? RequestAction { get; set; }

        [StringLength(9000)]
        public string RequestBody { get; set; }

        [StringLength(9000)]
        public string RequestHeaders { get; set; }

        [StringLength(1000)]
        public string? RequestUrl { get; set; }

        [StringLength(2000)]
        public string? ExceptionMessage { get; set; }
    }
}
