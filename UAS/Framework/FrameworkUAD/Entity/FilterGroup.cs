using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    public class FilterGroup
    {
        #region Properties
        [DataMember]
        public int FilterGroupID { get; set; }
        [DataMember]
        public int FilterID { get; set; }
        [DataMember]
        public int SortOrder { get; set; }
        #endregion
    }
}
