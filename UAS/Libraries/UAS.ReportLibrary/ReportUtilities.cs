using System.Collections.Generic;
using System.Data;
using System.Linq;
using ReportLibrary.Reports;

namespace UAS.ReportLibrary
{
    public class ReportUtilities : ReportUtilitiesBase
    {
        private static KMPlatform.Entity.User CurrentUser
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser; }
        }
        private static int CurrentClientID
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientID; }
        }
      

        public static string GetUADConnectionString()
        {
            
            if (Debug)
                return "Data Source=IT-LF\\LOCALGT;Initial Catalog=MTGMasterDB;User ID=sa;Password=sa";
            //return "Data Source=216.17.41.191;Initial Catalog=MTGMasterDB_TEst;User ID=webuser;Password=webuser#23#";
            else
            {
                KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(ClientId);
                return FrameworkUAD.DataAccess.DataFunctions.GetClientSqlConnection(client.ClientConnections).ConnectionString.ToString();
            }
                
        }

        public static DataTable GetCategorySummaryData(string filterQuery, int issueID)
        {
            KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(ClientId);
            DataTable rtnItem = new DataTable();

            return rtnItem = new FrameworkUAD.BusinessLogic.Report().SelectCategorySummaryMVC(client.ClientConnections, filterQuery, issueID);

        }
        
        public static Telerik.Reporting.TypeReportSource GetReportSource(FrameworkUAD_Lookup.Entity.Code c, FrameworkUAD.Entity.Report report, 
            KMPlatform.Entity.Client cl,List<FrameworkUAD_Lookup.Entity.Country> countries,FrameworkUAD.Object.ReportingXML objFilters, 
            int pubID, string pubName, int issueID, string issueName,string filterQuery, string folderpath, bool IncludeAddRemove=false)
        {
            DataTable dataTableResponse = new DataTable();
            
            var rptSource = new Telerik.Reporting.TypeReportSource();
            rptSource.TypeName = "UAS.ReportLibrary.Reports." + c.CodeName.Replace(" ", "") + ", UAS.ReportLibrary";
            if (IncludeAddRemove)
            {
                rptSource.Parameters.Add(new Telerik.Reporting.Parameter("IncludeAddRemove", true));
            }
            //rvReport.RefreshReport();
            if (c.CodeName == FrameworkUAD_Lookup.Enums.ReportTypes.Cross_Tab.ToString().Replace("_", " "))
            {
                rptSource.Parameters.Add(new Telerik.Reporting.Parameter("Row", report.Row.ToUpper()));
                rptSource.Parameters.Add(new Telerik.Reporting.Parameter("Col", report.Column.ToUpper()));
            }
            else if (c.CodeName == FrameworkUAD_Lookup.Enums.ReportTypes.Geo_Domestic_BreakDown.ToString().Replace("_", " "))
            {
                #region Geo Domestic
                //dataTableResponse = new FrameworkUAD.BusinessLogic.Report().GetStateAndCopiesMVC(filterQuery, issueID, client.ClientConnections);
                //if (dataTableResponse != null )
                //{
                //    try
                //    {
                //        if (!File.Exists("C:\\ADMS\\Applications\\Reporting\\states.shp"))
                //        {
                //            FileStream info = new FileStream(folderpath+"states.shp",FileMode.Open);
                //            if (info != null)
                //            {
                //                using (Stream s = info)
                //                {
                //                    using (var file = new FileStream("C:\\ADMS\\Applications\\Reporting\\states.shp", FileMode.Create, FileAccess.Write))
                //                    {
                //                        s.CopyTo(file);
                //                        file.Close();
                //                    }
                //                    s.Close();
                //                }
                //            }
                //        }
                //        if (!File.Exists("C:\\ADMS\\Applications\\Reporting\\states.dbf"))
                //        {
                //            FileStream info = new FileStream(folderpath + "states.shp", FileMode.Open);
                //            if (info != null)
                //            {
                //                using (Stream s = info)
                //                {
                //                    using (var file = new FileStream("C:\\ADMS\\Applications\\Reporting\\states.dbf", FileMode.Create, FileAccess.Write))
                //                    {
                //                        s.CopyTo(file);
                //                        file.Close();
                //                    }
                //                    s.Close();
                //                }
                //            }
                //        }
                //    }
                //    catch (Exception)
                //    {
                //        if (File.Exists("C:\\ADMS\\Applications\\Reporting\\states.shp"))
                //            File.Delete("C:\\ADMS\\Applications\\Reporting\\states.shp");
                //        if (File.Exists("C:\\ADMS\\Applications\\Reporting\\states.dbf"))
                //            File.Delete("C:\\ADMS\\Applications\\Reporting\\states.dbf");
                //        //Core.Utilities.WPF.MessageError("There was an issue while generating the report.")
                //    }
                //    Core_AMS.Utilities.FileFunctions ff = new Core_AMS.Utilities.FileFunctions();
                //    if (!Directory.Exists("C:\\ADMS\\Applications\\Reporting"))
                //        Directory.CreateDirectory("C:\\ADMS\\Applications\\Reporting");
                //    ff.CreateCSVFromDataTable(dataTableResponse, "C:\\ADMS\\Applications\\Reporting\\GeoDomesticStates.csv", true);
                //}
                #endregion
            }
            else if (c.CodeName == FrameworkUAD_Lookup.Enums.ReportTypes.Geo_International_BreakDown.ToString().Replace("_", " "))
            {
                #region Geo International
                //dataTableResponse = new FrameworkUAD.BusinessLogic.Report().GetCountryAndCopiesMVC(filterQuery, issueID, client.ClientConnections);
                //if (dataTableResponse != null)
                //{
                //    try
                //    {
                //        if (!File.Exists("C:\\ADMS\\Applications\\Reporting\\world.shp"))
                //        {
                //            FileStream info = new FileStream(folderpath + "\\world.shp", FileMode.Open);
                //            if (info != null)
                //            {
                //                using (Stream s = info)
                //                {
                //                    using (var file = new FileStream("C:\\ADMS\\Applications\\Reporting\\world.shp", FileMode.Create, FileAccess.Write))
                //                    {
                //                        s.CopyTo(file);
                //                        file.Close();
                //                    }
                //                    s.Close();
                //                }
                //            }
                //        }
                //        if (!File.Exists("C:\\ADMS\\Applications\\Reporting\\world.dbf"))
                //        {
                //            FileStream info = new FileStream(folderpath + "\\world.dbf", FileMode.Open);
                //            if (info != null)
                //            {
                //                using (Stream s = info)
                //                {
                //                    using (var file = new FileStream("C:\\ADMS\\Applications\\Reporting\\world.dbf", FileMode.Create, FileAccess.Write))
                //                    {
                //                        s.CopyTo(file);
                //                        file.Close();
                //                    }
                //                    s.Close();
                //                }
                //            }
                //        }
                //    }
                //    catch (Exception)
                //    {
                //        if (File.Exists("C:\\ADMS\\Applications\\Reporting\\world.shp"))
                //            File.Delete("C:\\ADMS\\Applications\\Reporting\\world.shp");
                //        if (File.Exists("C:\\ADMS\\Applications\\Reporting\\world.dbf"))
                //            File.Delete("C:\\ADMS\\Applications\\Reporting\\world.dbf");
                //        //Core.Utilities.WPF.MessageError("There was an issue while generating the report.")
                //    }
                //    Core_AMS.Utilities.FileFunctions ff = new Core_AMS.Utilities.FileFunctions();
                //    if (!Directory.Exists("C:\\ADMS\\Applications\\Reporting"))
                //        Directory.CreateDirectory("C:\\ADMS\\Applications\\Reporting");
                //    ff.CreateCSVFromDataTable(dataTableResponse, "C:\\ADMS\\Applications\\Reporting\\GeoInternationalCountries.csv", true);
                //}
                #endregion
            }
            else if (c.CodeName == FrameworkUAD_Lookup.Enums.ReportTypes.Single_Response.ToString().Replace("_", " ")
                    || c.CodeName == FrameworkUAD_Lookup.Enums.ReportTypes.DemoXQualification.ToString().Replace("_", " "))
            {
                rptSource.Parameters.Add(new Telerik.Reporting.Parameter("Row", report.Row.ToUpper()));
            }
            else if (c.CodeName == FrameworkUAD_Lookup.Enums.ReportTypes.Geo_Single_Country.ToString().Replace("_", " "))
            {
                FrameworkUAD_Lookup.Entity.Country ctr = countries.Where(x => x.ShortName.ToUpper() == report.Row.ToUpper()).FirstOrDefault();
                if (ctr != null)
                {
                    rptSource.Parameters.Add(new Telerik.Reporting.Parameter("CountryID", ctr.CountryID));
                }
            }
            rptSource.Parameters.Add(new Telerik.Reporting.Parameter("ProductID", pubID));
            rptSource.Parameters.Add(new Telerik.Reporting.Parameter("ProductName", pubName));
            rptSource.Parameters.Add(new Telerik.Reporting.Parameter("IssueName", issueName));
            rptSource.Parameters.Add(new Telerik.Reporting.Parameter("FilterQuery", filterQuery));
            rptSource.Parameters.Add(new Telerik.Reporting.Parameter("IssueID", issueID));

            return rptSource;
        }

        #region CrossTab
        public static DataTable GetCrossTabData(string filterQuery,  int issueID, string col, string row, bool includeAddRemove, bool includeReportGroups, int productID)
        {
           
            DataTable rtnItem = new DataTable();
            KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(ClientId);
            rtnItem = new FrameworkUAD.BusinessLogic.Report().SelectCrossTabMVC(client.ClientConnections,productID, row, col, includeAddRemove, filterQuery,  issueID, includeReportGroups);
            return rtnItem;
        }
        public static DataTable GetDemoSubReportData(string filterQuery, int issueID, string row, bool includeAddRemove, int productID)
        {
            DataTable rtnItem = new DataTable();
            KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(ClientId);
            rtnItem = new FrameworkUAD.BusinessLogic.Report().SelectDemoSubReportMVC(client.ClientConnections, productID, row,  includeAddRemove, filterQuery, issueID);
            return rtnItem;
        }
        public static DataTable GetSingleResponseData(string filterQuery, int issueID, string row, bool includeReportGroups, int productID)
        {
            DataTable rtnItem = new DataTable();
            KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(ClientId);
            rtnItem = new FrameworkUAD.BusinessLogic.Report().SelectSingleResponseMVC(client.ClientConnections, productID, row, includeReportGroups, filterQuery, issueID);
            return rtnItem;
        }
        public static List<FrameworkUAD.Entity.ResponseGroup> GetResponses(int productID)
        {
            List<FrameworkUAD.Entity.ResponseGroup> rtnItem = new List<FrameworkUAD.Entity.ResponseGroup>();
            
            KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(ClientId);
            rtnItem = new FrameworkUAD.BusinessLogic.ResponseGroup().Select( productID, client.ClientConnections);
            return rtnItem;
           
        }
        public static DataTable GetProfileFields()
        {
            DataTable rtnItem = new DataTable();
            KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(ClientId);
            rtnItem = new FrameworkUAD.BusinessLogic.Report().GetProfileFields(client.ClientConnections);
           
            return rtnItem;
        }
        #endregion

        #region QDate
        public static DataTable GetDemoXQualData(string filterQury, int issueID, string row, bool includeReportGroups, int productID)
        {
            DataTable rtnItem = new DataTable();
            KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(ClientId);

            rtnItem = new FrameworkUAD.BusinessLogic.Report().SelectDemoXQualificationMVC(client.ClientConnections, productID, row, filterQury, issueID, includeReportGroups);
            return rtnItem;
            
        }
        public static DataTable GetIssueDates(int productID)
        {
            DataTable rtnItem = new DataTable();
            KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(ClientId);

            rtnItem = new FrameworkUAD.BusinessLogic.Report().GetIssueDates(client.ClientConnections, productID);
            return rtnItem;
           
        }
        public static DataTable GetQSourceBreakDown(string filterQuery, int issueID, bool includeAddRemove, int productID)
        {
            DataTable rtnItem = new DataTable();
            KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(ClientId);

            rtnItem = new FrameworkUAD.BusinessLogic.Report().SelectQSourceBreakdownMVC(client.ClientConnections, productID, includeAddRemove, filterQuery, issueID);
            return rtnItem;
            
        }
        #endregion
        #region Geo BreakDown
        public static DataTable GetGeoBreakDownDomestic(string filterQuery, int issueID, bool includeAddRemove)
        {
            DataTable rtnItem = new DataTable();
            KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(ClientId);

            rtnItem = new FrameworkUAD.BusinessLogic.Report().SelectGeoBreakdown_DomesticMVC(client.ClientConnections, filterQuery, issueID, includeAddRemove);
            return rtnItem;
            
        }
        public static DataTable GetGeoBreakDownSingleCountry(string filterQuery, int issueID, int countryID)
        {
            DataTable rtnItem = new DataTable();
            KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(ClientId);

            rtnItem = new FrameworkUAD.BusinessLogic.Report().SelectGeoBreakdown_Single_CountryMVC(client.ClientConnections, filterQuery, issueID, countryID);
            return rtnItem;
           
        }
        public static DataTable GetGeoBreakDownInternational(string filtersQuery, int issueID,int productid, bool IncludeCustomRegion=false)
        {
            DataTable rtnItem = new DataTable();
            KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(ClientId);

            rtnItem = new FrameworkUAD.BusinessLogic.Report().SelectGeoBreakdownInternationalMVC(client.ClientConnections, filtersQuery, issueID, productid, IncludeCustomRegion);
            return rtnItem;
            
        }
        public static DataTable GetCountries()
        {
            DataTable rtnItem = new DataTable();
            KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(ClientId);

            rtnItem = new FrameworkUAD.BusinessLogic.Report().GetCountries(client.ClientConnections);
            return rtnItem;
           
          
        }
        #endregion
        public static DataTable GetPar3CData(string filterQuery, int issueID)
        {
            DataTable rtnItem = new DataTable();
            KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(ClientId);

            rtnItem = new FrameworkUAD.BusinessLogic.Report().SelectPar3cMVC(client.ClientConnections, filterQuery, issueID);
            return rtnItem;
        }
        public static DataTable GetSubFieldData(string filterQuery, string demo, int issueID)
        {
            DataTable rtnItem = new DataTable();
            KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(ClientId);

            rtnItem = new FrameworkUAD.BusinessLogic.Report().SelectSubFieldsMVC(client.ClientConnections, filterQuery, demo, issueID);
            return rtnItem;
          
        }
        public static DataTable GetSubSrcData(string filterQuery, bool includeAddRemove, int issueID)
        {
            DataTable rtnItem = new DataTable();
            KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(ClientId);

            rtnItem = new FrameworkUAD.BusinessLogic.Report().SelectSubsrcMVC(client.ClientConnections, filterQuery, includeAddRemove, issueID);
            return rtnItem;
           
        }

        #region SubscriberDetails
        public static DataTable GetSubscriberDetails(string filterQuery, int issueID)
        {
            DataTable rtnItem = new DataTable();
            KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(ClientId);

            rtnItem = new FrameworkUAD.BusinessLogic.Report().GetSubscriberDetailsMVC(client.ClientConnections, filterQuery, issueID);
            return rtnItem;
            
        }
        public static DataTable GetFullSubscriberDetails(string filterQuery, int issueID)
        {
            DataTable rtnItem = new DataTable();
            KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(ClientId);

            rtnItem = new FrameworkUAD.BusinessLogic.Report().GetFullSubscriberDetailsMVC(client.ClientConnections, filterQuery, issueID);
            return rtnItem;
            
        }
        public static DataTable GetSubscriberPaidDetails(string filterQuery)
        {
            DataTable rtnItem = new DataTable();
            KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(ClientId);

            rtnItem = new FrameworkUAD.BusinessLogic.Report().GetSubscriberPaidDetailsMVC(client.ClientConnections, filterQuery);
            return rtnItem;
           
        }
        public static DataTable GetSubscriberResponseDetails(string filterQuery, int issueID)
        {
            DataTable rtnItem = new DataTable();
            KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(ClientId);

            rtnItem = new FrameworkUAD.BusinessLogic.Report().GetSubscriberResponseDetailsMVC(client.ClientConnections, filterQuery, issueID);
            return rtnItem;
           
        }
        #endregion
    }
}
