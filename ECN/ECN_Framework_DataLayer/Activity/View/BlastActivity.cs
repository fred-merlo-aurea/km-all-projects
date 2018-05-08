using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Activity.View
{
    [Serializable]
    public class BlastActivity
    {
        public static List<ECN_Framework_Entities.Activity.View.BlastActivity> GetByEmailID(int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetEmailActivityLogsByEmailID";
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return GetList(cmd);
        }
        public static DataTable ActivityLogs(int emailID, int currentPage, int pageSize)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_ActivityLogSearch";
            cmd.Parameters.AddWithValue("@EmailID",emailID);
            cmd.Parameters.Add(new SqlParameter("@currentPage", currentPage));
            cmd.Parameters.Add(new SqlParameter("@pageSize", pageSize));
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }
        private static List<ECN_Framework_Entities.Activity.View.BlastActivity> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Activity.View.BlastActivity> retList = new List<ECN_Framework_Entities.Activity.View.BlastActivity>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
                {
                    if (rdr != null)
                    {
                        ECN_Framework_Entities.Activity.View.BlastActivity retItem = new ECN_Framework_Entities.Activity.View.BlastActivity();
                        var builder = DynamicBuilder<ECN_Framework_Entities.Activity.View.BlastActivity>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            if (retItem != null)
                            {
                                retList.Add(retItem);
                            }
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
                cmd.Connection.Close();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "ECN_Framework_DataLayer.Activity.View.BlastActivity.GetList(SqlCommand cmd)", int.Parse(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
            }
            return retList;
        }

        public static DataTable ChampionByProc(int customerID, int sampleID, bool justWinningBlastID, string abWinnerType)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetSampleInfoForChampion";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@SampleID", sampleID);
            cmd.Parameters.AddWithValue("@JustWinningBlastID", justWinningBlastID);
            cmd.Parameters.AddWithValue("@ABWinnerType", abWinnerType);
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
                scope.Complete();
            }
            return dt;
        }

        public static DataTable GetABSampleCount(int blastA, int blastB)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_GetABSampleCounts";
            cmd.Parameters.AddWithValue("@BlastA", blastA);
            cmd.Parameters.AddWithValue("@BlastB", blastB);
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
                scope.Complete();
            }
            return dt;
        }

        //verification still pending

        public static DataTable GetBlastReport(int customerID, int blastID, string udfName, string udfData)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_getBlastReportData";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@UDFname", udfName);
            cmd.Parameters.AddWithValue("@UDFdata", udfData);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
                scope.Complete();
            }
            return dt;
        }

        public static DataTable DownloadCampaignItemReportDetails(int customerID, int campaignItemID, string ReportType, string FilterType, string ISP, string startDate, 
                                                                    string endDate, string ProfileFilter, bool OnlyUnique)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "spDownloadCampaignItemDetails";

            cmd.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int));
            cmd.Parameters["@CustomerID"].Value = customerID;

            cmd.Parameters.Add(new SqlParameter("@CampaignItemID", SqlDbType.Int));
            cmd.Parameters["@CampaignItemID"].Value = campaignItemID;

            cmd.Parameters.Add(new SqlParameter("@ReportType", SqlDbType.VarChar));
            cmd.Parameters["@ReportType"].Value = ReportType;

            cmd.Parameters.Add(new SqlParameter("@FilterType", SqlDbType.VarChar));
            cmd.Parameters["@FilterType"].Value = FilterType;

            cmd.Parameters.Add(new SqlParameter("@ISP", SqlDbType.VarChar));
            cmd.Parameters["@ISP"].Value = ISP;

            cmd.Parameters.AddWithValue("@ProfileFilter", ProfileFilter);
            
            cmd.Parameters.Add(new SqlParameter("@OnlyUnique", SqlDbType.Bit));
            cmd.Parameters["@OnlyUnique"].Value =  OnlyUnique;
            
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);
            }
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }
        public static DataTable DownloadBlastLinkDetails(int customerID, int campaignItemID,string startDate,
                                                                string endDate, string ProfileFilter, bool OnlyUnique,string linkAlias)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "spDownloadBlastLinkDetails";

            cmd.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int));
            cmd.Parameters["@CustomerID"].Value = customerID;

            cmd.Parameters.Add(new SqlParameter("@CampaignItemID", SqlDbType.Int));
            cmd.Parameters["@CampaignItemID"].Value = campaignItemID;

            cmd.Parameters.AddWithValue("@ProfileFilter", ProfileFilter);

            cmd.Parameters.Add(new SqlParameter("@OnlyUnique", SqlDbType.Bit));
            cmd.Parameters["@OnlyUnique"].Value = OnlyUnique;

            cmd.Parameters.Add(new SqlParameter("@LinkURL", SqlDbType.VarChar));
            cmd.Parameters["@LinkURL"].Value = linkAlias;
            
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);
            }
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }

        public static DataTable DownloadBlastReportDetails(int blastID, string ReportType, string FilterType, string ISP, string startDate, string endDate, 
                                                            string ProfileFilter, bool OnlyUnique)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "spDownloadBlastEmails";

            cmd.Parameters.Add(new SqlParameter("@blastID", SqlDbType.VarChar));
            cmd.Parameters["@blastID"].Value = blastID;

            cmd.Parameters.Add(new SqlParameter("@ReportType", SqlDbType.VarChar));
            cmd.Parameters["@ReportType"].Value = ReportType;

            cmd.Parameters.Add(new SqlParameter("@FilterType", SqlDbType.VarChar));
            cmd.Parameters["@FilterType"].Value = FilterType;

            cmd.Parameters.Add(new SqlParameter("@ISP", SqlDbType.VarChar));
            cmd.Parameters["@ISP"].Value = ISP;

            cmd.Parameters.AddWithValue("@ProfileFilter", ProfileFilter);

            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);
            }
            cmd.Parameters.Add(new SqlParameter("@OnlyUnique", OnlyUnique));
            cmd.Parameters["@OnlyUnique"].Value = OnlyUnique;
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }

        public static DataSet GetCampaignItemReportDetails(int campaignItemID, int customerID, string ReportType, string FilterType, string ISP, int PageNo, int PageSize, string UDFName, 
                                                            string UDFData, bool OnlyUnique)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetCampaignItemReportWithSuppressed";

            cmd.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int));
            cmd.Parameters["@CustomerID"].Value = customerID;

            cmd.Parameters.Add(new SqlParameter("@CampaignItemID", SqlDbType.Int));
            cmd.Parameters["@CampaignItemID"].Value = campaignItemID;

            cmd.Parameters.Add(new SqlParameter("@ReportType", SqlDbType.VarChar));
            cmd.Parameters["@ReportType"].Value = ReportType;

            cmd.Parameters.Add(new SqlParameter("@FilterType", SqlDbType.VarChar));
            cmd.Parameters["@FilterType"].Value = FilterType;

            cmd.Parameters.Add(new SqlParameter("@ISP", SqlDbType.VarChar));
            cmd.Parameters["@ISP"].Value = ISP;

            cmd.Parameters.Add(new SqlParameter("@PageNo", SqlDbType.Int));
            cmd.Parameters["@PageNo"].Value = PageNo;

            cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int));
            cmd.Parameters["@PageSize"].Value = PageSize;

            cmd.Parameters.Add(new SqlParameter("@UDFname", SqlDbType.VarChar));
            cmd.Parameters["@UDFname"].Value = UDFName;

            cmd.Parameters.Add(new SqlParameter("@UDFdata", SqlDbType.VarChar));
            cmd.Parameters["@UDFdata"].Value = UDFData;

            cmd.Parameters.Add(new SqlParameter("@OnlyUnique", SqlDbType.Bit));
            cmd.Parameters["@OnlyUnique"].Value = OnlyUnique;
            return DataFunctions.GetDataSet(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }

        public static DataSet GetBlastReportDetails(int blastID, int customerID, string ReportType, string FilterType, string ISP, int PageNo, int PageSize, string UDFName,
                                                    string UDFData, bool OnlyUnique)
        {
            SqlCommand cmd = new SqlCommand("spGetBlastGroupReportWithSuppressed");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int));
            cmd.Parameters["@ID"].Value = blastID;

            cmd.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int));
            cmd.Parameters["@CustomerID"].Value = customerID;

            cmd.Parameters.Add(new SqlParameter("@IsBlastGroup", SqlDbType.VarChar));
            cmd.Parameters["@IsBlastGroup"].Value = "N";

            cmd.Parameters.Add(new SqlParameter("@ReportType", SqlDbType.VarChar));
            cmd.Parameters["@ReportType"].Value = ReportType;

            cmd.Parameters.Add(new SqlParameter("@FilterType", SqlDbType.VarChar));
            cmd.Parameters["@FilterType"].Value = FilterType;

            cmd.Parameters.Add(new SqlParameter("@ISP", SqlDbType.VarChar));
            cmd.Parameters["@ISP"].Value = ISP;

            cmd.Parameters.Add(new SqlParameter("@PageNo", SqlDbType.Int));
            cmd.Parameters["@PageNo"].Value = PageNo;

            cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int));
            cmd.Parameters["@PageSize"].Value = PageSize;

            cmd.Parameters.Add(new SqlParameter("@UDFname", SqlDbType.VarChar));
            cmd.Parameters["@UDFname"].Value = UDFName;

            cmd.Parameters.Add(new SqlParameter("@UDFdata", SqlDbType.VarChar));
            cmd.Parameters["@UDFdata"].Value = UDFData;

            cmd.Parameters.Add(new SqlParameter("@OnlyUnique", SqlDbType.Bit));
            cmd.Parameters["@OnlyUnique"].Value = OnlyUnique;

            return DataFunctions.GetDataSet(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }

        public static DataTable GetBlastEmails(int blastID, string reportType, string filterType)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spDownloadBlastEmails";
            cmd.Parameters.Add("@blastID", SqlDbType.Int).Value = blastID;
            cmd.Parameters.Add(new SqlParameter("@ReportType", SqlDbType.VarChar)).Value = reportType;
            cmd.Parameters.Add(new SqlParameter("@FilterType", SqlDbType.VarChar)).Value = filterType;
            cmd.Parameters.Add(new SqlParameter("@ISP", SqlDbType.VarChar)).Value = string.Empty;
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }

        //to do
        public static DataTable GetClicksForGrid(int customerID, DateTime startDate, DateTime endDate)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            string sqlQuery =
                " SELECT TOP 10 bacl.EMailID, e.EmailAddress, bacl.BlastID, b.EmailSubject, bacl.ClickTime as 'ActionDate', bacl.URL as 'ActionValue' " +
                " FROM BlastActivityClicks bacl JOIN ecn5_communicator..Emails e ON bacl.EmailID = e.EmailID JOIN ecn5_communicator..Blast b " +
                " ON bacl.BlastID = b.BlastID WHERE e.CustomerID = " + customerID + " AND bacl.ClickTime >= '" + startDate.ToString() + "' AND bacl.ClickTime <= '" + endDate.ToString() + "' ORDER BY bacl.ClickTime DESC ";
            cmd.CommandText = sqlQuery;
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }

        public static DataTable GetBouncesForGrid(int customerID, DateTime startDate, DateTime endDate)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            string sqlQuery =
                " SELECT TOP 10 babo.EMailID, e.EmailAddress, babo.BlastID, b.EmailSubject, babo.BounceTime as ActionDate, bc.BounceCode as ActionValue " +
                " FROM BlastActivityBounces babo JOIN ecn5_communicator..Emails e ON babo.EMailID = e.EMailID JOIN " +
                " ecn5_communicator..Blast b ON babo.BlastID = b.BlastID " +
                " join BounceCodes bc on bc.BounceCodeID = babo.BounceCodeID " +
                " WHERE e.CustomerID = " + customerID + " AND babo.BounceTime >= '" + startDate.ToString() + "' AND babo.BounceTime <= ' " + endDate.ToString() + "' ORDER BY ActionDate DESC ";
            cmd.CommandText = sqlQuery;
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }

        public static DataTable GetSubscribesForGrid(int customerID, DateTime startDate, DateTime endDate)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            string sqlQuery =
                " SELECT TOP 10 baus.EMailID, e.EmailAddress, baus.BlastID, b.EmailSubject, baus.UnsubscribeTime as 'ActionDate', usc.UnsubscribeCode as ActionValue " +
                " FROM BlastActivityUnSubscribes baus JOIN ecn5_communicator..Emails e ON baus.EMailID = e.EMailID " +
                " JOIN ecn5_communicator..Blast b ON baus.BlastID = b.BlastID " +
                " JOIN UnsubscribeCodes usc on usc.UnsubscribeCodeID = baus.UnsubscribeCodeID " +
                " WHERE e.CustomerID =  " + customerID +
                " AND baus.UnsubscribeTime >= '" + startDate.ToString() + "' AND baus.UnsubscribeTime <= '" + endDate.ToString() + "'" +
                " ORDER BY ActionDate DESC ";
            cmd.CommandText = sqlQuery;
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }

        public static DataTable GetRevenueConversionForGrid(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetRevenueConversionData";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
            cmd.Parameters["@blastID"].Value = blastID;
            cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar, 400));
            cmd.Parameters["@type"].Value = "detailed";
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }

        public static Decimal GetRevenueConversionTotal(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetRevenueConversionData";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
            cmd.Parameters["@blastID"].Value = blastID;
            cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar, 400));
            cmd.Parameters["@type"].Value = "simple";
            return Convert.ToDecimal(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Activity.ToString().ToString()));
        }

        public static DataTable GetISPReport(int blastID, string ISPs)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetISPReportingData";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@blastID", SqlDbType.Int));
            cmd.Parameters["@blastID"].Value = blastID;
            cmd.Parameters.Add(new SqlParameter("@ISPs", SqlDbType.VarChar, 400));
            cmd.Parameters["@ISPs"].Value = ISPs;
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }

        public static DataTable GetDetailedClickReport(int blastID, string link)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spClickActivity_DetailedReport";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
            cmd.Parameters["@BlastID"].Value = blastID;
            cmd.Parameters.Add(new SqlParameter("@LinkURL", SqlDbType.VarChar));
            cmd.Parameters["@LinkURL"].Value = link;
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }

        public static DataTable GetRevenueConversionData(int blastID, string type)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetRevenueConversionData";
            cmd.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
            cmd.Parameters["@BlastID"].Value = blastID;
            cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar));
            cmd.Parameters["@Type"].Value = type;
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }

        public static DataTable GetBlastReportDataByCampaignItemID(int CampaignItemID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetCampaignItemReportData";
            cmd.Parameters.Add(new SqlParameter("@CampaignItemID", SqlDbType.Int));
            cmd.Parameters["@CampaignItemID"].Value = CampaignItemID;
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }
        public static DataTable GetBlastMAReportDataByCampaignItemID(int CampaignItemID, string reportType)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetCampaignItemReportDataforMA";
            cmd.Parameters.Add(new SqlParameter("@CampaignItemID", SqlDbType.Int));
            cmd.Parameters["@CampaignItemID"].Value = CampaignItemID;
            cmd.Parameters.AddWithValue("@ReportType", reportType);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }

        public static DataTable GetBlastReportData(int blastID, string UDFName, string UDFData)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetBlastReportData";
            cmd.Parameters.Add(new SqlParameter("@blastID", SqlDbType.Int));
            cmd.Parameters["@blastID"].Value = blastID;
            cmd.Parameters.Add(new SqlParameter("@UDFname", SqlDbType.VarChar));
            cmd.Parameters["@UDFname"].Value = UDFName;
            cmd.Parameters.Add(new SqlParameter("@UDFdata", SqlDbType.VarChar));
            cmd.Parameters["@UDFdata"].Value = UDFData;
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }

        public static DataTable GetBlastGroupClicksData(int? blastID, int? blastGroupID, string howMuch, string isp, string reportType, string udfName, string udfData, string pageSize, string currentPage)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetBlastGroupClicksData";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@IsBlastGroup", SqlDbType.VarChar));
            if (blastID != null)
            {
                cmd.Parameters["@ID"].Value = blastID.Value.ToString();
                cmd.Parameters["@IsBlastGroup"].Value = "N";
            }
            else
            {
                cmd.Parameters["@ID"].Value = blastGroupID.Value.ToString();
                cmd.Parameters["@IsBlastGroup"].Value = "Y";
            }
            cmd.Parameters.Add(new SqlParameter("@HowMuch", SqlDbType.VarChar));
            cmd.Parameters["@HowMuch"].Value = howMuch;
            cmd.Parameters.Add(new SqlParameter("@ISP", SqlDbType.VarChar));
            cmd.Parameters["@ISP"].Value = isp;
            cmd.Parameters.Add(new SqlParameter("@ReportType", SqlDbType.VarChar));
            cmd.Parameters["@ReportType"].Value = reportType;
            cmd.Parameters.Add(new SqlParameter("@topClickURL", SqlDbType.VarChar));
            cmd.Parameters["@topClickURL"].Value = "";
            cmd.Parameters.Add(new SqlParameter("@PageNo", SqlDbType.VarChar));
            cmd.Parameters["@PageNo"].Value = currentPage;
            cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.VarChar));
            cmd.Parameters["@PageSize"].Value = pageSize;
            cmd.Parameters.Add(new SqlParameter("@UDFname", SqlDbType.VarChar));
            cmd.Parameters["@UDFname"].Value = udfName;
            cmd.Parameters.Add(new SqlParameter("@UDFdata", SqlDbType.VarChar));
            cmd.Parameters["@UDFdata"].Value = udfData;
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }

        public static DataTable FilterEmailsAllWithSmartSegment(int emailID, int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_FilterEmails_ALL_with_smartSegment_ByBlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }

        public static DataTable FilterEmailsAllWithSmartSegment(int groupID, int customerID, int filterID, string filter, int blastID, string blastID_and_BounceDomain, string actionType, int refBlastID, int? emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_FilterEmails_ALL_with_smartSegment";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            cmd.Parameters.AddWithValue("@Filter", filter);
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@BlastID_and_BounceDomain", blastID_and_BounceDomain);
            cmd.Parameters.AddWithValue("@ActionType", actionType);
            cmd.Parameters.AddWithValue("@refBlastID", refBlastID);
            if (emailID != null)
            {
                cmd.Parameters.AddWithValue("@EmailID", emailID);
            }
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }

    }
}
