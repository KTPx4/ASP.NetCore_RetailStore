using Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Final.Pages.Transaction_Page
{
    public class IndexModel : PageModel
    {

        private readonly Final.Models.MyDataContext _context;
      

        public IList<Product> Product { get; set; } = default!;

        public string search = "";


        public IndexModel(Final.Models.MyDataContext context)
        {


            _context = context;

        }

        public async Task OnGetAsync(string? search)
        {

            //OrderDetailList = new List<OrderDetail>(StaticOrderDetailList);
            IQueryable<Product> query = _context.Products;
          
            if (search != null)
            {
                this.search = search;
                query = query.Where(p => p.ProductName.Contains(search) || p.BarCode.Contains(search));
            }
       
            if (_context.Products != null)
            {
                Product = query.OrderByDescending(p => p.ProductName).ToList();
            }
        }


        // utility function 
        public int TotalProductQuantity()
        {

            // Sum up the quantity property of all OrderDetail objects in StaticOrderDetailList
            return StaticOrderDetailList.Sum(orderDetail => orderDetail.Quantity);

        }
        public int SumProductPrice()
        {
            // Sum up the TotalPrice property of all OrderDetail objects in StaticOrderDetailList
            return StaticOrderDetailList.Sum(orderDetail => orderDetail.TotalPrice);
        }





        // button action method for when add delete button is click 
        public static List<OrderDetail> StaticOrderDetailList = new List<OrderDetail>();
        public  List<OrderDetail> OrderDetailList { get; set; }  = new List<OrderDetail>(StaticOrderDetailList);
 
        public Product GetProductByBarcodeAsync(string barcode)
        {
            return _context.Products.FirstOrDefault(x => x.BarCode == barcode);
        }
        public IActionResult OnPostDeleteorder(string barcode)
        {
            var existingOrderDetail = StaticOrderDetailList.FirstOrDefault(o => o.BarCodeID == barcode);
            var product = _context.Products.FirstOrDefault(p => p.BarCode == barcode);
            if (existingOrderDetail != null)
            {
                // If the quantity is greater than 1, decrement by 1 and update the total price
                if (existingOrderDetail.Quantity > 1)
                {
                    existingOrderDetail.Quantity -= 1;
                    existingOrderDetail.TotalPrice = existingOrderDetail.Quantity * product.DisplayPrice;
                }
                else
                {
                    // If the quantity is 1, remove the order detail
                    StaticOrderDetailList.Remove(existingOrderDetail);
                }
            }

            return RedirectToPage();
        }


        public IActionResult OnPostAddorderAsync(string barcode)
        {
            // Assuming 'barcode' is the barcode
            var product = _context.Products.FirstOrDefault(p => p.BarCode == barcode);

            if (product != null)
            {
                var existingOrderDetail = StaticOrderDetailList.FirstOrDefault(o => o.BarCodeID == barcode);

                if (existingOrderDetail != null)
                {
                    // Update existing order detail
                    existingOrderDetail.Quantity += 1;
                    existingOrderDetail.TotalPrice = existingOrderDetail.Quantity * product.DisplayPrice;
                }
                else
                {
                    // Add a new order with quantity 1 and total price as product price
                    var newOrderDetail = new OrderDetail
                    {
                        BarCodeID = barcode,
                        Quantity = 1,
                        TotalPrice = product.DisplayPrice
                    };

                    StaticOrderDetailList.Add(newOrderDetail);
                }
            }

            // Redirect back to the page
            return RedirectToPage();
        }

        public IActionResult OnPostCheckOut()
        {
            // Perform actions before checking out (e.g., saving orders to the database)
            List<string> OrderDetailString = new List<string>();
            // Redirect to the Transaction page
            foreach (var orderDetail in StaticOrderDetailList)
            {
                string orderDetailString = $"{orderDetail.BarCodeID},{orderDetail.Quantity},{orderDetail.TotalPrice}";
                OrderDetailString.Add(orderDetailString);
            }


            return RedirectToPage("Transaction",new { list = OrderDetailString });
        }

        public IActionResult OnPostClearOrderList()
        {
            // Clear the StaticOrderDetailList
            StaticOrderDetailList.Clear();

            // Redirect back to the page
            return RedirectToPage();
        }
    }
   

}
