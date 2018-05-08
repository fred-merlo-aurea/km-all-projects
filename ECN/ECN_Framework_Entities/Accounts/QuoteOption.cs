using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class QuoteOption
    {
        public QuoteOption()
        {
            QuoteOptionID = -1;
            BaseChannelID = -1;
            Code = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            Quantity = -1;
            Rate = -1;
            DiscountRate = null;
            LicenseType = ECN_Framework_Common.Objects.Accounts.Enums.LicenseTypeEnum.AnnualTechAccess;
            PriceType = ECN_Framework_Common.Objects.Accounts.Enums.PriceTypeEnum.OneTime; 
            IsCustomerCredit = false;
            ProductIDs = string.Empty;
            ProductFeatureIDs = string.Empty;
            Services = string.Empty; 
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int QuoteOptionID { get; set; }
        [DataMember]
        public int BaseChannelID { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public double Rate { get; set; }
        [DataMember]
        public double? DiscountRate { get; set; }
        [DataMember]
        public ECN_Framework_Common.Objects.Accounts.Enums.LicenseTypeEnum LicenseType { get; set; }
        [DataMember]
        public ECN_Framework_Common.Objects.Accounts.Enums.PriceTypeEnum PriceType { get; set; }
        [DataMember]
        public bool IsCustomerCredit { get; set; }
        [DataMember]
        public string ProductIDs { get; set; }
        [DataMember]
        public string ProductFeatureIDs { get; set; }
        [DataMember]
        public string Services { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
}
}
