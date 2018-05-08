using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECN_Framework_Entities.Activity.Report;

namespace ECN_Framework_Entities.Activity.Report.Undeliverable
{
    public interface IUndeliverable
    {
        [ExportAttribute(Header = "Sendtime")]
        DateTime Sendtime { get; set; }
        [ExportAttribute(Header = "EmailID")]
        int EmailID { get; set; }
        [ExportAttribute(Header = "EmailAddress")]
        string EmailAddress { get; set; }
        [ExportAttribute(Header = "BlastID")]
        int BlastID { get; set; }
        [ExportAttribute(Header = "EmailSubject")]
        string EmailSubject { get; set; }
        [ExportAttribute(Header = "LayoutName")]
        string LayoutName { get; set; }
        [ExportAttribute(Header = "Message")]
        string Message { get; set; }
    }
}
