using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastFood.Domain.Entities
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public User Employee { get; set; } = null!;

        [Required]
        public DateTime ClockInTime { get; set; }

        public DateTime? ClockOutTime { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal HourlyRate { get; set; } = 30000; // Default 30k/h as seen in UI

        [NotMapped]
        public decimal TotalSalary
        {
            get
            {
                if (!ClockOutTime.HasValue) return 0;
                var duration = ClockOutTime.Value - ClockInTime;
                return (decimal)duration.TotalHours * HourlyRate;
            }
        }
    }
}
