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
    public class FilterSegmentationGroup
    {
        public FilterSegmentationGroup()
        {
            FilterSegmentationGroupID = 0;
            FilterSegmentationID = 0;
            FilterGroupID_Selected = null;
            FilterGroupID_Suppressed = null;
            SelectedOperation = string.Empty;
            SuppressedOperation = string.Empty;
        }

        #region Properties
        [DataMember]
        public int FilterSegmentationGroupID { get; set; }
        [DataMember]
        public int FilterSegmentationID { get; set; }
        [DataMember]
        public List<int> FilterGroupID_Selected { get; set; }
        [DataMember]
        public List<int> FilterGroupID_Suppressed { get; set; }
        [DataMember]
        public string SelectedOperation { get; set; }
        [DataMember]
        public string SuppressedOperation { get; set; }
        #endregion
    }
}
