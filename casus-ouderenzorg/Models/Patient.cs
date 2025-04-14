namespace casus_ouderenzorg.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // New: The ID of the caregiver assigned to this patient, if any.
        public int? CaregiverId { get; set; }
    }
}
