using System.ComponentModel.DataAnnotations;

namespace casus_ouderenzorg.Models
{
    public class Driver
    {
        [Key]
        public int DriverID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Transport> Transports { get; set; } = new List<Transport>();
    }
}
