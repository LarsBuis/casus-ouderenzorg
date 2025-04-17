using casus_ouderenzorg.DAL;
using casus_ouderenzorg.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Task = casus_ouderenzorg.Models.Task;

namespace casus_ouderenzorg.Pages.Agenda
{
    public class IndexModel : PageModel
    {
        private readonly TaskDal _taskDal;
        private const int CaregiverId = 1;

        public IndexModel(TaskDal taskDal)
        {
            _taskDal = taskDal;
        }

        [BindProperty(SupportsGet = true)]
        public string Date { get; set; } = string.Empty;

        public List<Task> Tasks { get; set; } = new List<Task>();
        public string SelectedDate { get; set; } = string.Empty;
        public string DisplayDate { get; set; } = string.Empty;

        public void OnGet()
        {
            // Parse the selected date or default to today
            DateTime selected;
            if (!DateTime.TryParse(Date, out selected))
            {
                selected = DateTime.Today;
            }

            SelectedDate = selected.ToString("yyyy-MM-dd");
            DisplayDate = selected.ToString("dd MMMM yyyy");

            Tasks = _taskDal.GetTasks()
                .Where(t => t.CaregiverID == CaregiverId && t.TaskDate.Date == selected.Date)
                .OrderBy(t => t.StartTime)
                .ToList();
        }
    }
}