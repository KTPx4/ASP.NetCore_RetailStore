using Final.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Final.Pages.Account
{
    public class ActiveModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private MyDataContext _dbContext;

        public String Message { get; set; }
        public bool TypeSuccess { get; set; }

        public ActiveModel(MyDataContext myDContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _dbContext = myDContext;
        }

        public async Task<ActionResult> OnGet(string token)
        {
            try
            {
                
                var secretKey = _configuration["Jwt:SecretKey"];
              
                // get secrekey and create jwt validator
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Convert.FromBase64String(secretKey);
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                ClaimsPrincipal claimsPrincipal;
                SecurityToken validatedToken;

                // Get Value from jwt
                claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                var userId = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;
                var userEmail = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;

                // Update Account - Active
                var account = _dbContext.Accounts.FirstOrDefault(x => x.Email.ToLower() == userEmail.ToLower());

                if(account == null)
                {
                    Message = "Your account was deleted or not Exists. Please contact your manager";
                    TypeSuccess = false;
                    
                }
                else 
                {

                    Message = "Active your Account Success";
                    TypeSuccess = true;

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, account.fullName),
                        new Claim(ClaimTypes.Email, account.Email),
                        new Claim(ClaimTypes.Role, account.Role),
                        new Claim("firstLogin", true.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                    var claimsPri = new ClaimsPrincipal(claimsIdentity);

                    // Đăng nhập người dùng                   
                    await HttpContext.SignInAsync("Cookies", claimsPri);
                    account.firstLogin = true;
                    account.isActive = true;
                    await _dbContext.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                // Log kết quả validation không thành công
                Console.WriteLine("Error At Active - OnGet -> " + ex);
                Message = "Token not correct or your url was expired, "; 
                TypeSuccess = false;

            }

            return Page();

        }
    }

}
