using System.ComponentModel.DataAnnotations;

namespace casus_ouderenzorg.Models
{
    public class Location
    {
        [Key]
        public int LocationID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Patient> Patients { get; set; } = new List<Patient>();
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
