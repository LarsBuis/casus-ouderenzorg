using System;
using System.ComponentModel.DataAnnotations;

namespace casus_ouderenzorg.Models
{
    public class Patient
    {
        [Key]
        public int PatientID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        public int? LocationID { get; set; }
        public Location? Location { get; set; }

        public int? CaregiverID { get; set; }

        [MaxLength(300)]
        public string BackgroundInfo { get; set; } = string.Empty;
    }
}