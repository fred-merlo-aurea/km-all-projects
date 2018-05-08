using System;
using System.Collections.Generic;
using System.Data;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.Report;
using EntityCounts = FrameworkUAD.Object.Counts;
using EntityQualificationBreakdownReport = FrameworkUAD.Object.QualificationBreakdownReport;
using EntityReport = FrameworkUAD.Entity.Report;
using EntityReporting = FrameworkUAD.Object.Reporting;

namespace UAD_WS.Service
{
    public class Reports : FrameworkServiceBase, IReports
    {
        private const string EntityName = "Reports";
        private const string MethodSelect = "Select";
        private const string MethodSelectDemoSubReport = "SelectDemoSubReport";
        private const string MethodGetResponses = "GetResponses";
        private const string MethodGetProfileFields = "GetProfileFields";
        private const string MethodSelectGeoBreakdownInternational = "SelectGeoBreakdownInternational";
        private const string MethodSelectListReport = "SelectListReport";
        private const string MethodSelectPar3C = "SelectPar3c";
        private const string MethodSelectSubFields = "SelectSubFields";
        private const string MethodGetSubscriberDetails = "GetSubscriberDetails";
        private const string MethodSelectIssueSplitsActiveCounts = "SelectIssueSplitsActiveCounts";
        private const string MethodGetCountriesAndCopies = "Get_Countries_And_Copies";
        private const string MethodSelectForAddRemoveReports = "Select_For_AddRemove_Reports";
        private const string MethodSave = "Save";
        private const string MethodSelectBpa = "SelectBPA";
        private const string MethodSelectCategorySummary = "SelectCategorySummary";
        private const string MethodSelectCrossTab = "SelectCrossTab";
        private const string MethodSelectSingleResponse = "SelectSingleResponse";
        private const string MethodSelectDemoXQualification = "SelectDemoXQualification";
        private const string MethodGetIssueDates = "GetIssueDates";
        private const string MethodSelectGeoBreakdownSingleCountry = "SelectGeoBreakdown_Single_Country";
        private const string MethodSelectGeoBreakdownDomestic = "SelectGeoBreakdown_Domestic";
        private const string MethodGetCountries = "GetCountries";
        private const string MethodSelectQSourceBreakdown = "SelectQSourceBreakdown";
        private const string MethodSelectSubSrc = "SelectSubsrc";
        private const string MethodSelectAddRemove = "SelectAddRemove";
        private const string MethodReqFlagSummary = "ReqFlagSummary";
        private const string MethodGetFullSubscriberDetails = "GetFullSubscriberDetails";
        private const string MethodGetSubscriberPaidDetails = "GetSubscriberPaidDetails";
        private const string MethodGetSubscriberResponseDetails = "GetSubscriberResponseDetails";
        private const string MethodSelectSubCountUad = "SelectSubCountUAD";
        private const string MethodGetStatesAndCopies = "Get_States_And_Copies";

        /// <summary>
        /// Selects a list of Report objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of Report objects</returns>
        public Response<List<EntityReport>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntityReport>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Report objects specifically for the Add Remove reports tab based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="pubID"></param>
        /// <returns>response.result will contain a list of Report objects</returns>
        public Response<List<EntityReport>> Select_For_AddRemove_Reports(Guid accessKey, KMPlatform.Object.ClientConnections client, int pubID)
        {
            var model = new ServiceRequestModel<List<EntityReport>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForAddRemoveReports,
                WorkerFunc = _ => new BusinessLogicWorker().SelectForAddRemoves(client, pubID)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the Report object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="entity">the <see cref="EntityReport"/> object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, KMPlatform.Object.ClientConnections client, EntityReport entity)
        {
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = new JsonFunctions().ToJson(entity),
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().Save(client, entity);
                    request.Succeeded = result > 0;
                    return result;
                }
            };

            return GetResponse(model);
        }

        #region BPA

        public Response<DataTable> SelectBPA(
            Guid accessKey,
            KMPlatform.Object.ClientConnections client,
            EntityReporting entity,
            string printColumns,
            bool download)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectBpa,
                WorkerFunc = _ => new BusinessLogicWorker().SelectBPA(client, entity, printColumns, download)
            };

            return GetResponse(model);
        }

        #endregion
        #region CategorySummary

        /// <summary>
        /// Gets a CategorySummary of the information within the given Reporting object, if download = true report additional data
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="client"></param>
        /// <param name="filters"></param>
        /// <param name="adHocFilters"></param>
        /// <param name="issueID"></param>
        /// <returns>response.result will contain a list of CategorySummaryReport objects</returns>
        public Response<DataTable> SelectCategorySummary(
            Guid accessKey,
            KMPlatform.Object.ClientConnections client,
            string filters,
            string adHocFilters,
            int issueID)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectCategorySummary,
                WorkerFunc = _ => new BusinessLogicWorker().SelectCategorySummary(client, filters, adHocFilters, issueID)
            };

            return GetResponse(model);
        }

        #endregion
        #region CrossTab

        public Response<DataTable> SelectCrossTab(
            Guid accessKey,
            KMPlatform.Object.ClientConnections client,
            int productID,
            string row,
            string col,
            bool includeAddRemove,
            string filters,
            string adHocFilters,
            int issueID,
            bool includeReportGroup)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectCrossTab,
                WorkerFunc = _ => new BusinessLogicWorker().SelectCrossTab(
                    client,
                    productID,
                    row,
                    col,
                    includeAddRemove,
                    filters,
                    adHocFilters,
                    issueID,
                    includeReportGroup)
            };

            return GetResponse(model);
        }

        public Response<DataTable> SelectDemoSubReport(Guid accessKey, KMPlatform.Object.ClientConnections client, int productID, string row, bool includeAddRemove, string filters,
                                                        string adHocFilters, int issueID)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectDemoSubReport,
                WorkerFunc = _ => new BusinessLogicWorker().SelectDemoSubReport(client, productID, row, includeAddRemove, filters, adHocFilters, issueID)
            };

            return GetResponse(model);
        }

        public Response<DataTable> SelectSingleResponse(
            Guid accessKey,
            KMPlatform.Object.ClientConnections client,
            int productID,
            string row,
            bool includeReportGroups,
            string filters,
            string adHocFilters,
            int issueID)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectSingleResponse,
                WorkerFunc = _ => new BusinessLogicWorker().SelectSingleResponse(
                    client,
                    productID,
                    row,
                    includeReportGroups,
                    filters,
                    adHocFilters,
                    issueID)
            };

            return GetResponse(model);
        }

        public Response<DataTable> GetResponses(Guid accessKey, KMPlatform.Object.ClientConnections client, int productID)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodGetResponses,
                WorkerFunc = _ => new BusinessLogicWorker().GetResponses(client, productID)
            };

            return GetResponse(model);
        }

        public Response<DataTable> GetProfileFields(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodGetProfileFields,
                WorkerFunc = _ => new BusinessLogicWorker().GetProfileFields(client)
            };

            return GetResponse(model);
        }

        #endregion            
        #region DemoXQualification

        public Response<DataTable> SelectDemoXQualification(
            Guid accessKey,
            KMPlatform.Object.ClientConnections client,
            int productid,
            string row,
            string filters,
            string adHocFilters,
            int issueID,
            bool includeReportGroups)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectDemoXQualification,
                WorkerFunc = _ => new BusinessLogicWorker().SelectDemoXQualification(
                    client,
                    productid,
                    row,
                    filters,
                    adHocFilters,
                    issueID,
                    includeReportGroups)
            };

            return GetResponse(model);
        }

        public Response<DataTable> GetIssueDates(Guid accessKey, KMPlatform.Object.ClientConnections client, int productid)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodGetIssueDates,
                WorkerFunc = _ => new BusinessLogicWorker().GetIssueDates(client, productid)
            };

            return GetResponse(model);
        }

        #endregion      
        #region Geo BreakDown

        public Response<DataTable> SelectGeoBreakdown_Single_Country(
            Guid accessKey,
            KMPlatform.Object.ClientConnections client,
            string filters,
            string adHocFilters,
            int issueID,
            int countryID)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectGeoBreakdownSingleCountry,
                WorkerFunc = _ => new BusinessLogicWorker().SelectGeoBreakdown_Single_Country(
                    client,
                    filters,
                    adHocFilters,
                    issueID,
                    countryID)
            };

            return GetResponse(model);
        }

        public Response<DataTable> SelectGeoBreakdownInternational(Guid accessKey, KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectGeoBreakdownInternational,
                WorkerFunc = _ => new BusinessLogicWorker().SelectGeoBreakdownInternational(client, filters, adHocFilters, issueID)
            };

            return GetResponse(model);
        }

        public Response<DataTable> SelectGeoBreakdown_Domestic(
            Guid accessKey,
            KMPlatform.Object.ClientConnections client,
            string filters,
            string adHocFilters,
            int issueID,
            bool includeAddRemoves)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectGeoBreakdownDomestic,
                WorkerFunc = _ => new BusinessLogicWorker().SelectGeoBreakdown_Domestic(
                    client,
                    filters,
                    adHocFilters,
                    issueID,
                    includeAddRemoves)
            };

            return GetResponse(model);
        }

        public Response<DataTable> GetCountries(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodGetCountries,
                WorkerFunc = _ => new BusinessLogicWorker().GetCountries(client)
            };

            return GetResponse(model);
        }

        #endregion    
        #region ListReport

        public Response<DataTable> SelectListReport(Guid accessKey, KMPlatform.Object.ClientConnections client, int reportID, string rowID, EntityReporting reporting, string printColumns, bool download)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectListReport,
                WorkerFunc = _ => new BusinessLogicWorker().SelectListReport(client, reporting, reportID, rowID, printColumns, download)
            };

            return GetResponse(model);
        }

        #endregion   
        #region Par3c

        public Response<DataTable> SelectPar3c(Guid accessKey, KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectPar3C,
                WorkerFunc = _ => new BusinessLogicWorker().SelectPar3c(client, filters, adHocFilters, issueID)
            };

            return GetResponse(model);
        }

        #endregion       
        #region QSourceBreakdown

        public Response<DataTable> SelectQSourceBreakdown(
            Guid accessKey,
            KMPlatform.Object.ClientConnections client,
            int productID,
            bool includeAddRemove,
            string filters,
            string adHocFilters,
            int issueID)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectQSourceBreakdown,
                WorkerFunc = _ => new BusinessLogicWorker().SelectQSourceBreakdown(
                    client,
                    productID,
                    includeAddRemove,
                    filters,
                    adHocFilters,
                    issueID)
            };

            return GetResponse(model);
        }

        #endregion    
        #region SubFields

        public Response<DataTable> SelectSubFields(Guid accessKey, KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, string demo, int issueID)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectSubFields,
                WorkerFunc = _ => new BusinessLogicWorker().SelectSubFields(client, filters, adHocFilters, demo, issueID)
            };

            return GetResponse(model);
        }

        #endregion         
        #region Subsrc

        public Response<DataTable> SelectSubsrc(
            Guid accessKey,
            KMPlatform.Object.ClientConnections client,
            string filters,
            string adHocFilters,
            bool includeAddRemoves,
            int issueID)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectSubSrc,
                WorkerFunc = _ => new BusinessLogicWorker().SelectSubsrc(
                    client,
                    filters,
                    adHocFilters,
                    includeAddRemoves,
                    issueID)
            };

            return GetResponse(model);
        }

        #endregion            
        #region Add Remove

        public Response<DataTable> SelectAddRemove(
            Guid accessKey,
            KMPlatform.Object.ClientConnections client,
            EntityReporting entity,
            int issueID,
            string printColumns,
            bool download)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectAddRemove,
                WorkerFunc = _ => new BusinessLogicWorker().SelectAddRemove(
                    client,
                    entity,
                    issueID,
                    printColumns,
                    download)
            };

            return GetResponse(model);
        }

        #endregion   
        #region IssueSplits

        public Response<DataTable> ReqFlagSummary(Guid accessKey, KMPlatform.Object.ClientConnections client, int productID)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodReqFlagSummary,
                WorkerFunc = _ => new BusinessLogicWorker().ReqFlagSummary(client, productID)
            };

            return GetResponse(model);
        }

        #endregion
        #region Subscriber Details

        public Response<DataTable> GetSubscriberDetails(Guid accessKey, KMPlatform.Object.ClientConnections client, string filters, string adHocFilters, int issueID)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodGetSubscriberDetails,
                WorkerFunc = _ => new BusinessLogicWorker().GetSubscriberDetails(client, filters, adHocFilters, issueID)
            };

            return GetResponse(model);
        }

        public Response<DataTable> GetFullSubscriberDetails(
            Guid accessKey,
            KMPlatform.Object.ClientConnections client,
            string filters,
            string adHocFilters,
            int issueID)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodGetFullSubscriberDetails,
                WorkerFunc = _ => new BusinessLogicWorker().GetFullSubscriberDetails(client, filters, adHocFilters, issueID)
            };

            return GetResponse(model);
        }

        public Response<DataTable> GetSubscriberPaidDetails(Guid accessKey, KMPlatform.Object.ClientConnections client, string filters, string adHocFilters)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodGetSubscriberPaidDetails,
                WorkerFunc = _ => new BusinessLogicWorker().GetSubscriberPaidDetails(client, filters, adHocFilters)
            };

            return GetResponse(model);
        }

        public Response<DataTable> GetSubscriberResponseDetails(
            Guid accessKey,
            KMPlatform.Object.ClientConnections client,
            string filters,
            string adHocFilters,
            int issueID)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodGetSubscriberResponseDetails,
                WorkerFunc = _ => new BusinessLogicWorker().GetSubscriberResponseDetails(client, filters, adHocFilters, issueID)
            };

            return GetResponse(model);
        }

        #endregion

        /// <summary>
        /// Gets a quick breakdown of a quilification report from the given Reporting object, if download = true report additional data
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="reporting">the Reporting object</param>
        /// <param name="printColumns">extra columns to print out for report</param>
        /// <param name="download">boolean to take additional data</param>
        /// <param name="years">years of qulification</param>
        /// <returns>response.result will contain a list of QualificationBreakdownReport objects</returns>
        public Response<List<EntityQualificationBreakdownReport>> SelectQualificationBreakDown(Guid accessKey, KMPlatform.Object.ClientConnections client, EntityReporting reporting, string printColumns, bool download, int years)
        {
            var model = new ServiceRequestModel<List<EntityQualificationBreakdownReport>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().SelectQualificationBreakDown(client, reporting, printColumns, download, years)
            };

            return GetResponse(model);
        }

        #region GetSubscriberCounts
        /// <summary>
        /// Gets the count of Subscribers in the database table
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="xml"></param>
        /// <param name="adHocXml"></param>
        /// <param name="includeAddRemove"></param>
        /// <param name="useArchive"></param>
        /// <param name="issueID"></param>
        /// <param name="client"></param>
        /// <returns>response.result will contain an integer list</returns>
        public Response<List<int>> SelectSubscriberCount(
            Guid accessKey,
            string xml,
            string adHocXml,
            bool includeAddRemove,
            bool useArchive,
            int issueID,
            KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<List<int>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().SelectSubscriberCount(
                    xml,
                    adHocXml,
                    includeAddRemove,
                    useArchive,
                    issueID,
                    client)
            };

            return GetResponse(model);
        }

        public Response<List<int>> SelectSubscriberCopies(Guid accessKey, FrameworkUAD.Object.Reporting reporting, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<List<int>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().SelectSubscriberCopies(reporting, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Gets a sub count of Susbscribers in the UAD database
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityReporting"/> object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer list</returns>
        public Response<List<int>> SelectSubCountUAD(Guid accessKey, EntityReporting entity, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<List<int>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectSubCountUad,
                WorkerFunc = _ => new BusinessLogicWorker().SelectSubCountUAD(entity, client)
            };

            return GetResponse(model);
        }

        public Response<EntityCounts> SelectIssueSplitsActiveCounts(Guid accessKey, int productID, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<EntityCounts>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectIssueSplitsActiveCounts,
                WorkerFunc = _ => new BusinessLogicWorker().SelectActiveIssueSplitsCounts(productID, client)
            };

            return GetResponse(model);
        }

        #endregion

        #region ReportData

        public Response<DataTable> Get_States_And_Copies(Guid accessKey, string filters, int issueID, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodGetStatesAndCopies,
                WorkerFunc = _ => new BusinessLogicWorker().GetStateAndCopies(filters, issueID, client)
            };

            return GetResponse(model);
        }

        public Response<DataTable> Get_Countries_And_Copies(Guid accessKey, string filters, int issueID, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<DataTable>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodGetCountriesAndCopies,
                WorkerFunc = _ => new BusinessLogicWorker().GetCountryAndCopies(filters, issueID, client)
            };

            return GetResponse(model);
        }

        #endregion
    }
}
