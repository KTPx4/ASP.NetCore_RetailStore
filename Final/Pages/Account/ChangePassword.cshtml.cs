using Final.Models;
using Final.Modules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using System.Security.Claims;

namespace Final.Pages.Account
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private MyDataContext _dbContext;

        public String newPass {  get; set; }
        public String confirmPass { get; set; }

        public String ErrorMess { get; set; }

        public ChangePasswordModel(MyDataContext myDContext, IConfiguration configuration)
        {
            _dbContext = myDContext;
            _configuration = configuration;
        }
                
        public async Task<ActionResult> OnGet()
        {
            return Page();
        }
        
        public async Task<ActionResult> OnPost(string newPass, string confirmPass)
        {
            try
            {
                this.newPass = newPass;
                this.confirmPass = confirmPass;

                String email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                Console.WriteLine(email);

                if (newPass == "" || newPass == null)
                {
                    ErrorMess = "Please input new password!";
                }
                else if (confirmPass == "" || confirmPass == null)
                {
                    ErrorMess = "Please input confirm password!";
                }
                else if (newPass != confirmPass)
                {
                    ErrorMess = "New password and confirm password not match!";
                }

                var account = _dbContext.Accounts.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());

                if (account == null)
                {
                    ErrorMess = "There was a problem with your login.";
                }
                else
                {
                    var hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(newPass);
                    account.Password = hashedNewPassword;
                    account.firstLogin = false;

                    await _dbContext.SaveChangesAsync();

                    // Cập nhật claim "firstLogin" trong HttpContext
                    await HttpContext.UpdateClaimAsync("firstLogin", false.ToString());

                    return Redirect("/");
                }

                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at Change Password - first login -> " + ex );
                return Page();
            }
           
        }
   

    }
}
