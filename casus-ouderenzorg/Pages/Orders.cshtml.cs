using casus_ouderenzorg.DAL;
using casus_ouderenzorg.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace casus_ouderenzorg.Pages.Patients
{
    public class OrdersModel : PageModel
    {
        private readonly OrderDal _orderDal;
        public OrdersModel(OrderDal orderDal)
        {
            _orderDal = orderDal;
        }

        public int PatientId { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();

        public IActionResult OnGet(int id)
        {
            PatientId = id;
            Orders = _orderDal.GetOrdersByPatient(id);
            return Page();
        }
    }
}