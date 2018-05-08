using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using UAS.ReportLibrary;
using UAS.Web.Controllers.Common;
using UAS.Web.Models.Circulations;
using UAS.Web.Models.UAD.Filter;
using UAS.Web.Service.Filter;

namespace UAS.Web.Controllers.Report
{
    public class ReportController : BaseController
    {
        // GET: Report
        public ActionResult Index()
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.RPT, KMPlatform.Enums.Access.FullAccess))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        public ActionResult RenderReport(FrameworkUAD.Object.FilterMVC filter, ReportViewModel reportmodel)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.RPT, KMPlatform.Enums.Access.FullAccess))
            {
                string filterQuery = string.Empty;

                if (reportmodel.IssueID > 0)
                {
                    List<FrameworkUAD.Entity.Issue> li = (new FrameworkUAD.BusinessLogic.Issue()).SelectPublication(reportmodel.PubID, CurrentClient.ClientConnections);

                    var allissuelist = new FrameworkUAD.BusinessLogic.Issue().Select(CurrentClient.ClientConnections);

                    FrameworkUAD.Entity.Issue issue = allissuelist.Find(i => i.IssueId == reportmodel.IssueID);

                    if (issue != null && issue.IsClosed && !reportmodel.IsAddRemove)
                        filterQuery = FrameworkUAD.BusinessLogic.FilterMVC.getProductArchiveFilterQuery(filter, "distinct ps.PubSubscriptionID", "", reportmodel.IssueID, CurrentClient.ClientConnections);
                    else
                        filterQuery = FrameworkUAD.BusinessLogic.FilterMVC.getProductFilterQuery(filter, CurrentClient.ClientConnections);
                }
                else
                {
                        filterQuery = FrameworkUAD.BusinessLogic.FilterMVC.getProductFilterQuery(filter, CurrentClient.ClientConnections, reportmodel.IsAddRemove);
                }
                var prod = new FrameworkUAD.BusinessLogic.Product().Select(reportmodel.PubID, CurrentClient.ClientConnections);
                reportmodel.ProductName = prod.PubName;
                var reportCodes = new FrameworkUAD_Lookup.BusinessLogic.Code().Select(FrameworkUAD_Lookup.Enums.CodeType.Report);
                FrameworkUAD.Entity.Report report = new FrameworkUAD.BusinessLogic.Report().Select(CurrentClient.ClientConnections).Where(x => x.ReportID == reportmodel.ReportID).First();
                FrameworkUAD_Lookup.Entity.Code c = reportCodes.SingleOrDefault(x => x.CodeId == report.ReportTypeID);
                ReportUtilities.ClientId = reportmodel.ClientID;
                ReportUtilities.ProductId = reportmodel.PubID;
                string path = Server.MapPath("~/ContentItems/");
                List<FrameworkUAD_Lookup.Entity.Country> countries = new FrameworkUAD_Lookup.BusinessLogic.Country().Select();
                reportmodel.reportSource = ReportUtilities.GetReportSource(c, report, CurrentClient, countries, null, reportmodel.PubID, reportmodel.ProductName, reportmodel.IssueID, reportmodel.IssueName, filterQuery, path, reportmodel.IsAddRemove);
                return PartialView("TelerikReportViewer", reportmodel);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

        }
        public ActionResult ExportReportBPA(FrameworkUAD.Object.FilterMVC filter, ReportViewModel reportmodel, string folderid)
        {
            bool flag = false;
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.RPT, KMPlatform.Enums.Access.FullAccess))
            {
                try
                {
                    var guid = Guid.NewGuid();
                    string filterQuery = "";
                    if (reportmodel.IssueID > 0)
                    {
                        List<FrameworkUAD.Entity.Issue> li = (new FrameworkUAD.BusinessLogic.Issue()).SelectPublication(reportmodel.PubID, CurrentClient.ClientConnections);

                        var allissuelist = new FrameworkUAD.BusinessLogic.Issue().Select(CurrentClient.ClientConnections);

                        FrameworkUAD.Entity.Issue issue = allissuelist.Find(i => i.IssueId == reportmodel.IssueID);

                        if (issue != null && issue.IsClosed)
                        {
                            filterQuery = FrameworkUAD.BusinessLogic.FilterMVC.getProductArchiveFilterQuery(filter, "distinct ps.PubSubscriptionID", "", reportmodel.IssueID, CurrentClient.ClientConnections);
                        }
                        else
                            filterQuery = FrameworkUAD.BusinessLogic.FilterMVC.getProductFilterQuery(filter, CurrentClient.ClientConnections);
                    }
                    else
                    {
                        filterQuery = FrameworkUAD.BusinessLogic.FilterMVC.getProductFilterQuery(filter, CurrentClient.ClientConnections);
                    }
                    reportmodel.filterquery = filterQuery;

                    var prod = new FrameworkUAD.BusinessLogic.Product().Select(reportmodel.PubID, CurrentClient.ClientConnections);
                    reportmodel.ProductName = prod.PubName;

                    FrameworkUAD.Entity.Report rpt = new FrameworkUAD.BusinessLogic.Report().Select(CurrentClient.ClientConnections).Where(x => x.ReportID == reportmodel.ReportID).First();
                    ReportUtilities.ProductId = reportmodel.PubID;
                    ReportUtilities.ClientId = reportmodel.ClientID;
                    string pathDownload = Server.MapPath("~/ContentItems/" + folderid);
                    if (!Directory.Exists(pathDownload))
                        Directory.CreateDirectory(pathDownload);
                    var reportCodes = new FrameworkUAD_Lookup.BusinessLogic.Code().Select(FrameworkUAD_Lookup.Enums.CodeType.Report);
                    List<FrameworkUAD_Lookup.Entity.Country> countries = new FrameworkUAD_Lookup.BusinessLogic.Country().Select();
                    FrameworkUAD_Lookup.Entity.Code c = reportCodes.SingleOrDefault(x => x.CodeId == rpt.ReportTypeID);
                    if (c != null)
                    {
                        Telerik.Reporting.TypeReportSource rptSource = ReportUtilities.GetReportSource(c, rpt, CurrentClient, countries, null, reportmodel.PubID, reportmodel.ProductName, reportmodel.IssueID, reportmodel.IssueName, reportmodel.filterquery, pathDownload);
                        Telerik.Reporting.Processing.ReportProcessor reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();
                        Telerik.Reporting.Processing.RenderingResult result = reportProcessor.RenderReport("PDF", rptSource, null);
                        string fileName = rpt.ReportName.Replace(" ", "") + guid;
                        string fullPath = System.IO.Path.Combine(pathDownload, fileName);
                        using (System.IO.FileStream fs = new System.IO.FileStream(fullPath, System.IO.FileMode.Create))
                        {
                            fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                        }
                    }
                    flag = true;
                }
                catch (Exception ex)
                {
                    flag = false;
                    Core_AMS.Utilities.WPF.Message("An error occurred while exporting your file. Please contact support if error keeps recurring.");
                    KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Desktop;
                    KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                    string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                    alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".ExportBPA", app, "Reports");
                }

            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }


            if (flag)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult DownloadCombinePDF(string folder)
        {
            try
            {
                string folderpath = Server.MapPath("~/ContentItems/" + folder);
                string filepath = folderpath + "Report_Package" + folder + ".pdf";
                Dictionary<int, string> files = new Dictionary<int, string>();
                System.IO.DirectoryInfo di = new DirectoryInfo(folderpath);
                int j = 0;
                foreach (FileInfo file in di.GetFiles())
                {
                    files.Add(j, file.FullName);
                    j++;
                }
                PdfReader reader = null;
                Document sourceDocument = null;
                PdfCopy pdfCopyProvider = null;
                PdfImportedPage importedPage;

                sourceDocument = new Document();
                pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(filepath, System.IO.FileMode.Create));

                //Open the output file
                sourceDocument.Open();

                try
                {
                    //Loop through the files list
                    for (int f = 0; f < files.Count; f++)
                    {
                        int pages = Get_Page_Count(files[f]);

                        reader = new PdfReader(files[f]);
                        //Add pages of current file
                        for (int i = 1; i <= pages; i++)
                        {
                            importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                            pdfCopyProvider.AddPage(importedPage);
                        }

                        reader.Close();
                    }
                    //At the end save the output file
                    sourceDocument.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                for (int i = 0; i < files.Count; i++)
                {
                    System.IO.File.Delete(files[i]);
                }
                byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
                System.IO.File.Delete(filepath);
                var dir = new DirectoryInfo(folderpath);
                dir.Delete();
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Pdf, "Report Package.pdf");
            }
            catch (Exception ex)
            {
                Core_AMS.Utilities.WPF.Message("An error occurred while exporting your file. Please contact support if error keeps recurring.");
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Desktop;
                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".ExportBPA", app, "Reports");
                return RedirectToAction("Error", "Error", new { errorType = "FileNotFound" });
            }
        }
        public FileResult GetFile(string guid, int PubID)
        {
            string pathDownload = Server.MapPath("~/ContentItems/" + CurrentUser.UserID + "/" + CurrentClient.ClientID + "/" + PubID + "/") + "ReportPackage_" + CurrentClient.ClientID + "_" + CurrentUser.UserID + "_" + guid + ".pdf";
            byte[] fileBytes = System.IO.File.ReadAllBytes(pathDownload);
            System.IO.File.Delete(pathDownload);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Pdf, "Report Package.pdf");
        }
        //public ActionResult ExportReportPackages(FrameworkUAD.Object.FilterMVC filter, ReportViewModel reportmodel)
        //{
        //    if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.RPT, KMPlatform.Enums.Access.FullAccess))
        //    {
        //        var guid = Guid.NewGuid();
        //        string filterQuery = "";
        //        if (reportmodel.IssueID > 0)
        //        {
        //            List<FrameworkUAD.Entity.Issue> li = (new FrameworkUAD.BusinessLogic.Issue()).SelectPublication(reportmodel.PubID, CurrentClient.ClientConnections);

        //            var allissuelist = new FrameworkUAD.BusinessLogic.Issue().Select(CurrentClient.ClientConnections);

        //            FrameworkUAD.Entity.Issue issue = allissuelist.Find(i => i.IssueId == reportmodel.IssueID);

        //            if (issue != null && issue.IsClosed)
        //            {
        //                filterQuery = FrameworkUAD.BusinessLogic.FilterMVC.getProductArchiveFilterQuery(filter, "distinct ps.PubSubscriptionID", "", reportmodel.IssueID, CurrentClient.ClientConnections);
        //            }
        //            else
        //                filterQuery = FrameworkUAD.BusinessLogic.FilterMVC.getProductFilterQuery(filter, CurrentClient.ClientConnections);
        //        }
        //        else
        //        {
        //            filterQuery = FrameworkUAD.BusinessLogic.FilterMVC.getProductFilterQuery(filter, CurrentClient.ClientConnections);
        //        }
        //        reportmodel.filterquery = filterQuery;
        //        var prod = new FrameworkUAD.BusinessLogic.Product().Select(reportmodel.PubID, CurrentClient.ClientConnections);
        //        reportmodel.ProductName = prod.PubName;
        //        List<FrameworkUAD.Entity.Report> reports = new FrameworkUAD.BusinessLogic.Report().Select(CurrentClient.ClientConnections).Where(x => reportmodel.ReportIDs.Contains(x.ReportID)).ToList();
        //        if (ExportBPA(reports, reportmodel, guid))
        //        {
        //            return Json(guid, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(0, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
        //    }

        //}



        //private bool ExportBPA(List<FrameworkUAD.Entity.Report> reports, ReportViewModel reportmodel, Guid guid)
        //{
        //    bool returnFlag = false;
        //    Dictionary<int, string> files = new Dictionary<int, string>();
        //    ReportUtilities.ProductID = reportmodel.PubID;
        //    ReportUtilities.ClientID = reportmodel.ClientID;
        //    string pathDownload = Server.MapPath("~/ContentItems/"+CurrentUser.UserID+"/"+CurrentClient.ClientID+"/"+ reportmodel.PubID+"/");//System.IO.Path.Combine(pathUser, "Downloads");
        //    if (!Directory.Exists(pathDownload))
        //        Directory.CreateDirectory(pathDownload);
        //    System.IO.DirectoryInfo di = new DirectoryInfo(pathDownload);
        //    foreach (FileInfo file in di.GetFiles())
        //    {
        //        file.Delete();
        //    }
        //    string filePath = pathDownload + "ReportPackage_" + CurrentClient.ClientID+"_" + CurrentUser.UserID+"_"+ guid+ ".pdf";
        //    var reportCodes = new FrameworkUAD_Lookup.BusinessLogic.Code().Select(FrameworkUAD_Lookup.Enums.CodeType.Report);
        //    List<FrameworkUAD_Lookup.Entity.Country> countries = new FrameworkUAD_Lookup.BusinessLogic.Country().Select();
        //    try
        //    {
        //        int reportCount = 0;
        //        foreach (FrameworkUAD.Entity.Report rpt in reports)
        //        {

        //            FrameworkUAD_Lookup.Entity.Code c = reportCodes.SingleOrDefault(x => x.CodeId == rpt.ReportTypeID);
        //            if (c != null)
        //            {

        //                Telerik.Reporting.TypeReportSource rptSource = ReportUtilities.GetReportSource(c, rpt, CurrentClient, countries, null, reportmodel.PubID, reportmodel.ProductName, reportmodel.IssueID, reportmodel.IssueName, reportmodel.filterquery, pathDownload);
        //                Telerik.Reporting.Processing.ReportProcessor reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();
        //                Telerik.Reporting.Processing.RenderingResult result =reportProcessor.RenderReport("PDF", rptSource, null);
        //                string fileName = rpt.ReportName.Replace(" ", "")+guid;
        //                string fullPath = System.IO.Path.Combine(pathDownload, fileName);
        //                using (System.IO.FileStream fs = new System.IO.FileStream(fullPath, System.IO.FileMode.Create))
        //                {
        //                    fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
        //                }
        //                files.Add(reportCount, fullPath);
        //                reportCount++;
        //            }
        //        }

        //        if (files.Count == reports.Count)
        //        {
        //            CombinePDFs(files, filePath);
        //            returnFlag = true;
        //        }
        //        else
        //        {
        //            foreach (var r in reports)
        //            {
        //                try
        //                {
        //                    if (files.ContainsKey(r.ReportID) == false)
        //                    {
        //                        FrameworkUAD_Lookup.Entity.Code c = reportCodes.SingleOrDefault(x => x.CodeId == r.ReportTypeID);
        //                        if (c != null)
        //                        {
        //                            Telerik.Reporting.TypeReportSource rptSource = ReportUtilities.GetReportSource(c, r, CurrentClient, countries, null, reportmodel.PubID, reportmodel.ProductName, reportmodel.IssueID, reportmodel.IssueName, reportmodel.filterquery, pathDownload);
        //                            Telerik.Reporting.Processing.ReportProcessor reportProcessor =  new Telerik.Reporting.Processing.ReportProcessor();
        //                            Telerik.Reporting.Processing.RenderingResult result =reportProcessor.RenderReport("PDF", rptSource, null);
        //                            string fileName = r.ReportName.Replace(" ", "") + guid;
        //                            string fullPath = System.IO.Path.Combine(pathDownload, fileName);
        //                            using (System.IO.FileStream fs = new System.IO.FileStream(fullPath, System.IO.FileMode.Create))
        //                            {
        //                                fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
        //                            }
        //                            files.Add(r.ReportID, fullPath);
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    returnFlag = false;
        //                    Core_AMS.Utilities.WPF.Message("Some of your requested reports failed to generate. Please contact support if error keeps recurring.");
        //                    KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Desktop;
        //                    KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
        //                    string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //                    alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".ExportBPA", app, "Reports");
        //                }
        //            }
        //            CombinePDFs(files, filePath);
        //            returnFlag = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        returnFlag = false;
        //        Core_AMS.Utilities.WPF.Message("An error occurred while exporting your file. Please contact support if error keeps recurring.");
        //        KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Desktop;
        //        KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
        //        string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //        alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".ExportBPA", app, "Reports");
        //    }

        //    return returnFlag;
        //}


        //private void CombinePDFs(Dictionary<int, string> files, string filepath)
        //{
        //    PdfReader reader = null;
        //    Document sourceDocument = null;
        //    PdfCopy pdfCopyProvider = null;
        //    PdfImportedPage importedPage;

        //    sourceDocument = new Document();
        //    pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(filepath, System.IO.FileMode.Create));

        //    //Open the output file
        //    sourceDocument.Open();

        //    try
        //    {
        //        //Loop through the files list
        //        for (int f = 0; f < files.Count; f++)
        //        {
        //            int pages = Get_Page_Count(files[f]);

        //            reader = new PdfReader(files[f]);
        //            //Add pages of current file
        //            for (int i = 1; i <= pages; i++)
        //            {
        //                importedPage = pdfCopyProvider.GetImportedPage(reader, i);
        //                pdfCopyProvider.AddPage(importedPage);
        //            }

        //            reader.Close();
        //        }
        //        //At the end save the output file
        //        sourceDocument.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    for (int i = 0; i < files.Count; i++)
        //    {
        //        System.IO.File.Delete(files[i]);
        //    }
        //}

        private int Get_Page_Count(string file)
        {
            using (StreamReader sr = new StreamReader(System.IO.File.OpenRead(file)))
            {
                Regex regex = new Regex(@"/Type\s*/Page[^s]");
                MatchCollection matches = regex.Matches(sr.ReadToEnd());

                return matches.Count;
            }
        }

        public ActionResult GetCircIssues(int pubID)
        {
            var allissuelist = new FrameworkUAD.BusinessLogic.Issue().Select(CurrentClient.ClientConnections);
            List<FrameworkUAD.Entity.Issue> issueList = allissuelist.Where(x => x.PublicationId == pubID && x.IsComplete == true).OrderByDescending(x => x.IssueId).ToList();
            FrameworkUAD.Entity.Issue issueNotComplete = allissuelist.Where(x => x.PublicationId == pubID && x.IsComplete == false).First();
            List<FrameworkUAD.Entity.Issue> finalList = new List<FrameworkUAD.Entity.Issue>();
            if (issueNotComplete != null)
            {
                issueNotComplete.IssueId = 0;
                finalList.Add(issueNotComplete);
                finalList.AddRange(issueList);
            }

            return Json(finalList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReports(int pubID, bool includeBPA = true)
        {
            List<FrameworkUAD.Entity.Report> reportList = new List<FrameworkUAD.Entity.Report>();
            if (includeBPA)
                reportList.Add(new FrameworkUAD.Entity.Report() { ReportID = -1, ReportName = "--Select Report--", ProductID = pubID });
            var reports = new FrameworkUAD.BusinessLogic.Report().Select(CurrentClient.ClientConnections);
            var rptList = reports.Where(x => x.ProductID == pubID).OrderBy(x => x.ReportName).ToList();
            if (includeBPA)
                rptList.Add(new FrameworkUAD.Entity.Report() { ReportID = 0, ReportName = "Report Package" });
            reportList.AddRange(rptList);
            return Json(reportList, JsonRequestBehavior.AllowGet);
        }

    }
}