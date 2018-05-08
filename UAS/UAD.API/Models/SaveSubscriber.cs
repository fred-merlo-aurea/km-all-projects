using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml;
using System.Runtime.Serialization;
using System.Collections;
using System.Xml.Serialization;

namespace UAD.API.Models
{
    [Serializable]
    [DataContract(Namespace = "")]
    public class SaveSubscriber
    {
        #region Exposed Properties
        /// <summary>
        /// IGrp No
        /// </summary>
        [DataMember]
		public Guid IGrp_No { get; set; }
        /// <summary>
        /// Sequence ID
        /// </summary>
        [DataMember]
        public int SequenceID { get; set; }
        /// <summary>
        /// Account Number
        /// </summary>
        [DataMember]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Pub Code
        /// </summary>
        [DataMember]
        public string PubCode { get; set; }

        /// <summary>
        /// Qualification Date
        /// </summary>
        [DataMember]
        public DateTime QualificationDate { get; set; }
        /// <summary>
        /// Category ID
        /// </summary>
        [DataMember]
        public int CategoryID { get; set; }
        /// <summary>
        /// Transaction ID
        /// </summary>
        [DataMember]
        public int TransactionID { get; set; }
        /// <summary>
        /// Q Source ID
        /// </summary>
        [DataMember]
        public int QSourceID { get; set; }
        /// <summary>
        /// Media Type (Print = A(default), Digital = B, Both = C)
        /// </summary>
        [DataMember(Name = "MediaType")]
        public string Demo7 { get; set; }        

        /// <summary>
        /// Copies
        /// </summary>
        [DataMember]
        public int Copies { get; set; }
        /// <summary>
        /// Grace Issues
        /// </summary>
        [DataMember]
        public int GraceIssues { get; set; }
        /// <summary>
        /// Subscriber Source Code
        /// </summary>
        [DataMember]
        public string SubscriberSourceCode { get; set; }
        /// <summary>
        /// Verify
        /// </summary>
        [DataMember]
        public string Verify { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [DataMember]
        public string Email { get; set; }
        /// <summary>
        /// Email Status
        /// </summary>
        [DataMember]
        public string EmailStatus { get; set; }

        /// <summary>
        /// First Name
        /// </summary>
        [DataMember]
        public string FirstName { get; set; }
        /// <summary>
        /// Last Name
        /// </summary>
        [DataMember]
        public string LastName { get; set; }
        /// <summary>
        /// Company
        /// </summary>
        [DataMember]
        public string Company { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        [DataMember]
        public string Title { get; set; }
        /// <summary>
        /// Occupation
        /// </summary>
        [DataMember]
        public string Occupation { get; set; }
        /// <summary>
        /// Address 1
        /// </summary>
        [DataMember]
        public string Address1 { get; set; }
        /// <summary>
        /// Address 2
        /// </summary>
        [DataMember]
        public string Address2 { get; set; }
        /// <summary>
        /// Address 3
        /// </summary>
        [DataMember]
        public string Address3 { get; set; }
        /// <summary>
        /// City
        /// </summary>
        [DataMember]
        public string City { get; set; }
        /// <summary>
        /// State
        /// </summary>
        [DataMember]
        public string State { get; set; }
        /// <summary>
        /// Zip Code
        /// </summary>
        [DataMember]
        public string ZipCode { get; set; }

        /// <summary>
        /// Plus 4
        /// </summary>
        [DataMember]
        public string Plus4 { get; set; }
        /// <summary>
        /// Country
        /// </summary>
        [DataMember]
        public string Country { get; set; }
        /// <summary>
        /// County
        /// </summary>
        [DataMember]
        public string County { get; set; }

        /// <summary>
        /// Phone
        /// </summary>
        [DataMember]
        public string Phone { get; set; }
        /// <summary>
        /// Phone Ext
        /// </summary>
        [DataMember]
        public string PhoneExt { get; set; }
        /// <summary>
        /// Fax
        /// </summary>
        [DataMember]
        public string Fax { get; set; }
        /// <summary>
        /// Website
        /// </summary>
        [DataMember]
        public string Website { get; set; }
        /// <summary>
        /// Mobile
        /// </summary>
        [DataMember]
        public string Mobile { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        [DataMember]
        public string Gender { get; set; }
        /// <summary>
        /// Age
        /// </summary>
        [DataMember]
        public int Age { get; set; }
        /// <summary>
        /// Birth date
        /// </summary>
        [DataMember]
        public DateTime Birthdate { get; set; }
        /// <summary>
        /// Income
        /// </summary>
        [DataMember]
        public string Income { get; set; }

        /// <summary>
        /// Email Renew Permission
        /// </summary>
        [DataMember]
        public bool EmailRenewPermission { get; set; }
        /// <summary>
        /// Fax Permission
        /// </summary>
        [DataMember]
        public bool FaxPermission { get; set; }
        /// <summary>
        /// Mail Permission
        /// </summary>
        [DataMember]
        public bool MailPermission { get; set; }
        /// <summary>
        /// Other Products Permission
        /// </summary>
        [DataMember]
        public bool OtherProductsPermission { get; set; }
        /// <summary>
        /// Phone Permission
        /// </summary>
        [DataMember]
        public bool PhonePermission { get; set; }
        /// <summary>
        /// Text Permission
        /// </summary>
        [DataMember]
        public bool TextPermission { get; set; }
        /// <summary>
        /// Third Party Permission
        /// </summary>
        [DataMember]
        public bool ThirdPartyPermission { get; set; }

        /// <summary>
        /// Subscriber Product Demographics
        /// </summary>
        [DataMember]
        public List<SubscriberProductDemographic> SubscriberProductDemographics { get; set; }

        /// <summary>
        /// Subscriber Product Custom Fields
        /// </summary>
        [DataMember]
        public List<SubscriberConsensusDemographic> SubscriberProductCustomFields { get; set; }

        /// <summary>
        /// Subscriber Consensus Custom Fields
        /// </summary>
        [DataMember]
        public List<SubscriberConsensusDemographic> SubscriberConsensusCustomFields { get; set; }


        #region Hidden Fields
        /// <summary>
        /// Subscription ID
        /// </summary>
        [IgnoreDataMember]
        public int SubscriptionID { get; set; }
        /// <summary>
        /// Process Code
        /// </summary>
        [IgnoreDataMember]
        public string ProcessCode { get; set; }
        /// <summary>
        /// Pub ID
        /// </summary>
        [IgnoreDataMember]
        public int PubID { get; set; }

        /// <summary>
        /// Is Pub Code Valid
        /// </summary>
        [IgnoreDataMember]
        public bool IsPubCodeValid { get; set; }
        /// <summary>
        /// Is Code Sheet Valid
        /// </summary>
        [IgnoreDataMember]
        public bool IsCodeSheetValid { get; set; }
        /// <summary>
        /// Is Product Subscriber Created
        /// </summary>
        [IgnoreDataMember]
        public bool IsProductSubscriberCreated { get; set; }

        /// <summary>
        /// Log Message
        /// </summary>
        [IgnoreDataMember]
        public string LogMessage { get; set; }
        /// <summary>
        /// Pub Code Message
        /// </summary>
        [IgnoreDataMember]
        public string PubCodeMessage { get; set; }
        /// <summary>
        /// Code Sheet Message
        /// </summary>
        [IgnoreDataMember]
        public string CodeSheetMessage { get; set; }
        /// <summary>
        /// Subscriber Produc tMessage
        /// </summary>
        [IgnoreDataMember]
        public string SubscriberProductMessage { get; set; }

        ///// <summary>
        ///// Subscriber Product Identifiers
        ///// </summary>
        //[IgnoreDataMember]
        //public Dictionary<Guid, string> SubscriberProductIdentifiers { get; set; }
        #endregion
        #endregion

        public SaveSubscriber()
        {
            IGrp_No = new Guid();
            SequenceID = 0;
            AccountNumber = string.Empty;
            PubCode = string.Empty;
            QualificationDate = DateTime.Now;
            CategoryID = 0;
            TransactionID = 0;
            QSourceID = 0;
            Demo7 = string.Empty;
            Copies = 0;
            GraceIssues = 0;
            SubscriberSourceCode = string.Empty;
            Verify = string.Empty;
            Email = string.Empty;
            EmailStatus = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
		    Company = string.Empty;
		    Title = string.Empty;
		    Occupation = string.Empty;
		    Address1 = string.Empty;
		    Address2 = string.Empty;
		    Address3 = string.Empty;
		    City = string.Empty;
		    State = string.Empty;
            ZipCode = string.Empty;
            Plus4 = string.Empty;
		    Country = string.Empty;
		    County = string.Empty;
		    Phone = string.Empty;
		    PhoneExt = string.Empty;
		    Fax = string.Empty;
		    Website = string.Empty;
		    Mobile = string.Empty;
		    Gender = string.Empty;
            Age = 0;
            Birthdate = DateTime.Now;
		    Income = string.Empty;
            EmailRenewPermission = false;
            FaxPermission = false;
            MailPermission = false;
            OtherProductsPermission = false;
            PhonePermission = false;
            TextPermission = false;
            ThirdPartyPermission = false;

            SubscriberProductDemographics = new List<SubscriberProductDemographic>();
            SubscriberProductCustomFields = new List<SubscriberConsensusDemographic>();
            SubscriberConsensusCustomFields = new List<SubscriberConsensusDemographic>();

            SubscriptionID = 0;
            ProcessCode = string.Empty;
            PubID = 0;
            IsPubCodeValid = false;
            IsCodeSheetValid = false;
            IsProductSubscriberCreated = false;
            LogMessage = string.Empty;
            PubCodeMessage = string.Empty;
            CodeSheetMessage = string.Empty;
            SubscriberProductMessage = string.Empty;
            //SubscriberProductIdentifiers = new Dictionary<Guid, string>();
        }               
    }
}