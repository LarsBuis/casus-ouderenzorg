using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using casus_ouderenzorg.Models;
using casus_ouderenzorg.DAL;
using Task = casus_ouderenzorg.Models.Task;

namespace casus_ouderenzorg.Pages
{
    public class TasksOverviewModel : PageModel
    {
        private readonly string _connectionString;
        private readonly TaskDal _taskDal;

        // Hardcode the patient id (for example, 1) to filter tasks.
        public int hardcodedCaregiverId { get; set; } = 1;

        // The list of tasks loaded from the dal.
        public List<Task> Tasks { get; set; } = new List<Task>();

        public TasksOverviewModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _taskDal = new TaskDal(_connectionString);
        }

        public void OnGet()
        {
            // Use the hardcoded patient ID to load tasks.
            //Tasks = _taskDal.GetTasksForPatient(HardcodedPatientId);
            // To load tasks by caregiver instead, comment out the above and use:
            Tasks = _taskDal.GetTasksForCaregiver(hardcodedCaregiverId);
        }
    }
}
