using Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Final.Pages.Transaction_Page
{   


    public class TransactionModel : PageModel
    {
        public static List<OrderDetail> StaticOrderDetailList { get; set; }
        private readonly Final.Models.MyDataContext _context;
        public static string CustomerPhoneNumber { get; set; }
        public static string CustomerName { get; set; }
        public static string CustomerAddress { get; set; }
        public static int CustomerInputPrice { get; set; }
        public static int ExchangePrice { get; set; }
        public int Number { get; set; }


        public IndexModel(Final.Models.MyDataContext context)
        {
            _context = context;
        }
        public void OnGet(List<string> list)
        {
            foreach (var orderDetailString in list)
            {
                string[] components = orderDetailString.Split(',');

                string barCodeID = components[0];
                int quantity = int.Parse(components[1]);
                int totalPrice = int.Parse(components[2]);

                // Create an OrderDetail object and add it to the list
                var orderDetail = new OrderDetail
                {
                    BarCodeID = barCodeID,
                    Quantity = quantity,
                    TotalPrice = totalPrice
                };

                // Initialize the list if it's null
                StaticOrderDetailList ??= new List<OrderDetail>();

                // Add the orderDetail to the StaticOrderDetailList
                StaticOrderDetailList.Add(orderDetail);
            }
        }
        public void OnPost()
        {



          
        }

        // util 
        public decimal SumTotalPrices()
        {
            return StaticOrderDetailList?.Sum(orderDetail => orderDetail.TotalPrice) ?? 0;
        }

    }
}
