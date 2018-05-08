using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using FrameworkUAD.Object;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.Service;
using Telerik.Reporting;
using static FrameworkUAD_Lookup.Enums;
using static FrameworkUAS.Object.AppData;
using EntityReport = FrameworkUAD.Entity.Report;
namespace ReportLibrary.Reports
{
    public class ReportUtilities : ReportUtilitiesBase
    {
        private const string Space = " ";
        private const string Underscore = "_";
        private const string UriStatesShp = "/ContentItems/states.shp";
        private const string UriStatesUdf = "/ContentItems/states.dbf";
        private const string FileStatesShp = "C:\\ADMS\\Applications\\Reporting\\states.shp";
        private const string FileStatesUdf = "C:\\ADMS\\Applications\\Reporting\\states.dbf";
        private const string DownloadDirectory = "C:\\ADMS\\Applications\\Reporting";
        private const string CsvStates = "C:\\ADMS\\Applications\\Reporting\\GeoDomesticStates.csv";
        private const string UriWorldShp = "/ContentItems/world.shp";
        private const string UriWorldUdf = "/ContentItems/world.dbf";
        private const string FileWorldShp = "C:\\ADMS\\Applications\\Reporting\\world.shp";
        private const string FileWorldUdf = "C:\\ADMS\\Applications\\Reporting\\world.dbf";
        private const string CsvWorld = "C:\\ADMS\\Applications\\Reporting\\GeoInternationalCountries.csv";
        private const string ReportyLibraryTypeNameFormatString = "ReportLibrary.Reports.{0}, ReportLibrary";
        private const string ParameterProductId = "ProductID";
        private const string ParameterProductName = "ProductName";
        private const string ParameterIssueName = "IssueName";
        private const string ParameterFilters = "Filters";
        private const string ParameterAdHocFilters = "AdHocFilters";
        private const string ParameterIssueId = "IssueID";
        private const string ParameterCountryId = "CountryId";
        private const string ParameterRow = "Row";
        private const string ParameterCol = "Col";

        public static string GetXMLFilter(string xml, object[] objs, object[] values)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i] != null && values[i] != null)
                {
                    string replaceObject = objs[i].ToString();
                    string value = values[i].ToString();
                    string open = "<" + replaceObject + ">";
                    string closed = "</" + replaceObject + ">";
                    if (xml.Contains(open))
                    {
                        int startIndex = xml.IndexOf(open) + open.Length;
                        int endIndex = xml.IndexOf(closed, startIndex);
                        string subStr = xml.Substring(startIndex, endIndex - startIndex);

                        //xml = xml.Replace(subStr, value + "," + subStr);
                        xml = xml.Replace(subStr, value);
                    }
                    else
                    {
                        xml = xml.Replace("<Filters>", "<Filters>" + open + value + closed);
                    }
                }
            }
            return xml;
        }

        public static string GetXMLCrossTabFilter(string xml, string rowID, string colID, string rowResponseID, string colResponseID)
        {
            string open = "<Responses>";
            string closed = "</Responses>";
            if (!string.IsNullOrEmpty(rowID) && !string.IsNullOrEmpty(colID) && !string.IsNullOrEmpty(rowResponseID) && !string.IsNullOrEmpty(colResponseID))
            {
                if (xml.Contains("<Responses>"))
                {
                    //string subStr = xml.Substring(xml.IndexOf(open), xml.IndexOf(closed) + (closed).Length);
                    //xml = xml.Replace(subStr, open + rowID + "_" + rowResponseID + "," + colID + "_" + colResponseID + closed);
                    int startIndex = xml.IndexOf(open) + open.Length;
                    int endIndex = xml.IndexOf(closed, startIndex);
                    string subStr = xml.Substring(startIndex, endIndex - startIndex);

                    xml = xml.Replace(subStr, rowID + "_" + rowResponseID + "," + colID + "_" + colResponseID + "," + subStr);
                }
                else
                {
                    xml = xml.Replace("<Filters>", "<Filters>" + open + rowID + "_" + rowResponseID + "," + colID + "_" + colResponseID + closed);
                }
            }

            return xml;
        }

        public static string GetDemoFilter(string xml, string rowID, string rowResponseID, object[] objs, object[] values)
        {
            string open = "<Responses>";
            string closed = "</Responses>";
            if (!string.IsNullOrEmpty(rowID) && !string.IsNullOrEmpty(rowResponseID))
            {
                if (xml.Contains("<Responses>"))
                {
                    //string subStr = xml.Substring(xml.IndexOf(open), xml.IndexOf(closed) + (closed).Length);
                    //xml = xml.Replace(subStr, open + rowID + "_" + rowResponseID + closed);
                    int startIndex = xml.IndexOf(open) + open.Length;
                    int endIndex = xml.IndexOf(closed, startIndex);
                    string subStr = xml.Substring(startIndex, endIndex - startIndex);

                    xml = xml.Replace(subStr, rowID + "_" + rowResponseID + "," + subStr);
                }
                else
                {
                    xml = xml.Replace("<Filters>", "<Filters>" + open + rowID + "_" + rowResponseID + closed);
                }
            }
            xml = GetXMLFilter(xml, objs, values);

            return xml;
        }

        public static string GetStandardAdHocFilter(string xml, object[] objs, object[] values, object[] conditions)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i] != null && values[i] != null && conditions[i] != null)
                {
                    string obj = objs[i].ToString();
                    string value = values[i].ToString();
                    string condition = conditions[i].ToString();

                    string open = "<FilterDetail><FilterField>" + obj + "</FilterField>";
                    string adhoc = "<FilterDetail><FilterField>" + obj + "</FilterField><SearchCondition>" + condition + "</SearchCondition><AdHocFieldValue>" + value + "</AdHocFieldValue>" +
                        "<FilterObjectType>Standard</FilterObjectType></FilterDetail>";
                    if (xml.Contains(open))
                    {
                        string subStr = xml.Substring(xml.IndexOf(open), xml.IndexOf("</FilterDetail>", xml.IndexOf(open)) + "</FilterDetail>".Length);
                        xml = xml.Replace(subStr, adhoc);
                    }
                    else
                    {
                        xml = xml.Replace("<XML>", "<XML>" + adhoc);
                    }
                }
            }

            return xml;
        }

        public static string GetDateRangeAdHocFilter(string xml, string obj, string fromValue, string toValue, string condition)
        {
            if (obj != null && fromValue != null && toValue != null)
            {
                string open = "<FilterDetail><FilterField>" + obj + "</FilterField>";
                string adhoc = "<FilterDetail><FilterField>" + obj + "</FilterField><SearchCondition>" + condition + "</SearchCondition><AdHocToField>" + toValue + "</AdHocToField>" +
                    "<AdHocFromField>" + fromValue + "</AdHocFromField><FilterObjectType>DateRange</FilterObjectType></FilterDetail>";
                if (xml.Contains(open))
                {
                    string subStr = xml.Substring(xml.IndexOf(open), xml.IndexOf("</FilterDetail>", xml.IndexOf(open)) + "</FilterDetail>".Length);
                    xml = xml.Replace(subStr, adhoc);
                }
                else
                {
                    xml = xml.Replace("<XML>", "<XML>" + adhoc);
                }
            }

            return xml;
        }

        public static string GetQSourceAdHocFilter(string xml, string year, string obj, string fromValue, string toValue, string condition)
        {
            if (obj != null && fromValue != null && toValue != null && year != null)
            {
                if (year == "1 Year")
                    year = "0";
                else if (year == "2 Year")
                    year = "1";
                else if (year == "3 Year")
                    year = "2";
                else if (year == "4 Year")
                    year = "3";
                else
                    year = "4";
                string yearStart = DateTime.Parse(fromValue).AddYears(-int.Parse(year)).ToShortDateString();
                string yearEnd = DateTime.Parse(toValue).AddYears(-int.Parse(year)).ToShortDateString();
                if (year == "4")
                    yearStart = "";
                if (yearStart == null || yearEnd == null)
                    return xml;
                string open = "<FilterDetail><FilterField>" + obj + "</FilterField>";
                string adhoc = "<FilterDetail><FilterField>" + obj + "</FilterField><SearchCondition>" + condition + "</SearchCondition><AdHocToField>" + yearEnd.ToString() + "</AdHocToField>" +
                    "<AdHocFromField>" + yearStart.ToString() + "</AdHocFromField><FilterObjectType>DateRange</FilterObjectType></FilterDetail>";
                if (xml.Contains(open))
                {
                    string subStr = xml.Substring(xml.IndexOf(open), xml.IndexOf("</FilterDetail>", xml.IndexOf(open)) + "</FilterDetail>".Length);
                    xml = xml.Replace(subStr, adhoc);
                }
                else
                {
                    xml = xml.Replace("<XML>", "<XML>" + adhoc);
                }
            }

            return xml;
        }

        public static string GetUADConnectionString()
        {
            if (Debug)
                //return "Data Source=.\\D2008R2;Initial Catalog=TradePressMasterDB_011916;User ID=sa;Password=t8*yh45w";
                return "Data Source=216.17.41.191;Initial Catalog=MTGMasterDB_TEst;User ID=webuser;Password=webuser#23#";
                //return "Data Source=216.17.41.251;Initial Catalog=FranceMasterDB;User ID=webuser;Password=webuser#23#";
            else
                return FrameworkUAD.DataAccess.DataFunctions.GetClientSqlConnection(myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).ConnectionString.ToString();
        }

        public static TypeReportSource GetReportSource(
            Code code,
            EntityReport report,
            IList<Country> countries,
            ReportingXML objFilters,
            int productId,
            string productName,
            int issueId,
            string issueName)
        {
            var reportSource = new TypeReportSource
                               {
                                   TypeName = string.Format(ReportyLibraryTypeNameFormatString, code.CodeName.Replace(Space, string.Empty))
                               };

            if (code.CodeName == ReportTypeToCodeString(ReportTypes.Cross_Tab))
            {
                reportSource.Parameters.Add(new Parameter(ParameterRow, report.Row.ToUpper()));
                reportSource.Parameters.Add(new Parameter(ParameterCol, report.Column.ToUpper()));
            }
            else if (code.CodeName == ReportTypeToCodeString(ReportTypes.Geo_Domestic_BreakDown))
            {
                CreateCsvForGeoBreakDown(() => GetDataTableResponseStatesAndCopies(objFilters, issueId), UriStatesShp, FileStatesShp, UriStatesUdf, FileStatesUdf, DownloadDirectory, CsvStates);
            }
            else if (code.CodeName == ReportTypeToCodeString(ReportTypes.Geo_International_BreakDown))
            {
                CreateCsvForGeoBreakDown(() => GetDataTableResponseGetCountriesAndCopies(objFilters, issueId), UriWorldShp, FileWorldShp, UriWorldUdf, FileWorldUdf, DownloadDirectory, CsvWorld);
            }
            else if (code.CodeName == ReportTypeToCodeString(ReportTypes.Single_Response) 
                    || code.CodeName == ReportTypeToCodeString(ReportTypes.DemoXQualification))
            {
                reportSource.Parameters.Add(new Parameter(ParameterRow, report.Row.ToUpper()));
            }
            else if (code.CodeName == ReportTypeToCodeString(ReportTypes.Geo_Single_Country))
            {
                var country = countries.FirstOrDefault(x => string.Equals(x.ShortName, report.Row, StringComparison.CurrentCultureIgnoreCase));
                if (country != null)
                {
                    reportSource.Parameters.Add(new Parameter(ParameterCountryId, country.CountryID));
                }
            }

            AddDefaultParameters(objFilters, productId, productName, issueId, issueName, reportSource);

            return reportSource;
        }

        private static void CreateCsvForGeoBreakDown(
            Func<Response<DataTable>> fetchAction,
            string uriShp,
            string fileShp,
            string uriDbf,
            string fileDbf,
            string downloadDirectory,
            string csvFile)
        {
            var dataTableResponse = fetchAction();
            if (dataTableResponse.Result != null && dataTableResponse.Status == ServiceResponseStatusTypes.Success)
            {
                try
                {
                    DownloadFileIfNotExists(uriShp, fileShp);
                    DownloadFileIfNotExists(uriDbf, fileDbf);
                }
                catch (Exception)
                {
                    // POSSIBLE BUG! : exception swallowed
                    DeleteFileIfExists(fileShp);
                    DeleteFileIfExists(fileDbf);
                }

                if (!Directory.Exists(downloadDirectory))
                {
                    Directory.CreateDirectory(downloadDirectory);
                }

                var fileFunctions = new Core_AMS.Utilities.FileFunctions();
                fileFunctions.CreateCSVFromDataTable(dataTableResponse.Result, csvFile);
            }
        }

        private static void DeleteFileIfExists(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        private static void DownloadFileIfNotExists(string uriString, string fileName)
        {
            if (File.Exists(fileName))
            {
                return;
            }

            var uri = new Uri(uriString, UriKind.Relative);
            var info = System.Windows.Application.GetContentStream(uri);
            if (info != null)
            {
                using (var infoStream = info.Stream)
                {
                    using (var file = new FileStream(
                        fileName,
                        FileMode.Create,
                        FileAccess.Write))
                    {
                        infoStream.CopyTo(file);
                    }
                }
            }
        }

        private static void AddDefaultParameters(
            ReportingXML reportingXml,
            int productId,
            string productName,
            int issueId,
            string issueName,
            ReportSource reportSource)
        {
            reportSource.Parameters.Add(new Parameter(ParameterProductId, productId));
            reportSource.Parameters.Add(new Parameter(ParameterProductName, productName));
            reportSource.Parameters.Add(new Parameter(ParameterIssueName, issueName));
            reportSource.Parameters.Add(new Parameter(ParameterFilters, reportingXml.Filters));
            reportSource.Parameters.Add(new Parameter(ParameterAdHocFilters, reportingXml.AdHocFilters));
            reportSource.Parameters.Add(new Parameter(ParameterIssueId, issueId));
        }

        private static string ReportTypeToCodeString(ReportTypes reportTypes)
        {
            return reportTypes.ToString().Replace(Underscore, Space);
        }

        private static Response<DataTable> GetDataTableResponseGetCountriesAndCopies(ReportingXML reportingXml, int issueId)
        {
            var reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();
            var dataTableResponse = reportData.Proxy.Get_Countries_And_Copies(
                myAppData.AuthorizedUser.AuthAccessKey,
                reportingXml.Filters,
                issueId,
                myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            return dataTableResponse;
        }

        private static Response<DataTable> GetDataTableResponseStatesAndCopies(ReportingXML reportingXml, int issuedId)
        {
            var reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();
            var dataTableResponse = reportData.Proxy.Get_States_And_Copies(
                myAppData.AuthorizedUser.AuthAccessKey,
                reportingXml.Filters,
                issuedId,
                myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            return dataTableResponse;
        }

        public static DataTable GetCategorySummaryData(string filters, string adHocFilters, int issueID)
        {
            FrameworkUAS.Service.Response<DataTable> dataTableResponse = new FrameworkUAS.Service.Response<DataTable>();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();
            DataTable rtnItem = new DataTable();

            dataTableResponse = reportData.Proxy.SelectCategorySummary(myAppData.AuthorizedUser.AuthAccessKey, myAppData.AuthorizedUser.User.CurrentClient.ClientConnections,
                filters, adHocFilters, issueID);
            if (dataTableResponse.Result != null && dataTableResponse.Status == ServiceResponseStatusTypes.Success)
            {
                rtnItem = dataTableResponse.Result;
            }
            return rtnItem;
        }

        #region CrossTab
        public static DataTable GetCrossTabData(string filters, string adHocFilters, int issueID, string col, string row, bool includeAddRemove, bool includeReportGroups, int productID)
        {
            FrameworkUAS.Service.Response<DataTable> dataTableResponse = new FrameworkUAS.Service.Response<DataTable>();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();
            DataTable rtnItem = new DataTable();

            dataTableResponse = reportData.Proxy.SelectCrossTab(myAppData.AuthorizedUser.AuthAccessKey, 
                myAppData.AuthorizedUser.User.CurrentClient.ClientConnections,
                productID, row, col, includeAddRemove, filters, adHocFilters, issueID, includeReportGroups);
            if (dataTableResponse.Result != null && dataTableResponse.Status == ServiceResponseStatusTypes.Success)
            {
                rtnItem = dataTableResponse.Result;
            }
            return rtnItem;
        }
        public static DataTable GetDemoSubReportData(string filters, string adHocFilters, int issueID, string row, bool includeAddRemove, int productID)
        {
            FrameworkUAS.Service.Response<DataTable> dataTableResponse = new FrameworkUAS.Service.Response<DataTable>();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();
            DataTable rtnItem = new DataTable();

            dataTableResponse = reportData.Proxy.SelectDemoSubReport(myAppData.AuthorizedUser.AuthAccessKey,
                myAppData.AuthorizedUser.User.CurrentClient.ClientConnections,
                productID, row, includeAddRemove, filters, adHocFilters, issueID);
            if (dataTableResponse.Result != null && dataTableResponse.Status == ServiceResponseStatusTypes.Success)
            {
                rtnItem = dataTableResponse.Result;
            }
            return rtnItem;
        }
        public static DataTable GetSingleResponseData(string filters, string adHocFilters, int issueID, string row, bool includeReportGroups, int productID)
        {
            FrameworkUAS.Service.Response<DataTable> dataTableResponse = new FrameworkUAS.Service.Response<DataTable>();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();
            DataTable rtnItem = new DataTable();

            dataTableResponse = reportData.Proxy.SelectSingleResponse(myAppData.AuthorizedUser.AuthAccessKey,
                myAppData.AuthorizedUser.User.CurrentClient.ClientConnections,
                productID, row, includeReportGroups, filters, adHocFilters, issueID);
            if (dataTableResponse.Result != null && dataTableResponse.Status == ServiceResponseStatusTypes.Success)
            {
                rtnItem = dataTableResponse.Result;
            }
            return rtnItem;
        }
        public static List<FrameworkUAD.Entity.ResponseGroup> GetResponses(int productID)
        {
            List<FrameworkUAD.Entity.ResponseGroup> rtnItem = new List<FrameworkUAD.Entity.ResponseGroup>();
            FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>> responses = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>>();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IResponseGroup> reportData = FrameworkServices.ServiceClient.UAD_ResponseGroupClient();

            responses = reportData.Proxy.Select(myAppData.AuthorizedUser.AuthAccessKey,
                myAppData.AuthorizedUser.User.CurrentClient.ClientConnections,
                productID);
            if (responses.Result != null && responses.Status == ServiceResponseStatusTypes.Success)
                rtnItem = responses.Result.Where(x=> x.DisplayName.ToUpper() != "PUBCODE" && x.DisplayName.ToUpper() != "SUBSCRIPTION").ToList();
            return rtnItem;
        }
        public static DataTable GetProfileFields()
        {
            DataTable rtnItem = new DataTable();
            FrameworkUAS.Service.Response<DataTable> responses = new FrameworkUAS.Service.Response<DataTable>();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();

            responses = reportData.Proxy.GetProfileFields(myAppData.AuthorizedUser.AuthAccessKey,
                myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (responses.Result != null && responses.Status == ServiceResponseStatusTypes.Success)
                rtnItem = responses.Result;
            return rtnItem;
        }
        #endregion
        #region QDate
        public static DataTable GetDemoXQualData(string filters, string adHocFilters, int issueID, string row, bool includeReportGroups, int productID)
        {
            FrameworkUAS.Service.Response<DataTable> dataTableResponse = new FrameworkUAS.Service.Response<DataTable>();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();
            DataTable rtnItem = new DataTable();

            dataTableResponse = reportData.Proxy.SelectDemoXQualification(myAppData.AuthorizedUser.AuthAccessKey,
                myAppData.AuthorizedUser.User.CurrentClient.ClientConnections,
                productID, row, filters, adHocFilters, issueID, includeReportGroups);
            if (dataTableResponse.Result != null && dataTableResponse.Status == ServiceResponseStatusTypes.Success)
                rtnItem = dataTableResponse.Result;
            return rtnItem;
        }
        public static DataTable GetIssueDates(int productID)
        {
            FrameworkUAS.Service.Response<DataTable> dataTableResponse = new FrameworkUAS.Service.Response<DataTable>();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();
            DataTable rtnItem = new DataTable();

            dataTableResponse = reportData.Proxy.GetIssueDates(myAppData.AuthorizedUser.AuthAccessKey,
                myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, productID);
            if (dataTableResponse.Result != null && dataTableResponse.Status == ServiceResponseStatusTypes.Success)
                rtnItem = dataTableResponse.Result;
            return rtnItem;
        }
        public static DataTable GetQSourceBreakDown(string filters, string adHocFilters, int issueID, bool includeAddRemove, int productID)
        {
            FrameworkUAS.Service.Response<DataTable> dataTableResponse = new FrameworkUAS.Service.Response<DataTable>();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();
            DataTable rtnItem = new DataTable();

            dataTableResponse = reportData.Proxy.SelectQSourceBreakdown(myAppData.AuthorizedUser.AuthAccessKey,
                myAppData.AuthorizedUser.User.CurrentClient.ClientConnections,
                productID, includeAddRemove, filters, adHocFilters, issueID);
            if (dataTableResponse.Result != null && dataTableResponse.Status == ServiceResponseStatusTypes.Success)
                rtnItem = dataTableResponse.Result;
            return rtnItem;
        }
        #endregion
        #region Geo BreakDown
        public static DataTable GetGeoBreakDownDomestic(string filters, string adHocFilters, int issueID, bool includeAddRemove)
        {
            FrameworkUAS.Service.Response<DataTable> dataTableResponse = new FrameworkUAS.Service.Response<DataTable>();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();
            DataTable rtnItem = new DataTable();

            dataTableResponse = reportData.Proxy.SelectGeoBreakdown_Domestic(myAppData.AuthorizedUser.AuthAccessKey,
                myAppData.AuthorizedUser.User.CurrentClient.ClientConnections,
                filters, adHocFilters, issueID, includeAddRemove);
            if (dataTableResponse.Result != null && dataTableResponse.Status == ServiceResponseStatusTypes.Success)
            {
                rtnItem = dataTableResponse.Result;
            }
            return rtnItem;
        }
        public static DataTable GetGeoBreakDownSingleCountry(string filters, string adHocFilters, int issueID, int countryID)
        {
            FrameworkUAS.Service.Response<DataTable> dataTableResponse = new FrameworkUAS.Service.Response<DataTable>();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();
            DataTable rtnItem = new DataTable();

            dataTableResponse = reportData.Proxy.SelectGeoBreakdown_Single_Country(myAppData.AuthorizedUser.AuthAccessKey,
                myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, filters, adHocFilters, issueID, countryID);
            if (dataTableResponse.Result != null && dataTableResponse.Status == ServiceResponseStatusTypes.Success)
            {
                rtnItem = dataTableResponse.Result;
            }
            return rtnItem;
        }
        public static DataTable GetGeoBreakDownInternational(string filters, string adHocFilters, int issueID)
        {
            FrameworkUAS.Service.Response<DataTable> dataTableResponse = new FrameworkUAS.Service.Response<DataTable>();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();
            DataTable rtnItem = new DataTable();

            dataTableResponse = reportData.Proxy.SelectGeoBreakdownInternational(myAppData.AuthorizedUser.AuthAccessKey,
                myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, filters, adHocFilters, issueID);
            if (dataTableResponse.Result != null && dataTableResponse.Status == ServiceResponseStatusTypes.Success)
            {
                rtnItem = dataTableResponse.Result;
            }
            return rtnItem;
        }
        public static DataTable GetCountries()
        {
            FrameworkUAS.Service.Response<DataTable> dataTableResponse = new FrameworkUAS.Service.Response<DataTable>();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();
            DataTable rtnItem = new DataTable();

            dataTableResponse = reportData.Proxy.GetCountries(myAppData.AuthorizedUser.AuthAccessKey,
                myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (dataTableResponse.Result != null && dataTableResponse.Status == ServiceResponseStatusTypes.Success)
            {
                rtnItem = dataTableResponse.Result;
            }
            return rtnItem;
        }
        #endregion
        public static DataTable GetPar3CData(string filters, string adHocFilters, int issueID)
        {
            DataTable rtnTable = new DataTable();
            FrameworkUAS.Service.Response<DataTable> dataTableResponse = new FrameworkUAS.Service.Response<DataTable>();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();

            dataTableResponse = reportData.Proxy.SelectPar3c(myAppData.AuthorizedUser.AuthAccessKey, myAppData.AuthorizedUser.User.CurrentClient.ClientConnections,
                filters, adHocFilters, issueID);

            if (dataTableResponse.Result != null && dataTableResponse.Status == ServiceResponseStatusTypes.Success)
            {
                rtnTable = dataTableResponse.Result;
            }
            return rtnTable;
        }
        public static DataTable GetSubFieldData(string filters, string adHocFilters, string demo, int issueID)
        {
            DataTable rtnTable = new DataTable();
            FrameworkUAS.Service.Response<DataTable> dataTableResponse = new FrameworkUAS.Service.Response<DataTable>();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();

            dataTableResponse = reportData.Proxy.SelectSubFields(myAppData.AuthorizedUser.AuthAccessKey, myAppData.AuthorizedUser.User.CurrentClient.ClientConnections,
                filters, adHocFilters, demo, issueID);

            if (dataTableResponse.Result != null && dataTableResponse.Status == ServiceResponseStatusTypes.Success)
            {
                rtnTable = dataTableResponse.Result;
            }
            return rtnTable;
        }
        public static DataTable GetSubSrcData(string filters, string adHocFilters, bool includeAddRemove, int issueID)
        {
            DataTable rtnTable = new DataTable();
            FrameworkUAS.Service.Response<DataTable> dataTableResponse = new FrameworkUAS.Service.Response<DataTable>();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();

            dataTableResponse = reportData.Proxy.SelectSubsrc(myAppData.AuthorizedUser.AuthAccessKey, myAppData.AuthorizedUser.User.CurrentClient.ClientConnections,
                filters, adHocFilters, includeAddRemove, issueID);

            if (dataTableResponse.Result != null && dataTableResponse.Status == ServiceResponseStatusTypes.Success)
            {
                rtnTable = dataTableResponse.Result;
            }
            return rtnTable;
        }
        #region SubscriberDetails
        public static DataTable GetSubscriberDetails(string filters, string adHocFilters, int issueID)
        {
            DataTable rtnTable = new DataTable();
            FrameworkUAS.Service.Response<DataTable> dataTableResponse = new FrameworkUAS.Service.Response<DataTable>();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();

            dataTableResponse = reportData.Proxy.GetSubscriberDetails(myAppData.AuthorizedUser.AuthAccessKey, myAppData.AuthorizedUser.User.CurrentClient.ClientConnections,
                filters, adHocFilters, issueID);

            if (dataTableResponse.Result != null && dataTableResponse.Status == ServiceResponseStatusTypes.Success)
            {
                rtnTable = dataTableResponse.Result;
            }
            return rtnTable;
        }
        public static DataTable GetFullSubscriberDetails(string filters, string adHocFilters, int issueID)
        {
            DataTable rtnTable = new DataTable();
            FrameworkUAS.Service.Response<DataTable> dataTableResponse = new FrameworkUAS.Service.Response<DataTable>();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();

            dataTableResponse = reportData.Proxy.GetFullSubscriberDetails(myAppData.AuthorizedUser.AuthAccessKey, myAppData.AuthorizedUser.User.CurrentClient.ClientConnections,
                filters, adHocFilters, issueID);

            if (dataTableResponse.Result != null && dataTableResponse.Status == ServiceResponseStatusTypes.Success)
            {
                rtnTable = dataTableResponse.Result;
            }
            return rtnTable;
        }
        public static DataTable GetSubscriberPaidDetails(string filters, string adHocFilters)
        {
            DataTable rtnTable = new DataTable();
            FrameworkUAS.Service.Response<DataTable> dataTableResponse = new FrameworkUAS.Service.Response<DataTable>();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();

            dataTableResponse = reportData.Proxy.GetSubscriberPaidDetails(myAppData.AuthorizedUser.AuthAccessKey, myAppData.AuthorizedUser.User.CurrentClient.ClientConnections,
                filters, adHocFilters);
            if (dataTableResponse.Result != null && dataTableResponse.Status == ServiceResponseStatusTypes.Success)
            {
                rtnTable = dataTableResponse.Result;
            }
            return rtnTable;
        }
        public static DataTable GetSubscriberResponseDetails(string filters, string adHocFilters, int issueID)
        {
            DataTable rtnTable = new DataTable();
            FrameworkUAS.Service.Response<DataTable> dataTableResponse = new FrameworkUAS.Service.Response<DataTable>();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();

            dataTableResponse = reportData.Proxy.GetSubscriberResponseDetails(myAppData.AuthorizedUser.AuthAccessKey, myAppData.AuthorizedUser.User.CurrentClient.ClientConnections,
                filters, adHocFilters, issueID);

            if (dataTableResponse.Result != null && dataTableResponse.Status == ServiceResponseStatusTypes.Success)
            {
                rtnTable = dataTableResponse.Result;
            }
            return rtnTable;
        }
        #endregion
    }
}
