using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class FilterGroup : FilterBase
    {
        public FilterGroup()
        {
            FilterGroupId = 0;
            FilterId = 0;
            SortOrder = 0;
        }

        [DataMember]
        public int FilterGroupId { get; set; }
        [DataMember]
        public int FilterId { get; set; }
        [DataMember]
        public int SortOrder { get; set; }
    }
}
