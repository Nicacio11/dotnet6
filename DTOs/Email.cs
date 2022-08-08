using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.DTOs
{
    public class Email
    {
        public string ToName { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string FromName { get; set; } = "Blog";
        public string FromEmail { get; set; } = "vitornicacio10@hotmail.com";
    }
}