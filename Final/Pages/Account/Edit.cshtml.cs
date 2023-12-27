using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Final.Models;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Final.Pages.Account
{
    public class EditModel : PageModel
    {
        private readonly Final.Models.MyDataContext _context;

        public EditModel(Final.Models.MyDataContext context)
        {
            _context = context;
        }

      

        [BindProperty]
        public EditAccountViewModel EditAccount { get; set; } = new EditAccountViewModel();


        [BindProperty]
        public IFormFile? profileImage { get; set; }

        public async Task<IActionResult> OnGetAsync(ObjectId id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account =  await _context.Accounts.FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }
            // Map dữ liệu từ Account sang EditAccountViewModel
            EditAccount = new EditAccountViewModel
            {
                Id = account.Id,
                FullName = account.fullName,
                Role = account.Role,
                IsActive = account.isActive,
                IsDeleted = account.isDeleted,
                FirstLogin = account.firstLogin,
                AgentID = account.AgentID,
                NameAvt = account.NameAvt,
                Email = account.Email,
                User = account.User,
            };

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {          

            try
            {
                var existingAccount = await _context.Accounts.FindAsync(EditAccount.Id);

                if (existingAccount != null)
                {
                    // Chỉ cập nhật những trường mong muốn
                    existingAccount.fullName = EditAccount.FullName;
                    existingAccount.Role = EditAccount.Role;
                    existingAccount.isActive = EditAccount.IsActive;
                    existingAccount.isDeleted = EditAccount.IsDeleted;
                    existingAccount.firstLogin = EditAccount.FirstLogin;
                    existingAccount.AgentID = EditAccount.AgentID;      
                    existingAccount.User = EditAccount.User;

                    // ... thêm các trường cần cập nhật khác

                    // Đánh dấu rằng đối tượng này đã thay đổi
                    _context.Entry(existingAccount).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Xử lý trường hợp không tìm thấy bản ghi
                    return NotFound();
                }

           
                // Check if an image is uploaded
                if (profileImage != null && profileImage.Length > 0)
                {
                    // Get the user's ID (you may need to convert it to a string or use a specific format)
                    string userId = existingAccount.Id.ToString();

                    // Path to save the image
                    string imagePath = $"wwwroot/public/account/img/{userId}/";
                    string imageName = existingAccount.NameAvt;
                   
                    // Check if the directory exists, if not, create it
                    if (!Directory.Exists(imagePath))
                    {
                        Directory.CreateDirectory(imagePath);
                    }

                    // Save the image
                    string imagePathWithName = Path.Combine(imagePath, imageName);
                    using (var stream = new FileStream(imagePathWithName, FileMode.Create))
                    {
                         await profileImage.CopyToAsync(stream);
                    }

                    // Update the image path in your model or database

                }
                else
                {
                    //Console.WriteLine("No Img");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(EditAccount.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Admin/Account");
        }

        private bool AccountExists(ObjectId id)
        {
          return (_context.Accounts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
    public class EditAccountViewModel
    {
        public ObjectId Id { get; set; }
       
        [BindNever]
        public string Email { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }

        [Display(Name ="User")]
        public string User { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Is Blocked")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Is First Login")]
        public bool FirstLogin { get; set; }

        [Display(Name = "Agent ID")]
        public string AgentID { get; set; }

        [Display(Name = "Avatar Name")]
        public string NameAvt { get; set; }
    }
}
