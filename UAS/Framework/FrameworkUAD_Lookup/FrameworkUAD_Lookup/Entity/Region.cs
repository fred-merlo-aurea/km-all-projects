using System;
using System.Runtime.Serialization;

namespace FrameworkUAD_Lookup.Entity
{
    [Serializable]
    [DataContract]
    public class Region
    {
        public Region() { }
        #region Properties
        [DataMember]
        public int RegionID { get; set; }
        [DataMember]
        public int CountryID { get; set; }
        [DataMember]
        public string RegionName { get; set; }
        [DataMember]
        public string RegionCode { get; set; }
        [DataMember]
        public string ZipCodeRange { get; set; }
        [DataMember]
        public int ZipCodeRangeSortOrder { get; set; }
        [DataMember]
        public int RegionGroupID { get; set; }
        #endregion
    }
}
