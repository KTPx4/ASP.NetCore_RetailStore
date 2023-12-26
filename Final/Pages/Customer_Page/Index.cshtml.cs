using Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using static NuGet.Packaging.PackagingConstants;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Final.Pages.Customer_Page
{
    public class IndexModel : PageModel
    {
        private readonly Final.Models.MyDataContext _context;

        public IList<Customer> Customers { get; set; } = new List<Customer>();


        public string search = "";
        public IndexModel(Final.Models.MyDataContext context)
        {
        
            _context = context;
        }

        public async Task OnGetAsync(string search)
        {
            IQueryable<Customer> query = _context.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                this.search = search;
                query = query.Where(c => c.fullName.Contains(search) || c.Phone.Contains(search));
            }

            // Retrieve customers from the database
            Customers = await query.OrderByDescending(c => c.fullName).ToListAsync();
        }


        public List<Order> GetOrdersByCustomerPhoneAsync(string customerPhone)
        {
            return _context.Orders.Where(x => x.CustomerPhone == customerPhone).ToList();
        }
    }
}