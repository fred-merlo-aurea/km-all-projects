using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class FilterSegmentation
    {
        public FilterSegmentation()
        {
            FilterSegmentationID = 0;
            FilterSegmentationName = string.Empty;
            Notes = string.Empty;
            FilterID = 0;
            IsDeleted = false;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            FilterSegmentationGroupList = new List<Entity.FilterSegmentationGroup>();
        }

        #region Properties
        [DataMember]
        public int FilterSegmentationID { get; set; }
        [DataMember]
        public string FilterSegmentationName { get; set; }
        [DataMember]
        public string Notes { get; set; }
        [DataMember]
        public int FilterID { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public List<Entity.FilterSegmentationGroup> FilterSegmentationGroupList { get; set; }
        #endregion
    }
}
