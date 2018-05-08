using System;
using System.Runtime.Serialization;

namespace FrameworkUAD_Lookup.Entity
{
    [Serializable]
    [DataContract]
    public class Country
    {
        public Country() { }
        #region Properties
        [DataMember]
        public int CountryID { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public int PhonePrefix { get; set; }
        [DataMember]
        public string Continent { get; set; }
        [DataMember]
        public string ContinentCode { get; set; }
        [DataMember]
        public string Area { get; set; }
        [DataMember]
        public string Alpha2 { get; set; }
        [DataMember]
        public string Alpha3 { get; set; }
        [DataMember]
        public string ISOCountryCode { get; set; }
        [DataMember]
        public int SortOrder { get; set; }
        [DataMember]
        public string BpaArea { get; set; }
        [DataMember]
        public string BpaName { get; set; }
        [DataMember]
        public int BpaAreaOrder { get; set; }
        #endregion
    }
}
