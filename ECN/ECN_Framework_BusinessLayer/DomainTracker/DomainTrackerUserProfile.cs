using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;

namespace ECN_Framework_BusinessLayer.DomainTracker
{
    public class DomainTrackerUserProfile
    {
        public static ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile Get(string EmailAddress, int BaseChannelID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile domainTrackerUserProfile = GetByEmailAddress(EmailAddress, BaseChannelID);
            if(domainTrackerUserProfile == null)
            {
                domainTrackerUserProfile= new ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile();
                domainTrackerUserProfile.EmailAddress = EmailAddress;
                domainTrackerUserProfile.BaseChannelID = BaseChannelID;
                domainTrackerUserProfile.CreatedUserID = user.UserID;
                if (EmailAddress.EndsWith("@unknownkmpsgroup.com"))
                    domainTrackerUserProfile.IsKnown = false;
                else
                    domainTrackerUserProfile.IsKnown = true;

                Save(domainTrackerUserProfile);
            }
            return domainTrackerUserProfile;
        }

        public static int Save(ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile domainTrackerUserProfile)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                domainTrackerUserProfile.ProfileID = ECN_Framework_DataLayer.DomainTracker.DomainTrackerUserProfile.Save(domainTrackerUserProfile);
                scope.Complete();
            }
            return domainTrackerUserProfile.ProfileID;
        }

        public static ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile GetByEmailAddress(string EmailAddress, int BaseChannelID)
        {
            ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile DomainTrackerUserProfile = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                DomainTrackerUserProfile = ECN_Framework_DataLayer.DomainTracker.DomainTrackerUserProfile.GetByEmailAddress(EmailAddress, BaseChannelID);
                scope.Complete();
            }
            return DomainTrackerUserProfile;
        }

        public static bool Exists(string EmailAddress, int BaseChannelID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.DomainTracker.DomainTrackerUserProfile.Exists(EmailAddress, BaseChannelID);
                scope.Complete();
            }
            return exists;
        }

        public static List<ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile> GetByDomainTrackerID(int domainTrackerID, int? CurrentPage, int? PageSize, KMPlatform.Entity.User user, string StartDate, string EndDate, string Email, string TypeFilter = "known", string sortColumn = "EmailAddress", string sortDirection = "ASC", string PageUrl = "")
        {
            List<ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile> itemList = new List<ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.DomainTracker.DomainTrackerUserProfile.GetByDomainTrackerID(domainTrackerID, CurrentPage, PageSize, StartDate, EndDate, Email, TypeFilter, sortColumn, sortDirection, PageUrl);
                scope.Complete();
            }
            return itemList.OrderBy(x=>x.EmailAddress).ToList();
        }

        public static DataTable GetByDomainTrackerID_Paging(int domainTrackerID, int? CurrentPage, int? PageSize, KMPlatform.Entity.User user, string StartDate, string EndDate, string Email)
        {
            DataTable itemList = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.DomainTracker.DomainTrackerUserProfile.GetByDomainTrackerID_Paging(domainTrackerID, CurrentPage, PageSize, StartDate, EndDate, Email);
                scope.Complete();
            }
            return itemList;
        }


        //public static List<ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile> GetUserProfilesByEmailaddress(int domainTrackerID, string emailaddress)
        //{
        //    List<ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile> itemList = new List<ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile>();
        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
        //    {
        //        itemList = ECN_Framework_DataLayer.DomainTracker.DomainTrackerUserProfile.GetUserProfilesByEmailaddress(domainTrackerID, emailaddress);
        //        scope.Complete();
        //    }
        //    return itemList;
        //}
    }
}
