using casus_ouderenzorg.DAL;
using casus_ouderenzorg.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace casus_ouderenzorg.Pages.Patients
{
    public class mypatientsModel : PageModel
    {
        private readonly PatientDal _patientDal;
        private const int CaregiverId = 1;

        public mypatientsModel(PatientDal patientDal)
        {
            _patientDal = patientDal;
        }

        public List<Patient> Patients { get; set; } = new List<Patient>();

        public void OnGet()
        {
            Patients = _patientDal.GetPatientsByCaregiver(CaregiverId);
        }
    }
}
