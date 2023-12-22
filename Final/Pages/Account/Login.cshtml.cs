using Final.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;

namespace Final.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly IConfiguration _configuration;
        private MyDataContext _dbContext;


        public String ErrorMess {  get; set; }
        public String UserName {  get; set; }
        public String Password { get; set; }

        public bool isLoading { get; set; }


        public LoginModel(MyDataContext myDContext, ILogger<LoginModel> logger, IConfiguration configuration)
        {
            _dbContext = myDContext;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<ActionResult> OnPost(string user, string password)
        {
            UserName = user;
            Password = password;

            if (UserName == null)
            {
                ErrorMess = "User Name must have value!";
            }
            else if (Password == null)
            {
                ErrorMess = "Password must have value!";
            }
            else
            {



                var account = _dbContext.Accounts.FirstOrDefault(x => x.User.ToLower() == UserName.ToLower());
                if (account == null || !BCrypt.Net.BCrypt.Verify(Password, account.Password))
                {
                    ErrorMess = "Account or Password is not Correct";
                }
                else 
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, account.fullName),
                        new Claim(ClaimTypes.Email, account.Email),
                        new Claim(ClaimTypes.Role, account.Role)
                        // Thêm các claim khác nếu cần
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    // Đăng nhập người dùng                   
                    await HttpContext.SignInAsync("Cookies", claimsPrincipal);

                    return RedirectToPage("/Index"); // Chuyển hướng đến trang chính
                }
            }

            return Page();
        }

     
        public IActionResult OnGetLogout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
