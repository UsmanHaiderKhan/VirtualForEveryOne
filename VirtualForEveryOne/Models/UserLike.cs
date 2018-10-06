namespace VirtualForEveryOne.Models
{
    public class UserLike
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string question { get; set; }
        public string image { get; set; }

        public string answer { get; set; }
        public int like { get; set; }
        public int dislike { get; set; }

    }
}
