using System;

namespace VirtualForEveryOne.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string postid { get; set; }
        public string comments { get; set; }
        public string status { get; set; }
        public DateTime time { get; set; }

    }
}
