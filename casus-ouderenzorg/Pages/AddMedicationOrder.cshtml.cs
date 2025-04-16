using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using casus_ouderenzorg.DAL;
using casus_ouderenzorg.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace casus_ouderenzorg.Pages
{
    public class AddMedicationOrderModel : PageModel
    {
        private readonly string _connectionString;
        private readonly MedicationOrderDal _medicationOrderDal;
        private readonly PharmacyDal _pharmacyDal;
        private readonly MedicationDal _medicationDal;

        public AddMedicationOrderModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _medicationOrderDal = new MedicationOrderDal(_connectionString);
            _pharmacyDal = new PharmacyDal(_connectionString);
            _medicationDal = new MedicationDal(_connectionString);
        }

        // PatientId is provided via query string (e.g., /AddMedicationOrder?patientId=1)
        [BindProperty(SupportsGet = true)]
        public int PatientId { get; set; }

        // Order header fields
        [BindProperty]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        [BindProperty]
        public string Status { get; set; } = "Pending";
        [BindProperty]
        public int PharmacyID { get; set; }

        // Order line details
        [BindProperty]
        public int MedicationID { get; set; }
        [BindProperty]
        public string Dosage { get; set; }
        [BindProperty]
        public string Frequency { get; set; }
        [BindProperty]
        public string TreatmentDuration { get; set; }

        public SelectList PharmacySelectList { get; set; }
        public SelectList MedicationSelectList { get; set; }

        public void OnGet()
        {
            // Load dropdown lists
            var pharmacies = _pharmacyDal.GetPharmacies();
            PharmacySelectList = new SelectList(pharmacies, "PharmacyID", "Name");

            var medications = _medicationDal.GetMedications();
            MedicationSelectList = new SelectList(medications, "MedicationID", "Name");
        }

        public IActionResult OnPost()
        {
            // Remove ModelState check if you're debugging issues
            if (!ModelState.IsValid)
            {
                OnGet(); // Reload dropdowns
                return Page();
            }

            // Create a new Order record.
            Order newOrder = new Order
            {
                OrderDate = OrderDate,
                Status = Status,
                PatientID = PatientId,
                // You might set CaregiverID here if applicable. Using 0 as a placeholder.
                CaregiverID = 0,
                PharmacyID = PharmacyID,
                OrderLines = new List<OrderLine>()
            };

            int newOrderId = _medicationOrderDal.InsertOrder(newOrder);

            // Create a new OrderLine record.
            OrderLine newOrderLine = new OrderLine
            {
                OrderID = newOrderId,
                MedicationID = MedicationID,
                Dosage = Dosage,
                Frequency = Frequency,
                TreatmentDuration = TreatmentDuration
            };

            _medicationOrderDal.InsertOrderLine(newOrderLine);

            // Redirect to MedicationOverview page.
            return RedirectToPage("/MedicationOverview", new { patientId = PatientId });
        }
    }
}
