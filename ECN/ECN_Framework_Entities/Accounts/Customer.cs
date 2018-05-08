using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects.Accounts;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class Customer
    {
        public Customer()
        {
            CustomerID = -1;
            BaseChannelID = null;
            CommunicatorChannelID = null;
            CollectorChannelID = null;
            CreatorChannelID = null;
            PublisherChannelID = null;
            CharityChannelID = null;
            CustomerName = string.Empty;
            ActiveFlag = string.Empty;
            CommunicatorLevel = string.Empty;
            CollectorLevel = string.Empty;
            CreatorLevel = string.Empty;
            PublisherLevel = string.Empty;
            CharityLevel = string.Empty;
            AccountsLevel = string.Empty;
            Address = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Country = string.Empty;
            Zip = string.Empty;
            Salutation = ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Unknown;
            ContactName = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            ContactTitle = string.Empty;
            Phone = string.Empty;
            Fax = string.Empty;
            Email = string.Empty;
            WebAddress = string.Empty;
            TechContact = string.Empty;
            TechEmail = string.Empty;
            TechPhone = string.Empty;
            SubscriptionsEmail = string.Empty;
            CustomerType = string.Empty;
            DemoFlag = string.Empty;
            AccountExecutiveID = null;
            AccountManagerID = null;
            IsStrategic = null;
            customer_udf1 = string.Empty;
            customer_udf2 = string.Empty;
            customer_udf3 = string.Empty;
            customer_udf4 = string.Empty;
            customer_udf5 = string.Empty;
            BlastConfigID = 3;
            BounceThreshold = null;
            SoftBounceThreshold = null;
            TextPowerKWD = string.Empty;
            TextPowerWelcomeMsg = string.Empty;
            BillingContact = new Contact();
            GeneralContant = new Contact();
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            MSCustomerID = null;
            ABWinnerType = null;
            DefaultBlastAsTest = false;
            TestBlastLimit = null;
            PlatformClientID = -1;
        }

        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public int? BaseChannelID { get; set; }
        [DataMember]
        public int? CommunicatorChannelID { get; set; }
        [DataMember]
        public int? CollectorChannelID { get; set; }
        [DataMember]
        public int? CreatorChannelID { get; set; }
        [DataMember]
        public int? PublisherChannelID { get; set; }
        [DataMember]
        public int? CharityChannelID { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string ActiveFlag { get; set; } //char
        [DataMember]
        public string CommunicatorLevel { get; set; }
        [DataMember]
        public string CollectorLevel { get; set; }
        [DataMember]
        public string CreatorLevel { get; set; }
        [DataMember]
        public string PublisherLevel { get; set; }
        [DataMember]
        public string CharityLevel { get; set; }
        [DataMember]
        public string AccountsLevel { get; set; }
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
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
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
        public string TechContact { get; set; }
        [DataMember]
        public string TechEmail { get; set; }
        [DataMember]
        public string TechPhone { get; set; }
        [DataMember]
        public string SubscriptionsEmail { get; set; }
        [DataMember]
        public string CustomerType { get; set; }
        [DataMember]
        public string DemoFlag { get; set; }//char
        [DataMember]
        public int? AccountExecutiveID { get; set; }
        [DataMember]
        public int? AccountManagerID { get; set; }
        [DataMember]
        public bool? IsStrategic { get; set; }
        [DataMember]
        public string customer_udf1 { get; set; }
        [DataMember]
        public string customer_udf2 { get; set; }
        [DataMember]
        public string customer_udf3 { get; set; }
        [DataMember]
        public string customer_udf4 { get; set; }
        [DataMember]
        public string customer_udf5 { get; set; }
        [DataMember]
        public int? BlastConfigID { get; set; }
        [DataMember]
        public int? BounceThreshold { get; set; }
        [DataMember]
        public int? SoftBounceThreshold { get; set; }
        [DataMember]
        public string TextPowerKWD { get; set; }
        [DataMember]
        public string TextPowerWelcomeMsg { get; set; }
        [DataMember]
        public string ABWinnerType { get; set; }
               
        //[DataMember]
        //public Enums.CommunicatiorLevel? CommunicatorLevel { get; set; }
        //[DataMember]
        //public Enums.CollectorLevel? CollectorLevel { get; set; }
        //[DataMember]
        //public Enums.CreatorLevel? CreatorLevel { get; set; }
        //[DataMember]
        //public Enums.PublisherLevel? PublisherLevel { get; set; }
        //[DataMember]
        //public Enums.CharityLevel? CharityLevel { get; set; }
        //[DataMember]
        //public Enums.AccountsLevel? AccountsLevel { get; set; }
        //[DataMember]
        //public Enums.Salutation? Salutation { get; set; }
        //[DataMember]
        //public Enums.CustomerType? CustomerType { get; set; }

        public Contact BillingContact { get; set; }
        public Contact GeneralContant { get; set; }
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
        [DataMember]
        public int? MSCustomerID { get; set; }
        [DataMember]
        public bool? DefaultBlastAsTest { get; set; }

        [DataMember]
        public int? TestBlastLimit { get; set; }
        [DataMember]
        public int PlatformClientID { get; set; }
    }
}
