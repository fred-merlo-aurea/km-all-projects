using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.mvc.Models
{
    public class Email
    {
        public Email() { }
        public Email(int EmailID, int GroupID = -1)
        {
            //var internalEmail = ECN_Framework_BusinessLayer.Communicator.Email.GetByGroupID();
        }
        public Email(ECN_Framework_Entities.Communicator.Email email, int GroupID)
        {
            EmailID = email.EmailID;
            EmailAddress = email.EmailAddress;
            FormatTypeCode = email.FormatTypeCode;
            SubscribeTypeCode = email.SubscribeTypeCode;
            DateAdded = email.DateAdded;
            //CreatedOn = email.DateAdded;
            //LastChanged = email.DateUpdated;
            DateUpdated = email.DateUpdated;
            EmailAddress = email.EmailAddress;
            FormatTypeCode = email.FormatTypeCode;
            Password = email.Password;
            BounceScore = email.BounceScore;
            SubscribeTypeCode = email.SubscribeTypeCode;
            SoftBounceScore = email.SoftBounceScore;
            Title = email.Title;
            FirstName = email.FirstName;
            LastName = email.LastName;
            FullName = email.FullName;
            Company = email.Company;
            Occupation = email.Occupation;
            Address = email.Address;
            Address2 = email.Address2;
            City = email.City;
            State = email.State;
            Zip = email.Zip;
            Country = email.Country;
            Voice = email.Voice;
            Mobile = email.Mobile;
            Fax = email.Fax;
            Income = email.Income;
            Gender = email.Gender;
            Website = email.Website;
            Age = email.Age;
            Birthdate = String.Format("{0:MM/dd/yyyy}", email.Birthdate);
            User1 = email.User1;
            User2 = email.User2;
            User3 = email.User3;
            User4 = email.User4;
            User5 = email.User5;
            User6 = email.User6;
            UserEvent1 = email.UserEvent1;
            UserEvent1Date = String.Format("{0:MM/dd/yyyy}", email.UserEvent1Date);
            UserEvent2 = email.UserEvent2;
            UserEvent2Date = String.Format("{0:MM/dd/yyyy}", email.UserEvent2Date);
            Notes = email.Notes;
            CurrentGroupID = GroupID;
            SMSOptIn = email.SMSOptIn;
            CarrierCode = email.CarrierCode;
            CustomerID = email.CustomerID;
            TotalCount = 0;
        }
        public int EmailID { get; set; }
        public string EmailAddress { get; set; }
        public int CustomerID { get; set; }
        public string FormatTypeCode { get; set; }
        public string SubscribeTypeCode { get; set; }
        private DateTime? _DateAdded { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? CreatedOn { get; set; }

        public DateTime? DateUpdated { get; set; }
        public DateTime? LastChanged { get; set; }
        public string Password { get; set; }
        public int? BounceScore { get; set; }
        public int? SoftBounceScore { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Company { get; set; }
        public string Occupation { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Voice { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Income { get; set; }
        public string Gender { get; set; }
        public string Website { get; set; }
        public string Age { get; set; }
        public string Birthdate { get; set; }
        public string User1 { get; set; }
        public string User2 { get; set; }
        public string User3 { get; set; }
        public string User4 { get; set; }
        public string User5 { get; set; }
        public string User6 { get; set; }
        public string UserEvent1 { get; set; }
        public string UserEvent1Date { get; set; }
        public string UserEvent2 { get; set; }
        public string UserEvent2Date { get; set; }
        public string Notes { get; set; }
        public int CurrentGroupID { get; set; }
        public string SMSOptIn { get; set; }
        public string CarrierCode { get; set; }

        public int TotalCount { get; set; }
    }
}