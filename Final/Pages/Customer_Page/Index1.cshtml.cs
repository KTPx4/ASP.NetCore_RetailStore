using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Final.Pages.Customer_Page
{
    public class Index1Model : PageModel
    {
        public string MyVariable { get; set; } = "Initial Value";
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            // Handle the form submission
            MyVariable = "New Value";

            // Redirect back to the page to update the displayed value
            return RedirectToPage();
        }
    }
}
