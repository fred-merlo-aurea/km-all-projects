using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using KMPlatform.Object;

namespace FrameworkUAD.BusinessLogic
{
    public class Report
    {
        public List<Entity.Report> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Report> retList = null;
            retList = DataAccess.Report.Select(client);
            return retList;
        }

        public List<Entity.Report> SelectForAddRemoves(KMPlatform.Object.ClientConnections client, int pubID)
        {
            List<Entity.Report> retList = null;
            retList = DataAccess.Report.SelectForAddRemoves(client, pubID);
            return retList;
        }

        public int Save(KMPlatform.Object.ClientConnections client, Entity.Report x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.ReportID = DataAccess.Report.Save(client, x);
                scope.Complete();
            }

            return x.ReportID;
        }

        #region BPA
        public DataTable SelectBPA(KMPlatform.Object.ClientConnections client, Object.Reporting r, string printColumns, bool download)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectBPA(client, r, printColumns, download);

            return dt;
        }
        #endregion     
        #region CategorySummary
        public DataTable SelectCategorySummary(KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectCategorySummary(client, filters, adHocFilters, issueID);

            return dt;
        }

        public List<int> SelectSubCountUAD(FrameworkUAD.Object.Reporting r, KMPlatform.Object.ClientConnections client)
        {
            List<int> retList = new List<int>();
            retList = DataAccess.Report.SelectSubCountUAD(r, client);
            return retList;
        }

        #endregion   
        #region CrossTab
        public DataTable SelectCrossTab(KMPlatform.Object.ClientConnections client, int productID, string row, string col, bool includeAddRemove, string filters, string adHocFilters, int issueID,
            bool includeReportGroup)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectCrossTab(client, productID, row, col, includeAddRemove, filters, adHocFilters, issueID, includeReportGroup);
            return dt;
        }
        public DataTable SelectDemoSubReport(KMPlatform.Object.ClientConnections client, int productID, string row, bool includeAddRemove, string filters, string adHocFilters, int issueID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectDemoSubReport(client, productID, row, includeAddRemove, filters, adHocFilters, issueID);
            return dt;
        }
        public DataTable SelectSingleResponse(KMPlatform.Object.ClientConnections client, int productID, string row, bool includeReportGroups, string filters, string adHocFilters, int issueID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectSingleResponse(client, productID, row, includeReportGroups, filters, adHocFilters, issueID);
            return dt;
        }
        public DataTable GetResponses(KMPlatform.Object.ClientConnections client, int productID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.GetResponses(client, productID);
            return dt;
        }
        public DataTable GetProfileFields(KMPlatform.Object.ClientConnections client)
        {
            DataTable dt = null;
            dt = DataAccess.Report.GetProfileFields(client);
            return dt;
        }
        #endregion
        #region DemoXQualification
        public DataTable SelectDemoXQualification(KMPlatform.Object.ClientConnections client, int productid, string row, string filters, string adHocFilters, int issueID, bool includeReportGroups)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectDemoXQualification(client, productid, row, filters, adHocFilters, issueID, includeReportGroups);

            return dt;
        }
        public DataTable GetIssueDates(KMPlatform.Object.ClientConnections client, int productid)
        {
            DataTable dt = null;
            dt = DataAccess.Report.GetIssueDates(client, productid);

            return dt;
        }
        #endregion   
        #region Geo BreakDown
        public DataTable SelectGeoBreakdown_Domestic(KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID, bool includeAddRemoves)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectGeoBreakdown_Domestic(client, filters, adHocFilters, issueID, includeAddRemoves);

            return dt;
        }
        public DataTable SelectGeoBreakdown_Single_Country(KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID, int countryID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectGeoBreakdown_Single_Country(client, filters, adHocFilters, issueID, countryID);

            return dt;
        }
        public DataTable SelectGeoBreakdownInternational(KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectGeoBreakdownInternational(client, filters, adHocFilters, issueID);

            return dt;
        }
        public DataTable GetCountries(KMPlatform.Object.ClientConnections client)
        {
            DataTable dt = null;
            dt = DataAccess.Report.Get_Countries(client);

            return dt;
        }
        #endregion    
        #region ListReport
        public DataTable SelectListReport(KMPlatform.Object.ClientConnections client, Object.Reporting r, int reportID, string rowID, string printColumns, bool download)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectListReport(client, r, reportID, rowID, printColumns, download);

            return dt;
        }
        #endregion    
        #region Par3c
        public DataTable SelectPar3c(KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectPar3c(client, filters, adHocFilters, issueID);

            return dt;
        }
        #endregion       
        #region QSourceBreakdown
        public DataTable SelectQSourceBreakdown(KMPlatform.Object.ClientConnections client, int productID, bool includeAddRemove, string filters, string adHocFilters, int issueID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectQSourceBreakdown(client, productID, includeAddRemove, filters, adHocFilters, issueID);

            return dt;
        }
        #endregion
        #region SubFields
        public DataTable SelectSubFields(KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, string demo, int issueID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectSubFields(client, filters, adHocFilters, demo, issueID);

            return dt;
        }
        #endregion        
        #region Subsrc
        public DataTable SelectSubsrc(KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, bool includeAddRemoves, int issueID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectSubsrc(client, filters, adHocFilters, includeAddRemoves, issueID);

            return dt;
        }
        #endregion        
        #region Add Remove
        public DataTable SelectAddRemove(KMPlatform.Object.ClientConnections client, Object.Reporting r, int issueID, string printColumns, bool download)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectAddRemove(client, r, issueID, printColumns, download);

            return dt;
        }
        #endregion                    
        #region IssueSplits
        public DataTable ReqFlagSummary(KMPlatform.Object.ClientConnections client, int productID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.ReqFlagSummary(client, productID);

            return dt;
        }
        #endregion
        #region Subscriber Details
        public DataTable GetSubscriberDetails(KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.GetSubscriberDetails(client, filters, adHocFilters, issueID);

            return dt;
        }
        public DataTable GetFullSubscriberDetails(KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.GetFullSubscriberDetails(client, filters, adHocFilters, issueID);

            return dt;
        }
        public DataTable GetSubscriberPaidDetails(KMPlatform.Object.ClientConnections client, string filters, string adHocFilters)
        {
            DataTable dt = null;
            dt = DataAccess.Report.GetSubscriberPaidDetails(client, filters, adHocFilters);

            return dt;
        }
        public DataTable GetSubscriberResponseDetails(KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.GetSubscriberResponseDetails(client, filters, adHocFilters, issueID);

            return dt;
        }
        #endregion

        public List<Object.QualificationBreakdownReport> SelectQualificationBreakDown(KMPlatform.Object.ClientConnections client, Object.Reporting r, string printColumns, bool download, int years)
        {
            List<Object.QualificationBreakdownReport> retList = null;
            retList = DataAccess.Report.SelectQualificationBreakDown(client, r, printColumns, download, years);
            return retList;
        }

        #region GetSubscriberCounts
        public List<int> SelectSubscriberCount(string xml, string adHocXml, bool includeAddRemove, bool useArchive, int issueID, KMPlatform.Object.ClientConnections client)
        {
            List<int> retList = new List<int>();
            retList = DataAccess.Report.SelectSubscriberCount(xml, adHocXml, includeAddRemove, useArchive, issueID, client);
            return retList;
        }
        public List<int> SelectSubscriberCountMVC(string filterquery, KMPlatform.Object.ClientConnections client)
        {
            List<int> retList = new List<int>();
            retList = DataAccess.Report.SelectSubscriberCountMVC(filterquery, client);
            return retList;
        }
        public List<int> SelectSubscriberCopies(FrameworkUAD.Object.Reporting r, KMPlatform.Object.ClientConnections client)
        {
            List<int> retList = new List<int>();
            retList = DataAccess.Report.SelectSubscriberCopies(r, client);
            return retList;
        }
        public Object.Counts SelectActiveIssueSplitsCounts(int productID, KMPlatform.Object.ClientConnections client)
        {
            Object.Counts retItem = new Object.Counts();
            retItem = DataAccess.Report.SelectActiveIssueSplitsCounts(productID, client);
            return retItem;
        }
        public DataTable SelectDemoSubReportMVC(KMPlatform.Object.ClientConnections client, int productID, string row, bool includeAddRemove, string filterQuery, int issueID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectDemoSubReportMVC(client, productID, row, includeAddRemove, filterQuery, issueID);
            return dt;
        }
        public DataTable SelectCategorySummaryMVC(ClientConnections clientConnections, string filterQuery, int issueID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectCategorySummaryMVC(clientConnections, filterQuery, issueID);

            return dt;
        }
        #endregion

        #region Report Data
        public DataTable GetStateAndCopies(string filters, int issueID, KMPlatform.Object.ClientConnections client)
        {
            DataTable retItem = new DataTable();
            retItem = DataAccess.Report.GetStateAndCopies(filters, issueID, client);
            return retItem;
        }
        public DataTable GetCountryAndCopies(string filters, int issueID, KMPlatform.Object.ClientConnections client)
        {
            DataTable retItem = new DataTable();
            retItem = DataAccess.Report.GetCountryAndCopies(filters, issueID, client);
            return retItem;
        }
        public DataTable GetStateAndCopiesMVC(string filterQuery, int issueID, KMPlatform.Object.ClientConnections client)
        {
            DataTable retItem = new DataTable();
            retItem = DataAccess.Report.GetStateAndCopiesMVC(filterQuery, issueID, client);
            return retItem;
        }
        public DataTable GetCountryAndCopiesMVC(string filterQuery, int issueID, KMPlatform.Object.ClientConnections client)
        {
            DataTable retItem = new DataTable();
            retItem = DataAccess.Report.GetCountryAndCopiesMVC(filterQuery, issueID, client);
            return retItem;
        }

        public DataTable SelectCrossTabMVC(ClientConnections clientConnections, int productID, string row, string col, bool includeAddRemove, string filterQuery, int issueID, bool includeReportGroups)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectCrossTabMVC(clientConnections, productID, row, col, includeAddRemove, filterQuery, issueID, includeReportGroups);
            return dt;
        }

        public DataTable SelectDemoXQualificationMVC(ClientConnections clientConnections, int productID, string row, string filterQury, int issueID, bool includeReportGroups)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectDemoXQualificationMVC(clientConnections, productID, row, filterQury, issueID, includeReportGroups);

            return dt;
        }

        public DataTable GetFullSubscriberDetailsMVC(ClientConnections clientConnections, string filterQuery, int issueID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.GetFullSubscriberDetailsMVC(clientConnections, filterQuery, issueID);

            return dt;
        }

        public DataTable SelectGeoBreakdown_DomesticMVC(ClientConnections clientConnections, string filterQuery, int issueID, bool includeAddRemove)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectGeoBreakdown_DomesticMVC(clientConnections, filterQuery, issueID, includeAddRemove);

            return dt;
        }

        public DataTable SelectGeoBreakdownInternationalMVC(ClientConnections clientConnections, string filtersQuery, int issueID,int productid, bool IncludeCustomRegion=false)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectGeoBreakdownInternationalMVC(clientConnections, filtersQuery, issueID, productid, IncludeCustomRegion);

            return dt;
        }

        public DataTable SelectGeoBreakdown_Single_CountryMVC(ClientConnections clientConnections, string filterQuery, int issueID, int countryID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectGeoBreakdown_Single_CountryMVC(clientConnections, filterQuery, issueID, countryID);

            return dt;
        }

        public DataTable SelectPar3cMVC(ClientConnections clientConnections, string filterQuery, int issueID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectPar3cMVC(clientConnections, filterQuery, issueID);

            return dt;
        }

        public DataTable SelectSingleResponseMVC(ClientConnections clientConnections, int productID, string row, bool includeReportGroups, string filterQuery, int issueID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectSingleResponseMVC(clientConnections, productID, row, includeReportGroups, filterQuery, issueID);
            return dt;
        }

        public DataTable SelectQSourceBreakdownMVC(ClientConnections clientConnections, int productID, bool includeAddRemove, string filterQuery, int issueID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectQSourceBreakdownMVC(clientConnections, productID, includeAddRemove, filterQuery, issueID);

            return dt;
        }

        public DataTable SelectSubFieldsMVC(ClientConnections clientConnections, string filterQuery, string demo, int issueID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectSubFieldsMVC(clientConnections, filterQuery, demo, issueID);

            return dt;
        }

        public DataTable GetSubscriberDetailsMVC(ClientConnections client, string filterQuery, int issueID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.GetSubscriberDetailsMVC(client, filterQuery, issueID);

            return dt;
        }

        public DataTable GetSubscriberPaidDetailsMVC(ClientConnections clientConnections, string filterQuery)
        {
            DataTable dt = null;
            dt = DataAccess.Report.GetSubscriberPaidDetailsMVC(clientConnections, filterQuery);

            return dt;
        }

        public DataTable GetSubscriberResponseDetailsMVC(ClientConnections clientConnections, string filterQuery, int issueID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.GetSubscriberResponseDetailsMVC(clientConnections, filterQuery, issueID);

            return dt;
        }

        public DataTable SelectSubsrcMVC(ClientConnections clientConnections, string filterQuery, bool includeAddRemove, int issueID)
        {
            DataTable dt = null;
            dt = DataAccess.Report.SelectSubsrcMVC(clientConnections, filterQuery, includeAddRemove, issueID);

            return dt;
        }
        #endregion
    }
}
