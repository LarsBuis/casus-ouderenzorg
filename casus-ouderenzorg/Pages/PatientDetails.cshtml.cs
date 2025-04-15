using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using casus_ouderenzorg.Models;
using casus_ouderenzorg.DAL;
using Microsoft.Extensions.Configuration;

namespace casus_ouderenzorg.Pages
{
    public class PatientDetailsModel : PageModel
    {
        private readonly string _connectionString;
        private readonly PatientDal _patientDal;

        public PatientDetailsModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _patientDal = new PatientDal(_connectionString);
        }

        [BindProperty]
        public Patient Patient { get; set; }

        public IActionResult OnGet(int id)
        {
            // Retrieve the specific patient by id.
            Patient = _patientDal.GetPatientById(id);
            if (Patient == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
