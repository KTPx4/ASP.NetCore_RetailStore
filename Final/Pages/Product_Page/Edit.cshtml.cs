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
using Final.util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Final.Pages.Product_Page
{
    public class EditModel : PageModel
    {   




        //cái này là máy cái biến dùng để dùng giá trị của web page nhu giá trị có khi khởi tạo
        private readonly Final.Models.MyDataContext _context;
        private readonly IWebHostEnvironment environment;
        private readonly ImageUtil _imageUtil;

        public  List<string> CheckBoxList;
        public string ImageUrl { get; set; } 
        // CategoryString dùng để lấy categories trong checkbox ,  Product dùng lấy giá trị mà không có biến đặt biệt , linkImgFileName lấy  file ành upload 
        [BindProperty]
        public List<string> CategoryString { get; set; }

        [BindProperty]
        public Product Product { get; set; } = default!;

        [BindProperty]
        public IFormFile? linkImgFileName { get; set; }
       

        public EditModel(Final.Models.MyDataContext context, IWebHostEnvironment environment)
        {
            _context = context;
            this.environment = environment;
            _imageUtil = new ImageUtil();
        }


        public async Task<IActionResult> OnGetAsync(ObjectId id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            Product = product;

            //Product = product;
            CategoryString = Product.Category.ToList() ;

            //CheckBoxList = CategoryString;
            //ImageUrl = Product.linkImg; 
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (linkImgFileName == null)
            {
                ModelState.AddModelError("product.ImageFileName", "The image file name is required");
            }


           
            Product.linkImg = _imageUtil.SaveImage(linkImgFileName, environment.WebRootPath);
            Product.CreateAt = DateTime.Now;
            Product.Category = CategoryString.ToArray();


            _context.SaveChanges();

            _context.Attach(Product).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ProductExists(ObjectId id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
