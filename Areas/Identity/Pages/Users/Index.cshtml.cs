using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SimpleApp.Areas.Identity.Data;
using SimpleApp.Models;

namespace SimpleApp.Areas.Identity.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<SimpleAppUser> SimpleAppUser { get;set; }

        public async Task OnGetAsync()
        {
            SimpleAppUser = await _context.Users.ToListAsync();
        }
    }
}
