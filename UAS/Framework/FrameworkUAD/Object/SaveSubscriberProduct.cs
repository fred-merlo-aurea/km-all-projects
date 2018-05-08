using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FrameworkUAD.Entity;

namespace FrameworkUAD.Object
{
    [DataContract(Name = "SubscriberProduct")]
    public class SaveSubscriberProduct : CoreUserProperty
    {
        public SaveSubscriberProduct() : base(false)
        {
        }

        [DataMember]
        public int Copies { get; set; }
        [DataMember]
        public int GraceIssues { get; set; }
        [DataMember]
        public string OnBehalfOf { get; set; }
        [DataMember]
        public string SubscriberSourceCode { get; set; }
        [DataMember]
        public string OrigsSrc { get; set; }
        [DataMember]
        public int SequenceID { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public string Verify { get; set; }
        [DataMember(Name = "State")]
        public string RegionCode { get; set; }
        [DataMember]
        public Guid? IGrp_No { get; set; }
        [DataMember]
        public int ReqFlag { get; set; }
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
        [DataMember]
        public int EmailID { get; set; }
        [DataMember]
        public bool? MailPermission { get; set; } //Demo31
        [DataMember]
        public bool? FaxPermission { get; set; } //Demo32
        [DataMember]
        public bool? PhonePermission { get; set; } //Demo33
        [DataMember]
        public bool? OtherProductsPermission { get; set; } //Demo34
        [DataMember]
        public bool? ThirdPartyPermission { get; set; } //Demo35
        [DataMember]
        public bool? EmailRenewPermission { get; set; } //Demo36
        [DataMember]
        public bool? TextPermission { get; set; }

        [DataMember(Name = "SubscriberProductDemographics")]
        public List<Object.SubscriberProductDemographic> SubscriberProductDemographics { get; set; }
    }
}
