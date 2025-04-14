using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using casus_ouderenzorg.Models;
using casus_ouderenzorg.DAL;
using System.Linq;
using Task = casus_ouderenzorg.Models.Task;

namespace casus_ouderenzorg.Pages
{
    public class AgendaModel : PageModel
    {
        private readonly string _connectionString;
        private readonly TaskDal _taskDal;

        // Hardcoded caregiver ID (for example, 1) for demonstration.
        public int HardcodedCaregiverId { get; set; } = 1;

        // List of tasks loaded for this caregiver.
        public List<Task> Tasks { get; set; } = new List<Task>();

        // This property binds the checkbox values (the task IDs that are marked as complete)
        [BindProperty]
        public List<int> CompletedTaskIds { get; set; } = new List<int>();

        public AgendaModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _taskDal = new TaskDal(_connectionString);
        }

        public void OnGet()
        {
            // Load tasks for the hardcoded caregiver.
            Tasks = _taskDal.GetTasksForCaregiver(HardcodedCaregiverId);
        }

        public IActionResult OnPost()
        {
            // Reload the tasks.
            Tasks = _taskDal.GetTasksForCaregiver(HardcodedCaregiverId);

            // For each task, check if its ID is in the posted CompletedTaskIds list.
            foreach (var task in Tasks)
            {
                bool shouldBeCompleted = CompletedTaskIds.Contains(task.Id);
                // Update the task completion status if needed.
                if (task.IsCompleted != shouldBeCompleted)
                {
                    _taskDal.UpdateTaskCompletion(task.Id, shouldBeCompleted);
                }
            }

            // Reload updated tasks.
            Tasks = _taskDal.GetTasksForCaregiver(HardcodedCaregiverId);

            // Optionally, display a message or redirect.
            return Page();
        }
    }
}
