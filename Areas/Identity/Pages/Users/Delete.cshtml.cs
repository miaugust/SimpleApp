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
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SimpleAppUser SimpleAppUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SimpleAppUser = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (SimpleAppUser == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SimpleAppUser = await _context.Users.FindAsync(id);

            if (SimpleAppUser != null)
            {
                _context.Users.Remove(SimpleAppUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
