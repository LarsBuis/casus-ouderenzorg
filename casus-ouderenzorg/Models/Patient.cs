using System;

namespace casus_ouderenzorg.Models
{
    public class Patient
    {
        public int Id { get; set; }

        // Patient's full name.
        public string Name { get; set; } = string.Empty;

        // Background information about the patient.
        public string BackgroundInfo { get; set; } = string.Empty;

        // Descriptive traits for the patient.
        public string Traits { get; set; } = string.Empty;

        // Patient's date of birth.
        public DateTime? DateOfBirth { get; set; }

        // Patient's address.
        public string Address { get; set; } = string.Empty;

        // Optional foreign key referencing a ContactPerson.
        public int? ContactPersonId { get; set; }

        // Navigation property for the assigned contact person.
        public ContactPerson? ContactPerson { get; set; }

        // Optional caregiver ID (if needed).
        public int? CaregiverId { get; set; }
    }
}
