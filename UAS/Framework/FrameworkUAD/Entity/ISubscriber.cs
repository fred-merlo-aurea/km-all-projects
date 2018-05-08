using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.Entity
{
    public interface ISubscriber
    {
        string PubCode { get; set; }
        int Sequence { get; set; }
        string FName { get; set; }
        string LName { get; set; }
        string Title { get; set; }
        string Company { get; set; }
        string Address { get; set; }
        string MailStop { get; set; }
        string City { get; set; }
        string State { get; set; }
        string Zip { get; set; }
        string Plus4 { get; set; }
        string ForZip { get; set; }
        string County { get; set; }
        string Country { get; set; }
        int CountryID { get; set; }
        string Phone { get; set; }
        string Fax { get; set; }
        string Email { get; set; }
        int CategoryID { get; set; }
        int TransactionID { get; set; }
        DateTime? TransactionDate { get; set; }
        DateTime? QDate { get; set; }
        int QSourceID { get; set; }
        string RegCode { get; set; }
        string Verified { get; set; }
        string SubSrc { get; set; }
        string OrigsSrc { get; set; }
        string Par3C { get; set; }
        bool? MailPermission { get; set; }
        bool? FaxPermission { get; set; }
        bool? PhonePermission { get; set; }
        bool? OtherProductsPermission { get; set; }
        bool? ThirdPartyPermission { get; set; }
        bool? EmailRenewPermission { get; set; }
        bool? TextPermission { get; set; }
        string Source { get; set; }
        string Priority { get; set; }
        string Sic { get; set; }
        string SicCode { get; set; }
        string Gender { get; set; }
        string Address3 { get; set; }
        string Home_Work_Address { get; set; }
        string Demo7 { get; set; }
        string Mobile { get; set; }
        decimal Latitude { get; set; }
        decimal Longitude { get; set; }
        int ImportRowNumber { get; set; }
        bool IsActive { get; set; }
        string ProcessCode { get; set; }
        int ExternalKeyId { get; set; }
        string AccountNumber { get; set; }
        int EmailID { get; set; }
        int Copies { get; set; }
        int GraceIssues { get; set; }
        bool IsComp { get; set; }
        bool IsPaid { get; set; }
        bool IsSubscribed { get; set; }
        string Occupation { get; set; }
        int SubscriptionStatusID { get; set; }
        int SubsrcID { get; set; }
        string Website { get; set; }
        int CreatedByUserID { get; set; }
        Guid SORecordIdentifier { get; set; }
    }
}
