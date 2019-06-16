using System.ComponentModel.DataAnnotations;

namespace VirtualForEveryOne.Models
{
    public class Group
    {
        public int Id { get; set; }
        [Required]

        public int groupid { get; set; }
        [Required]
        [MaxLength(20), MinLength(3)]
        public string groupname { get; set; }
        [Required]
        [MaxLength(20), MinLength(3)]

        public string groupadmin { get; set; }

        public string image { get; set; }
        [Required]
        public string info { get; set; }

    }
}
