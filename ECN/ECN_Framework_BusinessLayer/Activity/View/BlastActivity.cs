using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Functions;

namespace ECN_Framework_BusinessLayer.Activity.View
{
    [Serializable]
    public class BlastActivity
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Blast;

        //these are all checked for security against customerid in the sql query so we don't need to call access check
        public static bool HasPermission(KMPlatform.Enums.Access AccessCode, KMPlatform.Entity.User user, KMPlatform.Enums.ServiceFeatures tempSF = KMPlatform.Enums.ServiceFeatures.Blast)
        {
            if (AccessCode == KMPlatform.Enums.Access.View)
            {
                //if (KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Campaigns.ToString()) ||
                //    KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Create_Regular_Blast.ToString()))
                if (KM.Platform.User.HasAccess(user, ServiceCode, tempSF, KMPlatform.Enums.Access.View))
                    return true;
            }
            else if (AccessCode == KMPlatform.Enums.Access.Edit)
            {
                //if (KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Create_Regular_Blast.ToString()))
                if (KM.Platform.User.HasAccess(user, ServiceCode, tempSF, KMPlatform.Enums.Access.Edit))
                    return true;
            }
            return false;
        }

        public static List<ECN_Framework_Entities.Activity.View.BlastActivity> GetByEmailID(int emailID, KMPlatform.Entity.User user)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return ECN_Framework_DataLayer.Activity.View.BlastActivity.GetByEmailID(emailID);
        }
        public static DataTable ActivityLogs(KMPlatform.Entity.User user, int emailID, int currentPage, int pageSize)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, KMPlatform.Enums.ServiceFeatures.EmailSearch, KMPlatform.Enums.Access.FullAccess))
                throw new ECN_Framework_Common.Objects.SecurityException();

            DataTable logs = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                logs = ECN_Framework_DataLayer.Activity.View.BlastActivity.ActivityLogs(emailID, currentPage, pageSize);
                scope.Complete();
            }
            return logs;
        }
        public static DataTable ChampionByProc(int sampleID, bool justWinningBlastID, KMPlatform.Entity.User user, string ABWinnerType)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Activity.View.BlastActivity.ChampionByProc(user.CustomerID, sampleID, justWinningBlastID, ABWinnerType);
                scope.Complete();
            }
            return dt;
        }

        public static DataTable ChampionByProc_NoAccessCheck(int sampleID,int customerID, bool justWinningBlastID, string ABWinnerType)
        {

            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Activity.View.BlastActivity.ChampionByProc(customerID, sampleID, justWinningBlastID, ABWinnerType);
                scope.Complete();
            }
            return dt;
        }

        public static DataTable GetABSampleCount(int blastA, int blastB)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Activity.View.BlastActivity.GetABSampleCount(blastA, blastB);
                scope.Complete();
            }
            return dt;
        }

        //wgh - these were created from old framework and cannot be used until they are verified

        public static DataTable GetBlastReport(int blastID, string udfName, string udfData, KMPlatform.Entity.User user)
        {


            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Activity.View.BlastActivity.GetBlastReport(user.CustomerID, blastID, udfName, udfData);
                scope.Complete();
            }
            return dt;
        }

        public static DataTable DownloadBlastReportDetails(int ID, bool IsCampaignItem, string ReportType, string FilterType, string ISP, KMPlatform.Entity.User user, string startDate = "", string endDate = "", string ProfileFilter = "ProfilePlusStandalone", bool OnlyUnique = false)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user, KMPlatform.Enums.ServiceFeatures.BlastReport))
                throw new ECN_Framework_Common.Objects.SecurityException();

            DataTable dtBlastReportDetails = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if(IsCampaignItem)
                    dtBlastReportDetails = ECN_Framework_DataLayer.Activity.View.BlastActivity.DownloadCampaignItemReportDetails(user.CustomerID, ID, ReportType, FilterType, ISP, startDate, endDate, ProfileFilter, OnlyUnique);
                else
                    dtBlastReportDetails = ECN_Framework_DataLayer.Activity.View.BlastActivity.DownloadBlastReportDetails(ID, ReportType, FilterType, ISP, startDate, endDate, ProfileFilter, OnlyUnique);
                scope.Complete();
            }

            return dtBlastReportDetails;
        }

        public static DataTable DownloadBlastReportDetails(int ID, bool IsCampaignItem, string ReportType, string FilterType, string ISP, int customerID, KMPlatform.Entity.User user, string startDate = "", string endDate = "", string ProfileFilter = "ProfilePlusStandalone", bool OnlyUnique = false)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user, KMPlatform.Enums.ServiceFeatures.BlastReport))
                throw new ECN_Framework_Common.Objects.SecurityException();

            DataTable dtBlastReportDetails = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (IsCampaignItem)
                    dtBlastReportDetails = ECN_Framework_DataLayer.Activity.View.BlastActivity.DownloadCampaignItemReportDetails(customerID, ID, ReportType, FilterType, ISP, startDate, endDate, ProfileFilter, OnlyUnique);
                else
                    dtBlastReportDetails = ECN_Framework_DataLayer.Activity.View.BlastActivity.DownloadBlastReportDetails(ID, ReportType, FilterType, ISP, startDate, endDate, ProfileFilter, OnlyUnique);
                scope.Complete();
            }

            return dtBlastReportDetails;
        }
        public static DataTable DownloadBlastLinkDetails(int ID,string LinkAlias,KMPlatform.Entity.User user, string startDate = "", string endDate = "", string ProfileFilter = "ProfilePlusStandalone", bool OnlyUnique = false)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user, KMPlatform.Enums.ServiceFeatures.BlastReport))
                throw new ECN_Framework_Common.Objects.SecurityException();

            DataTable dtBlastReportDetails = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
               dtBlastReportDetails = ECN_Framework_DataLayer.Activity.View.BlastActivity.DownloadBlastLinkDetails(user.CustomerID, ID, startDate, endDate, ProfileFilter, OnlyUnique, LinkAlias);
               scope.Complete();
            }

            return dtBlastReportDetails;
        }

        public static DataTable DownloadBlastLinkDetails(int ID, string LinkAlias, int customerID, KMPlatform.Entity.User user, string startDate = "", string endDate = "", string ProfileFilter = "ProfilePlusStandalone", bool OnlyUnique = false)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user, KMPlatform.Enums.ServiceFeatures.BlastReport))
                throw new ECN_Framework_Common.Objects.SecurityException();

            DataTable dtBlastReportDetails = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtBlastReportDetails = ECN_Framework_DataLayer.Activity.View.BlastActivity.DownloadBlastLinkDetails(customerID, ID, startDate, endDate, ProfileFilter, OnlyUnique, LinkAlias);
                scope.Complete();
            }

            return dtBlastReportDetails;
        }

        public static DataTable DownloadBlastReportDetails_NoAccessCheck(int ID, int CustomerID, bool IsCampaignItem, string ReportType, string FilterType, string ISP, string startDate = "", string endDate = "", string ProfileFilter = "ProfilePlusStandalone", string downloadFilter = "all", bool OnlyUnique = false)
        {

            DataTable dtBlastReportDetails = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (IsCampaignItem)
                    dtBlastReportDetails = ECN_Framework_DataLayer.Activity.View.BlastActivity.DownloadCampaignItemReportDetails(CustomerID, ID, ReportType, FilterType, ISP, startDate, endDate, ProfileFilter, OnlyUnique);
                else
                    dtBlastReportDetails = ECN_Framework_DataLayer.Activity.View.BlastActivity.DownloadBlastReportDetails(ID, ReportType, FilterType, ISP, startDate, endDate, ProfileFilter, OnlyUnique);
                scope.Complete();
            }

            return dtBlastReportDetails;
        }

        public static DataTable GetBlastEmails(int blastID, string reportType, string filterType, KMPlatform.Entity.User user)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Activity.View.BlastActivity.GetBlastEmails(blastID, reportType, filterType);
                scope.Complete();
            }
            return dt;
        }

        public static DataSet GetBlastReportDetails(int ID, bool IsCampaignItem, string ReportType, string FilterType, string ISP, int PageNo, int PageSize, string UDFName, string UDFData, KMPlatform.Entity.User user, bool OnlyUnique=false)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user, KMPlatform.Enums.ServiceFeatures.BlastReport))
                throw new ECN_Framework_Common.Objects.SecurityException();

            DataSet dsBlastReportDetails = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (IsCampaignItem)
                    dsBlastReportDetails = ECN_Framework_DataLayer.Activity.View.BlastActivity.GetCampaignItemReportDetails(ID, user.CustomerID, ReportType, FilterType, ISP, PageNo, PageSize, UDFName, UDFData, OnlyUnique);
                else
                    dsBlastReportDetails = ECN_Framework_DataLayer.Activity.View.BlastActivity.GetBlastReportDetails(ID, user.CustomerID, ReportType, FilterType, ISP, PageNo, PageSize, UDFName, UDFData, OnlyUnique);
     
                scope.Complete();
            }

            return dsBlastReportDetails;
        }

        public static DataSet GetBlastReportDetails_NoAccessCheck(int ID, bool IsCampaignItem, string ReportType, string FilterType, string ISP, int PageNo, int PageSize, string UDFName, string UDFData, KMPlatform.Entity.User user, bool OnlyUnique = false)
        {
            DataSet dsBlastReportDetails = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (IsCampaignItem)
                    dsBlastReportDetails = ECN_Framework_DataLayer.Activity.View.BlastActivity.GetCampaignItemReportDetails(ID, user.CustomerID, ReportType, FilterType, ISP, PageNo, PageSize, UDFName, UDFData, OnlyUnique);
                else
                    dsBlastReportDetails = ECN_Framework_DataLayer.Activity.View.BlastActivity.GetBlastReportDetails(ID, user.CustomerID, ReportType, FilterType, ISP, PageNo, PageSize, UDFName, UDFData, OnlyUnique);

                scope.Complete();
            }

            return dsBlastReportDetails;
        }

        public static DataTable GetClicksForGrid(int customerID, DateTime startDate, DateTime endDate)
        {
            DataTable dtClicks = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtClicks = ECN_Framework_DataLayer.Activity.View.BlastActivity.GetClicksForGrid(customerID, startDate, endDate);
                scope.Complete();
            }

            return dtClicks;
        }

        public static DataTable GetBouncesForGrid(int customerID, DateTime startDate, DateTime endDate)
        {
            DataTable dtBounces = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtBounces = ECN_Framework_DataLayer.Activity.View.BlastActivity.GetBouncesForGrid(customerID, startDate, endDate);
                scope.Complete();
            }

            return dtBounces;
        }

        public static DataTable GetSubscribesForGrid(int customerID, DateTime startDate, DateTime endDate)
        {
            DataTable dtSubscribes = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtSubscribes = ECN_Framework_DataLayer.Activity.View.BlastActivity.GetSubscribesForGrid(customerID, startDate, endDate);
                scope.Complete();
            }

            return dtSubscribes;
        }

        public static DataTable GetRevenueConversionForGrid(int blastID)
        {
            DataTable dtRevenueConversion = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtRevenueConversion = ECN_Framework_DataLayer.Activity.View.BlastActivity.GetRevenueConversionForGrid(blastID);
                scope.Complete();
            }

            return dtRevenueConversion;
        }

        public static Decimal GetRevenueConversionTotal(int blastID)
        {
            Decimal total = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                total = ECN_Framework_DataLayer.Activity.View.BlastActivity.GetRevenueConversionTotal(blastID);
                scope.Complete();
            }

            return total;
        }

        public static DataTable GetISPReport(int blastID, string ISPs)
        {
            DataTable dtISPReport = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtISPReport = ECN_Framework_DataLayer.Activity.View.BlastActivity.GetISPReport(blastID, ISPs);
                scope.Complete();
            }

            return dtISPReport;
        }

        public static DataTable GetDetailedClickReport(int blastID, string link)
        {
            DataTable dtISPReport = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtISPReport = ECN_Framework_DataLayer.Activity.View.BlastActivity.GetDetailedClickReport(blastID, link);
                scope.Complete();
            }

            return dtISPReport;
        }

        public static DataTable GetBlastGroupClicksData(int? blastID, int? blastGroupID, string howMuch, string isp, string reportType, string udfName, string udfData, string pageSize, string currentPage)
        {
            DataTable dtBlastGroupClicks = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtBlastGroupClicks = ECN_Framework_DataLayer.Activity.View.BlastActivity.GetBlastGroupClicksData(blastID, blastGroupID, howMuch, isp, reportType, udfName, udfData, pageSize, currentPage);
                scope.Complete();
            }

            return dtBlastGroupClicks;
        }

        public static DataTable GetRevenueConversionData(int blastID, string type)
        {
            DataTable dtRevenueConversionData = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtRevenueConversionData = ECN_Framework_DataLayer.Activity.View.BlastActivity.GetRevenueConversionData(blastID, type);
                scope.Complete();
            }

            return dtRevenueConversionData;
        }

        public static DataTable GetBlastReportData(int blastID, string UDFName, string UDFData)
        {
            DataTable dtBlastReportData = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtBlastReportData = ECN_Framework_DataLayer.Activity.View.BlastActivity.GetBlastReportData(blastID, UDFName, UDFData);
                scope.Complete();
            }

            return dtBlastReportData;
        }

        public static DataTable GetBlastReportDataByCampaignItemID(int CampaignItemID)
        {
            DataTable dtBlastReportData = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtBlastReportData = ECN_Framework_DataLayer.Activity.View.BlastActivity.GetBlastReportDataByCampaignItemID(CampaignItemID);
                scope.Complete();
            }

            return dtBlastReportData;
        }
        
         public static DataTable GetBlastMAReportDataByCampaignItemID(int CampaignItemID, string reportType)
        {
            DataTable dtBlastReportData = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtBlastReportData = ECN_Framework_DataLayer.Activity.View.BlastActivity.GetBlastMAReportDataByCampaignItemID(CampaignItemID, reportType);
                scope.Complete();
            }

            return dtBlastReportData;
        }
        public static DataTable FilterEmailsAllWithSmartSegment(int emailID, int blastID)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Activity.View.BlastActivity.FilterEmailsAllWithSmartSegment(emailID, blastID);
                scope.Complete();
            }
            return dt;
        }

        public static DataTable FilterEmailsAllWithSmartSegment(int groupID, int customerID, int filterID, string filter, int blastID, string blastID_and_BounceDomain, string actionType, int refBlastID, int? emailID)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Activity.View.BlastActivity.FilterEmailsAllWithSmartSegment(groupID, customerID, filterID, filter, blastID, blastID_and_BounceDomain, actionType, refBlastID, emailID);
                scope.Complete();
            }
            return dt;
        }

    }
}
