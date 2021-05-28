using System;

namespace HackerNewsPortal.Models
{
    public class Story
    {
        public int id { get; set; }

        public string by { get; set; }

        public string title { get; set; }

        public Uri url { get; set; }
    }
}
