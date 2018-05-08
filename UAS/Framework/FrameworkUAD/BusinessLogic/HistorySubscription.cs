using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class HistorySubscription
    {
        public List<Entity.HistorySubscription> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.HistorySubscription> x = null;
            x = DataAccess.HistorySubscription.Select(client).ToList();
            return x;
        }

        public int Save(Entity.HistorySubscription x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.SubscriptionID = DataAccess.HistorySubscription.Save(x, client);
                scope.Complete();
            }

            return x.SubscriptionID;
        }

        public int Save(FrameworkUAD.Entity.ProductSubscription productSubscription, int userID, KMPlatform.Object.ClientConnections client)
        {
            FrameworkUAD.Entity.HistorySubscription sh = new FrameworkUAD.Entity.HistorySubscription();

            productSubscription.Phone = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(productSubscription.Phone);
            productSubscription.Mobile = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(productSubscription.Mobile);
            productSubscription.Fax = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(productSubscription.Fax);

            sh.SubscriptionID = productSubscription.SubscriptionID;
            sh.PubSubscriptionID = productSubscription.PubSubscriptionID;
            sh.PubID = productSubscription.PubID;
            sh.PubCategoryID = productSubscription.PubCategoryID;
            sh.PubTransactionID = productSubscription.PubTransactionID;
            sh.SubscriptionStatusID = productSubscription.SubscriptionStatusID;
            sh.StatusUpdatedReason = productSubscription.StatusUpdatedReason;
            sh.IsComp = productSubscription.IsComp;
            sh.IsPaid = productSubscription.IsPaid;
            sh.PubQSourceID = productSubscription.PubQSourceID;
            sh.QualificationDate = productSubscription.QualificationDate;
            sh.Demo7 = productSubscription.Demo7;
            sh.IsSubscribed = productSubscription.IsSubscribed;
            sh.SubscriberSourceCode = productSubscription.SubscriberSourceCode;
            sh.Copies = productSubscription.Copies;
            sh.OrigsSrc = productSubscription.OrigsSrc;
            sh.AccountNumber = productSubscription.AccountNumber;
            sh.GraceIssues = productSubscription.GraceIssues;
            sh.IsNewSubscription = productSubscription.IsNewSubscription;
            sh.OnBehalfOf = productSubscription.OnBehalfOf;
            sh.Par3CID = productSubscription.Par3CID;
            sh.SequenceID = productSubscription.SequenceID;
            sh.EmailStatusID = productSubscription.EmailStatusID;
            sh.AddRemoveID = productSubscription.AddRemoveID;
            sh.SubSrcID = productSubscription.SubSrcID;
            sh.IMBSeq = productSubscription.IMBSeq;
            sh.Verified = productSubscription.Verify;
            sh.IsActive = productSubscription.IsActive;
            sh.Status = productSubscription.Status;
            sh.FirstName = productSubscription.FirstName;
            sh.LastName = productSubscription.LastName;
            sh.Company = productSubscription.Company;
            sh.Title = productSubscription.Title;
            sh.AddressTypeID = productSubscription.AddressTypeCodeId;
            sh.AddressTypeCodeId = productSubscription.AddressTypeCodeId;
            sh.Address1 = productSubscription.Address1;
            sh.Address2 = productSubscription.Address2;
            sh.Address3 = productSubscription.Address3;
            sh.City = productSubscription.City;
            sh.RegionCode = productSubscription.RegionCode;
            sh.RegionID = productSubscription.RegionID;
            sh.ZipCode = productSubscription.ZipCode;
            sh.Plus4 = productSubscription.Plus4;
            sh.CarrierRoute = productSubscription.CarrierRoute;
            sh.County = productSubscription.County;
            sh.Country = productSubscription.Country;
            sh.CountryID = productSubscription.CountryID;
            sh.MemberGroup = productSubscription.MemberGroup;
            sh.IsAddressValidated = productSubscription.IsAddressValidated;
            sh.AddressValidationDate = productSubscription.AddressValidationDate;
            sh.AddressValidationSource = productSubscription.AddressValidationSource;
            sh.AddressValidationMessage = productSubscription.AddressValidationMessage;
            sh.Email = productSubscription.Email;
            sh.Phone = productSubscription.Phone;
            sh.Fax = productSubscription.Fax;
            sh.Mobile = productSubscription.Mobile;
            sh.Website = productSubscription.Website;
            sh.DateCreated = productSubscription.DateCreated;
            sh.DateUpdated = productSubscription.DateUpdated;
            sh.CreatedByUserID = productSubscription.CreatedByUserID;
            sh.UpdatedByUserID = productSubscription.UpdatedByUserID;
            sh.DateCreated = productSubscription.DateCreated;
            sh.CreatedByUserID = productSubscription.CreatedByUserID;
            sh.IsLocked = productSubscription.IsLocked;
            sh.LockDate = productSubscription.LockDate;
            sh.LockDateRelease = productSubscription.LockDateRelease;
            sh.LockedByUserID = productSubscription.LockedByUserID;
            sh.PhoneExt = sh.PhoneExt;
            sh.ExternalKeyID = productSubscription.ExternalKeyID;
            sh.Occupation = productSubscription.Occupation;
            sh.Latitude = productSubscription.Latitude;
            sh.Longitude = productSubscription.Longitude;
            sh.Birthdate = productSubscription.Birthdate;
            sh.Age = productSubscription.Age;
            sh.Income = productSubscription.Income;
            sh.Gender = productSubscription.Gender;
            sh.IsInActiveWaveMailing = productSubscription.IsInActiveWaveMailing;
            sh.AddressUpdatedSourceTypeCodeId = productSubscription.AddressUpdatedSourceTypeCodeId;
            sh.WaveMailingID = productSubscription.WaveMailingID;
            sh.IGrp_No = productSubscription.IGrp_No;
            sh.SFRecordIdentifier = productSubscription.SFRecordIdentifier;

            BusinessLogic.HistorySubscription hsWorker = new HistorySubscription();
            sh.HistorySubscriptionID = hsWorker.Save(sh, client);
            return sh.HistorySubscriptionID;
        }

    }
}
