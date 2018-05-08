using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.DomainTracker
{
    public class DomainTrackerActivity
    {
        

        public static int Save(ECN_Framework_Entities.DomainTracker.DomainTrackerActivity domainTrackerActivity)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                domainTrackerActivity.DomainTrackerActivityID = ECN_Framework_DataLayer.DomainTracker.DomainTrackerActivity.Save(domainTrackerActivity);
                scope.Complete();
            }
            return domainTrackerActivity.DomainTrackerActivityID;
        }

        public static DataTable GetBrowserStats(int domainTrackerID)
        {
            DataTable dt = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.DomainTracker.DomainTrackerActivity.GetBrowserStats(domainTrackerID);
                scope.Complete();
            }
            return dt;
        }

        public static DataTable GetHeatMapDataTableStats(int domainTrackerID, string startDate, string endDate, string Filter = "", string TypeFilter = "known")
        {
            DataTable dt = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.DomainTracker.DomainTrackerActivity.GetHeatMapDataTableStats(domainTrackerID, startDate, endDate, Filter, TypeFilter);
                scope.Complete();
            }
            return dt;
        }

        public static DataTable GetHeatMapKnown_UnknownStats(int domainTrackerID, string startDate, string endDate, string Filter = "")
        {
            DataTable dt = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.DomainTracker.DomainTrackerActivity.GetHeatMapKnown_UnknownStats(domainTrackerID, startDate, endDate, Filter);
                scope.Complete();
            }
            return dt;
        }
        public static DataTable GetBrowserStats(int domainTrackerID, string startDate, string endDate, string Filter = "", string TypeFilter = "known")
        {
            DataTable dt = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.DomainTracker.DomainTrackerActivity.GetBrowserStats(domainTrackerID, startDate, endDate, Filter, TypeFilter);
                scope.Complete();
            }
            return dt;
        }

        public static DataTable GetBrowserStats_Known_Unknown(int domainTrackerID, string startDate, string endDate, string Filter = "")
        {
            DataTable dt = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.DomainTracker.DomainTrackerActivity.GetBrowserStats_Known_Unknown(domainTrackerID, startDate, endDate, Filter);
                scope.Complete();
            }
            return dt;
        }
        public static DataTable GetOSStats(int domainTrackerID)
        {
            DataTable dt = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.DomainTracker.DomainTrackerActivity.GetOSStats(domainTrackerID);
                scope.Complete();
            }
            return dt;
        }
        public static DataTable GetOSStats(int domainTrackerID, string startDate, string endDate, string Filter = "", string TypeFilter = "known")
        {
            DataTable dt = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.DomainTracker.DomainTrackerActivity.GetOSStats(domainTrackerID, startDate, endDate, Filter, TypeFilter);
                scope.Complete();
            }
            return dt;
        }

        public static DataTable GetOSStats_Known_Unknown(int domainTrackerID, string startDate, string endDate, string Filter = "")
        {
            DataTable dt = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.DomainTracker.DomainTrackerActivity.GetOSStats_Known_Unknown(domainTrackerID, startDate, endDate, Filter);
                scope.Complete();
            }
            return dt;
        }
        public static DataTable GetURLStats(int domainTrackerID, string startDate, string endDate, string Filter = "", int TopRow = 0, string TypeFilter = "Known")
        {
            DataTable dt = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.DomainTracker.DomainTrackerActivity.GetURLStats(domainTrackerID, startDate, endDate, Filter, TopRow, TypeFilter);
                scope.Complete();
            }
            return dt;
        }

        

             public static DataTable GetURLStats_Known_Unknown(int domainTrackerID, string startDate, string endDate, string Filter = "", int TopRow = 0)
        {
            DataTable dt = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.DomainTracker.DomainTrackerActivity.GetURLStats_Known_Unknown(domainTrackerID, startDate, endDate, Filter, TopRow);
                scope.Complete();
            }
            return dt;
        }

        public static DataTable GetTotalViews(int domainTrackerID)
        {
            DataTable dt = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.DomainTracker.DomainTrackerActivity.GetTotalViews(domainTrackerID);
                scope.Complete();
            }
            return dt;
        }
        public static DataTable GetTotalViews(int domainTrackerID, string startDate, string endDate, string Filter = "", string TypeFilter = "all")
        {
            DataTable dt = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.DomainTracker.DomainTrackerActivity.GetTotalViews(domainTrackerID, startDate, endDate,TypeFilter, Filter);
                scope.Complete();
            }
            return dt;
        }
        public static DataTable GetPageViewsPerDay(int domainTrackerID)
        {
            DataTable dt = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.DomainTracker.DomainTrackerActivity.GetPageViewsPerDay(domainTrackerID);
                scope.Complete();
            }
            return dt;
        }
        public static DataTable GetPageViewsPerDay(int domainTrackerID, string fromDate, string toDate, string Filter = "", string TypeFilter = "known")
        {
            DataTable dt = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.DomainTracker.DomainTrackerActivity.GetPageViewsPerDay(domainTrackerID, fromDate, toDate, Filter, TypeFilter);
                scope.Complete();
            }
            return dt;
        }
        public static DataTable GetPageViewsPerDay_Known(int domainTrackerID, string fromDate, string toDate, string Filter = "")
        {
            DataTable dt = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.DomainTracker.DomainTrackerActivity.GetPageViewsPerDay_Known(domainTrackerID, fromDate, toDate, Filter);
                scope.Complete();
            }
            return dt;
        }

        public static List<ECN_Framework_Entities.DomainTracker.DomainTrackerActivity> GetByProfileID(int profileID, int domainTrackerID, KMPlatform.Entity.User user, string StartDate = "01/01/1885", string EndDate = "01/01/2400", string PageUrl = "")
        {
            List<ECN_Framework_Entities.DomainTracker.DomainTrackerActivity> itemList = new List<ECN_Framework_Entities.DomainTracker.DomainTrackerActivity>();
            DateTime Start = new DateTime();
            DateTime End = new DateTime();
            DateTime.TryParse(StartDate, out Start);
            DateTime.TryParse(EndDate, out End);
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.DomainTracker.DomainTrackerActivity.GetByProfileID(profileID, domainTrackerID, Start, End, PageUrl);
                scope.Complete();
            }
            return itemList;
        }

        public static List<ECN_Framework_Entities.DomainTracker.FieldsValuePair> GetFieldValuesByProfileID(int? ProfileID, int domainTrackerID, string StartDate, string EndDate, string PageUrl = "")//, int recordcount)
        {
            List<ECN_Framework_Entities.DomainTracker.FieldsValuePair> itemList = new List<ECN_Framework_Entities.DomainTracker.FieldsValuePair>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.DomainTracker.DomainTrackerActivity.GetFieldValuesByProfileID(ProfileID, domainTrackerID, StartDate, EndDate, PageUrl);//, recordcount);
                scope.Complete();
            }
            if (itemList == null)
                itemList = new List<ECN_Framework_Entities.DomainTracker.FieldsValuePair>();
            return itemList;
        }

        public static void MergeAnonActivity(string anonEmail, string actualEmail, int baseChannelID)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ECN_Framework_DataLayer.DomainTracker.DomainTrackerActivity.MergeAnonActivity(anonEmail, actualEmail, baseChannelID);
                scope.Complete();
            }
        }
    }
}
