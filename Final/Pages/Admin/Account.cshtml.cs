using Final.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Final.Pages.Admin
{
	[Authorize]
	[Authorize(Roles ="Admin")]
    public class AccountModel : PageModel
    {

		private MyDataContext _dbContext;
		public List<Models.Account> Accounts { get; set; }
		public Models.Account User { get; set; }

		[BindProperty(SupportsGet = true)]
		public string SearchText { get; set; }

		[BindProperty]
		public NewAccount newAccount { get; set; }

		[TempData]
		public string ErrorMessage { get; set; }

		[TempData]
		public string SuccessMessage { get; set; }

		public AccountModel(MyDataContext myDContext)
        {
            _dbContext = myDContext;

        }

		public void OnGet()
		{
			LoadAccounts();
		}

		public void OnPost()
		{
			LoadAccounts();
		}
		public IActionResult OnPostCreateAccount()
		{
			
			if (newAccount.FullName == null || newAccount.FullName == "" || newAccount.Email == "" || newAccount.Email == null)
			{
				ErrorMessage = "No";
				return Redirect(Request.Headers["Referer"].ToString());		
				
			}
			else
			{
				if (_dbContext.Accounts.Any(a => a.Email.ToLower() == newAccount.Email.ToLower()))
				{
					ErrorMessage = "Email already exists.";
					return Redirect(Request.Headers["Referer"].ToString());
				}
				SuccessMessage = "Success";
				// Thêm tài khoản mới vào cơ sở dữ liệu
				//_dbContext.Accounts.Add(NewAccount);
				//_dbContext.SaveChanges();

				return Redirect(Request.Headers["Referer"].ToString());

			}

		}

		private async void LoadAccounts()
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
