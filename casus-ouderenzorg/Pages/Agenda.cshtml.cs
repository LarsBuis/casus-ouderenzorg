using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using casus_ouderenzorg.Models;
using casus_ouderenzorg.DAL;
using System;
using System.Collections.Generic;
using Task = casus_ouderenzorg.Models.Task;

namespace casus_ouderenzorg.Pages
{
    public class AgendaModel : PageModel
    {
        private readonly string _connectionString;
        private readonly TaskDal _taskDal;

        public AgendaModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _taskDal = new TaskDal(_connectionString);
        }

        [BindProperty(SupportsGet = true)]
        public DateTime SelectedDate { get; set; } = DateTime.Today;

        public List<Task> Tasks { get; set; } = new List<Task>();

        [BindProperty]
        public List<int> CompletedTaskIds { get; set; } = new List<int>();

        public void OnGet()
        {
            Tasks = _taskDal.GetTasksByDate(SelectedDate);
        }

        public IActionResult OnPost()
        {
            // Reload tasks for the selected date.
            Tasks = _taskDal.GetTasksByDate(SelectedDate);

            // Update each task’s completion status.
            foreach (var task in Tasks)
            {
                bool shouldBeCompleted = CompletedTaskIds.Contains(task.Id);
                if (task.IsCompleted != shouldBeCompleted)
                {
                    _taskDal.UpdateTaskCompletion(task.Id, shouldBeCompleted);
                }
            }

            // Reload tasks after updating and return to the page.
            Tasks = _taskDal.GetTasksByDate(SelectedDate);
            return Page();
        }
    }
}
