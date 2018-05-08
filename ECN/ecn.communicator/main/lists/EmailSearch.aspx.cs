using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework.Consts;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using Microsoft.Reporting.WebForms;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ecn.communicator.main.lists
{
    public partial class EmailSearch : System.Web.UI.Page
    {
        private const int EmailSearchPageSize = 10000000;
        private const string ExportFormatCsv = "CSV";
        private const char SingleComma = ',';
        private const string ZeroChannel = "0";
        private const string ZeroCustomer = "0";
        private const string ReportDataSourceName = "DS_EmailSearch";
        private const string ReportDefinitionFile = "ECN_Framework_Common.Reports.rpt_EmailSearch.rdlc";
        private const string ReportDefinitionAssembly = "~/bin/ECN_Framework_Common.dll";

        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            //Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.EMAILSEARCH;
            Master.SubMenu = "Email Search";
            Master.Heading = "Email Search";
            //Master.HelpContent = "<B>To Add Individual Emails:</B><br/><div id='par1'><ul><li>Choose the Group you would like to add the emails to</li><li>Choose the Subscribe Type (in most circumstances you will want to choose HTML; if your subscriber can only receive text emails, the system will automatically send them text emails. If you only want to send text messages, then you would select the Text subscribe type)</li><li>Choose the Format Type</li><li>In the “Addresses” box, type in each email address; <b>one email per line</b>. (Do not include separators such as a comma ( , ) or  ( ; ) between the addresses.)</li><li>Once all of your emails have been entered, click <em>Add</em>.</li><li>ECN will show you how many records were then successfully entered.</li>";
            Master.HelpTitle = "Email Search";

            if (!IsPostBack)
            {
                ViewState["gSortField"] = "EmailAddress";
                ViewState["gSortDirection"] = "ASC";
                ViewState["gPageSize"] = 15;
                ViewState["gCurrentPage"] = 1;
                ViewState["gTotalCount"] = 0;
            }

            searchTypeTitle.InnerText = KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser) ? "Find email within all base channels" : KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser) || Master.UserSession.CurrentUser.CurrentSecurityGroup.ClientGroupID > 0 ?  "Find email within all customers" : "Find email within current customer";
        }

        public void SortCommand(Object sender, DataGridSortCommandEventArgs e)
        {
            var sortExpression = e.SortExpression;
            string lastSortField = ViewState["gSortField"].ToString();

            if (sortExpression == lastSortField)
            {
                string sortDirection = ViewState["gSortDirection"].ToString();
                if (sortDirection == "ASC")
                {
                    ViewState["gSortDirection"] = "DESC";
                }
                else
                {
                    ViewState["gSortDirection"] = "ASC";
                }
            }
            else
            {
                ViewState["gSortField"] = sortExpression;
            }
            LoadEmailGrid(FilterType.SelectedValue, searchTerm.Text);
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            var curPage = Convert.ToInt32(ViewState["gCurrentPage"].ToString());
            if (curPage <= 1)
            {
                ViewState["gCurrentPage"] = 1;
            }
            else
            {
                ViewState["gCurrentPage"] = --curPage;
            }
            LoadEmailGrid(FilterType.SelectedValue, searchTerm.Text);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            var curPage = Convert.ToInt32(ViewState["gCurrentPage"].ToString());
            var ttlPages = Convert.ToInt32(ViewState["gTotalPages"].ToString());
            if (curPage >= ttlPages)
            {
                ViewState["gCurrentPage"] = ttlPages;
            }
            else
            {
                ViewState["gCurrentPage"] = ++curPage;
            }
            LoadEmailGrid(FilterType.SelectedValue, searchTerm.Text);
        }

        protected void ddlPageSizeGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddlSender = (DropDownList) sender;
            ViewState["gPageSize"] = ddlSender.SelectedValue;
            LoadEmailGrid(FilterType.SelectedValue, searchTerm.Text);
        }

        protected void GoToPageGroup_TextChanged(object sender, EventArgs e)
        {
            int currentPage = Convert.ToInt32(ViewState["gCurrentPage"].ToString());
            int totalPages = Convert.ToInt32(ViewState["gTotalPages"].ToString());

            if (currentPage > totalPages)
            {
                ViewState["gCurrentPage"] = totalPages;
            }
            else
            {
                var txtBox = (TextBox)sender;
                ViewState["gCurrentPage"] = txtBox.Text;
            }
            LoadEmailGrid(FilterType.SelectedValue, searchTerm.Text);
        }

        protected void GetResults_Click(object sender, EventArgs e)
        {
            LoadEmailGrid(FilterType.SelectedValue, searchTerm.Text);
        }

        public void LoadEmailGrid(string FilterTypeValue, string searchTermValue)
        {
            StringBuilder sbChannels = new StringBuilder();
            StringBuilder sbCustomers = new StringBuilder();
            if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser)) { sbChannels.Append("0"); sbCustomers.Append("0"); }
            else if(KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            {
                KMPlatform.BusinessLogic.SecurityGroup sgWorker = new KMPlatform.BusinessLogic.SecurityGroup();
                
                foreach(KMPlatform.Entity.UserClientSecurityGroupMap ucsgm in Master.UserSession.CurrentUser.UserClientSecurityGroupMaps)
                {
                    KMPlatform.Entity.SecurityGroup sg = sgWorker.Select(ucsgm.SecurityGroupID, false, false);
                    if(sg.AdministrativeLevel == KMPlatform.Enums.SecurityGroupAdministrativeLevel.ChannelAdministrator)
                    {
                        sbChannels.Append(ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByPlatformClientGroupID(sg.ClientGroupID).BaseChannelID.ToString() + ",");
                    }
                }
                sbCustomers.Append("0");
            }
            else if(Master.UserSession.CurrentUser.CurrentSecurityGroup.ClientGroupID > 0)
            {
                sbChannels.Append(Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString());
                sbCustomers.Append("0");
            }
            else
            {
                sbChannels.Append("0");
                sbCustomers.Append(Master.UserSession.CurrentCustomer.CustomerID.ToString());
            }

            int currentPage = Convert.ToInt32(ViewState["gCurrentPage"].ToString());
            int pageSize = Convert.ToInt32(ViewState["gPageSize"].ToString());
            string sortColumn = ViewState["gSortField"].ToString();
            string sortDirection = ViewState["gSortDirection"].ToString();

            DataTable emails = ECN_Framework_BusinessLayer.Communicator.Email.EmailSearch(FilterTypeValue, searchTermValue, Master.UserSession.CurrentUser, sbChannels.ToString().TrimEnd(SingleComma), sbCustomers.ToString(), currentPage, pageSize, sortColumn, sortDirection);

            List<ECN_Framework_Entities.Communicator.EmailSearch> emailSearches = ConvertToClass(emails);

            ResultsGrid.DataSource = emailSearches;
            ResultsGrid.DataBind();

            int ttlCount = 0;
            if (emailSearches.Any())
            {
                var tmp = emailSearches.First();
                ttlCount = tmp.TotalRowsCount;    
            }

            SetGridPageing(currentPage, pageSize, ttlCount);
        }

        protected void SetGridPageing(int currentPage,int pageSize,int ttlCount)
        {
            txtGoToPageGroup.Text = currentPage.ToString();
            lblTotalRecords.Text = ttlCount.ToString();
            ddlPageSizeGroup.SelectedValue = pageSize.ToString();
            ViewState["gTotalPages"] = lblTotalNumberOfPagesGroup.Text = (Math.Ceiling((double)ttlCount / pageSize)).ToString();
            
            pageDiv.Style["display"] = "block";
        }

        protected List<ECN_Framework_Entities.Communicator.EmailSearch> ConvertToClass(DataTable emails)
        {
            List<ECN_Framework_Entities.Communicator.EmailSearch> emailSearches = new List<ECN_Framework_Entities.Communicator.EmailSearch>();
            foreach (DataRow email in emails.Rows)
            {
                ECN_Framework_Entities.Communicator.EmailSearch es = new ECN_Framework_Entities.Communicator.EmailSearch();
                es.TotalRowsCount = Convert.ToInt32(email["TotalCount"]);
                es.BaseChannelName = email["BaseChannelName"].ToString();
                es.CustomerName = email["CustomerName"].ToString();
                es.GroupName = email["GroupName"].ToString();
                es.EmailAddress = email["EmailAddress"].ToString();
                es.Subscribe = email["SubscribeTypeCode"].ToString();
                if (!string.IsNullOrWhiteSpace(email["DateCreated"].ToString()))
                {
                    try
                    {
                        es.DateCreated = DateTime.Parse(email["DateCreated"].ToString());
                    }
                    catch { }
                }
                if (!string.IsNullOrWhiteSpace(email["DateModified"].ToString()))
                {
                    try
                    {
                        es.DateModified = DateTime.Parse(email["DateModified"].ToString());
                    }
                    catch { }
                }
                emailSearches.Add(es);
            }
            return emailSearches;
        }

        public void ExportReport_Click(object sender, EventArgs e)
        {
            var exportFormat = ddlExportType.SelectedValue;
            var filterType = FilterType.SelectedValue;
            var searchfor = searchTerm.Text;
            var sortColumn = ViewState["gSortField"].ToString();
            var sortDirection = ViewState["gSortDirection"].ToString();

            var emailsDataTable = ECN_Framework_BusinessLayer.Communicator.Email.EmailSearch(
                filterType,
                searchfor,
                Master.UserSession.CurrentUser,
                GetChannelsString().TrimEnd(SingleComma),
                GetCustomersString(),
                1,
                EmailSearchPageSize,
                sortColumn,
                sortDirection);

            var emailSearches = ConvertToClass(emailsDataTable);
            
            if (exportFormat.Equals(ExportFormatCsv, StringComparison.OrdinalIgnoreCase))
            {
                ExportToCsv(emailSearches);
            }
            else
            {
                ExportToNonCsv(emailSearches, exportFormat);
            }
        }

        private string GetChannelsString()
        {
            var currentUser = Master.UserSession.CurrentUser;

            if (KM.Platform.User.IsSystemAdministrator(currentUser))
            {
                return ZeroChannel;
            }

            if (KM.Platform.User.IsChannelAdministrator(currentUser))
            {
                var channelsStringBuilder = new StringBuilder();
                var securityGroupManager = new KMPlatform.BusinessLogic.SecurityGroup();

                foreach (var userSecurityGroup in currentUser.UserClientSecurityGroupMaps)
                {
                    var securityGroup = securityGroupManager.Select(userSecurityGroup.SecurityGroupID, false, false);
                    if (securityGroup.AdministrativeLevel == KMPlatform.Enums.SecurityGroupAdministrativeLevel.ChannelAdministrator)
                    {
                        var baseChannel = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByPlatformClientGroupID(
                            securityGroup.ClientGroupID);
                        channelsStringBuilder.Append(baseChannel.BaseChannelID + SingleComma);
                    }
                }

                return channelsStringBuilder.ToString();
            }

            if (currentUser.CurrentSecurityGroup.ClientGroupID > 0)
            {
                return Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString();
            }

            return ZeroChannel;
        }

        private string GetCustomersString()
        {
            var currentUser = Master.UserSession.CurrentUser;

            if (KM.Platform.User.IsSystemAdministrator(currentUser) ||
                KM.Platform.User.IsChannelAdministrator(currentUser) ||
                currentUser.CurrentSecurityGroup.ClientGroupID > 0)
            {
                return ZeroCustomer;
            }

            return Master.UserSession.CurrentCustomer.CustomerID.ToString();
        }

        private void ExportToNonCsv(List<CommunicatorEntities.EmailSearch> emailSearches, string exportFormat)
        {
            InitializeReportViewer(emailSearches);

            switch (exportFormat.ToUpper())
            {
                case "PDF":
                    InitializeReportViewerResponse("PDF", exportFormat, "application/pdf");
                    break;
                case "XLS":
                    InitializeReportViewerResponse("EXCEL", exportFormat, "application/vnd.ms-excel");
                    break;
                case "TXT":
                    InitializeTextResponse(emailSearches);
                    break;
            }
        }

        private void InitializeReportViewer(List<CommunicatorEntities.EmailSearch> emailSearches)
        {
            var assembly = Assembly.LoadFrom(HttpContext.Current.Server.MapPath(ReportDefinitionAssembly));
            var reportStream = assembly.GetManifestResourceStream(ReportDefinitionFile);
            ReportViewer1.LocalReport.LoadReportDefinition(reportStream);
            var dataSource = new ReportDataSource(ReportDataSourceName, emailSearches);
            ReportViewer1.Visible = false;
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(dataSource);
            ReportViewer1.LocalReport.Refresh();
        }

        private void InitializeTextResponse(List<CommunicatorEntities.EmailSearch> emailSearches)
        {
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();

            string attachment = $"attachment; filename={Master.UserSession.CurrentUser.CustomerID}_EmailSearch.txt";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "text/csv";
            HttpContext.Current.Response.AddHeader("Pragma", "public");
            HttpContext.Current.Response.Write(ConvertListToString(emailSearches));
            HttpContext.Current.Response.End();
        }

        private void InitializeReportViewerResponse(string reportFormat, string exportFormat, string contentType)
        {
            byte[] bytes;
            string mimeType;
            string encoding;
            string extension;
            string[] streamids;
            Warning[] warnings;

            bytes = ReportViewer1.LocalReport.Render(
                reportFormat,
                string.Empty,
                out mimeType,
                out encoding,
                out extension,
                out streamids,
                out warnings);

            Response.ContentType = contentType;
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", $"attachment; filename=EmailSearch.{exportFormat}");
            Response.BinaryWrite(bytes);
            Response.End();
        }

        private void ExportToCsv(List<CommunicatorEntities.EmailSearch> emailSearches)
        {
            var emailSearchCsvList = new List<EmailSearchCSV>();
            foreach (var email in emailSearches)
            {
                var emailCsv = new EmailSearchCSV
                {
                    EmailAddress = email.EmailAddress,
                    BaseChannelName = email.BaseChannelName,
                    CustomerName = email.CustomerName,
                    GroupName = email.GroupName,
                    Subscribe = email.Subscribe,
                    DateAdded = email.DateCreated.Value.ToShortDateString(),
                    DateModified = email.DateModified?.ToShortDateString() ?? string.Empty
                };
                emailSearchCsvList.Add(emailCsv);
            }

            var tfile = $"{Master.UserSession.CurrentUser.CustomerID}-EmailSearch";
            ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.ExportCSV(emailSearchCsvList, tfile);
        }

        public static string ConvertListToString(IEnumerable<ECN_Framework_Entities.Communicator.EmailSearch> emails)
        {
            const char DataSeparator = SingleComma;
            string LineSeparator = Environment.NewLine;

            var stringBuilder = new StringBuilder();

            stringBuilder.Append("Base Channel Name");
            stringBuilder.Append(DataSeparator);
            stringBuilder.Append("Customer Name");
            stringBuilder.Append(DataSeparator);
            stringBuilder.Append("Group Name");
            stringBuilder.Append(DataSeparator);
            stringBuilder.Append("Email Address");
            stringBuilder.Append(DataSeparator);
            stringBuilder.Append("Subscribe");
            stringBuilder.Append(DataSeparator);
            stringBuilder.Append("Date Added");
            stringBuilder.Append(DataSeparator);
            stringBuilder.Append("Date Modified");
            stringBuilder.Append(LineSeparator);

            // Build the usesrs string.
            foreach (ECN_Framework_Entities.Communicator.EmailSearch EmailSearch in emails)
            {
                stringBuilder.Append(EmailSearch.BaseChannelName);
                stringBuilder.Append(DataSeparator);
                stringBuilder.Append(EmailSearch.CustomerName);
                stringBuilder.Append(DataSeparator);
                stringBuilder.Append(EmailSearch.GroupName);
                stringBuilder.Append(DataSeparator);
                stringBuilder.Append(EmailSearch.EmailAddress);
                stringBuilder.Append(DataSeparator);
                stringBuilder.Append(EmailSearch.Subscribe);
                stringBuilder.Append(DataSeparator);
                stringBuilder.Append(EmailSearch.DateCreated);
                stringBuilder.Append(DataSeparator);
                stringBuilder.Append(EmailSearch.DateModified);
                stringBuilder.Append(LineSeparator);
            }

            // Remove trailing separator.
            stringBuilder.Remove(stringBuilder.Length - 1, 1);

            return stringBuilder.ToString();
        }

    }
}