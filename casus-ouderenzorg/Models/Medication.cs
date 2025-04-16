namespace casus_ouderenzorg.Models
{
    public class Medication
    {
        public int MedicationID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Concentration { get; set; } = string.Empty;
        public string Form { get; set; } = string.Empty;
        public string StandardDosage { get; set; } = string.Empty;
        public int MedicationInteractionID { get; set; }
        public MedicationInteraction? MedicationInteraction { get; set; }
    }
}
