using Final.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Final.Pages.Account
{
    public class ActiveModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private MyDataContext _dbContext;

        public String Message { get; set; }
        public bool TypeSuccess { get; set; }
        private String EmailUser { get; set; }

        public ActiveModel(MyDataContext myDContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _dbContext = myDContext;
        }
       
        public async Task<IActionResult> OnGet(string token)
        {
           
            bool isValid = IsTokenValid(token);

            if (isValid)
            {
                // Update Account - Active
                var account = _dbContext.Accounts.FirstOrDefault(x => x.Email.ToLower() == EmailUser.ToLower());

                if (account == null)
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
            else
            {
                ViewData["Message"] = "The token is not valid.";
                Message = "Token not correct or your url was expired";
                TypeSuccess = false;
            }

            return Page();
        }

        private bool IsTokenValid(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["Jwt:SecretKey"]; // Thay thế bằng secret key bạn đã sử dụng để tạo token

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey.PadRight(32))),
                ValidateIssuer = false, // Tắt kiểm tra Issuer nếu bạn không cần kiểm tra
                ValidateAudience = false, // Tắt kiểm tra Audience nếu bạn không cần kiểm tra
                ClockSkew = TimeSpan.Zero // Không chấp nhận độ chệch thời gian
            };

            SecurityToken validatedToken;
            try
            {
                var principal = handler.ValidateToken(token, validationParameters, out validatedToken);
                EmailUser  = principal.FindFirst(ClaimTypes.Email)?.Value;
                return true; // Nếu không có ngoại lệ, token được coi là hợp lệ
            }
            catch (SecurityTokenException ex)
            {
                Console.WriteLine("Error At Active =>> " + ex);

                return false; // Nếu có ngoại lệ, token không hợp lệ
            }
        }

       
    
    }

}
