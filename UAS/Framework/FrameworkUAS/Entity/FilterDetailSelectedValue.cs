using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class FilterDetailSelectedValue : FilterBase
    {
        public FilterDetailSelectedValue()
        {
            FilterDetailSelectedValueId = 0;
            FilterDetailId = 0;
            SelectedValue = string.Empty;
        }

        [DataMember]
        public int FilterDetailSelectedValueId { get; set; }
        [DataMember]
        public int FilterDetailId { get; set; }
        [DataMember]
        public string SelectedValue { get; set; }
    }
}
