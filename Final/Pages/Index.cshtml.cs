﻿using Final.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Final.Pages
{
    [Authorize]
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private  MyDataContext _dbContext;

        public IndexModel(MyDataContext myDContext)
        {
            _dbContext = myDContext;

        }

        public IActionResult OnGet()
        {
            return Page();
        }
    }
}