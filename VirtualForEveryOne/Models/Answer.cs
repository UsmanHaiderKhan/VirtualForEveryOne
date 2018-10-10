using System;

namespace VirtualForEveryOne.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string a_username { get; set; }
        public string question { get; set; }
        public string image { get; set; }
        public string ans { get; set; }
        public string s_profilepic { get; set; }
        public string a_profilepic { get; set; }
        public int? like { get; set; }
        public int? dislike { get; set; }
        public DateTime time { get; set; }
        public string status { get; set; }

    }
}
