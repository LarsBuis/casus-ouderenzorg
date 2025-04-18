using casus_ouderenzorg.DAL;
using casus_ouderenzorg.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace casus_ouderenzorg.Pages.Transport
{
    public class TransportModel : PageModel
    {
        private readonly TransportDal _transportDal;
        private const int CaregiverId = 1;

        public TransportModel(TransportDal transportDal)
        {
            _transportDal = transportDal;
        }

        public List<TransportView> Transports { get; set; } = new List<TransportView>();

        public void OnGet()
        {
            Transports = _transportDal.GetTransportsByCaregiver(CaregiverId);
        }
    }
}