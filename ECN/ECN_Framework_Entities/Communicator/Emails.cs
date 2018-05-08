using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class Email
    {
        public Email()
        {
            EmailID = -1;
            EmailAddress = string.Empty;
            CustomerID = -1;
            Title = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            FullName = string.Empty;
            Company = string.Empty;
            Occupation = string.Empty;
            Address = string.Empty;
            Address2 = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;
            Country = string.Empty;
            Voice = string.Empty;
            Mobile = string.Empty;
            Fax = string.Empty;
            Website = string.Empty;
            Age = string.Empty;
            Income = string.Empty;
            Gender = string.Empty;
            User1 = string.Empty;
            User2 = string.Empty;
            User3 = string.Empty;
            User4 = string.Empty;
            User5 = string.Empty;
            User6 = string.Empty;
            Birthdate = null;
            UserEvent1 = string.Empty;
            UserEvent1Date = null;
            UserEvent2 = string.Empty;
            UserEvent2Date = null;
            Notes = string.Empty;
            Password = string.Empty;
            BounceScore = null;
            SoftBounceScore = null;
            SMSOptIn = string.Empty;
            CarrierCode = string.Empty;
            FormatTypeCode = string.Empty;
            SubscribeTypeCode = string.Empty;
            DateAdded = null;
            DateUpdated = null;
            //CreatedUserID = null;
            //UpdatedUserID = null;
        }

        [DataMember]
        public int EmailID { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public string Occupation { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Address2 { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Zip { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string Voice { get; set; }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Website { get; set; }
        [DataMember]
        public string Age { get; set; }
        [DataMember]
        public string Income { get; set; }
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public string User1 { get; set; }
        [DataMember]
        public string User2 { get; set; }
        [DataMember]
        public string User3 { get; set; }
        [DataMember]
        public string User4 { get; set; }
        [DataMember]
        public string User5 { get; set; }
        [DataMember]
        public string User6 { get; set; }
        [DataMember]
        public DateTime? Birthdate { get; set; }
        [DataMember]
        public string UserEvent1 { get; set; }
        [DataMember]
        public DateTime? UserEvent1Date { get; set; }
        [DataMember]
        public string UserEvent2 { get; set; }
        [DataMember]
        public DateTime? UserEvent2Date { get; set; }
        [DataMember]
        public string Notes { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public int? BounceScore { get; set; }
        [DataMember]
        public int? SoftBounceScore { get; set; }
        [DataMember]
        public string SMSOptIn { get; set; }
        [DataMember]
        public string CarrierCode { get; set; }
        [DataMember]
        public string FormatTypeCode { get; set; }
        [DataMember]
        public string SubscribeTypeCode { get; set; }
        [DataMember]
        public DateTime? DateAdded { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        //[DataMember]
        //public int? CreatedUserID { get; set; }
        //[DataMember]
        //public int? UpdatedUserID { get; set; }

    }
}
