using System;

namespace VirtualForEveryOne.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string postid { get; set; }
        public string username { get; set; }
        public string notifier { get; set; }
        public string type { get; set; }
        public bool state { get; set; }
        public DateTime time { get; set; }


    }
}
