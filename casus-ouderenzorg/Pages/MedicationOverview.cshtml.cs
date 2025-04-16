using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using casus_ouderenzorg.DAL;
using casus_ouderenzorg.Models;
using System.Collections.Generic;

namespace casus_ouderenzorg.Pages
{
    public class MedicationOverviewModel : PageModel
    {
        private readonly string _connectionString;
        private readonly OrderDal _orderDal;

        // Bind from querystring: ?patientId=value
        [BindProperty(SupportsGet = true)]
        public int PatientId { get; set; }

        // Property to hold the patient's name (could be fetched from a PatientDal)
        public string PatientName { get; set; }

        // List of orders for this patient (English name for Bestellingen)
        public List<Order> Orders { get; set; } = new List<Order>();

        public MedicationOverviewModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _orderDal = new OrderDal(_connectionString);
        }

        public void OnGet()
        {
            // Retrieve medication orders for the given patient.
            Orders = _orderDal.GetOrdersByPatientId(PatientId);

            // In a real scenario, you might load the patient details from a PatientDal.
            // For now, we simply use a placeholder:
            PatientName = $"Patient {PatientId}";
        }
    }
}
