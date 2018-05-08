using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.Object
{
    public class Subscriber
    {
        public Subscriber()
        {
        }

        #region Properties
        public int SubscriptionID { get; set; }
        public string Magazine_Name { get; set; }
        public int Sequence { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string MailStop { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Plus4 { get; set; }
        public string ForZip { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public int? CountryID { get; set; }
        public string Phone { get; set; }
        public bool PhoneExists { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public bool FaxExists { get; set; }
        public string Email { get; set; }
        public bool EmailExists { get; set; }
        public int? CategoryID { get; set; }
        public int? TransactionID { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime QDate { get; set; }
        public int? QSourceID { get; set; }
        public string RegCode { get; set; }
        public string Verified { get; set; }
        public string Subsrc { get; set; }
        public string Origssrc { get; set; }
        public string Par3c { get; set; }
        public bool? MailPermission { get; set; }
        public bool? FaxPermission { get; set; }
        public bool? PhonePermission { get; set; }
        public bool? OtherProductsPermission { get; set; }
        public bool? ThirdPartyPermission { get; set; }
        public bool? EmailRenewPermission { get; set; }
        public bool? TextPermission { get; set; }
        public string Source { get; set; }
        public string Priority { get; set; }
        public int IGRP_CNT { get; set; }
        public string Demo7 { get; set; }
        public int SubscriberID { get; set; }
        public int Score { get; set; }
        public string TransactionCodeName { get; set; }
        public string QSourceName { get; set; }
        public string Notes { get; set; }
        public bool IsMailable { get; set; }
        public bool IsLatLonValid { get; set; }
        public Guid IGrp_No { get; set; }
        public int UpdatedByUserID { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        #endregion
    }
}
