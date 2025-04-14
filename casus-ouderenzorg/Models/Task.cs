using System;

namespace casus_ouderenzorg.Models
{
    // Note: "Task" is used for domain purposes.
    // In production you might prefer another name to avoid conflicting with System.Threading.Tasks.Task.
    public class Task
    {
        public int Id { get; set; }

        // Foreign key linking this task to a DayPlanning record.
        public int DayPlanningId { get; set; }

        public string Description { get; set; } = string.Empty;

        // Task-specific location (e.g., "Room 101").
        public string Location { get; set; } = string.Empty;

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        // The date is retrieved via a join with DayPlanning.
        public DateTime Date { get; set; }
    }
}
