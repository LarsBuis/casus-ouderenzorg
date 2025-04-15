using System;
using System.Collections.Generic;

namespace casus_ouderenzorg.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int CaregiverID { get; set; }
        public int PatientID { get; set; }
        public int PharmacyID { get; set; }
        public Pharmacy Pharmacy { get; set; }
        public List<OrderLine> OrderLines { get; set; }
    }
}
