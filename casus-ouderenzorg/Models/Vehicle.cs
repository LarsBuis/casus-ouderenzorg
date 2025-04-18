using System.ComponentModel.DataAnnotations;

namespace casus_ouderenzorg.Models
{
    public class Vehicle
    {
        [Key]
        public int VehicleID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public ICollection<Transport> Transports { get; set; } = new List<Transport>();
    }
}
