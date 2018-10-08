using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SimpleApp.Areas.Identity.Data;
using SimpleApp.Models;
using SimpleApp.Data;

namespace SimpleApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<SimpleAppUser> _signInManager;
        private readonly UserManager<SimpleAppUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<SimpleAppUser> userManager,
            SignInManager<SimpleAppUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            AppDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _db = db;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Required]
            [Display(Name = "Imię")]
            public string FirstName { get; set; }
            [Required]
            [Display(Name = "Nazwisko")]
            public string LastName { get; set; }
            [Required]
            [Display(Name = "Odział")]
            public string Division { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Hasło")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potwierdź Hasło")]
            [Compare("Password", ErrorMessage = "Hasła różnią się od siebie.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new SimpleAppUser 
                    { 
                        UserName = Input.Email,
                        Email = Input.Email,
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        Division = Input.Division };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {

                    if (!await _roleManager.RoleExistsAsync(SD.SuperAdmin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.SuperAdmin));
                    }
                    if (!await _roleManager.RoleExistsAsync(SD.Admin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Admin));
                    }
                    if (!await _roleManager.RoleExistsAsync(SD.User))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.User));
                    } 
                    var superAdmins = (await _userManager.GetUsersInRoleAsync(SD.SuperAdmin)); 
                    if(superAdmins.Count == 0)
                    {
                        await _userManager.AddToRoleAsync(user, SD.SuperAdmin);
                    }
                    else
                    {   
                        await _userManager.AddToRoleAsync(user, SD.User); 
                    }             
                
                    _logger.LogInformation("User created a new account with password.");

                    // var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    // var callbackUrl = Url.Page(
                    //     "/Account/ConfirmEmail",
                    //     pageHandler: null,
                    //     values: new { userId = user.Id, code = code },
                    //     protocol: Request.Scheme);

                    // await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //     $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
