using Final.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Final.Pages.Customer_Page
{
    public class DetailOrderModel : PageModel
    {
        private readonly Final.Models.MyDataContext _context;

        public DetailOrderModel(Final.Models.MyDataContext context)
        {
            _context = context;
        }

        public Final.Models.Account? Account { get; set; }
        public Order? Order { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; } = new List<OrderDetail>();
        public Customer? Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(ObjectId id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            // Assuming you have a method to retrieve account details by ID
           Order = _context.Orders.Find(id);



            // Assuming you have a method to retrieve order details by account ID
            OrderDetails = _context.OrderDetails.Where(od => od.OrderID == id).ToList();



            Account = _context.Accounts.FirstOrDefault(ac => ac.Email == Order.StaffEmail);

            // Assuming you have a method to retrieve customer details by account ID
            Customer = _context.Customers.FirstOrDefault(c => c.Phone == Order.CustomerPhone);

            return Page();
        }

        public Product GetProductByBarCode(string barcode)
        {
            return _context.Products.SingleOrDefault(x => x.BarCode == barcode);
        }
    }
}
