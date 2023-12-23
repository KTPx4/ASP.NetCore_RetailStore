using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Final.Pages.Account
{
    public class ForbiddenModel : PageModel
    {
        public ActionResult OnGet()
        {
            String Role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if(Role.ToLower().Contains("admin"))
            {
                return Redirect("/");
            }
            return Page();
        }
    }
}
