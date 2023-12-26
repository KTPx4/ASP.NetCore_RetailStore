using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Final.Pages.Transaction_Page
{
    public class SourcePageModel : PageModel
    {
        public void OnGet()
        {
           
        }
        [TempData]
        public List<string> MyList { get; set; }
        public IActionResult OnPost()
        {
            // Populate or manipulate the list if needed
            MyList = new List<string> { "Item1", "Item2", "Item3" };


            return RedirectToPage("DestinationPage" );
        }
    }
}
