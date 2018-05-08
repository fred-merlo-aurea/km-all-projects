using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class Reports
    {
        public Reports()
        {
            ReportID = -1;
            ReportName = string.Empty;
            ControlName = string.Empty;
            ShowInSetup = false;
            IsExport = false;
        }

        [DataMember]
        public int ReportID { get; set; }
        [DataMember]
        public string ReportName { get; set; }
        [DataMember]
        public string ControlName { get; set; }
        [DataMember]
        public bool ShowInSetup { get; set; }

        [DataMember]
        public bool IsExport { get; set; }
    }
}
