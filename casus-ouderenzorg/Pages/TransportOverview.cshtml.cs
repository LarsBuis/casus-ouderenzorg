using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using casus_ouderenzorg.Models;
using casus_ouderenzorg.DAL;
using Microsoft.Extensions.Configuration;

namespace casus_ouderenzorg.Pages
{
    public class TransportOverviewModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly TransportRepository _repository;

        public List<PlannedTransport> PlannedTransports { get; set; }

        public TransportOverviewModel(IConfiguration configuration)
        {
            _configuration = configuration;
            // Assumes your connection string is named "DefaultConnection"
            _repository = new TransportRepository(_configuration.GetConnectionString("DefaultConnection"));
        }

        public void OnGet()
        {
            // Retrieve the list of planned transports from your MSSQL database
            PlannedTransports = _repository.GetPlannedTransports();
        }
    }
}
