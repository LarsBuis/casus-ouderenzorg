using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class LoginModel : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; }

    public string ErrorMessage { get; set; }

    public void OnGet()
    {
        // Initialize anything if needed.
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Hardcoded login credentials
        var hardcodedUsername = "admin";
        var hardcodedPassword = "admin";

        if (Input.Username == hardcodedUsername && Input.Password == hardcodedPassword)
        {
            // Login successful. Redirect to a secure page (e.g., Index).
            return RedirectToPage("Index");
        }
        else
        {
            // Login failed, show an error message.
            ErrorMessage = "Invalid username or password.";
            return Page();
        }
    }

    public class InputModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
