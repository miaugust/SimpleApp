using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SimpleApp.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the SimpleAppUser class
    public class SimpleAppUser : IdentityUser
    {   
        [Required]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Division { get; set; }
    }
}
