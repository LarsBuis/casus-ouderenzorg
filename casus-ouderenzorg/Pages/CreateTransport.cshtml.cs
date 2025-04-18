using System;
using casus_ouderenzorg.DAL;
using casus_ouderenzorg.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace casus_ouderenzorg.Pages.Transport
{
    public class CreateTransportModel : PageModel
    {
        private readonly TransportDal _transportDal;
        private readonly PatientDal _patientDal;
        private readonly DriverDal _driverDal;
        private readonly VehicleDal _vehicleDal;
        private const int CaregiverId = 1;

        [BindProperty]
        public casus_ouderenzorg.Models.Transport Transport { get; set; }

        public SelectList PatientList { get; set; }
        public SelectList DriverList { get; set; }
        public SelectList VehicleList { get; set; }

        // Inject all DALs via constructor
        public CreateTransportModel(
            TransportDal transportDal,
            PatientDal patientDal,
            DriverDal driverDal,
            VehicleDal vehicleDal)
        {
            _transportDal = transportDal;
            _patientDal = patientDal;
            _driverDal = driverDal;
            _vehicleDal = vehicleDal;
        }

        public void OnGet()
        {
            Transport = new casus_ouderenzorg.Models.Transport
            {
                CaregiverID = CaregiverId,
                TransportDate = DateTime.Today,
                StartTime = TimeSpan.Zero,
                ReturnTime = TimeSpan.Zero
            };

            PatientList = new SelectList(_patientDal.GetAllPatients(), "PatientID", "Name");
            DriverList = new SelectList(_driverDal.GetAllDrivers(), "DriverID", "Name");
            VehicleList = new SelectList(_vehicleDal.GetAllVehicles(), "VehicleID", "Name");
        }

        public IActionResult OnPost()
        {
            PatientList = new SelectList(_patientDal.GetAllPatients(), "PatientID", "Name");
            DriverList = new SelectList(_driverDal.GetAllDrivers(), "DriverID", "Name");
            VehicleList = new SelectList(_vehicleDal.GetAllVehicles(), "VehicleID", "Name");

            if (!ModelState.IsValid)
                return Page();

            Transport.CaregiverID = CaregiverId;

            _transportDal.CreateTransport(Transport);
            return RedirectToPage("Transport");
        }
    }
}