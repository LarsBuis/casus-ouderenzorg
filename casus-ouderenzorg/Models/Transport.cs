using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace casus_ouderenzorg.Models
{
    public class Transport
    {
        [Key]
        public int TransportID { get; set; }

        [Required]
        public DateTime TransportDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan ReturnTime { get; set; }

        [MaxLength(100)]
        public string? Departure { get; set; }

        [MaxLength(100)]
        public string? Destination { get; set; }

        [MaxLength(255)]
        public string? Reason { get; set; }

        public int? PatientID { get; set; }
        [ForeignKey("PatientID")]
        public Patient? Patient { get; set; }

        public int? DriverID { get; set; }
        [ForeignKey("DriverID")]
        public Driver? Driver { get; set; }

        public int? CaregiverID { get; set; }
        [ForeignKey("CaregiverID")]
        public Caregiver? Caregiver { get; set; }

        public int? VehicleID { get; set; }
        [ForeignKey("VehicleID")]
        public Vehicle? Vehicle { get; set; }
    }
}
