using System;

namespace casus_ouderenzorg.Models
{
    // Note: While "Task" is used here for domain purposes, in production you might choose a less conflicting name.
    public class Task
    {
        public int Id { get; set; }

        // Foreign key linking this task to a DayPlanning record.
        public int DayPlanningId { get; set; }

        public string Description { get; set; } = string.Empty;

        // The location where the task is performed.
        public string Location { get; set; } = string.Empty;

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        // The Date of the task is pulled from the DayPlanning record.
        public DateTime Date { get; set; }

        // New property: whether the task is marked as completed.
        public bool IsCompleted { get; set; }
    }
}
