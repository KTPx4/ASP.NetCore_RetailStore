using Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Final.Pages.Product_Page
{
    public class IndexModel : PageModel
    {
        private readonly Final.Models.MyDataContext _context;

        public IndexModel(Final.Models.MyDataContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Products != null)
            {
                Product = await _context.Products.ToListAsync();
            }
        }
     
    }
}
