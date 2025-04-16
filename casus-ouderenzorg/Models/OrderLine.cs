namespace casus_ouderenzorg.Models
{
    public class OrderLine
    {
        public int OrderLineID { get; set; }
        public int OrderID { get; set; }
        public int MedicationID { get; set; }
        public string Dosage { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public string TreatmentDuration { get; set; } = string.Empty;
        public Medication Medication { get; set; }
    }
}
