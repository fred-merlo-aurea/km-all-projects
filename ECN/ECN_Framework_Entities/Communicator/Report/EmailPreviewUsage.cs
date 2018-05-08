using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Communicator.Report
{
    [Serializable]
    [DataContract]
    public class EmailPreviewUsage
    {
        public EmailPreviewUsage() 
        { 
        }
        [ECN_Framework_Entities.Activity.Report.ExportAttribute(FieldOrder = 1, Format = 0)]
        public int CustomerID { get; set; }
        [ECN_Framework_Entities.Activity.Report.ExportAttribute(FieldOrder = 2, Format = 0)]
        public string CustomerName { get; set; }
        [ECN_Framework_Entities.Activity.Report.ExportAttribute(FieldOrder = 3, Format = 0)]
        public int Counts { get; set; }
    }
}
