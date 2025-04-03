using casus_ouderenzorg.Dal;
using casus_ouderenzorg.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace casus_ouderenzorg.Pages
{
    public class TransportModel : PageModel
    {
        private readonly string _connectionString;
        public List<PlannedTransport> PlannedTransports { get; set; } = new List<PlannedTransport>();

        public TransportModel(IConfiguration configuration)
        {
            // Assumes a connection string named "DefaultConnection" in appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task OnGetAsync()
        {
            // Create an instance of the DAL and retrieve the transports
            var dal = new TransportDal(_connectionString);
            PlannedTransports = dal.GetPlannedTransports();
            await Task.CompletedTask; // For a true async scenario, you can refactor GetPlannedTransports to be async.
        }
    }
}
