using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aspNetMime;

namespace BounceEngine
{
    public class MessageProperties
    {
        public MessageProperties()
        {
            EmailAddress = string.Empty;
            FromAddress = string.Empty;
            MessageDate = string.Empty;
            Text = string.Empty;
            EmailId = 0;
            BlastId = 0;
        }

        public MimeMessage MimeMessage { get; set; }
        public string FromAddress { get; set; }
        public string MessageDate { get; set; }
        public string EmailAddress { get; set; }
        public string Text { get; set; }
        public int EmailId { get; set; }
        public int BlastId { get; set; }
    }
}
