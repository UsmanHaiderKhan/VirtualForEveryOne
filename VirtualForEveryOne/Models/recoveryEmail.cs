using System.ComponentModel.DataAnnotations;

namespace VirtualForEveryOne.Models
{
    public class recoveryEmail
    {
        [Required, Display(Name = "Email"), EmailAddress]
        public string Email { get; set; }
    }
}
