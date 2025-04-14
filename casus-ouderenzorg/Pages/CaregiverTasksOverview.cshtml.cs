using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using casus_ouderenzorg.Models;
using casus_ouderenzorg.DAL;
using Task = casus_ouderenzorg.Models.Task;

namespace casus_ouderenzorg.Pages
{
    public class CaregiverTasksOverviewModel : PageModel
    {
        private readonly string _connectionString;
        private readonly TaskDal _taskDal;

        // Hardcoded caregiver ID (for example, 1) for demonstration.
        public int HardcodedCaregiverId { get; set; } = 1;

        // List of tasks loaded for this caregiver.
        public List<Task> Tasks { get; set; } = new List<Task>();

        public CaregiverTasksOverviewModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _taskDal = new TaskDal(_connectionString);
        }

        public void OnGet()
        {
            // Use the hardcoded caregiver ID to load tasks.
            Tasks = _taskDal.GetTasksForCaregiver(HardcodedCaregiverId);
        }
    }
}
