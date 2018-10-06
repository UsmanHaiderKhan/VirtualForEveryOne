namespace VirtualForEveryOne.Models
{
    public class Post
    {
        public int Id { set; get; }
        public string username { set; get; }
        public string question { set; get; }
        public string image { set; get; }
        public string tag { set; get; }
        public bool anonymous { set; get; }

    }
}
