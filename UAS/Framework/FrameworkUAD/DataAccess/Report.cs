using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM.Common;
using KMPlatform.Object;

using UASDataFunctions = FrameworkUAS.DataAccess.DataFunctions;

namespace FrameworkUAD.DataAccess
{
    [Serializable]
    public class Report
    {
        public static readonly string SpSubFieldsMvc = "rpt_SubFieldsMVC";
        public static readonly string SpResponseMvc = "rpt_Single_ResponseMVC";
        public static readonly string SpCrossTabMvc = "rpt_CrossTabMVC";
        public static readonly string SpPubSubscriptionsSelectAllFieldsWithFilterMvc = "e_PubSubscriptions_Select_All_Fields_With_FilterMVC";
        public static readonly string SpGeoBreakdownDomesticMvc = "rpt_Geo_Breakdown_DomesticMVC";
        public static readonly string SpGeoBreakdownDomestic = "rpt_Geo_Breakdown_Domestic";
        public static readonly string SpAddRemove = "rpt_Add_Remove";
        public static readonly string SpQualificationBreakdown = "rpt_QualificationBreakdown";
        public static readonly string SpGetSubscriptionIdsFromFilterUad = "rpt_Get_SubscriptionIDs_From_Filter_UAD";
        public static readonly string SpGetSubscriptionIdsCopiesFromFilter = "rpt_GetSubscriptionIDs_Copies_From_Filter";
        public static readonly string SpViewResponseTotals = "rpt_ViewResponseTotals";

        public static readonly string ParamReportName = "@ReportName";
        public static readonly string ParamIsActive = "@IsActive";
        public static readonly string ParamDateCreated = "@DateCreated";
        public static readonly string ParamDateUpdated = "@DateUpdated";
        public static readonly string ParamreatedByUserId = "@CreatedByUserID";
        public static readonly string ParamUpdatedByUserId = "@UpdatedByUserID";
        public static readonly string ParamProvideId = "@ProvideID";
        public static readonly string ParamUrl = "@URL";
        public static readonly string ParamIsCrossTabReport = "@IsCrossTabReport";
        public static readonly string ParamColumn = "@Column";
        public static readonly string ParamSuppressTotal = "@SuppressTotal";
        public static readonly string ParamStatus = "@Status";
        public static readonly string ParamReportTypeId = "@ReportTypeID";
        public static readonly string ParamFilters = "@Filters";
        public static readonly string ParamAdHocFilters = "@AdHocFilters";
        public static readonly string ParamPubId = "@PubID";
        public static readonly string ParamDemo = "@Demo";
        public static readonly string ParamRow = "@Row";
        public static readonly string ParamCol = "@Col";
        public static readonly string ParamIncludeReportGroup = "@IncludeReportGroup";
        public static readonly string ParamIncludeAddRemove = "@IncludeAddRemove";
        public static readonly string ParamCountryId = "@CountryID";
        public static readonly string ParamQueries = "@Queries";
        public static readonly string SpGeoSingleCountryMvc = "rpt_Geo_SingleCountryMVC";
        public static readonly string ParamRegions = "@Regions";
        public static readonly string ParamIncludeAllStates = "@includeAllStates";
        public static readonly string ParamCategoryIDs = "@CategoryIDs";
        public static readonly string ParamTransactionIDs = "@TransactionIDs";
        public static readonly string ParamQsourceIDs = "@QsourceIDs";
        public static readonly string ParamStateIds = "@StateIDs";
        public static readonly string ParamCountryIDs = "@CountryIDs";
        public static readonly string ParamEmail = "@Email";
        public static readonly string ParamPhone = "@Phone";
        public static readonly string ParamFax = "@Fax";
        public static readonly string ParamCategoryCodes = "@CategoryCodes";
        public static readonly string ParamResponseIds = "@ResponseIDs";
        public static readonly string ParamDemo7 = "@Demo7";
        public static readonly string ParamAdHocXml = "@AdHocXML";
        public static readonly string ParamMobile = "@Mobile";
        public static readonly string ParamYear = "@Year";
        public static readonly string ParamStartDate = "@startDate";
        public static readonly string ParamEndDate = "@endDate";
        public static readonly string ParamTransactionCodes = "@TransactionCodes";
        public static readonly string ParamPubIds = "@PubIDs";
        public static readonly string ParamDemo31 = "@Demo31";
        public static readonly string ParamDemo32 = "@Demo32";
        public static readonly string ParamDemo33 = "@Demo33";
        public static readonly string ParamDemo34 = "@Demo34";
        public static readonly string ParamDemo35 = "@Demo35";
        public static readonly string ParamDemo36 = "@Demo36";
        public static readonly string ParamUadResponseIds = "@UADResponseIDs";
        public static readonly string ParamIsMailable = "@IsMailable";
        public static readonly string ParamEmailStatusIds = "@EmailStatusIDs";
        public static readonly string ParamOpenSearchType = "@OpenSearchType";
        public static readonly string ParamOpenCount = "@OpenCount";
        public static readonly string ParamOpenDateFrom = "@OpenDateFrom";
        public static readonly string ParamOpenDateTo = "@OpenDateTo";
        public static readonly string ParamOpenBlastId = "@OpenBlastID";
        public static readonly string ParamOpenEmailSubject = "@OpenEmailSubject";
        public static readonly string ParamOpenEmailFromDate = "@OpenEmailFromDate";
        public static readonly string ParamOpenEmailToDate = "@OpenEmailToDate";
        public static readonly string ParamClickSearchType = "@ClickSearchType";
        public static readonly string ParamClickCount = "@ClickCount";
        public static readonly string ParamClickUrl = "@ClickURL";
        public static readonly string ParamClickDateFrom = "@ClickDateFrom";
        public static readonly string ParamClickDateTo = "@ClickDateTo";
        public static readonly string ParamClickBlastId = "@ClickBlastID";
        public static readonly string ParamClickEmailSubject = "@ClickEmailSubject";
        public static readonly string ParamClickEmailFromDate = "@ClickEmailFromDate";
        public static readonly string ParamClickEmailToDate = "@ClickEmailToDate";
        public static readonly string ParamDomain = "@Domain";
        public static readonly string ParamVisitsUrl = "@VisitsURL";
        public static readonly string ParamVisitsDateFrom = "@VisitsDateFrom";
        public static readonly string ParamVisitsDateTo = "@VisitsDateTo";
        public static readonly string ParamBrandId = "@BrandID";
        public static readonly string ParamSearchType = "@SearchType";
        public static readonly string ParamRangeMaxLatMin = "@RangeMaxLatMin";
        public static readonly string ParamRangeMaxLatMax = "@RangeMaxLatMax";
        public static readonly string ParamRangeMaxLonMin = "@RangeMaxLonMin";
        public static readonly string ParamRangeMaxLonMax = "@RangeMaxLonMax";
        public static readonly string ParamRangeMinLatMin = "@RangeMinLatMin";
        public static readonly string ParamRangeMinLatMax = "@RangeMinLatMax";
        public static readonly string ParamRangeMinLonMin = "@RangeMinLonMin";
        public static readonly string ParamRangeMinLonMax = "@RangeMinLonMax";
        public static readonly string ParamProductId = "@ProductID";
        public static readonly string ParamIssueId = "@IssueID";
        public static readonly string ParamWaveMail = "@WaveMail";
        public static readonly string ParamReportId = "@ReportID";
        public static readonly string ParamRowId = "@RowID";
        public static readonly string ParamPrintColumns = "@PrintColumns";
        public static readonly string ParamDownload = "@Download";
        public static readonly string ParamYears = "@years";
        public static readonly string ParamPublicationId = "@PublicationID";

        public static List<Entity.Report> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Report> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Reports_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Report> SelectForAddRemoves(KMPlatform.Object.ClientConnections client, int pubID)
        {
            List<Entity.Report> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Reports_Select_For_AddRemove";
            cmd.Parameters.Add(new SqlParameter(ParamPubId, pubID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.Report Get(SqlCommand cmd)
        {
            Entity.Report retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Report();
                        DynamicBuilder<Entity.Report> builder = DynamicBuilder<Entity.Report>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retItem;
        }
        private static List<Entity.Report> GetList(SqlCommand cmd)
        {
            List<Entity.Report> retList = new List<Entity.Report>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.Report retItem = new Entity.Report();
                        DynamicBuilder<Entity.Report> builder = DynamicBuilder<Entity.Report>.CreateBuilder(rdr);
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
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }
        private static Object.Counts GetCounts(SqlCommand cmd)
        {
            Object.Counts retItem = new Object.Counts();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Object.Counts();
                        DynamicBuilder<Object.Counts> builder = DynamicBuilder<Object.Counts>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retItem;
        }
        public static int Save(KMPlatform.Object.ClientConnections client, Entity.Report x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Reports_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter(ParamReportId, x.ReportID));
            cmd.Parameters.Add(new SqlParameter(ParamReportName, x.ReportName));
            cmd.Parameters.Add(new SqlParameter(ParamIsActive, x.IsActive));
            cmd.Parameters.Add(new SqlParameter(ParamDateCreated, x.DateCreated));
            cmd.Parameters.Add(new SqlParameter(ParamDateUpdated, (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter(ParamreatedByUserId, x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter(ParamUpdatedByUserId, (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter(ParamProvideId, x.IsActive));
            cmd.Parameters.Add(new SqlParameter(ParamProductId, (object)x.ProductID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter(ParamUrl, (object)x.URL ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter(ParamIsCrossTabReport, (object)x.IsCrossTabReport ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter(ParamRow, (object)x.Row ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter(ParamColumn, (object)x.Column ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter(ParamSuppressTotal, (object)x.SuppressTotal ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter(ParamStatus, (object)x.Status ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter(ParamReportTypeId, (object)x.ReportTypeID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        #region BPA
        public static DataTable SelectBPA(KMPlatform.Object.ClientConnections client, Object.Reporting r, string printColumns, bool download)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "";

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        #endregion

        #region CategorySummary
        public static DataTable SelectCategorySummary(KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID)
        {
            DataTable retItem = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "rpt_CategorySummary";
            cmd.Parameters.AddWithValue(ParamFilters, filters);
            cmd.Parameters.AddWithValue(ParamAdHocFilters, adHocFilters);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }

        public static List<Object.QualificationBreakdownReport> SelectQualificationBreakDown(KMPlatform.Object.ClientConnections client, Object.Reporting r, string printColumns, bool download, int years)
        {
            var connection = DataFunctions.GetClientSqlConnection(client);
            var cmd = CreateCommandAndFillBase(connection, r);

            cmd.Parameters.AddWithValue(ParamProductId, r.PublicationIDs);
            cmd.Parameters.AddWithValue(ParamRegions, r.Regions);
            cmd.Parameters.AddWithValue(ParamPrintColumns, printColumns);
            cmd.Parameters.AddWithValue(ParamDownload, download);
            cmd.Parameters.AddWithValue(ParamYears, r.Year);

            cmd.CommandText = SpQualificationBreakdown;

            return GetQualificationList(cmd);
        }

        private static List<Object.CategorySummaryReport> GetCategoryList(SqlCommand cmd)
        {
            List<Object.CategorySummaryReport> retList = new List<Object.CategorySummaryReport>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
            {
                if (rdr != null)
                {
                    Object.CategorySummaryReport retItem = new Object.CategorySummaryReport();
                    DynamicBuilder<Object.CategorySummaryReport> builder = DynamicBuilder<Object.CategorySummaryReport>.CreateBuilder(rdr);
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
            return retList;
        }

        internal static DataTable SelectDemoSubReportMVC(ClientConnections client, int productID, string row, bool includeAddRemove, string filterQuery, int issueID)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "rpt_Demo_SubReportMVC";
            cmd.Parameters.AddWithValue(ParamProductId, productID);
            cmd.Parameters.AddWithValue(ParamRow, row);
            cmd.Parameters.AddWithValue(ParamIncludeAddRemove, includeAddRemove);
            cmd.Parameters.AddWithValue(ParamQueries, filterQuery);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);
            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }

        internal static DataTable SelectCategorySummaryMVC(ClientConnections clientConnections, string filterQuery, int issueID)
        {
            DataTable retItem = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientConnections);
            cmd.CommandText = "rpt_CategorySummaryMVC";
            cmd.Parameters.AddWithValue(ParamQueries, filterQuery);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }

        private static List<Object.QualificationBreakdownReport> GetQualificationList(SqlCommand cmd)
        {
            List<Object.QualificationBreakdownReport> retList = new List<Object.QualificationBreakdownReport>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
            {
                if (rdr != null)
                {
                    Object.QualificationBreakdownReport retItem = new Object.QualificationBreakdownReport();
                    DynamicBuilder<Object.QualificationBreakdownReport> builder = DynamicBuilder<Object.QualificationBreakdownReport>.CreateBuilder(rdr);
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
            return retList;
        }

        internal static DataTable SelectDemoXQualificationMVC(ClientConnections clientConnections, int productID, string row, string filterQury, int issueID, bool includeReportGroups)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientConnections);
            cmd.CommandText = "rpt_DemoXQualificationMVC";
            cmd.Parameters.AddWithValue(ParamQueries, filterQury);
            cmd.Parameters.AddWithValue(ParamProductId, productID);
            cmd.Parameters.AddWithValue(ParamRow, row);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);
            cmd.Parameters.AddWithValue(ParamIncludeReportGroup, includeReportGroups);
            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }

        internal static DataTable SelectGeoBreakdownInternationalMVC(ClientConnections clientConnections, string filtersQuery, int issueID,int productid, bool IncludeCustomRegion)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientConnections);
            cmd.CommandText = "rpt_Geo_BreakDown_InternationalMVC";
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);
            cmd.Parameters.AddWithValue(ParamQueries, filtersQuery);
            cmd.Parameters.AddWithValue("@IncludeCustomRegion", IncludeCustomRegion);
            cmd.Parameters.AddWithValue(ParamPubId, productid);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }

        internal static DataTable SelectPar3cMVC(ClientConnections clientConnections, string filterQuery, int issueID)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientConnections);
            cmd.CommandText = "rpt_PAR3CMVC";
            cmd.Parameters.AddWithValue(ParamQueries, filterQuery);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }

        internal static DataTable SelectQSourceBreakdownMVC(ClientConnections clientConnections, int productID, bool includeAddRemove, string filterQuery, int issueID)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientConnections);
            cmd.CommandText = "rpt_QualificationBreakdownMVC";
            cmd.Parameters.AddWithValue(ParamProductId, productID);
            cmd.Parameters.AddWithValue(ParamIncludeAddRemove, includeAddRemove);
            cmd.Parameters.AddWithValue(ParamQueries, filterQuery);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }

        internal static DataTable GetSubscriberResponseDetailsMVC(ClientConnections clientConnections, string filterQuery, int issueID)
        {
            DataTable retItem = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientConnections);
            cmd.CommandText = "e_PubSubscriptions_Select_ResponsesMVC";
            cmd.Parameters.AddWithValue(ParamQueries, filterQuery);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }

        internal static DataTable SelectSubsrcMVC(ClientConnections clientConnections, string filterQuery, bool includeAddRemove, int issueID)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientConnections);
            cmd.CommandText = "rpt_SubSourceSummaryMVC";
            cmd.Parameters.AddWithValue(ParamQueries, filterQuery);
            cmd.Parameters.AddWithValue(ParamIncludeAddRemove, includeAddRemove);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }

        internal static DataTable GetSubscriberPaidDetailsMVC(ClientConnections clientConnections, string filterQuery)
        {
            DataTable retItem = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientConnections);
            cmd.CommandText = "e_PubSubscriptions_Select_Paid_Info";
            cmd.Parameters.AddWithValue(ParamQueries, filterQuery);
            //cmd.Parameters.AddWithValue("@AdHocFilters", adHocFilters);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }

        internal static DataTable GetSubscriberDetailsMVC(ClientConnections client, string filterQuery, int issueID)
        {
            DataTable retItem = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "e_PubSubscriptions_Select_Fields_With_FilterMVC";
            cmd.Parameters.AddWithValue(ParamQueries, filterQuery);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }

        internal static DataTable SelectSubFieldsMVC(ClientConnections clientConnections, string filterQuery, string demo, int issueID)
        {
            var connection = DataFunctions.GetClientSqlConnection(clientConnections);
            var cmd = CreateAndFillDemoCommand(connection, filterQuery, demo, issueID);
            cmd.CommandText = SpSubFieldsMvc;

            var retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }

        internal static DataTable SelectSingleResponseMVC(ClientConnections clientConnections, int productID, string row, bool includeReportGroups, string filterQuery, int issueID)
        {
            var connection = DataFunctions.GetClientSqlConnection(clientConnections);
            var cmd = CreateAndFillDemoCommand(connection, filterQuery, row, issueID);
            cmd.Parameters.AddWithValue(ParamIncludeReportGroup, includeReportGroups);
            cmd.Parameters.AddWithValue(ParamPubId, productID);
            cmd.CommandText = SpResponseMvc;

            var retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }

        internal static DataTable SelectGeoBreakdown_Single_CountryMVC(ClientConnections clientConnections, string filterQuery, int issueID, int countryID)
        {
            var connection = DataFunctions.GetClientSqlConnection(clientConnections);
            var cmd = CreateAndFillIssueCommand(connection, filterQuery, issueID);
            cmd.Parameters.AddWithValue(ParamCountryId, countryID);

            cmd.CommandText = SpGeoSingleCountryMvc;

            var retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }

        internal static DataTable SelectGeoBreakdown_DomesticMVC(ClientConnections clientConnections, string filterQuery, int issueID, bool includeAddRemove)
        {
            var connection = DataFunctions.GetClientSqlConnection(clientConnections);
            var cmd = CreateAndFillIssueCommand(connection, filterQuery, issueID);
            cmd.Parameters.AddWithValue(ParamIncludeAddRemove, includeAddRemove);

            cmd.CommandText = SpGeoBreakdownDomesticMvc;

            var retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }

        internal static DataTable GetFullSubscriberDetailsMVC(ClientConnections clientConnections, string filterQuery, int issueID)
        {
            var connection = DataFunctions.GetClientSqlConnection(clientConnections);
            var cmd = CreateAndFillIssueCommand(connection, filterQuery, issueID);

            cmd.CommandText = SpPubSubscriptionsSelectAllFieldsWithFilterMvc;

            var retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;

        }

        internal static DataTable SelectCrossTabMVC(ClientConnections clientConnections, int productID, string row, string col, bool includeAddRemove, string filterQuery, int issueID, bool includeReportGroups)
        {
            var connection = DataFunctions.GetClientSqlConnection(clientConnections);
            var cmd = CreateAndFillIssueCommand(connection, filterQuery, issueID);

            cmd.CommandText = SpCrossTabMvc;

            cmd.Parameters.AddWithValue(ParamProductId, productID);
            cmd.Parameters.AddWithValue(ParamRow, row);
            cmd.Parameters.AddWithValue(ParamCol, col);
            cmd.Parameters.AddWithValue(ParamIncludeAddRemove, includeAddRemove);
            cmd.Parameters.AddWithValue(ParamIncludeReportGroup, includeReportGroups);

            var retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }

        private static List<int> GetIntList(SqlCommand cmd)
        {
            List<int> retList = new List<int>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
            {
                if (rdr != null)
                {
                    while (rdr.Read())
                    {
                        retList.Add(rdr.GetInt32(0));
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }
        #endregion

        #region CrossTab
        public static DataTable SelectCrossTab(KMPlatform.Object.ClientConnections client, int productID, string row, string col, bool includeAddRemove, string filters, string adHocFilters, int issueID,
            bool includeReportGroup)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "rpt_CrossTab";
            cmd.Parameters.AddWithValue(ParamProductId, productID);
            cmd.Parameters.AddWithValue(ParamRow, row);
            cmd.Parameters.AddWithValue(ParamCol, col);
            cmd.Parameters.AddWithValue(ParamIncludeAddRemove, includeAddRemove);
            cmd.Parameters.AddWithValue(ParamFilters, filters);
            cmd.Parameters.AddWithValue(ParamAdHocFilters, adHocFilters);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);
            cmd.Parameters.AddWithValue(ParamIncludeReportGroup, includeReportGroup);


            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        public static DataTable SelectDemoSubReport(KMPlatform.Object.ClientConnections client, int productID, string row, bool includeAddRemove, string filters, string adHocFilters, int issueID)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "rpt_Demo_SubReport";
            cmd.Parameters.AddWithValue(ParamProductId, productID);
            cmd.Parameters.AddWithValue(ParamRow, row);
            cmd.Parameters.AddWithValue(ParamIncludeAddRemove, includeAddRemove);
            cmd.Parameters.AddWithValue(ParamFilters, filters);
            cmd.Parameters.AddWithValue(ParamAdHocFilters, adHocFilters);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);


            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        public static DataTable GetResponses(KMPlatform.Object.ClientConnections client, int productID)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "o_GetDemosAndProfileFields";
            cmd.Parameters.AddWithValue(ParamProductId, productID);


            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        public static DataTable GetProfileFields(KMPlatform.Object.ClientConnections client)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "o_GetProfileFields";

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        public static DataTable SelectSingleResponse(KMPlatform.Object.ClientConnections client, int productID, string row, bool includeReportGroups, string filters, string adHocFilters, int issueID)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "rpt_Single_Response";
            cmd.Parameters.AddWithValue(ParamPubId, productID);
            cmd.Parameters.AddWithValue(ParamDemo, row);
            cmd.Parameters.AddWithValue(ParamIncludeReportGroup, includeReportGroups);
            cmd.Parameters.AddWithValue(ParamFilters, filters);
            cmd.Parameters.AddWithValue(ParamAdHocFilters, adHocFilters);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        #endregion

        #region DemoXQualification
        public static DataTable SelectDemoXQualification(KMPlatform.Object.ClientConnections client, int productid, string row, string filters, string adHocFilters, int issueID, bool includeReportGroups)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "rpt_DemoXQualification";
            cmd.Parameters.AddWithValue(ParamProductId, productid);
            cmd.Parameters.AddWithValue(ParamRow, row);
            cmd.Parameters.AddWithValue(ParamFilters, filters);
            cmd.Parameters.AddWithValue(ParamAdHocFilters, adHocFilters);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);
            cmd.Parameters.AddWithValue(ParamIncludeReportGroup, includeReportGroups);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        public static DataTable GetIssueDates(KMPlatform.Object.ClientConnections client, int productid)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "o_GetIssueDates";
            cmd.Parameters.AddWithValue(ParamProductId, productid);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        #endregion

        #region GeoBreakdown
        public static DataTable SelectGeoBreakdownInternational(KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "rpt_Geo_BreakDown_International";
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);
            cmd.Parameters.AddWithValue(ParamFilters, filters);
            cmd.Parameters.AddWithValue(ParamAdHocFilters, adHocFilters);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        public static DataTable SelectGeoBreakdown_Single_Country(KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID, int countryID)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "rpt_Geo_SingleCountry";
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);
            cmd.Parameters.AddWithValue(ParamFilters, filters);
            cmd.Parameters.AddWithValue(ParamAdHocFilters, adHocFilters);
            cmd.Parameters.AddWithValue(ParamCountryId, countryID);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        public static DataTable SelectGeoBreakdown_Domestic(KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID, bool includeAddRemoves)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = SpGeoBreakdownDomestic;
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);
            cmd.Parameters.AddWithValue(ParamFilters, filters);
            cmd.Parameters.AddWithValue(ParamAdHocFilters, adHocFilters);
            cmd.Parameters.AddWithValue(ParamIncludeAddRemove, includeAddRemoves);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        public static DataTable Get_Countries(KMPlatform.Object.ClientConnections client)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "e_Country_Select_For_Report";

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        #endregion

        #region GeoBreakdown_FCI
        public static DataTable SelectGeoBreakdown_FCI(KMPlatform.Object.ClientConnections client, Object.Reporting r, string printColumns, bool download)
        {
            var connection = DataFunctions.GetClientSqlConnection(client);
            var cmd = CreateCommandAndFillBase(connection, r);

            cmd.Parameters.AddWithValue(ParamProductId, r.PublicationIDs);
            cmd.Parameters.AddWithValue(ParamRegions, r.Regions);
            cmd.Parameters.AddWithValue(ParamIncludeAllStates, false);
            cmd.Parameters.AddWithValue(ParamWaveMail, r.WaveMail);

            cmd.CommandText = SpGeoBreakdownDomestic;

            var retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        #endregion

        #region ListReport
        public static DataTable SelectListReport(KMPlatform.Object.ClientConnections client, Object.Reporting r, int reportID, string rowID, string printColumns, bool download)
        {
            var connection = DataFunctions.GetClientSqlConnection(client);
            var cmd = CreateCommandAndFillBase(connection, r);

            cmd.Parameters.AddWithValue(ParamProductId, r.PublicationIDs);
            cmd.Parameters.AddWithValue(ParamRegions, r.Regions);
            cmd.Parameters.AddWithValue(ParamReportId, reportID);
            cmd.Parameters.AddWithValue(ParamRowId, rowID);
            cmd.Parameters.AddWithValue(ParamWaveMail, r.WaveMail);

            cmd.CommandText = SpViewResponseTotals;

            var retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        #endregion

        #region Par3c
        public static DataTable SelectPar3c(KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "rpt_PAR3C";
            cmd.Parameters.AddWithValue(ParamFilters, filters);
            cmd.Parameters.AddWithValue(ParamAdHocFilters, adHocFilters);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        #endregion

        #region QSourceBreakdown
        public static DataTable SelectQSourceBreakdown(KMPlatform.Object.ClientConnections client, int productID, bool includeAddRemove, string filters, string adHocFilters, int issueID)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = SpQualificationBreakdown;
            cmd.Parameters.AddWithValue(ParamProductId, productID);
            cmd.Parameters.AddWithValue(ParamIncludeAddRemove, includeAddRemove);
            cmd.Parameters.AddWithValue(ParamFilters, filters);
            cmd.Parameters.AddWithValue(ParamAdHocFilters, adHocFilters);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        #endregion

        #region SubFields
        public static DataTable SelectSubFields(KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, string demo, int issueID)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "rpt_SubFields";
            cmd.Parameters.AddWithValue(ParamFilters, filters);
            cmd.Parameters.AddWithValue(ParamAdHocFilters, adHocFilters);
            cmd.Parameters.AddWithValue(ParamDemo, demo);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);


            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        #endregion

        #region Subsrc
        public static DataTable SelectSubsrc(KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, bool includeAddRemoves, int issueID)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "rpt_SubSourceSummary";
            cmd.Parameters.AddWithValue(ParamFilters, filters);
            cmd.Parameters.AddWithValue(ParamAdHocFilters, adHocFilters);
            cmd.Parameters.AddWithValue(ParamIncludeAddRemove, includeAddRemoves);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);


            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        #endregion

        #region Add Remove
        public static DataTable SelectAddRemove(KMPlatform.Object.ClientConnections client, Object.Reporting r, int issueID, string printColumns, bool download)
        {
            var connection = DataFunctions.GetClientSqlConnection(client);
            var cmd = CreateCommandAndFillBase(connection, r);

            cmd.Parameters.AddWithValue(ParamProductId, r.PublicationIDs);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);
            cmd.Parameters.AddWithValue(ParamWaveMail, r.WaveMail);

            cmd.CommandText = SpAddRemove;

            var retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        #endregion            

        #region IssueSplits
        public static DataTable ReqFlagSummary(KMPlatform.Object.ClientConnections client, int productID)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "rpt_ReqFlagSummary";
            cmd.Parameters.AddWithValue(ParamProductId, productID);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        #endregion

        #region SubscriberDetails
        public static DataTable GetSubscriberDetails(KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID)
        {
            DataTable retItem = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "e_PubSubscriptions_Select_Fields_With_Filter";
            cmd.Parameters.AddWithValue(ParamFilters, filters);
            cmd.Parameters.AddWithValue(ParamAdHocFilters, adHocFilters);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        public static DataTable GetFullSubscriberDetails(KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID)
        {
            DataTable retItem = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "e_PubSubscriptions_Select_All_Fields_With_Filter";
            cmd.Parameters.AddWithValue(ParamFilters, filters);
            cmd.Parameters.AddWithValue(ParamAdHocFilters, adHocFilters);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        public static DataTable GetSubscriberPaidDetails(KMPlatform.Object.ClientConnections client, string filters, string adHocFilters)
        {
            DataTable retItem = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "e_PubSubscriptions_Select_Paid_Info";
            cmd.Parameters.AddWithValue(ParamFilters, filters);
            cmd.Parameters.AddWithValue(ParamAdHocFilters, adHocFilters);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        public static DataTable GetSubscriberResponseDetails(KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID)
        {
            DataTable retItem = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "e_PubSubscriptions_Select_Responses";
            cmd.Parameters.AddWithValue(ParamFilters, filters);
            cmd.Parameters.AddWithValue(ParamAdHocFilters, adHocFilters);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        #endregion


        #region SubscriberCounts
        public static List<int> SelectSubscriberCount(string xml, string adHocXml, bool includeAddRemove, bool useArchive, int issueID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_GetSubscriptionIDs_Copies_From_Filter_XML";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@FilterString", xml);
            cmd.Parameters.AddWithValue(ParamIncludeAddRemove, includeAddRemove);
            cmd.Parameters.AddWithValue(ParamAdHocXml, adHocXml);
            cmd.Parameters.AddWithValue("@UseArchive", useArchive); //issueID > 0 ? true : false);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);

            return GetIntList(cmd);
        }
        public static List<int> SelectSubscriberCountMVC(string filterQuery, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_GetSubscriptionIDs_Copies_From_FilterQuery";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue(ParamQueries, filterQuery);
            return GetIntList(cmd);
        }
        public static List<int> SelectSubscriberCopies(Object.Reporting r, KMPlatform.Object.ClientConnections client)
        {
            var connection = DataFunctions.GetClientSqlConnection(client);
            var cmd = CreateCommandAndFillBase(connection, r);

            cmd.Parameters.AddWithValue(ParamPublicationId, r.PublicationIDs);
            cmd.Parameters.AddWithValue(ParamWaveMail, r.WaveMail);

            cmd.CommandText = SpGetSubscriptionIdsCopiesFromFilter;

            return GetIntList(cmd);
        }

        public static List<int> SelectSubCountUAD(Object.Reporting r, KMPlatform.Object.ClientConnections client)
        {
            var connection = DataFunctions.GetClientSqlConnection(client);
            var cmd = CreateCommandAndFillBaseBase(connection, r);

            cmd.Parameters.AddWithValue(ParamPubIds, r.PublicationIDs);
            cmd.Parameters.AddWithValue(ParamRegions, r.Regions);
            cmd.Parameters.AddWithValue(ParamDemo31, r.Demo31);
            cmd.Parameters.AddWithValue(ParamDemo32, r.Demo32);
            cmd.Parameters.AddWithValue(ParamDemo33, r.Demo33);
            cmd.Parameters.AddWithValue(ParamDemo34, r.Demo34);
            cmd.Parameters.AddWithValue(ParamDemo35, r.Demo35);
            cmd.Parameters.AddWithValue(ParamDemo36, r.Demo36);
            cmd.Parameters.AddWithValue(ParamUadResponseIds, r.UADResponseIDs);
            cmd.Parameters.AddWithValue(ParamIsMailable, r.IsMailable);
            cmd.Parameters.AddWithValue(ParamEmailStatusIds, r.EmailStatusIDs);
            cmd.Parameters.AddWithValue(ParamOpenSearchType, r.OpenSearchType);
            cmd.Parameters.AddWithValue(ParamOpenCount, r.OpenCount);
            cmd.Parameters.AddWithValue(ParamOpenDateFrom, r.OpenDateFrom);
            cmd.Parameters.AddWithValue(ParamOpenDateTo, r.OpenDateTo);
            cmd.Parameters.AddWithValue(ParamOpenBlastId, r.OpenBlastID);
            cmd.Parameters.AddWithValue(ParamOpenEmailSubject, r.OpenEmailSubject);
            cmd.Parameters.AddWithValue(ParamOpenEmailFromDate, r.OpenEmailFromDate);
            cmd.Parameters.AddWithValue(ParamOpenEmailToDate, r.OpenEmailToDate);
            cmd.Parameters.AddWithValue(ParamClickSearchType, r.ClickSearchType);
            cmd.Parameters.AddWithValue(ParamClickCount, r.ClickCount);
            cmd.Parameters.AddWithValue(ParamClickUrl, r.ClickURL);
            cmd.Parameters.AddWithValue(ParamClickDateFrom, r.ClickDateFrom);
            cmd.Parameters.AddWithValue(ParamClickDateTo, r.ClickDateTo);
            cmd.Parameters.AddWithValue(ParamClickBlastId, r.ClickBlastID);
            cmd.Parameters.AddWithValue(ParamClickEmailSubject, r.ClickEmailSubject);
            cmd.Parameters.AddWithValue(ParamClickEmailFromDate, r.ClickEmailFromDate);
            cmd.Parameters.AddWithValue(ParamClickEmailToDate, r.ClickEmailToDate);
            cmd.Parameters.AddWithValue(ParamDomain, r.Domain);
            cmd.Parameters.AddWithValue(ParamVisitsUrl, r.VisitsURL);
            cmd.Parameters.AddWithValue(ParamVisitsDateFrom, r.VisitsDateFrom);
            cmd.Parameters.AddWithValue(ParamVisitsDateTo, r.VisitsDateTo);
            cmd.Parameters.AddWithValue(ParamBrandId, r.BrandID);
            cmd.Parameters.AddWithValue(ParamSearchType, r.SearchType);
            cmd.Parameters.AddWithValue(ParamRangeMaxLatMin, r.RangeMaxLatMin);
            cmd.Parameters.AddWithValue(ParamRangeMaxLatMax, r.RangeMaxLatMax);
            cmd.Parameters.AddWithValue(ParamRangeMaxLonMin, r.RangeMaxLonMin);
            cmd.Parameters.AddWithValue(ParamRangeMaxLonMax, r.RangeMaxLonMax);
            cmd.Parameters.AddWithValue(ParamRangeMinLatMin, r.RangeMinLatMin);
            cmd.Parameters.AddWithValue(ParamRangeMinLatMax, r.RangeMinLatMax);
            cmd.Parameters.AddWithValue(ParamRangeMinLonMin, r.RangeMinLonMin);
            cmd.Parameters.AddWithValue(ParamRangeMinLonMax, r.RangeMinLonMax);

            cmd.CommandText = SpGetSubscriptionIdsFromFilterUad;
            return GetIntList(cmd);
        }

        public static Object.Counts SelectActiveIssueSplitsCounts(int productID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_IssueSplitsActiveCounts";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue(ParamProductId, productID);

            return GetCounts(cmd);
        }
        #endregion

        #region Report Data
        public static DataTable GetStateAndCopies(string filters, int issueID, KMPlatform.Object.ClientConnections client)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_Get_PubSubscriptionStateData";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue(ParamFilters, filters);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        public static DataTable GetStateAndCopiesMVC(string filterQuery, int issueID, KMPlatform.Object.ClientConnections client)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_Get_PubSubscriptionStateDataMVC";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue(ParamQueries, filterQuery);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        public static DataTable GetCountryAndCopies(string filters, int issueID, KMPlatform.Object.ClientConnections client)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_Get_PubSubscriptionCountryData";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue(ParamFilters, filters);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        public static DataTable GetCountryAndCopiesMVC(string filterQuery, int issueID, KMPlatform.Object.ClientConnections client)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_Get_PubSubscriptionCountryDataMVC";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue(ParamQueries, filterQuery);
            cmd.Parameters.AddWithValue(ParamIssueId, issueID);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        #endregion

        private static SqlCommand CreateCommandAndFillBase(
            SqlConnection connection, 
            Object.Reporting reporting)
        {
            if (reporting == null)
            {
                throw new ArgumentNullException(nameof(reporting));
            }

            var command = CreateCommandAndFillBaseBase(connection, reporting);

            command.Parameters.AddWithValue(ParamMobile, reporting.Mobile);
            command.Parameters.AddWithValue(ParamYear, reporting.Year);
            command.Parameters.AddWithValue(ParamStartDate, reporting.FromDate);
            command.Parameters.AddWithValue(ParamEndDate, reporting.ToDate);
            command.Parameters.AddWithValue(ParamTransactionCodes, reporting.TransactionCodes);

            return command;
        }

        private static SqlCommand CreateAndFillIssueCommand(
            SqlConnection connection, 
            string filterQuery, 
            int issueId)
        {
            var cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = connection;
            cmd.Parameters.AddWithValue(ParamQueries, filterQuery);
            cmd.Parameters.AddWithValue(ParamIssueId, issueId);

            return cmd;
        }

        private static SqlCommand CreateAndFillDemoCommand(
            SqlConnection connection, 
            string filterQuery, 
            string demo, 
            int issueId)
        {
            var cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = connection;
            cmd.Parameters.AddWithValue(ParamQueries, filterQuery);
            cmd.Parameters.AddWithValue(ParamDemo, demo);
            cmd.Parameters.AddWithValue(ParamIssueId, issueId);

            return cmd;
        }

        private static SqlCommand CreateCommandAndFillBaseBase(
            SqlConnection connection, 
            Object.Reporting reporting)
        {
            if (reporting == null)
            {
                throw new ArgumentNullException(nameof(reporting));
            }

            var command = new SqlCommand()
            {
                CommandType = CommandType.StoredProcedure,
                Connection = connection
            };
            
            command.Parameters.AddWithValue(ParamCategoryIDs, reporting.CategoryIDs);
            command.Parameters.AddWithValue(ParamTransactionIDs, reporting.TransactionIDs);
            command.Parameters.AddWithValue(ParamQsourceIDs, reporting.QSourceIDs);
            command.Parameters.AddWithValue(ParamStateIds, reporting.StateIDs);
            command.Parameters.AddWithValue(ParamCountryIDs, reporting.CountryIDs);
            command.Parameters.AddWithValue(ParamEmail, reporting.Email);
            command.Parameters.AddWithValue(ParamPhone, reporting.Phone);
            command.Parameters.AddWithValue(ParamFax, reporting.Fax);
            command.Parameters.AddWithValue(ParamCategoryCodes, reporting.CategoryCodes);
            command.Parameters.AddWithValue(ParamResponseIds, reporting.ResponseIDs);
            command.Parameters.AddWithValue(ParamDemo7, reporting.Media);
            command.Parameters.AddWithValue(ParamAdHocXml, reporting.AdHocXML);

            return command;
        }
    }
}