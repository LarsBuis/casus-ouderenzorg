using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace casus_ouderenzorg.Models
{
    public class Task
    {
        [Key]
        public int TaskID { get; set; }

        [Required]
        [MaxLength(100)]
        public string TaskName { get; set; } = string.Empty;

        [Required]
        public DateTime TaskDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        public int? CaregiverID { get; set; }
        [ForeignKey("CaregiverID")]
        public Caregiver? Caregiver { get; set; }

        public int? LocationID { get; set; }
        [ForeignKey("LocationID")]
        public Location? Location { get; set; }

        public int? PatientID { get; set; }
        [ForeignKey("PatientID")]
        public Patient? Patient { get; set; }
    }
}
