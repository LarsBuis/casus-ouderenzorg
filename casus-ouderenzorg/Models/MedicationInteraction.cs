namespace casus_ouderenzorg.Models
{
    public class MedicationInteraction
    {
        public int MedicationInteractionID { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
    }
}
