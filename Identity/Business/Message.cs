using System;

namespace Identity.Business
{
    public class Message
    {
        public DateTime Now { get; set; }
        public string WhoAmI { get; set; }
        public string Content { get; set; }
    }
}