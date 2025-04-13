using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using casus_ouderenzorg.DAL;
using casus_ouderenzorg.Models;

namespace casus_ouderenzorg.Pages
{
    public class AddTransportModel : PageModel
    {
        private readonly string _connectionString;
        private readonly TransportRepository _repository;

        public AddTransportModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _repository = new TransportRepository(_connectionString);
        }

        // Data loaded from the database
        public List<Driver> Drivers { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public List<Caregiver> Caregivers { get; set; }
        public List<Patient> Patients { get; set; }

        [BindProperty]
        public PlannedTransport PlannedTransport { get; set; } = new PlannedTransport();

        // Properties to store user selections.
        [BindProperty]
        public int SelectedDriverId { get; set; }
        [BindProperty]
        public int SelectedVehicleId { get; set; }
        [BindProperty]
        public int SelectedCaregiverId { get; set; }
        [BindProperty]
        public List<int> SelectedPatientIds { get; set; } = new List<int>();

        public SelectList DriverSelectList { get; set; }
        public SelectList VehicleSelectList { get; set; }
        public SelectList CaregiverSelectList { get; set; }
        public MultiSelectList PatientSelectList { get; set; }

        public void OnGet()
        {
            LoadData();
            PopulateSelectLists();
        }

        public IActionResult OnPost()
        {
            LoadData();
            PopulateSelectLists();

            // Retrieve the selected objects based on user choices.
            PlannedTransport.SelectedDriver = Drivers.FirstOrDefault(d => d.Id == SelectedDriverId);
            PlannedTransport.SelectedVehicle = Vehicles.FirstOrDefault(v => v.Id == SelectedVehicleId);
            PlannedTransport.SelectedCaregiver = Caregivers.FirstOrDefault(c => c.Id == SelectedCaregiverId);

            // Add patients as passengers (limit based on vehicle capacity minus one for caregiver).
            if (PlannedTransport.SelectedVehicle != null)
            {
                int maxPassengers = PlannedTransport.SelectedVehicle.Capacity - 1;
                foreach (var patientId in SelectedPatientIds.Take(maxPassengers))
                {
                    var patient = Patients.FirstOrDefault(p => p.Id == patientId);
                    if (patient != null)
                    {
                        PlannedTransport.AddPassenger(patient);
                    }
                }
            }

            // Persist the new transport record to the database.
            int newTransportId = _repository.AddPlannedTransport(PlannedTransport);
            return RedirectToPage("TransportSummary", new { id = newTransportId });
        }

        private void LoadData()
        {
            // Retrieve lists from the MSSQL database using the repository.
            Drivers = _repository.GetDrivers();
            Vehicles = _repository.GetVehicles();
            Caregivers = _repository.GetCaregivers();
            Patients = _repository.GetPatients();
        }

        private void PopulateSelectLists()
        {
            DriverSelectList = new SelectList(Drivers, "Id", "Name");
            VehicleSelectList = new SelectList(Vehicles, "Id", "Name");
            CaregiverSelectList = new SelectList(Caregivers, "Id", "Name");
            PatientSelectList = new MultiSelectList(Patients, "Id", "Name");
        }
    }
}
