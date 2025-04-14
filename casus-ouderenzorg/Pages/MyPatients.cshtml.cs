using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using casus_ouderenzorg.Models;
using casus_ouderenzorg.DAL;

namespace casus_ouderenzorg.Pages
{
    public class MyPatientsModel : PageModel
    {
        private readonly string _connectionString;
        private readonly PatientDal _patientDal;

        // The list of patients assigned to caregiver ID 1.
        public List<Patient> Patients { get; set; } = new List<Patient>();

        public MyPatientsModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _patientDal = new PatientDal(_connectionString);
        }

        public void OnGet()
        {
            // Retrieve patients with CaregiverId = 1.
            Patients = _patientDal.GetPatientsByCaregiverId(1);
        }
    }
}
