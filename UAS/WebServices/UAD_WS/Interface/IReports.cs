using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;
using System.Data;

namespace UAD_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IReports
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.Report>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.Report>> Select_For_AddRemove_Reports(Guid accessKey, KMPlatform.Object.ClientConnections client, int pubID);

        [OperationContract]
        Response<int> Save(Guid accessKey, KMPlatform.Object.ClientConnections client, FrameworkUAD.Entity.Report x);
        
        #region BPA
        [OperationContract]
        Response<DataTable> SelectBPA(Guid accessKey, KMPlatform.Object.ClientConnections client, FrameworkUAD.Object.Reporting r, string printColumns, bool download);         
        #endregion    
        #region CategorySummary
        [OperationContract]
        Response<DataTable> SelectCategorySummary(Guid accessKey, KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID);
        #endregion  
        #region CrossTab
        [OperationContract]
        Response<DataTable> SelectCrossTab(Guid accessKey, KMPlatform.Object.ClientConnections client, int productID, string row, string col, bool includeAddRemove, string filters, 
            string adHocFilters, int issueID, bool includeReportGroup);
        [OperationContract]
        Response<DataTable> SelectDemoSubReport(Guid accessKey, KMPlatform.Object.ClientConnections client, int productID, string row, bool includeAddRemove, string filters,
            string adHocFilters, int issueID);
        [OperationContract]
        Response<DataTable> SelectSingleResponse(Guid accessKey, KMPlatform.Object.ClientConnections client, int productID, string row, bool includeReportGroups, string filters,
            string adHocFilters, int issueID);
        [OperationContract]
        Response<DataTable> GetResponses(Guid accessKey, KMPlatform.Object.ClientConnections client, int productID);
        [OperationContract]
        Response<DataTable> GetProfileFields(Guid accessKey, KMPlatform.Object.ClientConnections client);
        #endregion
        #region DemoXQualification
        [OperationContract]
        Response<DataTable> SelectDemoXQualification(Guid accessKey, KMPlatform.Object.ClientConnections client, int productid, string row, string filters, string adHocFilters, int issueID, bool includeReportGroups);
        [OperationContract]
        Response<DataTable> GetIssueDates(Guid accessKey, KMPlatform.Object.ClientConnections client, int productid);
        #endregion 
        #region Geo BreakDown
        [OperationContract]
        Response<DataTable> SelectGeoBreakdown_Domestic(Guid accessKey, KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID, bool includeAddRemoves);
        [OperationContract]
        Response<DataTable> SelectGeoBreakdown_Single_Country(Guid accessKey, KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID, int countryID);   
        [OperationContract]
        Response<DataTable> SelectGeoBreakdownInternational(Guid accessKey, KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID);
        [OperationContract]
        Response<DataTable> GetCountries(Guid accessKey, KMPlatform.Object.ClientConnections client);
        #endregion    
        #region ListReport
        [OperationContract]
        Response<DataTable> SelectListReport(Guid accessKey, KMPlatform.Object.ClientConnections client, int reportID, string rowID, FrameworkUAD.Object.Reporting r, string printColumns, bool download);
        #endregion   
        #region Par3c
        [OperationContract]
        Response<DataTable> SelectPar3c(Guid accessKey, KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID);
        #endregion       
        #region QSourceBreakdown
        [OperationContract]
        Response<DataTable> SelectQSourceBreakdown(Guid accessKey, KMPlatform.Object.ClientConnections client, int productID, bool includeAddRemove, string filters, string adHocFilters, int issueID);
        #endregion            
        #region SubFields
        [OperationContract]
        Response<DataTable> SelectSubFields(Guid accessKey, KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, string demo, int issueID);
        #endregion                
        #region Subsrc
        [OperationContract]
        Response<DataTable> SelectSubsrc(Guid accessKey, KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, bool includeAddRemoves, int issueID);
        #endregion        
        #region IssueSplits
        [OperationContract]
        Response<DataTable> ReqFlagSummary(Guid accessKey, KMPlatform.Object.ClientConnections client, int productID);
        #endregion
        #region Subscriber Details
        [OperationContract]
        Response<DataTable> GetSubscriberDetails(Guid accessKey, KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID);
        [OperationContract]
        Response<DataTable> GetFullSubscriberDetails(Guid accessKey, KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID);
        [OperationContract]
        Response<DataTable> GetSubscriberPaidDetails(Guid accessKey, KMPlatform.Object.ClientConnections client, string filters, string adHocFilters);
        [OperationContract]
        Response<DataTable> GetSubscriberResponseDetails(Guid accessKey, KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID);
        #endregion

        #region Add Remove
        [OperationContract]
        Response<DataTable> SelectAddRemove(Guid accessKey, KMPlatform.Object.ClientConnections client, FrameworkUAD.Object.Reporting r, int issueID, string printColumns, bool download);
        #endregion                    

        [OperationContract]
        Response<List<FrameworkUAD.Object.QualificationBreakdownReport>> SelectQualificationBreakDown(Guid accessKey, KMPlatform.Object.ClientConnections client, FrameworkUAD.Object.Reporting r, string printColumns, bool download, int years);

        [OperationContract]
        Response<List<int>> SelectSubscriberCount(Guid accessKey, string xml, string adHocXml, bool includeAddRemove, bool useArchive, int issueID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<List<int>> SelectSubscriberCopies(Guid accessKey, FrameworkUAD.Object.Reporting obj, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<int>> SelectSubCountUAD(Guid accessKey, FrameworkUAD.Object.Reporting obj, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<FrameworkUAD.Object.Counts> SelectIssueSplitsActiveCounts(Guid accessKey, int productID, KMPlatform.Object.ClientConnections client);

        #region ReportData
        [OperationContract]
        Response<DataTable> Get_States_And_Copies(Guid accessKey, string filters, int issueID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<DataTable> Get_Countries_And_Copies(Guid accessKey, string filters, int issueID, KMPlatform.Object.ClientConnections client);
        #endregion
    }
}
