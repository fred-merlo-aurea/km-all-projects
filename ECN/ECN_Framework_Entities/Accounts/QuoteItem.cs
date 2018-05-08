using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class QuoteItem
    {
        public QuoteItem()
        {
            QuoteItemID = -1;
            QuoteID = -1;
            Code = null;
            Name = string.Empty;
            Description = string.Empty;
            Quantity = -1;
            Rate = -1;
            DiscountRate = null;
            LicenseType = ECN_Framework_Common.Objects.Accounts.Enums.LicenseTypeEnum.AnnualTechAccess;
            PriceType = ECN_Framework_Common.Objects.Accounts.Enums.PriceTypeEnum.OneTime; 
            FrequencyType = ECN_Framework_Common.Objects.Accounts.Enums.FrequencyTypeEnum.Annual;
            IsCustomerCredit = false;
            IsActive = true;
            ProductIDs = string.Empty;
            ProductFeatureIDs = string.Empty;
            Services = string.Empty; 
            RecurringProfileID = string.Empty;
            CustomerID = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        public QuoteItem(ECN_Framework_Common.Objects.Accounts.Enums.FrequencyTypeEnum frequency, string code, string name, string description, int quantity, double rate, ECN_Framework_Common.Objects.Accounts.Enums.LicenseTypeEnum licenseType, ECN_Framework_Common.Objects.Accounts.Enums.PriceTypeEnum priceType)
        {
            FrequencyType = frequency; 
            Code = code;
            Name = name;
            Description = description;
            Quantity = quantity;
            Rate = rate;
            LicenseType = licenseType;
            PriceType = priceType;
        }

        //public QuoteItem(ECN_Framework_Common.Objects.Accounts.Enums.FrequencyTypeEnum frequency, string code, string name, string description, long quantity, double rate, ECN_Framework_Common.Objects.Accounts.Enums.LicenseTypeEnum licenseType, ECN_Framework_Common.Objects.Accounts.Enums.PriceTypeEnum priceType, bool isCustomerCredit) :
        //    base(code, name, description, quantity, rate, licenseType, priceType)
        //{
        //    Frequency = frequency;
        //    IsCustomerCredit = isCustomerCredit;
        //}

        [DataMember]
        public int QuoteItemID { get; set; }
        [DataMember]
        public int QuoteID { get; set; }
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
        public ECN_Framework_Common.Objects.Accounts.Enums.FrequencyTypeEnum FrequencyType { get; set; }
        [DataMember]
        public bool IsCustomerCredit { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string ProductIDs { get; set; }
        [DataMember]
        public string ProductFeatureIDs { get; set; }
        [DataMember]
        public string Services { get; set; }
        [DataMember]
        public string RecurringProfileID { get; set; }
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
