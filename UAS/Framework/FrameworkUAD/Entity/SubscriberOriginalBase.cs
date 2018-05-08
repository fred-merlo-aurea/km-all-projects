using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class SubscriberOriginalBase : SubscriberBase
    {
        public SubscriberOriginalBase()
        {
            Initialize();
        }

        public SubscriberOriginalBase(string processCode) : base(processCode)
        {
            Initialize();
        }

        [DataMember]
        [StringLength(100)]
        public string FName { get; set; }
        [DataMember]
        [StringLength(100)]
        public string LName { get; set; }
        [DataMember]
        [StringLength(255)]
        public string Address { get; set; }
        [DataMember]
        [StringLength(255)]
        public string MailStop { get; set; }
        [DataMember]
        [StringLength(50)]
        public string State { get; set; }
        [DataMember]
        [StringLength(50)]
        public string Zip { get; set; }
        [DataMember]
        [StringLength(100)]
        public string Verified { get; set; }
        [DataMember]
        [StringLength(50)]
        public string SubSrc { get; set; }
        [DataMember]
        public bool? MailPermission { get; set; }
        [DataMember]
        public bool? FaxPermission { get; set; }
        [DataMember]
        public bool? PhonePermission { get; set; }
        [DataMember]
        public bool? OtherProductsPermission { get; set; }
        [DataMember]
        public bool? ThirdPartyPermission { get; set; }
        [DataMember]
        public bool? EmailRenewPermission { get; set; }

        public string FullName => $"{FName} {LName}";

        private void Initialize()
        {
            FName = string.Empty;
            LName = string.Empty;
            Address = string.Empty;
            MailStop = string.Empty;
            State = string.Empty;
            Zip = string.Empty;
            Verified = string.Empty;
            SubSrc = string.Empty;
            ImportRowNumber = NoInt;
        }
    }
}
