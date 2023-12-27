using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Final.Models;
using MongoDB.Bson;

namespace Final.Pages.Product_Page
{
    public class DeleteModel : PageModel
    {
        private readonly Final.Models.MyDataContext _context;

        public DeleteModel(Final.Models.MyDataContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

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
            else
            {
                Product = product;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(ObjectId id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            // Check if the product barcode is in any order details
            bool isProductInOrder = await _context.OrderDetails.AnyAsync(od => od.BarCodeID == product.BarCode);

            if (isProductInOrder)
            {
                TempData["error"] = "Product  is in a order ";
                return RedirectToPage("Index");

            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            TempData["success"] = "Product deleted successfully";
            return RedirectToPage("Index");
        }
    }
}
