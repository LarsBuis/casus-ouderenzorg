using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using casus_ouderenzorg.Models;
using casus_ouderenzorg.Dal;  
using Microsoft.Extensions.Configuration;

namespace casus_ouderenzorg.Pages
{
    public class TransportDetailsModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public PlannedTransport PlannedTransport { get; set; }
        public string ConnectionString { get; }

        public TransportDetailsModel(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult OnGet(int id)
        {
            var dal = new TransportDal(ConnectionString);
            PlannedTransport = dal.GetPlannedTransportById(id);

            if (PlannedTransport == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
