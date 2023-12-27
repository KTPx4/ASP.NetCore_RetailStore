using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Final.Models;
using Final.util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Final.Pages.Product_Page
{
    public class CreateModel : PageModel
    {   

        //cái này là máy cái biến dùng để dùng giá trị của web page nhu giá trị có khi khởi tạo
        private readonly Final.Models.MyDataContext _context;
        private readonly IWebHostEnvironment environment;


        // CategoryString dùng để lấy categories trong checkbox ,  Product dùng lấy giá trị mà không có biến đặt biệt , linkImgFileName lấy  file ành upload 
        [BindProperty] 
        public List<string> CategoryString { get; set; } 
        [BindProperty]
        public Product Product { get; set; } = default!;

        [BindProperty]
        public IFormFile? linkImgFileName { get; set; }
        private readonly ImageUtil _imageUtil;


        public CreateModel(Final.Models.MyDataContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _imageUtil = new ImageUtil();
            this.environment = environment;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

       

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            try


            {

                if (linkImgFileName == null)
                {
                    ModelState.AddModelError("product.ImageFileName", " the image file name is required");

                }
                if ( !ModelState.IsValid ||  _context.Products == null || Product == null)
                {
                    return Page();
                }
                var selectedCategories = Request.Form["Category"].ToArray();
              
                Product.Category ??= Array.Empty<string>();
                var existingProduct = await _context.Products
                 .FirstOrDefaultAsync(p => p.BarCode == Product.BarCode);

                if (existingProduct != null)
                {
                    ModelState.AddModelError("product.Barcode", "A product with the same barcode already exists.");
                    return Page();
                }



                Product.OriginPrice = Product.DisplayPrice; 
                Product.linkImg = _imageUtil.SaveImage(linkImgFileName,this.environment.WebRootPath);
                Product.CreateAt = DateTime.Now; 
                Product.Category = CategoryString.ToArray();



                _context.Products.Add(Product); 
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
                // Handle the error as needed, you might want to log it or display a user-friendly message
                return Page();
            }
        }

    }
}
