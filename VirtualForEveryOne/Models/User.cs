using System.ComponentModel.DataAnnotations;

namespace VirtualForEveryOne.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]

        [StringLength(20, MinimumLength = 3)]

        public string username { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string fullname { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string email { get; set; }
        [Required]
        [StringLength(12, MinimumLength = 6)]
        public string password { get; set; }
        public string dob { get; set; }
        [Required]
        public string skills { get; set; }
        public string gender { get; set; }
        public string bio { get; set; }
        public string profilepic { get; set; }
        public string coverpic { get; set; }
        public bool status { get; set; }

    }
}
