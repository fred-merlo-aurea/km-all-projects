using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class ReportingXML
    {
        public ReportingXML()
        {
            this.Filters = "<XML></XML>";
            this.AdHocFilters = "<XML></XML>";
        }

        [DataMember]
        public string Filters { get; set; }
        [DataMember]
        public string AdHocFilters { get; set; }
    }
}
