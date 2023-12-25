using Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Final.Pages.Product_Page
{
    public class IndexModel : PageModel
    {
        private readonly Final.Models.MyDataContext _context;

        public bool IsAdmin { get; private set; }

        public string search = "";

        public IndexModel(Final.Models.MyDataContext context)
        {
            
            _context = context;
        }

        public IList<Product> Product { get; set; } = default!;

        public async Task OnGetAsync(string? search)
        {


            IQueryable<Product> query = _context.Products;
            string role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (search != null)
            {
                this.search = search;
                query = query.Where(p => p.ProductName.Contains(search) || p.BarCode.Contains(search));
            }
            IsAdmin = role?.ToLower() == "admin";
            if (_context.Products != null)
            {
                Product = query.OrderByDescending(p => p.ProductName).ToList();
            }
        }
     
    }
}
