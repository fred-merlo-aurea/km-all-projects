using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class Filter
    {
        public Filter()
        {
            FilterID = 0;
            ProductID = 0;
            FilterName = "";
            FilterDetails = "";
        }
        #region Properties
        [DataMember]
        public int FilterID { get; set; }
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public string FilterName { get; set; }
        [DataMember]
        public string FilterDetails { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        #endregion
    }
}
