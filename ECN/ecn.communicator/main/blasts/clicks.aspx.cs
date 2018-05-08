using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Activity;
using KM.Common.Extensions;
using Microsoft.Reporting.WebForms;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using Blast = ECN_Framework_BusinessLayer.Communicator.Blast;
using BlastAbstract = ECN_Framework_Entities.Communicator.BlastAbstract;
using BusinessLayerReport = ECN_Framework_BusinessLayer.Activity.Report;
using FrameworkEntitiesReport = ECN_Framework_Entities.Activity.Report;
using FrameworkEntitiesUniqueLink = ECN_Framework_Entities.Communicator.UniqueLink;
using RSSFeed = ECN_Framework_BusinessLayer.Communicator.ContentReplacement.RSSFeed;

namespace ecn.communicator.blastsmanager
{
    public partial class clicks_main : ECN_Framework.WebPageHelper
    {
        private const string ClicksByURLReport = "Clicks by URL Report";
        private const string HelpContent = "<p><b>Clicks by URL </b><br />Lists all recepients who clicked on the URL links in your email Blast<br />Displays the time clicked, the URL link clicked.<br />Click on the email address to view the profile of that email address.";
        private const string TopClicks = "topclicks";
        private const string AllClicks = "allclicks";
        private const string All = "all";
        private const string TopVisitors = "topvisitors";
        private const string HeatMap = "heatmap";
        private const string Selected = "selected";
        private const string TopClicksSortField = "tcSortField";
        private const string TopClicksSortDirection = "tcSortDirection";
        private const string TopVisitorSortField = "tvSortField";
        private const string TopVisitorsSortDirection = "tvSortDirection";
        private const string AllClicksSortField = "acSortField";
        private const string AllClicksSortDirection = "acSortDirection";
        private const string AssociatedLayoutTemplateRemoved = "Associated Layout or Template has been removed.";
        private const string Space = " ";
        private const string HashHtmlEncoding = "%23";
        private const string Hash = "#";
        private const string AmpersandHtmlEncoding = "&amp;";
        private const string Ampersand = "&";
        private const string Zero = "0";
        private const string Summary = "summary";
        private const string NameBlastId = "BlastID";
        private const string NameCampaignItemId = "CampaignItemID";
        private const string NameReportPath = "ReportPath";
        private const string NameExcel = "EXCEL";
        private const string ExcelApplicationType = "application/vnd.ms-excel";
        private const string ContentDisposition = "content-disposition";
        private const string BlastClickSummary = "BlastClickSummary";
        private const string BlastClickDetail = "BlastClickDetail";
        private const string Detail = "detail";
        private const string ClickCount = "ClickCount";
        private const string BytesIsNull = "bytes is null";
        private const string DefaultBlastId = "-1";

        private delegate DataSet DlgGetBlastGroupClicksData(string reportType, int pageNo, int pageSize, string startDateValue = "", string endDateValue = "", bool unique = false);

        public int allclicksCurrentPage
        {
            set { ViewState["allclicksCurrentPage"] = value; }
            get { return Convert.ToInt32(ViewState["allclicksCurrentPage"]); }
        }

        public int topClickCurrentPage
        {
            set { ViewState["topClickCurrentPage"] = value; }
            get { return Convert.ToInt32(ViewState["topClickCurrentPage"]); }
        }

		private static List<ECN_Framework_Entities.Activity.Report.BlastClickSummary_SubReport> subReportList;

        ArrayList columnHeadings = new ArrayList();
        IEnumerator aListEnum = null;

        DataTable emailstable;

        #region getURL Params


        public int getBlastID()
        {
            if (Request.QueryString["BlastID"] != null)
                return Convert.ToInt32(Request.QueryString["BlastID"].ToString());
            else
                return 0;
        }

        public int getCampaignItemID()
        {
            if (Request.QueryString["CampaignItemID"] != null)
                return Convert.ToInt32(Request.QueryString["CampaignItemID"].ToString());
            else
                return 0;
        }

        public string getClicksLinkURL()
        {
            try
            {
                var returnURL = string.Empty;

                if (getBlastID() > 0)
                {
                    returnURL = "?BlastID=" + getBlastID();

                    if (getUDFData() != string.Empty)
                        returnURL += "&UDFName=" + getUDFName() + "&UDFData=" + getUDFData();

                    return returnURL;
                }
                else
                {
                    return "?CampaignItemID=" + getCampaignItemID();
                }
            }
            catch (Exception E) { return "?BlastID=" + getBlastID(); }
        }

        private string getAction()
        {
            try { return Request.QueryString["action"].ToString(); }
            catch { return string.Empty; }
        }

        private string getActionURL()
        {
            try { return Request.QueryString["actionURL"].ToString(); }
            catch { return string.Empty; }

        }

        private string getISP()
        {
            try
            {
                if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ISPReporting))
                    return Request.QueryString["isp"].ToString();
                else
                    return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string getUDFName()
        {
            try
            {
                return Request.QueryString["UDFName"].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.SetValues(MenuCode.REPORTS, string.Empty, ClicksByURLReport, HelpContent, BlastManager);

            if (!IsPostBack)
            {

                if (getCampaignItemID() != 0)
                {
                    DateTime campaignSendDate = new DateTime();
                    ECN_Framework_Entities.Communicator.CampaignItem campaignItem = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(getCampaignItemID(), false);
                    campaignSendDate = campaignItem.SendTime ?? DateTime.Now.AddDays(-365);

                    txtstartDate.Text = campaignSendDate.ToString("MM/dd/yyyy");
                    txtendDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else if (getBlastID() != 0)
                {
                    DateTime blastSendDate = new DateTime();
                    ECN_Framework_Entities.Communicator.Blast blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(getBlastID(), false);
                    blastSendDate = blast.SendTime ?? DateTime.Now.AddDays(-365);

                    txtstartDate.Text = blastSendDate.ToString("MM/dd/yyyy");
                    txtendDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    txtstartDate.Text = DateTime.Now.AddDays(-14).ToString("MM/dd/yyyy");
                    txtendDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                ViewState[TopClicksSortField] = ClickCount;
                ViewState[TopClicksSortDirection] = "DESC";

                ViewState[TopVisitorSortField] = ClickCount;
                ViewState[TopVisitorsSortDirection] = "DESC";

                ViewState[AllClicksSortField] = "ClickTime";
                ViewState[AllClicksSortDirection] = "DESC";

                if (getBlastID() > 0 && KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportClicks, KMPlatform.Enums.Access.DownloadDetails))
                {
                    TopGrid.Columns[4].Visible = true;
                }
                else
                {
                    TopGrid.Columns[4].Visible = false;
                }
                topClickCurrentPage = 0;
                allclicksCurrentPage = 0;

                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportClicks, KMPlatform.Enums.Access.View))
                {
                    loadGrid(TopClicks);
                    ShowHideColumns();
                    ShowHideDownload_withDetails(TopClicks);

                    if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportClicks, KMPlatform.Enums.Access.ViewDetails))
                    {
                        btnTopVisitors.Visible = false;
                        btnClicksbyTime.Visible = false;
                    }
                    else
                    {
                        btnTopVisitors.Visible = true;
                        btnClicksbyTime.Visible = true;

                    }
                    if (getAction().Equals("report"))
                    {
                        DownloadClickReport();
                    }
                }
                else
                {
                    throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
                }
            }
        }

        private void loadGrid(string reportType)
        {
            lblDateRangeError.Visible = false;
            DateTime startDate;
            DateTime endDate;
            DateTime.TryParse(txtstartDate.Text, out startDate);
            DateTime.TryParse(txtendDate.Text, out endDate);
            DownloadEmailsButton.Enabled = false;

            var dataSet = new DataSet();

            var hasViewDetails = KM.Platform.User.HasAccess(
                Master.UserSession.CurrentUser,
                KMPlatform.Enums.Services.EMAILMARKETING,
                KMPlatform.Enums.ServiceFeatures.BlastReportClicks,
                KMPlatform.Enums.Access.ViewDetails);

            DlgGetBlastGroupClicksData dlgGetBlastGroupClicksData;

            if (getBlastID() > 0)
            {
                dlgGetBlastGroupClicksData = GetBlastGroupClicksDataBlast;
            }
            else
            {
                dlgGetBlastGroupClicksData = GetBlastGroupClicksDataCampaign;
            }

            if (reportType.EqualsIgnoreCase(TopClicks))
            {
                dataSet = dlgGetBlastGroupClicksData(reportType, topClickCurrentPage, TopClicksPager.PageSize);
            }
            else if (reportType.EqualsIgnoreCase(AllClicks))
            {
                if (ValidateDateRange(startDate, endDate))
                {
                    var isAll = ddlClicksType.SelectedValue == All;
                    dataSet = dlgGetBlastGroupClicksData(reportType, allclicksCurrentPage, ClicksPager.PageSize, startDate.ToShortDateString(), endDate.ToShortDateString(), !isAll);
                }
                else
                {
                    lblDateRangeError.Visible = true;
                    return;
                }
            }
            else if (reportType.EqualsIgnoreCase(TopVisitors) && hasViewDetails)
            {
                dataSet = dlgGetBlastGroupClicksData(reportType, 0, 50);
            }

            ProcessReportType(reportType, dataSet, hasViewDetails);
        }

        private void ProcessReportType(string reportType, DataSet dataSet, bool hasViewDetails)
        {
            if (reportType.EqualsIgnoreCase(TopClicks))
            {
                TopGrid.DataSource = new DataView(dataSet.Tables[0]) { Sort = $"{ViewState[TopClicksSortField]}{' '}{ViewState[TopClicksSortDirection]}" };
                TopGrid.DataBind();
                TopClicksPager.RecordCount = dataSet.Tables[0].Rows.Count;
                TopClicksPager.Visible = dataSet.Tables[0].Rows.Count > 0;
                SetReportTypeUserInterface(isTopClicks: true);
            }
            else if (reportType.EqualsIgnoreCase(TopVisitors) && hasViewDetails)
            {
                EmailsGrid.DataSource = new DataView(dataSet.Tables[0]) { Sort = $"{ViewState[TopVisitorSortField]}{' '}{ViewState[TopVisitorsSortDirection]}" };
                EmailsGrid.DataBind();
                SetReportTypeUserInterface(isTopVisitor: true);
            }
            else if (reportType.EqualsIgnoreCase(AllClicks))
            {
                ClicksGrid.DataSource = new DataView(dataSet.Tables[1]) { Sort = $"{ViewState[AllClicksSortField]}{' '}{ViewState[AllClicksSortDirection]}" };
                ClicksGrid.CurrentPageIndex = allclicksCurrentPage;
                ClicksGrid.DataBind();

                Func<string, int> intTryParse = (input) =>
                    {
                        int returnValue;

                        if (!int.TryParse(input, out returnValue))
                        {
                            throw new InvalidCastException($"{input} cannot be parsed to integer");
                        }

                        return returnValue;
                    };

                ClicksPager.RecordCount = intTryParse(dataSet.Tables[0].Rows[0][0].ToString());

                DownloadEmailsButton.Enabled = true;
                DownloadPanel.Visible = true;
                ClicksPager.Visible = true;
                SetReportTypeUserInterface(isClicksByTime: true);
            }
            else if (reportType.EqualsIgnoreCase(HeatMap))
            {
                if (getBlastID() > 0)
                {
                    ShowHeatMap(getBlastID(), true);
                }
                else
                {
                    ShowHeatMap(getCampaignItemID(), false);
                }

                SetReportTypeUserInterface(isClicksHeatMap: true);
            }
        }

        private void SetReportTypeUserInterface(bool isTopClicks = false, bool isTopVisitor = false, bool isClicksByTime = false, bool isClicksHeatMap = false)
        {
            phTopClicks.Visible = isTopClicks;
            phTopVisitors.Visible = isTopVisitor;
            phClicksbyTime.Visible = isClicksByTime;
            phClicksHeatMap.Visible = isClicksHeatMap;
            btnTopClicks.CssClass = isTopClicks
                                        ? Selected
                                        : string.Empty;
            btnTopVisitors.CssClass = isTopVisitor
                                          ? Selected
                                          : string.Empty;
            btnClicksbyTime.CssClass = isClicksByTime
                                           ? Selected
                                           : string.Empty;
            btnClicksHeatMap.CssClass = isClicksHeatMap
                                            ? Selected
                                            : string.Empty;
        }

        private DataSet GetBlastGroupClicksData(
            int? campaignItemId,
            int? getBlastId,
            string reportType,
            int pageNo,
            int pageSize,
            string startDateValue = "",
            string endDateValue = "",
            bool unique = false)
        {
            return Blast.GetBlastGroupClicksData(
                campaignItemId,
                getBlastId,
                ClickSelectionDD.SelectedValue.Trim(),
                getISP(),
                reportType,
                string.Empty,
                pageNo,
                pageSize,
                getUDFName(),
                getUDFData(),
                startDateValue,
                endDateValue,
                unique);
        }

        private DataSet GetBlastGroupClicksDataBlast(
            string reportType,
            int pageNo,
            int pageSize,
            string startDateValue = "",
            string endDateValue = "",
            bool unique = false)
        {
            return GetBlastGroupClicksData(null, getBlastID(), reportType, pageNo, pageSize, startDateValue, endDateValue, unique);
        }

        private DataSet GetBlastGroupClicksDataCampaign(
            string reportType,
            int pageNo,
            int pageSize,
            string startDateValue = "",
            string endDateValue = "",
            bool unique = false)
        {
            return GetBlastGroupClicksData(getCampaignItemID(), null, reportType, pageNo, pageSize, startDateValue, endDateValue, unique);
        }

        #region downloads
        public void downloadClickEmails(object sender, System.EventArgs e)
        {
            lblDateRangeError.Visible = false;
            string newline = string.Empty;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            DateTime.TryParse(txtstartDate.Text, out startDate);
            DateTime.TryParse(txtendDate.Text, out endDate);
            if (ValidateDateRange(startDate, endDate))
            {
                string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/downloads/");
                if (!Directory.Exists(txtoutFilePath))
                    Directory.CreateDirectory(txtoutFilePath);

                string downloadType = DownloadType.SelectedItem.Value.ToString();
                string clicksType = ddlClicksType.SelectedItem.Value.ToString();

                DateTime date = DateTime.Now;
                String tfile = Master.UserSession.CurrentUser.CustomerID + "-" + getBlastID() + "-click-emails" + downloadType;
                string outfileName = txtoutFilePath + tfile;


                if (File.Exists(outfileName))
                {
                    File.Delete(outfileName);
                }

                TextWriter txtfile = File.AppendText(outfileName);


                emailstable = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.DownloadBlastReportDetails(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "click", clicksType, getISP(), Master.UserSession.CurrentUser, startDate.ToShortDateString(), endDate.ToShortDateString());

                string endFile = string.Empty;
                if (downloadType.Equals(".csv") || downloadType.Equals(".txt"))
                {
                    endFile = ECN_Framework_Common.Functions.DataTableFunctions.ToCSV(emailstable);
                }
                else if (downloadType.Equals(".xls"))
                {
                    endFile = ECN_Framework_Common.Functions.DataTableFunctions.ToTabDelimited(emailstable);
                }

                txtfile.Write(endFile);
                txtfile.Close();
                //for (int i = 0; i < emailstable.Columns.Count; i++)
                //{
                //    columnHeadings.Add(emailstable.Columns[i].ColumnName.ToString());
                //}

                //aListEnum = columnHeadings.GetEnumerator();
                //while (aListEnum.MoveNext())
                //{
                //    newline += aListEnum.Current.ToString() + (downloadType == ".xls" ? "\t" : ", ");
                //}
                //txtfile.WriteLine(newline);

                //foreach (DataRow dr in emailstable.Rows)
                //{
                //    newline = "";
                //    aListEnum.Reset();
                //    while (aListEnum.MoveNext())
                //    {
                //        newline += dr[aListEnum.Current.ToString()].ToString() + (downloadType == ".xls" ? "\t" : ", ");
                //    }
                //    txtfile.WriteLine(newline);
                //}
                //txtfile.Close();

                if (downloadType == ".xls")
                {
                    Response.ContentType = "application/vnd.ms-excel";
                }
                else
                {
                    Response.ContentType = "text/csv";
                }
                Response.AddHeader("content-disposition", "attachment; filename=" + tfile);
                Response.WriteFile(outfileName);
                Response.Flush();
                Response.End();

            }
            else
            {
                ClicksGrid.DataSource = null;
                ClicksGrid.DataBind();
                ClicksPager.RecordCount = 0;


                lblDateRangeError.Visible = true;
            }
        }

        public void DownloadClickReport()
        {
            DataTable dt = ECN_Framework_BusinessLayer.Communicator.Blast.ClickActivityDetailedReport(getBlastID(), Master.UserSession.CurrentUser.CustomerID, getActionURL().ToString());
            string newline = string.Empty;
            string contentType = "application/vnd.ms-excel";
            string responseFile = "Clicks.xls";
            string outFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/downloads/");

            if (!Directory.Exists(outFilePath))
                Directory.CreateDirectory(outFilePath);

            DateTime date = DateTime.Now;
            String tfile = getBlastID().ToString() + "_Clicks.XLS";
            string outfileName = outFilePath + tfile;

            if (File.Exists(outfileName))
            {
                File.Delete(outfileName);
            }

            TextWriter txtfile = File.AppendText(outfileName);

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                columnHeadings.Add(dt.Columns[i].ColumnName.ToString());
            }
            aListEnum = columnHeadings.GetEnumerator();
            while (aListEnum.MoveNext())
            {
                newline += aListEnum.Current.ToString() + "\t";
            }
            txtfile.WriteLine(newline);

            foreach (DataRow dr in dt.Rows)
            {
                newline = string.Empty;
                aListEnum.Reset();
                while (aListEnum.MoveNext())
                {
                    newline += dr[aListEnum.Current.ToString()].ToString() + "\t";
                }
                txtfile.WriteLine(newline);
            }

            txtfile.Close();

            Response.ContentType = contentType;
            Response.AddHeader("content-disposition", "attachment; filename=" + responseFile);
            Response.WriteFile(outfileName);
            Response.Flush();
            Response.End();

            if (File.Exists(outfileName))
            {
                File.Delete(outfileName);
            }
        }

        private void ShowHeatMap(int blastId, bool isBlast)
        {
            string body;
            BlastAbstract blast;
            var blastIds = ShowHeatMapProcessBlasts(blastId, isBlast, out blast);

            if (blast.Layout == null)
            {
                body = AssociatedLayoutTemplateRemoved;
                LabelPreview.Text = body;
            }
            else
            {
                body = Layout.EmailBody_NoAccessCheck(
                    blast.Layout.Template.TemplateSource,
                    blast.Layout.Template.TemplateText,
                    blast.Layout.TableOptions,
                    blast.Layout.ContentSlot1,
                    blast.Layout.ContentSlot2,
                    blast.Layout.ContentSlot3,
                    blast.Layout.ContentSlot4,
                    blast.Layout.ContentSlot5,
                    blast.Layout.ContentSlot6,
                    blast.Layout.ContentSlot7,
                    blast.Layout.ContentSlot8,
                    blast.Layout.ContentSlot9,
                    ContentTypeCode.HTML,
                    false,
                    null,
                    null,
                    blast.BlastID);

                var clickList = new List<BlastActivityClicks>();
                var uniqueLinkList = new List<ECN_Framework_Entities.Communicator.UniqueLink>();

                foreach (var id in blastIds)
                {
                    clickList.AddRange(ECN_Framework_BusinessLayer.Activity.BlastActivityClicks.GetByBlastID(id));
                    uniqueLinkList.AddRange(UniqueLink.GetByBlastID(id));
                }

                RSSFeed.Replace(
                    ref body,
                    ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID,
                    false,
                    blast.BlastID);

                var totalClicks = clickList.Count;

                if (totalClicks > 0)
                {
                    var linkRegex = new Regex(
                        @"
 <\s*               # any whitespace character
([^>]*)            # any character that doesn't close the tag
(href\s*=\s*)      # followed by href=, with spaces allowed on either side of the equal sign
([""'])            # followed by open single or double quote
([^>]*?([^>]*?))  # followed by some text that doesn't close the tag 
\3                 # followed by a close quote matching the prior open quote type
([^>]*>)           # followed by some more stuff and then a close tag
",
                        RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);

                    var ecnIdRe = new Regex(@"ecn_id=([""']?)(.*?)\1", RegexOptions.IgnorePatternWhitespace);
                    Func<Match, string> getEcnId = linkMatch =>
                        {
                            var toSearch = string.Join(Space, linkMatch.Groups[1].Value, linkMatch.Groups[6].Value);
                            var ecnIdMatch = ecnIdRe.Match(toSearch);
                            return ecnIdMatch.Success
                                       ? ecnIdMatch.Groups[2].Value
                                       : null;
                        };

                    body = ProcessLinkRegex(clickList, linkRegex, body, getEcnId, uniqueLinkList);
                }

                LabelPreview.Text = body;
            }
        }

        private List<int> ShowHeatMapProcessBlasts(int blastId, bool isBlast, out BlastAbstract blast)
        {
            var blastIds = new List<int>();

            if (!isBlast)
            {
                foreach (var campaignItemBlast in CampaignItemBlast.GetByCampaignItemID_NoAccessCheck(getCampaignItemID(), false))
                {
                    if (campaignItemBlast.BlastID != null)
                    {
                        blastIds.Add(campaignItemBlast.BlastID.Value);
                    }
                }
            }
            else
            {
                blastIds.Add(blastId);
            }

            blast = Blast.GetByBlastID_NoAccessCheck(blastIds.FirstOrDefault(), true);
            return blastIds;
        }

        private string ProcessLinkRegex(List<BlastActivityClicks> clickList, Regex linkRegex, string body, Func<Match, string> getEcnId, List<FrameworkEntitiesUniqueLink> uniqueLinkList)
        {
            var resultList = (from src in clickList
                              group src by new { src.URL, src.UniqueLinkID }
                              into gp
                              orderby gp.Count() descending
                              select new { gp.Key.URL, gp.Key.UniqueLinkID, Clicks = gp.Count() }).ToList();

            foreach (Match match in linkRegex.Matches(body))
            {
                var ecnId = getEcnId(match);
                var originalA = match.Groups[0].Value;
                var toReplaceLater = string.Empty;

                try
                {
                    var url = match.Groups[5].Value.Replace(HashHtmlEncoding, Hash);
                    var indexOfHref = originalA.IndexOf(match.Groups[2].Value);

                    if (string.IsNullOrWhiteSpace(ecnId))
                    {
                        var clickCount = (from result in resultList
                                          group result by result.URL
                                          into gp2
                                          where gp2.Key.Replace(AmpersandHtmlEncoding, Ampersand).Replace(HashHtmlEncoding, Hash)
                                                == url.Replace(AmpersandHtmlEncoding, Ampersand).Replace(HashHtmlEncoding, Hash)
                                          select gp2.Sum(c => c.Clicks)).ToArray();

                        if (!clickCount.FirstOrDefault().ToString().Equals(Zero))
                        {
                            var clicksCount = clickCount.Sum();
                            toReplaceLater = originalA.Insert(indexOfHref - 1, $" clickvalue=\"{clicksCount}\" ");
                        }
                    }
                    else
                    {
                        var actualClickCount = (from result in resultList
                                                join uLink in uniqueLinkList on result.UniqueLinkID equals uLink.UniqueLinkID
                                                where result.URL.Replace(AmpersandHtmlEncoding, Ampersand).Replace(HashHtmlEncoding, Hash)
                                                      == url.Replace(AmpersandHtmlEncoding, Ampersand).Replace(HashHtmlEncoding, Hash)
                                                      && uLink.UniqueID == ecnId
                                                select result.Clicks).Sum();

                        if (actualClickCount != 0)
                        {
                            toReplaceLater = originalA.Insert(indexOfHref - 1, $@" clickvalue=""{actualClickCount}""");
                        }
                    }

                    var firstIndexOf = body.IndexOf(originalA);

                    if (!string.IsNullOrWhiteSpace(toReplaceLater))
                    {
                        body = body.Remove(firstIndexOf, originalA.Length);
                        body = body.Insert(firstIndexOf, toReplaceLater.Replace(HashHtmlEncoding, Hash));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.ToString());
                }
            }

            return body;
        }

        #endregion
        
        public void ClickSelectionDD_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            TopGrid.CurrentPageIndex = 0;
            TopClicksPager.CurrentPage = 1;
            loadGrid(TopClicks);
        }

        protected void btnTopClicks_Click(object sender, System.EventArgs e)
        {
            if (KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportClicks, KMPlatform.Enums.Access.View))
            {
                ShowHideDownload_withDetails(TopClicks);
                loadGrid(TopClicks);
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }
        }

        protected void btnTopVisitors_Click(object sender, System.EventArgs e)
        {
            if (KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportClicks, KMPlatform.Enums.Access.ViewDetails))
            {
                loadGrid(TopVisitors);
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }
        }

        protected void btnClicksbyTime_Click(object sender, System.EventArgs e)
        {
            if (KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportClicks, KMPlatform.Enums.Access.ViewDetails))
            {
                loadGrid(AllClicks);
                ShowHideDownload_withDetails();
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }
        }

        protected void btnClicksHeatMap_Click(object sender, System.EventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportClicks, KMPlatform.Enums.Access.View))
            {
                loadGrid("HeatMap");
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }

        }

        protected void ClicksPager_IndexChanged(object sender, EventArgs e)
        {
            allclicksCurrentPage = ClicksPager.CurrentPage - 1;
            loadGrid(AllClicks);
        }

        protected void TopClicksPager_IndexChanged(object sender, EventArgs e)
        {
            topClickCurrentPage = TopClicksPager.CurrentPage - 1;
            loadGrid(TopClicks);
        }

        protected void btnHome_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("reports.aspx?" + (getBlastID() > 0 ? "BlastID=" + getBlastID() : "CampaignItemID=" + getCampaignItemID()) + (getUDFData() != string.Empty ? "&UDFName=" + getUDFName() + "&UDFdata=" + getUDFData() : string.Empty));
        }

        public void TopGrid_Sort(Object sender, DataGridSortCommandEventArgs e)
        {

            if (e.SortExpression.ToString() == ViewState[TopClicksSortField].ToString())
            {
                switch (ViewState[TopClicksSortDirection].ToString())
                {
                    case "ASC":
                        ViewState[TopClicksSortDirection] = "DESC";
                        break;
                    case "DESC":
                        ViewState[TopClicksSortDirection] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState[TopClicksSortField] = e.SortExpression;
                ViewState[TopClicksSortDirection] = "ASC";
            }
            loadGrid(TopClicks);
        }

        public void EmailsGrid_Sort(Object sender, DataGridSortCommandEventArgs e)
        {

            if (e.SortExpression.ToString() == ViewState[TopVisitorSortField].ToString())
            {
                switch (ViewState[TopVisitorsSortDirection].ToString())
                {
                    case "ASC":
                        ViewState[TopVisitorsSortDirection] = "DESC";
                        break;
                    case "DESC":
                        ViewState[TopVisitorsSortDirection] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState[TopVisitorSortField] = e.SortExpression;
                ViewState[TopVisitorsSortDirection] = "ASC";
            }
            loadGrid(TopVisitors);
        }

        public void ClicksGrid_Sort(Object sender, DataGridSortCommandEventArgs e)
        {
            SetSortCommand(AllClicksSortField, AllClicksSortDirection, e.SortExpression);
            loadGrid(AllClicks);
        }

        protected void btnDownloadView_Click(object sender, EventArgs e)
        {
            var isSummary = ddlDownloadView.SelectedItem.Text.EqualsIgnoreCase(Summary);
            var selectedName = isSummary
                                   ? BlastClickSummary
                                   : BlastClickDetail;

            var blastAbstracts = Blast.GetByCampaignItemID_NoAccessCheck(getCampaignItemID(), false);
            var blastClickSummaries = new List<FrameworkEntitiesReport.BlastClickSummary>();
            var blastClickDetails = new List<FrameworkEntitiesReport.BlastClickDetail>();

            foreach (var blastAbstract in blastAbstracts)
            {
                if (blastAbstract.TestBlast == "N")
                {
                    if (isSummary)
                    {
                        blastClickSummaries.AddRange(BusinessLayerReport.BlastClickSummary.Get(blastAbstract.BlastID));
                    }
                    else
                    {
                        blastClickDetails.AddRange(BusinessLayerReport.BlastClickDetail.Get(blastAbstract.BlastID));
                    }
                }
            }

            IEnumerable dataSourceValue;

            if (getBlastID() > 0)
            {
                dataSourceValue = BusinessLayerReport.BlastClickSummary.Get(getBlastID()).AsEnumerable();
            }
            else
            {
                if (isSummary)
                {
                    dataSourceValue = blastClickSummaries.GroupBy(x => x.URL)
                        .Select(
                            x => new
                            {
                                x.First().CampaignItemName,
                                x.First().TotalCampaignClicks,
                                x.First().IssueDate,
                                x.First().URL,
                                TotalClicks = x.Sum(xx => xx.TotalClicks),
                                TotalDelivered = x.Sum(xx => xx.TotalDelivered),
                                TotalSent = x.Sum(xx => xx.TotalSent),
                                UniqueClicks = x.Sum(xx => xx.UniqueClicks),
                                Open = x.Sum(xx => xx.Open)
                            });
                }
                else
                {
                    dataSourceValue = blastClickDetails.AsEnumerable();
                }
            }

            DownloadViewWriteResponse(selectedName, dataSourceValue, isSummary);
        }

        private void DownloadViewWriteResponse(string selectedName, IEnumerable dataSourceValue, bool isSummary)
        {
            var reportDataSource = new ReportDataSource($"DS_{selectedName}", dataSourceValue);
            ReportViewer1.Visible = false;
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(reportDataSource);
            var path = ConfigurationManager.AppSettings[NameReportPath];
            ReportViewer1.LocalReport.ReportPath = Server.MapPath(Path.Combine(path, $"rpt_{selectedName}.rdlc"));

            if (isSummary)
            {
                var parameters = new ReportParameter[2];
                var blastIdValue = getBlastID() > 0
                                       ? getBlastID().ToString()
                                       : DefaultBlastId;
                parameters[0] = new ReportParameter(NameBlastId, blastIdValue);
                parameters[1] = new ReportParameter(NameCampaignItemId, getCampaignItemID().ToString());
                ReportViewer1.LocalReport.SetParameters(parameters);
                subReportList = new List<FrameworkEntitiesReport.BlastClickSummary_SubReport> { new FrameworkEntitiesReport.BlastClickSummary_SubReport() };
                ReportViewer1.LocalReport.SubreportProcessing += BlastClickSummary_SubReportProcessing;
            }

            ReportViewer1.LocalReport.Refresh();

            byte[] bytes = null;

            try
            {
                Warning[] warnings;
                string[] streamIds;
                string mimeType;
                string encoding;
                string extension;
                bytes = ReportViewer1.LocalReport.Render(NameExcel, string.Empty, out mimeType, out encoding, out extension, out streamIds, out warnings);
            }
            catch (LocalProcessingException ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }

            Response.ContentType = ExcelApplicationType;
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader(ContentDisposition, $"attachment; filename={selectedName}.xls");

            if (bytes == null)
            {
                throw new InvalidOperationException(BytesIsNull);
            }

            Response.BinaryWrite(bytes);
            Response.End();
        }

        void BlastClickSummary_SubReportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            if (e.Parameters["BlastID"] != null)
            {
                int blastID = -1;
                int.TryParse(e.Parameters["BlastID"].Values[0].ToString(), out blastID);
                if (blastID == -1)
                {
                    int campaignItemID = -1;
                    int.TryParse(e.Parameters["CampaignItemID"].Values[0].ToString(), out campaignItemID);
                    if (campaignItemID > 0)
                    {
                        List<ECN_Framework_Entities.Communicator.BlastAbstract> blastIDs = ECN_Framework_BusinessLayer.Communicator.Blast.GetByCampaignItemID_NoAccessCheck(getCampaignItemID(), false);

                        foreach (ECN_Framework_Entities.Communicator.BlastAbstract ba in blastIDs)
                        {
                            if (ba.TestBlast == "N")
                            {
                                List<ECN_Framework_Entities.Activity.Report.BlastClickSummary_SubReport> sourceList = new List<ECN_Framework_Entities.Activity.Report.BlastClickSummary_SubReport>();
                                sourceList = ECN_Framework_BusinessLayer.Activity.Report.BlastClickSummary_SubReport.Get(ba.BlastID);

                                //this is in case they want the campaign item stats, so we only return one item for this subreport
                                subReportList[0].Open += sourceList[0].Open;
                                subReportList[0].TotalClicks += sourceList[0].TotalClicks;
                                subReportList[0].TotalDelivered += sourceList[0].TotalDelivered;
                                subReportList[0].TotalSent += sourceList[0].TotalSent;
                                subReportList[0].Name = sourceList[0].Name;
                                subReportList[0].IssueDate = sourceList[0].IssueDate;
                            }
                        }

                    }
                }
                else
                {
                    List<ECN_Framework_Entities.Activity.Report.BlastClickSummary_SubReport> sourceList = new List<ECN_Framework_Entities.Activity.Report.BlastClickSummary_SubReport>();


                    sourceList = ECN_Framework_BusinessLayer.Activity.Report.BlastClickSummary_SubReport.Get(blastID);

                    //this is in case they want the campaign item stats, so we only return one item for this subreport
                    subReportList[0].Open += sourceList[0].Open;
                    subReportList[0].TotalClicks += sourceList[0].TotalClicks;
                    subReportList[0].TotalDelivered += sourceList[0].TotalDelivered;
                    subReportList[0].TotalSent += sourceList[0].TotalSent;
                    subReportList[0].Name = sourceList[0].Name;
                    subReportList[0].IssueDate = sourceList[0].IssueDate;
                }
                ReportDataSource rds = new ReportDataSource("DS_BlastClickSummary_SubReport", subReportList);
                e.DataSources.Add(rds);
            }


        }

        private static bool ValidateDateRange(DateTime startDate, DateTime endDate)
        {
            DateTime minDate = new DateTime(2000, 1, 1);
            if (startDate <= endDate && startDate > minDate && endDate > minDate)
            {
                return true;
            }
            else
                return false;
        }

        protected void ddlCLicksType_indexChanged(object sender, EventArgs e)
        {
            ClicksGrid.CurrentPageIndex = 0;
            ClicksPager.CurrentPage = 1;

            loadGrid(AllClicks);
        }
        protected void txtStartDate_textChanged(object sender, EventArgs e)
        {
            ClicksGrid.CurrentPageIndex = 0;
            ClicksPager.CurrentPage = 1;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            DateTime.TryParse(txtstartDate.Text, out startDate);
            DateTime.TryParse(txtendDate.Text, out endDate);
            if (ValidateDateRange(startDate, endDate))
            {
                loadGrid(AllClicks);
            }
            else
            {
                lblDateRangeError.Visible = true;
            }
        }
        protected void txtEndDate_textChanged(object sender, EventArgs e)
        {
            ClicksGrid.CurrentPageIndex = 0;
            ClicksPager.CurrentPage = 1;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            DateTime.TryParse(txtstartDate.Text, out startDate);
            DateTime.TryParse(txtendDate.Text, out endDate);
            if (ValidateDateRange(startDate, endDate))
            {
                loadGrid(AllClicks);
            }
            else
            {
                lblDateRangeError.Visible = true;
            }
        }

        protected void ShowHideColumns()
        { //if user does not have permission to see the details - add a column with TopClicks data but no URL; hide TopClicks
            if (KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportClicks, KMPlatform.Enums.Access.ViewDetails))
            {
                TopGrid.Columns[0].Visible = false;
                TopGrid.Columns[1].Visible = true;
            }
            else
            {
                TopGrid.Columns[0].Visible = true;
                TopGrid.Columns[1].Visible = false;

            }
        }

        protected void ShowHideDownload_withDetails(string panelName = "")
        {
            if (KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportClicks, KMPlatform.Enums.Access.Download))
            {
                DownloadPanel.Visible = true;

                if (panelName.Equals(TopClicks) && !KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportClicks, KMPlatform.Enums.Access.DownloadDetails))
                {
                    ListItem removeItem = ddlDownloadView.Items.FindByValue("detail");
                    ddlDownloadView.Items.Remove(removeItem);
                }

            }
            else
            {
                DownloadPanel.Visible = false;
            }

        }

    }
}