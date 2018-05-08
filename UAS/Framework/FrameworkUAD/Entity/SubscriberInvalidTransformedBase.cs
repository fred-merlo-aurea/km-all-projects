using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    public class SubscriberInvalidTransformedBase : SubscriberBase
    {
        public SubscriberInvalidTransformedBase()
        {
            SORecordIdentifier = Guid.Empty;
            ImportRowNumber = NoInt;
            ExternalKeyId = ZeroId;
        }

        public SubscriberInvalidTransformedBase(int sourceFileID, Guid soRecordIdentifier, string processCode)
            : base(processCode)
        {
            SORecordIdentifier = soRecordIdentifier;
            SourceFileID = sourceFileID;
        }

        [DataMember]
        public Guid SORecordIdentifier { get; set; }

        [DataMember]
        [StringLength(100)]
        [DisplayName("FirstName")]
        public string FName { get; set; } = string.Empty;

        [DataMember]
        [StringLength(100)]
        [DisplayName("LastName")]
        public string LName { get; set; } = string.Empty;

        [DataMember]
        [StringLength(255)]
        [DisplayName("Address1")]
        public string Address { get; set; } = string.Empty;

        [DataMember]
        [StringLength(255)]
        [DisplayName("Address2")]
        public string MailStop { get; set; } = string.Empty;

        [DataMember]
        [StringLength(50)]
        [DisplayName("RegionCode")]
        public string State { get; set; } = string.Empty;

        [DataMember]
        [StringLength(50)]
        [DisplayName("ZipCode")]
        public string Zip { get; set; } = string.Empty;

        [DataMember]
        [DisplayName("QualificationDate")]
        public DateTime? QDate { get; set; } = DateTime.Now;

        [DataMember]
        [StringLength(100)]
        [DisplayName("Verify")]
        public string Verified { get; set; } = string.Empty;

        [DataMember]
        [StringLength(50)]
        [DisplayName("SubscriberSourcecode")]
        public string SubSrc { get; set; } = string.Empty;

        [DataMember]
        public int? EmailStatusID { get; set; } = StatusOne;

        [DataMember]
        [DisplayName("Demo31")]
        public bool? MailPermission { get; set; }

        [DataMember]
        [DisplayName("Demo32")]
        public bool? FaxPermission { get; set; }

        [DataMember]
        [DisplayName("Demo33")]
        public bool? PhonePermission { get; set; }

        [DataMember]
        [DisplayName("Demo34")]
        public bool? OtherProductsPermission { get; set; }

        [DataMember]
        [DisplayName("Demo35")]
        public bool? ThirdPartyPermission { get; set; }

        [DataMember]
        [DisplayName("Demo36")]
        public bool? EmailRenewPermission { get; set; }

        public string FullName => string.Format("{0} {1}", FName, LName);
    }
}
