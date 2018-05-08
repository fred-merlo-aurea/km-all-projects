using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class DataImportExport
    {
        #region Properties
        [DataMember]
        public int SubscriberID { get; set; }
        [DataMember]
        public string Hisbatch1 { get; set; }
        [DataMember]
        public string Hisbatch2 { get; set; }
        [DataMember]
        public string Hisbatch3 { get; set; }
        [DataMember]
        public string NANQ { get; set; }
        [DataMember]
        public int EmailID { get; set; }
        [DataMember]
        public string Verify { get; set; }
        [DataMember]
        public string Interview { get; set; }
        [DataMember]
        public string Mail { get; set; }
        [DataMember]
        public DateTime PrevQDate { get; set; }
        [DataMember]
        public string PrevQSource { get; set; }
        [DataMember]
        public int MemberID { get; set; }
        [DataMember]
        public string MemberFlag { get; set; }
        [DataMember]
        public string MemberReject { get; set; }
        [DataMember]
        public int IMBSerial1 { get; set; }
        [DataMember]
        public int IMBSerial2 { get; set; }
        [DataMember]
        public int IMBSerial3 { get; set; }
        [DataMember]
        public string HomeValue { get; set; }
        [DataMember]
        public int IssuesToGo { get; set; }
        [DataMember]
        public double AmountEarned { get; set; }
        [DataMember]
        public double AmountDeferred { get; set; }
        [DataMember]
        public DateTime NewExpire { get; set; }
        #endregion
    }
}
