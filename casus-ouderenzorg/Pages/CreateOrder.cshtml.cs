using casus_ouderenzorg.DAL;
using casus_ouderenzorg.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace casus_ouderenzorg.Pages.Patients
{
    public class CreateOrderModel : PageModel
    {
        private readonly OrderDal _orderDal;

        public CreateOrderModel(OrderDal orderDal)
        {
            _orderDal = orderDal;
        }

        [BindProperty]
        public Order Order { get; set; }

        public IActionResult OnGet(int patientId)
        {
            Order = new Order
            {
                PatientID = patientId,
                OrderDate = DateTime.Now
            };
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            Order.OrderDate = DateTime.Now;

            _orderDal.CreateOrder(Order);
            return RedirectToPage("Orders", new { id = Order.PatientID });
        }

    }
}
