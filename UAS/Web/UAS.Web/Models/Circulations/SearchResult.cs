using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAS.Web.Models.Circulations
{
    public class SearchResult
    {
        public int id { get; set; }
        public string Status { get; set; }
        public int SequenceNumber { get; set; }
        public string Publisher { get; set; }
        public string Product { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get {return FirstName + " " + LastName;} }
        public string Title { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public int OldAccountNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Zipcode { get; set; }
        public string Website { get; set; }
        public string CategoryType { get; set; }
        public string PaymentOption { get; set; }
        public string MemberGroup { get; set; }
        public string OrgSubsrc { get; set; }
        public string Subsrc { get; set; }
        public string SubsrcType { get; set; }
        public string MediaType { get; set; }
        public string Verify { get; set; }
        public string CreatedDate { get; set; }
        public string LastUpdated { get; set; }
        public int Copies { get; set; }
        public bool qualMail { get; set; }
        public bool qualFax { get; set; }
        public bool qualPhone { get; set; }
        public bool qualOtherProducts { get; set; }
        public bool qualEmailRenew { get; set; }
        public bool qualThirdParty { get; set; }
        public bool qualText { get; set; }
        public bool qualMobile { get; set; }
        public DateTime QualificationDate { get; set; }
        public string QualificationSource { get; set; }
        public string Par3C { get; set; }
        public string Airmail { get; set; }
        public string SalesRep { get; set; }
        public string Function { get; set; }
        public string BusinessOption { get; set; }
        public string EmployOption { get; set; }
        public string SalesOption { get; set; }
        public bool InBriefNews { get; set; }
        public bool Subscription { get; set; }
        public string GlobalSuppressionSource { get; set; }
        public string Mediadef { get; set; }
        public string excl_inv { get; set; }
        public string excl_renew { get; set; }
        public string UnsubscribeReason { get; set; }
        public string AddressType { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

    }
}