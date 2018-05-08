using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class DataCompareProfile
    {
        public DataCompareProfile() { }
        #region Properties
        [DataMember]
        public int SubscriberFinalId { get; set; }
        [DataMember]
        public Guid SFRecordIdentifier { get; set; }
        [DataMember]
        public int SourceFileId { get; set; }
        [DataMember]
        public string ProcessCode { get; set; }
        [DataMember]
        public int ExternalKeyId { get; set; }
        [DataMember]
        public string PubCode { get; set; }
        [DataMember]
        public string FName { get; set; }
        [DataMember]
        public string LName { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Occupation { get; set; }
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string MailStop { get; set; }
        [DataMember]
        public string Address3 { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Zip { get; set; }
        [DataMember]
        public string Plus4 { get; set; }
        [DataMember]
        public string ForZip { get; set; }
        [DataMember]
        public string County { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public int CountryID { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public int EmailStatusID { get; set; }
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public string Website { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public int ImportRowNumber { get; set; }
        [DataMember]
        public Guid IGrp_No { get; set; }
        [DataMember]
        public bool IsNew { get; set; }
        #endregion
    }
}
