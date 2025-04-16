using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace casus_ouderenzorg.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            // Any GET logic here.
        }

        public IActionResult OnPost()
        {
            // Hardcoded login check
            if (Username == "admin" && Password == "admin")
            {
                // On success, redirect to a page of your choice.
                return RedirectToPage("/account");
            }
            else
            {
                ErrorMessage = "Invalid username or password.";
                return Page();
            }
        }
    }
}
