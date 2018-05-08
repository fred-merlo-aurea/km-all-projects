using System;
using System.Linq;
using System.Runtime.Serialization;

namespace KMPlatform.Object
{
    [Serializable]
    [DataContract]
    public class CategorySummaryReport
    {
        #region Properties
        [DataMember]
        public int categorygroup_ID { get; set; }
        [DataMember]
        public int category_ID { get; set; }
        [DataMember]
        public string categorygroup_name { get; set; }
        [DataMember]
        public string category_name { get; set; }
        [DataMember]
        public int total { get; set; }
        #endregion
    }
}
