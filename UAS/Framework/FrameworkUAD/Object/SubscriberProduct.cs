using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class SubscriberProduct
    {
        public SubscriberProduct() 
        {
            Sequence = 0;
            FName = string.Empty;
            LName = string.Empty;
            Title = string.Empty;
            Company = string.Empty;
            Address = string.Empty;
            MailStop = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;
            Plus4 = string.Empty;
            ForZip = string.Empty;
            County = string.Empty;
            Country = string.Empty;
            Phone = string.Empty;
            Fax = string.Empty;
            Gender = string.Empty;
            Address = string.Empty;
            Home_Work_Address = string.Empty;
            Mobile = string.Empty;
            Score = 0;
            Latitude = 0;
            Longitude = 0;
            Demo7 = string.Empty;
            Par3C = string.Empty;
            Email = string.Empty;
            TransactionDate = DateTime.Now;
            QDate = DateTime.Now;
            IsActive = true;
            Demographics = new List<SubscriberProductDemographic>();
        }

        #region Properties
        [DataMember(Name = "ExternalKeyID")]
        public int Sequence { get; set; }
        [DataMember(Name = "FirstName")]
        public string FName { get; set; }
        [DataMember(Name = "LastName")]
        public string LName { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember(Name = "Address2")]
        public string MailStop { get; set; }
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
        public string Phone { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public bool MailPermission { get; set; }//rename to MailPermission Demo31
        [DataMember]
        public bool FaxPermission { get; set; }//rename to FaxPermission Demo32
        [DataMember]
        public bool PhonePermission { get; set; }//rename to PhonePermission Demo33
        [DataMember]
        public bool OtherProductsPermission { get; set; }//rename to OtherProductsPermission Demo34
        [DataMember]
        public bool ThirdPartyPermission { get; set; }//rename to ThirdPartyPermission Demo35
        [DataMember]
        public bool EmailRenewPermission { get; set; }//rename to EmailRenewPermission Demo36
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public string Address3 { get; set; }
        [DataMember]
        public string Home_Work_Address { get; set; }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public int Score { get; set; }
        [DataMember]
        public decimal Latitude { get; set; }
        [DataMember]
        public decimal Longitude { get; set; }
        [DataMember]
        public string Demo7 { get; set; }
        [DataMember]
        public Guid IGrp_No { get; set; }
        [DataMember]
        public string Par3C { get; set; }
        [DataMember]
        public DateTime? TransactionDate { get; set; }
        [DataMember]
        public DateTime? QDate { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public int SubscriptionID { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public List<SubscriberProductDemographic> Demographics { get; set; }
        #endregion
    }
}
