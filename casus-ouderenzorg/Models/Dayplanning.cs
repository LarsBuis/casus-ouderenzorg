using System;

namespace casus_ouderenzorg.Models
{
    // Represents a day plan for a caregiver.
    public class DayPlanning
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        // The caregiver's ID assigned to this day.
        public int CaregiverId { get; set; }
    }
}
