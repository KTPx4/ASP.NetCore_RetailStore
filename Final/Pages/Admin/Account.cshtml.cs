using BCrypt.Net;
using Final.Models;
using Final.Modules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using NuGet.Common;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Final.Pages.Admin
{
    [Authorize]
	[Authorize(Roles ="Admin")]
    public class AccountModel : PageModel
    {

		private MyDataContext _dbContext;
		private readonly GmailSender _emailService;
		private readonly IConfiguration _configuration;
		public List<Models.Account> Accounts { get; set; }
		public Models.Account User { get; set; }

		[BindProperty(SupportsGet = true)]
		public string SearchText { get; set; }

		[BindProperty]
		public NewAccount newAccount { get; set; }
		//[BindProperty]
		//public String IdStatus { get; set; }

		[TempData]
		public string ErrorMessage { get; set; }

		[TempData]
		public string SuccessMessage { get; set; }

		public AccountModel(MyDataContext myDContext, GmailSender emailService,  IConfiguration configuration)
        {
            _dbContext = myDContext;
			_emailService = emailService;
			_configuration = configuration;
		}

		public void OnGet()
		{
			LoadAccounts();
		}

		public void OnPost()
		{
			LoadAccounts();
		}

		public async Task<IActionResult> OnPostBlockAccount(String accountId)
		{
			

			if (accountId == null || accountId == "")
			{
				ErrorMessage = "Please provide ID Employee";
			}
			else 
			{
				try
				{
					var account = _dbContext.Accounts.First(a => a.Id.ToString() == accountId);
					if (account == null)
					{
						ErrorMessage = "Account not exists.";
					}
					else
					{
						// Set isDeleted to the opposite value
						account.isDeleted = !account.isDeleted;

						// Lưu thay đổi vào database
						_dbContext.SaveChanges();
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine( "Error at Admin Account - Block Account" + ex);
				}
			}
			return Redirect(Request.Headers["Referer"].ToString());
		}

		public async Task<IActionResult> OnPostSendActive(String accountId)
		{
			if (accountId == null || accountId == "")
			{
				ErrorMessage = "Please provide ID Employee";
			}
			else
			{
				try
				{
					var account = _dbContext.Accounts.First(a => a.Id.ToString() == accountId);
					if (account == null)
					{
						ErrorMessage = "Account not exists.";
					}
					else
					{
						SendActive(account);
						SuccessMessage = "Send Email Success";
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error at Admin Account - Block Account" + ex);
				}
			}

			return Redirect(Request.Headers["Referer"].ToString());
		}

		public async Task<IActionResult> OnPostCreateAccount()
		{
			// Gửi email thông báo sử dụng GmailSender
			//await _emailService.SendEmailAsync(newAccount.Email, "Subject of Email", "Body of Email");
			
			if (newAccount.FullName == null || newAccount.FullName == "" || newAccount.Email == "" || newAccount.Email == null)
			{
				ErrorMessage = "No";	
			}
			else
			{

				try
				{
					if (_dbContext.Accounts.Any(a => a.Email.ToLower() == newAccount.Email.ToLower()))
					{
						ErrorMessage = "Email already exists.";						
					}
					else
					{

						SuccessMessage = "Create Success";
						var Username = newAccount.Email.Split("@")[0];
						var PassHash = BCrypt.Net.BCrypt.HashPassword(Username, 10);
						
						var newAcc = new Models.Account()
						{
							fullName = newAccount.FullName,
							Email = newAccount.Email,
							Password = PassHash,
							User = Username,
							Role = "User",
							isActive = false,
							isDeleted = false,
							firstLogin = true
						};

						_dbContext.Accounts.Add(newAcc);
						_dbContext.SaveChanges();			
					

						
						var account = _dbContext.Accounts.FirstOrDefault(e => e.Email.ToLower() == newAccount.Email.ToLower());
						if (account != null)
						{
							account.NameAvt = account.Id.ToString() + ".png";
						}
						_dbContext.SaveChanges();

						SendActive(account);
						
					}

					// Thêm tài khoản mới vào cơ sở dữ liệu
					//_dbContext.Accounts.Add(NewAccount);
					//_dbContext.SaveChanges();
				}
				catch (Exception ex)
				{
					ErrorMessage = "Create Failed. Try again!";
					Console.WriteLine("Error At add new Staff - Account.cshtml.cs ->" + ex.ToString());
				}
				


			}
				return Redirect(Request.Headers["Referer"].ToString());

		}
		
		private async void SendActive(Models.Account account)
		{
			var Server = _configuration["Web:Server"];
			var tokenString = CreateToken(account);
			var html = $"<p> Hí bạn ,</p> <p> Vui lòng kích hoạt tài khoản của bạn <a href = \"{Server}/Account/Active?token={tokenString}\" > Tại đây </a> </p> <strong> Liên Kết sẽ hết hạn trong 1 phút, vui lòng nhanh cái tay lên ^^</strong> <p> Thank you </p> ";

			await _emailService.SendEmail(account.Email, "Active Account", html);

		}

		private String CreateToken(Models.Account account)
		{
            var secretKey = _configuration["Jwt:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey.PadRight(32)));
            //var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //var token = new JwtSecurityToken(
            //	issuer: null,
            //	audience: null,
            //	claims: null,
            //	expires: DateTime.UtcNow.AddMinutes(1), // Thời hạn 1 phút
            //	signingCredentials: credentials
            //);

            // Tạo JWT Token

            var tokenHandler = new JwtSecurityTokenHandler();


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name, account.fullName),
                        new Claim(ClaimTypes.Email, account.Email)
                }),
                Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;

		}

		private async void LoadAccounts()
		{
			try
			{
				// Lấy danh sách tài khoản từ cơ sở dữ liệu
				String Email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

				if (string.IsNullOrEmpty(SearchText))
				{
					Accounts = _dbContext.Accounts
						.Where(account => account.Email.ToLower() != Email.ToLower())
						.ToList();
				}
				else
				{
					Accounts = _dbContext.Accounts
						.Where(account =>
							account.Email.ToLower() != Email.ToLower() &&
							(account.User.ToLower().Contains(SearchText.ToLower()) ||
							 account.fullName.ToLower().Contains(SearchText.ToLower()) ||
							 account.Email.ToLower().Contains(SearchText.ToLower())))
						.ToList();
				}

				User = _dbContext.Accounts
					.Where(a => a.Email.ToLower() == Email.ToLower())
					.First();

				string FolderImage = $"wwwroot/public/account/img";

				foreach (var account in Accounts)
				{
					string folderUser = FolderImage + $"/{account.Id.ToString()}";
					string ImgUrl = $"{folderUser}/{account.NameAvt}";

					if (!Directory.Exists(folderUser))
					{
						Directory.CreateDirectory(folderUser);
						// Kiểm tra xem tệp hình ảnh có tồn tại hay không
						if (!Directory.Exists(ImgUrl))
						{
							// Nếu không tồn tại, sao chép từ tệp mẫu "user.png"
							// Save the image

							string defaultImagePath = Path.Combine("wwwroot", "public", "account", "img", "user.png");
							string destinationPath = Path.Combine(ImgUrl);

							using (var sourceStream = new FileStream(defaultImagePath, FileMode.Open))
							using (var destinationStream = new FileStream(destinationPath, FileMode.Create))
							{
								await sourceStream.CopyToAsync(destinationStream);
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine("Error At LoadAccount - Account.cshtml.cs => " + ex);
			}
			
		
		}
	}

	public class NewAccount
	{

		[Required(ErrorMessage = "Full Name is required.")]
		public string FullName { get; set; }

		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Invalid email format.")]
		public string Email { get; set; }
	}
}
