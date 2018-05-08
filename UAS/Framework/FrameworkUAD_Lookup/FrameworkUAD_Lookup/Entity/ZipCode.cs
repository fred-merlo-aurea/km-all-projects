using System;
using System.Runtime.Serialization;

namespace FrameworkUAD_Lookup.Entity
{
    [Serializable]
    [DataContract]
    public class ZipCode
    {
        public ZipCode() { }
        #region Properties
        [DataMember]
        public int ZipCodeId { get; set; }
        [DataMember]
	    public int? ZipCodeTypeId { get; set; }
        [DataMember]
	    public string Zip { get; set; }
        [DataMember]
	    public string PrimaryCity { get; set; }
        [DataMember]
	    public int? RegionId { get; set; }
        [DataMember]
	    public int? CountryId { get; set; }
        [DataMember]
	    public string County { get; set; }
        [DataMember]
	    public string AreaCodes { get; set; }
        #endregion
    }
}
