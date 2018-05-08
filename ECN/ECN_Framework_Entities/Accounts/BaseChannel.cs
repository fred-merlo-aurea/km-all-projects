using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class BaseChannel
    {
        public BaseChannel() 
        {
            BaseChannelID = -1;
            BaseChannelName = string.Empty;
            ChannelURL = string.Empty;
            BounceDomain = string.Empty;
            //AccessCommunicator = null;
            //AccessCreator = null;
            //AccessCollector = null;
            //AccessPublisher = null;
            //AccessCharity = null;
            DisplayAddress = string.Empty;
            Address = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Country = string.Empty;
            Zip = string.Empty;
            Salutation = ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Unknown;
            ContactName = string.Empty;
            ContactTitle = string.Empty;
            Phone = string.Empty;
            Fax = string.Empty;
            Email = string.Empty;
            WebAddress = string.Empty;
            InfoContentID = null;
            MasterCustomerID = null;
            ChannelPartnerType =  ECN_Framework_Common.Objects.Accounts.Enums.ChannelPartnerType.NotInitialized;
            RatesXml = string.Empty;
            ChannelType = string.Empty;
            ChannelTypeCode = ECN_Framework_Common.Objects.Accounts.Enums.ChannelType.Unknown;
            IsBranding = null;
            EmailThreshold = null;
            BounceThreshold = null;
            SoftBounceThreshold = null;
            HeaderSource = string.Empty;
            FooterSource = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            //Contact = new Contact();
            //Channels = null;
            MSCustomerID = null;
            BrandLogo = string.Empty;
            BrandSubDomain = string.Empty;
            BaseChannelGuid = null;
            IsPublisher = null;
            TestBlastLimit = null;
            AccessKey = null;
            PlatformClientGroupID = -1;
        }

        [DataMember]
        public int BaseChannelID { get; set; }
        [DataMember]
        public string BaseChannelName { get; set; }
        [DataMember]
        public string ChannelURL { get; set; }
        [DataMember]
        public string BounceDomain { get; set; }
        //[DataMember]
        //public bool? AccessCommunicator { get; set; }
        //[DataMember]
        //public bool? AccessCreator { get; set; }
        //[DataMember]
        //public bool? AccessCollector { get; set; }
        //[DataMember]
        //public bool? AccessPublisher { get; set; }
        //[DataMember]
        //public bool? AccessCharity { get; set; }
        [DataMember]
        public string DisplayAddress { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string Zip { get; set; }
        [DataMember]
        public ECN_Framework_Common.Objects.Accounts.Enums.Salutation Salutation { get; set; }
        [DataMember]
        public string ContactName { get; set; }
        [DataMember]
        public string ContactTitle { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string WebAddress { get; set; }
        [DataMember]
        public int? InfoContentID { get; set; }
        [DataMember]
        public int? MasterCustomerID { get; set; }
        [DataMember]
        public ECN_Framework_Common.Objects.Accounts.Enums.ChannelPartnerType ChannelPartnerType { get; set; }
        [DataMember]
        public string RatesXml { get; set; }
        [DataMember]
        public string ChannelType { get; set; }
        [DataMember]
        public bool? IsBranding { get; set; }
        [DataMember]
        public int? EmailThreshold { get; set; }
        [DataMember]
        public int? BounceThreshold { get; set; }
        [DataMember]
        public int? SoftBounceThreshold { get; set; }
        [DataMember]
        public string HeaderSource { get; set; }
        [DataMember]
        public string FooterSource { get; set; }
        [DataMember]
        public string BrandLogo { get; set; }
        [DataMember]
        public string BrandSubDomain { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; private set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; private set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
        public ECN_Framework_Common.Objects.Accounts.Enums.ChannelType ChannelTypeCode { get; set; }
        //public Contact Contact { get; set; }
        //public List<Channel> Channels { get; set; }
        [DataMember]
        public int? MSCustomerID { get; set; }
        [DataMember]
        public Guid? BaseChannelGuid { get; set; }
        [DataMember]
        public bool? IsPublisher { get; set; }

        [DataMember]
        public int? TestBlastLimit { get; set; }
        [DataMember]
        public Guid? AccessKey { get; set; }
        [DataMember]
        public int PlatformClientGroupID { get; set; }
    }
}
