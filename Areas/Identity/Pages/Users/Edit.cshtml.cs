using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimpleApp.Areas.Identity.Data;
using SimpleApp.Data;
using SimpleApp.Models;

namespace SimpleApp.Areas.Identity.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly UserManager<SimpleAppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context, UserManager<SimpleAppUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [BindProperty]
        public SimpleAppUser SimpleAppUser { get; set; }
        [BindProperty]
        public bool EmailConfirmed { get; set; }

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
            EmailConfirmed = SimpleAppUser.EmailConfirmed;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            string id = SimpleAppUser.Id;
            var user = _context.Users.Find(id);

            if(EmailConfirmed)
            {
                await _userManager.AddToRoleAsync(user, SD.Admin);
                user.EmailConfirmed = true;
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(user, SD.Admin);
                user.EmailConfirmed = false;
            }
            
           // _context.Attach(SimpleAppUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SimpleAppUserExists(SimpleAppUser.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SimpleAppUserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
