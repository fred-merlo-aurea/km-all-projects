using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_Entities.Activity.Report.Undeliverable
{
    public class Undeliverable : IUndeliverable
    {
        public DateTime Sendtime { get; set; }
        public int EmailID { get; set; }
        public string EmailAddress { get; set; }
        public int BlastID { get; set; }
        public string EmailSubject { get; set; }
        public string LayoutName { get; set; }
        public string Message { get; set; }
    }
}
