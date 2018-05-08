using System;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Web;
using System.Configuration;
using System.Collections;

namespace ecn.common.classes
{

    public enum CRExportEnum
    {
        PDF = 1,
        XLS = 2,
        DOC = 3,
        HTML = 4,
        XLSDATA = 5
    }

    public class CRReport
    {
        public CRReport()
        {
        }

        public static ReportDocument GetReport(string reportName, Hashtable cParams)
        {
            return GetReport(reportName, cParams, "");
        }

        public static ReportDocument GetReport(string reportName, Hashtable cParams, string database)
        {

            Database crDatabase;
            Tables crTables;
            TableLogOnInfo crTableLogOnInfo;

            ReportDocument report = new ReportDocument();

            report.Load(reportName);

            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            crConnectionInfo.ServerName = ConfigurationManager.AppSettings["Server"].ToString();
            if (database == string.Empty)
            {
                crConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["Database"].ToString();
            }
            else
            {
                crConnectionInfo.DatabaseName = database;
            }
            crConnectionInfo.UserID = ConfigurationManager.AppSettings["UserID"].ToString();
            crConnectionInfo.Password = ConfigurationManager.AppSettings["Password"].ToString();

            crDatabase = report.Database;
            crTables = crDatabase.Tables;

            //Apply the logon information to each table in the collection
            // there seems to be no foreach ...Engine.StoredProcedure ???
            foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in
                crTables)
            {
                crTableLogOnInfo = crTable.LogOnInfo;
                crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                crTable.ApplyLogOnInfo(crTableLogOnInfo);
            }

            IDictionaryEnumerator en = cParams.GetEnumerator();

            while (en.MoveNext())
            {
                report.SetParameterValue(en.Key.ToString(), en.Value.ToString());
            }

            Sections crSections = report.ReportDefinition.Sections;

            // loop through all the sections to find all the report objects 
            foreach (Section crSection in crSections)
            {
                ReportObjects crReportObjects = crSection.ReportObjects;

                //loop through all the report objects in there to find all subreports 
                foreach (ReportObject crReportObject in crReportObjects)
                {
                    if (crReportObject.Kind == ReportObjectKind.SubreportObject)
                    {
                        SubreportObject crSubreportObject = (SubreportObject)crReportObject;
                        //open the subreport object and logon as for the general report 
                        ReportDocument crSubreportDocument = crSubreportObject.OpenSubreport(crSubreportObject.SubreportName);
                        crDatabase = crSubreportDocument.Database;

                        crTables = crDatabase.Tables;

                        foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
                        {
                            crTableLogOnInfo = crTable.LogOnInfo;
                            crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                            crTable.ApplyLogOnInfo(crTableLogOnInfo);
                        }

                    }
                }

            }

            return report;

        }


        public static ReportDocument GetReport1(string reportName, Hashtable cParams)
        {
            Database crDatabase;
            Tables crTables;
            TableLogOnInfo crTableLogOnInfo;

            ReportDocument report = new ReportDocument();

            report.Load(reportName);

            ConnectionInfo ci = new ConnectionInfo();

            ci.ServerName = ConfigurationManager.AppSettings["Server"].ToString();
            ci.DatabaseName = ConfigurationManager.AppSettings["Database"].ToString();
            ci.UserID = ConfigurationManager.AppSettings["UserID"].ToString();
            ci.Password = ConfigurationManager.AppSettings["Password"].ToString();

            crDatabase = report.Database;
            crTables = crDatabase.Tables;

            foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in
                crTables)
            {
                crTableLogOnInfo = crTable.LogOnInfo;
                crTableLogOnInfo.ConnectionInfo = ci;
                crTable.ApplyLogOnInfo(crTableLogOnInfo);
            }


            //if (!ApplyLogon(report, ci))
            //    throw new Exception("Crystal Report Login Failed");

            //foreach (ReportObject obj in report.ReportDefinition.ReportObjects)
            //{
            //    if (obj.Kind == ReportObjectKind.SubreportObject)
            //    {
            //        subObj = (SubreportObject)obj;

            //        if (!ApplyLogon(report.OpenSubreport(subObj.SubreportName), ci))
            //            throw new Exception("Crystal Subreport Report Login");
            //    }
            //}

            IDictionaryEnumerator en = cParams.GetEnumerator();

            while (en.MoveNext())
            {
                report.SetParameterValue(en.Key.ToString(), en.Value.ToString());
            }

            return report;
        }

        // Helper method that iterates through all tables // in a report document 
        private static bool ApplyLogon(ReportDocument cr, ConnectionInfo ci)
        {
            TableLogOnInfo li;
            // for each table apply connection info 
            foreach (Table tbl in cr.Database.Tables)
            {
                li = tbl.LogOnInfo;
                li.ConnectionInfo = ci;
                tbl.ApplyLogOnInfo(li);

                //// check if logon was successful 
                //// if TestConnectivity returns false, check 
                //// logon credentials 
                //if (tbl.TestConnectivity())
                //{
                //    // drop fully qualified table location 
                //    if (tbl.Location.IndexOf(".") > 0)
                //    {
                //        tbl.Location = tbl.Location.Substring(tbl.Location.LastIndexOf(".") + 1);
                //    }
                //    else
                //        tbl.Location = tbl.Location;
                //}
                //else return (false);
            }
            return (true);
        }


        public static void Export(ReportDocument report, CRExportEnum ExportFormat, string FileName)
        {
            if (ExportFormat != CRExportEnum.HTML)
            {
                //Export to PDF
                string tempFileName;
                tempFileName = System.IO.Path.GetTempFileName();
                tempFileName = tempFileName.Substring(0, tempFileName.Length - 3);

                if (ExportFormat == CRExportEnum.XLS || ExportFormat == CRExportEnum.XLSDATA)
                    tempFileName += ".xls";
                else if (ExportFormat == CRExportEnum.PDF)
                    tempFileName += ".pdf";
                else if (ExportFormat == CRExportEnum.DOC)
                    tempFileName += ".doc";

                CrystalDecisions.Shared.DiskFileDestinationOptions dfo = new CrystalDecisions.Shared.DiskFileDestinationOptions();
                dfo.DiskFileName = tempFileName;

                CrystalDecisions.Shared.ExportOptions eo = report.ExportOptions;
                eo.DestinationOptions = dfo;
                eo.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;

                if (ExportFormat == CRExportEnum.XLS)
                {
                    eo.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
                    ExcelFormatOptions objExcelOptions = new ExcelFormatOptions();
                    objExcelOptions.ExcelUseConstantColumnWidth = false;
                    eo.FormatOptions = objExcelOptions;
                }
                else if (ExportFormat == CRExportEnum.PDF)
                    eo.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
                else if (ExportFormat == CRExportEnum.DOC)
                    eo.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.RichText;
                else if (ExportFormat == CRExportEnum.XLSDATA)
                    eo.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.ExcelRecord;

                report.Export();

                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();

                if (ExportFormat == CRExportEnum.XLS || ExportFormat == CRExportEnum.XLSDATA)
                {
                    HttpContext.Current.Response.ContentType = "application/x-msexcel";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                }
                else if (ExportFormat == CRExportEnum.PDF)
                {
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                }
                else if (ExportFormat == CRExportEnum.DOC)
                {
                    HttpContext.Current.Response.ContentType = "application/msword";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                }

                HttpContext.Current.Response.WriteFile(tempFileName);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                //HttpContext.Current.Response.Close();

                System.IO.File.Delete(tempFileName);
            }
        }
    }
}
