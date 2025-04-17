using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace casus_ouderenzorg.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Medication { get; set; } = string.Empty;

        [Required]
        public DateTime OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [MaxLength(50)]
        public string? Concentration { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }

        public int? Amount { get; set; }

        public int? PatientID { get; set; }
        [ForeignKey("PatientID")]
        public Patient? Patient { get; set; }

        public int? CaregiverID { get; set; }
        [ForeignKey("CaregiverID")]
        public Caregiver? Caregiver { get; set; }

        public int? SupplierID { get; set; }
        [ForeignKey("SupplierID")]
        public Supplier? Supplier { get; set; }
    }
}
