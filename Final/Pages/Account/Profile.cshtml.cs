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
    using System.Security.Claims;
    using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace Final.Pages.Account
{
        [Authorize]
        public class ProfileModel : PageModel
        {
            private readonly Final.Models.MyDataContext _context;
            public String ErrorMess { get; set; }
            public String SuccessMess { get; set; }

            public String NameAvt { get; set; }
            public ProfileModel(Final.Models.MyDataContext context)
            {
                _context = context;
           

            }


            public async Task<ActionResult> OnGet()
            {
                if (_context.Accounts == null)
                {
                    return NotFound();
                }
                String Email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
           
                var account =  await _context.Accounts.FirstOrDefaultAsync(m => m.Email.ToLower() == Email.ToLower());
                if (account == null )
                {
                    return NotFound();
                }
                ViewData["UserEmail"] = account.Email;
                ViewData["UserRole"] = account.Role;
                ViewData["UserName"] = account.fullName;
                ViewData["NameAvt"] = account.Id.ToString() + "/" + account.NameAvt;
                Console.WriteLine( account.NameAvt);
                // Path to save the image
                string FolderImage = $"wwwroot/public/account/img/{account.Id.ToString()}/";         
                string ImgUrl = FolderImage + "/" + account.NameAvt;

                // Check if the directory exists, if not, create it
                if (!Directory.Exists(FolderImage))
                {
                    Directory.CreateDirectory(FolderImage);
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

                ErrorMess = TempData["ErrorMess"]?.ToString();
                SuccessMess = TempData["SuccessMess"]?.ToString();
                return Page();
            }

            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see https://aka.ms/RazorPagesCRUD.
            public async Task<ActionResult> OnPost(String fullName,String oldPass, String newPass, IFormFile profileImage)
            {
              
               if(fullName == null || fullName == "")
               {

                    TempData["ErrorMess"] = "Please Input Full Name";
               }

                try
                {
                    var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                    var account = _context.Accounts.FirstOrDefault(e => e.Email.ToLower() == email.ToLower());
                    if (account == null)
                    {
                        TempData["ErrorMess"] = "Account not exists";
                    }
                    else
                    {
                        account.fullName = fullName;
                        bool isS = true;
                        if(oldPass != null && oldPass != "")
                        {
                            var trust = BCrypt.Net.BCrypt.Verify(oldPass, account.Password);
                            if(!trust)
                            {
                                TempData["ErrorMess"] = "Old Password not correct";
                                isS = false;
                            }
                            else 
                            {
                                var newhash = BCrypt.Net.BCrypt.HashPassword(newPass, 10);
                                account.Password = newhash;
                            }
                        }
                    
                        // Check if an image is uploaded
                        if (profileImage != null && profileImage.Length > 0)
                        {
                            // Get the user's ID (you may need to convert it to a string or use a specific format)
                            string userId = account.Id.ToString();

                            // Path to save the image
                            string imagePath = $"wwwroot/public/account/img/{userId}/";
                            string imageName = account.NameAvt;
                      
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

                        if (isS) TempData["SuccessMess"] = "Update Success";
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMess"] = "Some problem with your profile";
                    Console.WriteLine("Error at Profile - Update -> " + ex);
                }
            

                return RedirectToPage();
            }

            private bool AccountExists(ObjectId id)
            {
              return (_context.Accounts?.Any(e => e.Id == id)).GetValueOrDefault();
            }
        }
    }
