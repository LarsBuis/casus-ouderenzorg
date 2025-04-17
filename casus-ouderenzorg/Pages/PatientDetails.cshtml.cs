using casus_ouderenzorg.DAL;
using casus_ouderenzorg.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace casus_ouderenzorg.Pages.Patients
{
    public class DetailsModel : PageModel
    {
        private readonly PatientDal _patientDal;
        public DetailsModel(PatientDal patientDal)
        {
            _patientDal = patientDal;
        }

        public Patient Patient { get; set; }

        public IActionResult OnGet(int id)
        {
            Patient = _patientDal.GetPatientById(id);
            if (Patient == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}