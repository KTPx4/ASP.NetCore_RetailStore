using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Final.Pages.Transaction_Page
{
    public class Index1Model : PageModel
    {
        public static List<int> numberList = new List<int>();

        public string Message { get; set; } = "Default GET method";


        public List<int> nonstaticlist = numberList; 
        public void OnPostAddNumber(string number)
        {
            
            Message = $"Added {number} to the list.";
        }

        public void OnGet()
        {
            Message = $"Numbers in the list: {string.Join(", ", numberList)}";
        }

        public void OnDeleteDeleteNumber(int number)
        {
            if (numberList.Contains(number))
            {
                numberList.Remove(number);
                Message = $"Deleted {number} from the list.";
            }
            else
            {
                Message = $"{number} not found in the list.";
            }
        }
    }
}
