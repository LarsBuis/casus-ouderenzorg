using casus_ouderenzorg.Dal;
using casus_ouderenzorg.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace casus_ouderenzorg.Pages
{
    public class AddTransportModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public string ConnectionString { get; }

        public AddTransportModel(IConfiguration configuration)
        {
            _configuration = configuration;
            // Assumes a connection string named "DefaultConnection" in appsettings.json
            ConnectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        [BindProperty]
        public PlannedTransport PlannedTransport { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Insert the new planned transport using the custom DAL
            var dal = new TransportDal(ConnectionString);
            dal.AddPlannedTransport(PlannedTransport);

            // Redirect back to the list of transports
            return RedirectToPage("Transport");
        }
    }
}
