using System.ComponentModel.DataAnnotations;

namespace SimpleApp.Models
{
    public class Employee
    {
        [Key]
        [Display(Name="Nr kontrolny")]
        public string ControllNumber { get; set; }
        public string FirstName { get; set; }
    }
    
    
}