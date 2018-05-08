using ecn.communicator.main.Helpers;
using ECN.Common.Helpers;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using KM.Common;
using KMPlatform.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessCampaignItem = ECN_Framework_BusinessLayer.Communicator.CampaignItem;
using BusinessCampaignItemBlast = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast;
using BusinessCampaignItemLinkTracking = ECN_Framework_BusinessLayer.Communicator.CampaignItemLinkTracking;
using BusinessLinkTrackingParam = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParam;
using BusinessLinkTrackingParamOption = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption;
using BusinessLinkTrackingParamSettings = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings;
using BusinessReportQueue = ECN_Framework_BusinessLayer.Communicator.ReportQueue;
using BusinessReportSchedule = ECN_Framework_BusinessLayer.Communicator.ReportSchedule;
using CampaignItemTemplate = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate;
using Client = KMPlatform.BusinessLogic.Client;
using CommonStringFunctions = KM.Common.StringFunctions;
using Email = ECN_Framework_BusinessLayer.Communicator.Email;
using EntitiesLinkTrackingParamOption = ECN_Framework_Entities.Communicator.LinkTrackingParamOption;
using EntitiesLinkTrackingParamSettings = ECN_Framework_Entities.Communicator.LinkTrackingParamSettings;
using Enums = ECN_Framework_Common.Objects.Enums;
using LinkTrackingSettings = ECN_Framework_BusinessLayer.Communicator.LinkTrackingSettings;
using StringFunctions = ECN_Framework_Common.Functions.StringFunctions;

namespace ecn.communicator.main.ECNWizard.Controls
{
    public partial class WizardSchedule : System.Web.UI.UserControl, IECNWizard
    {
        private const int GoogleLinkTrackingId = 1;
        private const int OmnitureLinkTrackingId = 3;
        private const char Comma = ',';
        private const int BlastReportId = 9;
        private const int FtpReportId = 10;
        private const int MaxCarbonCopyLimit = 5;

        delegate void HidePopup();

        class Group
        {
            public int GroupID;
            public string GroupName;
            public int Subscribers;

        }
        #region Properties

        /// <summary>
        /// This returns the current customer from the database using the customerID from the CurrentCustomer stored in the ECNSession.
        /// This approach works around issues where the ECNSession contains a stale cached version of the customer object.
        /// </summary>
        public ECN_Framework_Entities.Accounts.Customer CurrentCustomer
        {
            get
            {
                //return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer;
                if (_currentCustomer == null)
                {
                    int customerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID;
                    _currentCustomer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(customerID, false);
                }

                return _currentCustomer;
            }
        }
        private ECN_Framework_Entities.Accounts.Customer _currentCustomer;

        public string fromEmail
        {
            get
            {
                if (ViewState["fromEmail"] != null)
                {
                    return (string)ViewState["fromEmail"];
                }
                else
                {
                    return "";
                }
            }
            set
            {
                ViewState["fromEmail"] = value;
            }
        }
        public string toEmail
        {
            get
            {
                if (ViewState["toEmail"] != null)
                {
                    return (string)ViewState["toEmail"];
                }
                else
                {
                    return "";
                }
            }
            set
            {
                ViewState["toEmail"] = value;
            }
        }
        public string emailSubject
        {
            get
            {
                if (ViewState["emailSubject"] != null)
                {
                    return (string)ViewState["emailSubject"];
                }
                else
                {
                    return "";
                }
            }
            set
            {
                ViewState["emailSubject"] = value;
            }
        }
        public string fromName
        {
            get { return ViewStateHelper.GetFromViewState(ViewState, nameof(fromName), string.Empty); }
            set { ViewStateHelper.SetViewState(ViewState, nameof(fromName), value); }
        }

        public List<string> ccList
        {
            get { return ViewStateHelper.GetFromViewState(ViewState, nameof(ccList), new List<string>()); }
            set { ViewStateHelper.SetViewState(ViewState, nameof(ccList), value); }
        }

        public DateTime reportDate
        {
            get
            {
                if (ViewState["reportDate"] != null)
                {
                    return (DateTime)ViewState["reportDate"];
                }
                else
                {
                    return new DateTime();
                }
            }
            set
            {
                ViewState["reportDate"] = value;
            }
        }
        public string reportTime
        {
            get
            {
                if (ViewState["reportTime"] != null)
                {
                    return (string)ViewState["reportTime"];
                }
                else
                {
                    return "";
                }
            }
            set
            {
                ViewState["reportTime"] = value;
            }
        }
        public bool scheduleBlastReport
        {
            get
            {
                if (ViewState["scheduleBlastReport"] != null)
                {
                    return (bool)ViewState["scheduleBlastReport"];
                }
                else
                {
                    return false;
                }
            }
            set
            {
                ViewState["scheduleBlastReport"] = value;
            }
        }
        public bool scheduleFtpExport
        {
            get
            {
                if (ViewState["scheduleFtpExport"] != null)
                {
                    return (bool)ViewState["scheduleFtpExport"];
                }
                else
                {
                    return false;
                }
            }
            set
            {
                ViewState["scheduleFtpExport"] = value;
            }
        }
        public bool hasScheduledReport
        {
            get
            {
                if (ViewState["hasScheduledReport"] != null)
                {
                    return (bool)ViewState["hasScheduledReport"];
                }
                else
                {
                    return false;
                }
            }
            set
            {
                ViewState["hasScheduledReport"] = value;
            }
        }
        public List<string> ftpExports
        {
            get
            {
                if (ViewState["ftpExports"] != null)
                {
                    return (List<string>)ViewState["ftpExports"];
                }
                else
                {
                    return new List<string>();
                }
            }
            set
            {
                ViewState["ftpExports"] = value;
            }
        }

        public string ftpUrl
        {
            get { return ViewStateHelper.GetFromViewState(ViewState, nameof(ftpUrl), string.Empty); }
            set { ViewStateHelper.SetViewState(ViewState, nameof(ftpUrl), value); }
        }

        public string ftpUsername
        {
            get { return ViewStateHelper.GetFromViewState(ViewState, nameof(ftpUsername), string.Empty); }
            set { ViewStateHelper.SetViewState(ViewState, nameof(ftpUsername), value); }
        }

        public string ftpPassword
        {
            get
            {
                if (ViewState["ftpPassword"] != null)
                {
                    return (string)ViewState["ftpPassword"];
                }
                else
                {
                    return "";
                }
            }
            set
            {
                ViewState["ftpPassword"] = value;
            }
        }

        public string ftpExportFormat
        {
            get { return ViewStateHelper.GetFromViewState(ViewState, nameof(ftpExportFormat), string.Empty); }
            set { ViewStateHelper.SetViewState(ViewState, nameof(ftpExportFormat), value); }
        }

        public string modalError
        {
            get { return ViewStateHelper.GetFromViewState(ViewState, nameof(modalError), string.Empty); }
            set { ViewStateHelper.SetViewState(ViewState, nameof(modalError), value); }
        }

        int _campaignItemID = 0;
        public int CampaignItemID
        {
            set
            {
                _campaignItemID = value;
            }
            get
            {
                return _campaignItemID;
            }
        }

        string _errormessage = string.Empty;
        public string ErrorMessage
        {
            set
            {
                _errormessage = value;
            }
            get
            {
                return _errormessage;
            }
        }

        public int SampleID
        {
            get
            {
                if (Request.QueryString["SampleID"] != null)
                    return Convert.ToInt32(Request.QueryString["SampleID"].ToString());
                else
                    return -1;
            }
        }

        private DataTable OptoutGroups_DT
        {
            get
            {
                if (ViewState["OptoutGroups_DT"] != null)
                {
                    return (DataTable)ViewState["OptoutGroups_DT"];
                }
                else
                {
                    DataTable dt = new DataTable();
                    DataColumn CampaignItemOptOutID = new DataColumn("CampaignItemOptOutID", typeof(string));
                    dt.Columns.Add(CampaignItemOptOutID);

                    DataColumn GroupID = new DataColumn("GroupID", typeof(int));
                    dt.Columns.Add(GroupID);

                    DataColumn GroupName = new DataColumn("GroupName", typeof(string));
                    dt.Columns.Add(GroupName);

                    DataColumn IsDeleted = new DataColumn("IsDeleted", typeof(bool));
                    dt.Columns.Add(IsDeleted);
                    dt.AcceptChanges();
                    ViewState["OptoutGroups_DT"] = dt;
                    return dt;
                }
            }
            set
            {
                ViewState["OptoutGroups_DT"] = value;
            }
        }

        private DataTable LinkTrackingDomain_DT
        {
            get
            {
                try
                {
                    return (DataTable)ViewState["LinkTrackingDomain_DT"];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                ViewState["LinkTrackingDomain_DT"] = value;
            }
        }

        #endregion
        #region nonScheduleReportCode

        #region PageEvents
        protected void Page_Load(object sender, EventArgs e)
        {
            lblDomainError.Visible = false;
            phError.Visible = false;

            HidePopup delGroupsLookupPopup = new HidePopup(GroupsLookupPopupHide);
            this.ctrlgroupsLookup1.hideGroupsLookupPopup = delGroupsLookupPopup;
            ctrlgroupsLookup1.ShowArchiveFilter = false;
            ecn.communicator.MasterPages.Communicator master = (ecn.communicator.MasterPages.Communicator)Page.Master;
            master.MasterRegisterButtonForPostBack(btnCancelDetailEdits);
            
            if (!IsPostBack)
            {
                bool setupTest = CurrentCustomer.DefaultBlastAsTest.HasValue ? CurrentCustomer.DefaultBlastAsTest.Value : false;
                if (!getCampaignItemType().ToLower().Equals("regular"))
                    setupTest = false;
                //BlastScheduler1.ResetSchedule(setupTest);
            }
        }

        private void GroupsLookupPopupHide()
        {
            ctrlgroupsLookup1.Visible = false;
        }

        protected override bool OnBubbleEvent(object sender, EventArgs e)
        {
            try
            {
                string source = sender.ToString();
                if (source.Equals("GroupSelected"))
                {
                    int groupID = ctrlgroupsLookup1.selectedGroupID;
                    bool exists = false;
                    if (OptoutGroups_DT.Rows.Count > 0)
                    {
                        foreach (DataRow drow in OptoutGroups_DT.AsEnumerable())
                        {
                            if ((drow["GroupID"].Equals(groupID) && drow["IsDeleted"].ToString().ToLower().Equals("false")))
                            {
                                exists = true;
                                break;
                            }
                        }
                    }

                    if (!exists)
                    {
                        ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(groupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                        DataRow dr = OptoutGroups_DT.NewRow();
                        dr["CampaignItemOptOutID"] = Guid.NewGuid().ToString();
                        dr["GroupName"] = group.GroupName;
                        dr["GroupID"] = groupID;
                        dr["IsDeleted"] = false;
                        OptoutGroups_DT.Rows.Add(dr);
                        OptoutGroups_DT.AcceptChanges();
                        loadOptoutGroupsGrid();
                    }
                    this.ctrlgroupsLookup1.Visible = false;
                }
            }
            catch { }
            return true;
        }

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            BlastScheduler1.checkedHandler += new main.blasts.BlastScheduler.TestChecked(BlastScheduler1_checkedHandler);
        }
        #endregion

        #region QueryString Values
        private string getCampaignItemType()
        {
            try
            {
                return Request.QueryString["campaignItemType"].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region LoadData
        private void LoadFoldersDR(int FolderID)
        {
            List<ECN_Framework_Entities.Communicator.Folder> folderList =
            ECN_Framework_BusinessLayer.Communicator.Folder.GetByType(CurrentCustomer.CustomerID, "GRP", ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            drpFolder.DataSource = folderList;
            drpFolder.DataValueField = "FolderID";
            drpFolder.DataTextField = "FolderName";
            drpFolder.DataBind();
            drpFolder.Items.Insert(0, new ListItem("root", "0"));
            drpFolder.Items.FindByValue(FolderID.ToString()).Selected = true;
        }

        private void loadLinkTrackingDomainData()
        {
            int LTID = Convert.ToInt32(hfLinkTrackingDomain.Value);
            List<ECN_Framework_Entities.Communicator.LinkTrackingDomain> ltDomainList =
            ECN_Framework_BusinessLayer.Communicator.LinkTrackingDomain.GetByCustomerID(CurrentCustomer.CustomerID, LTID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

            DataTable dt = new DataTable();
            DataColumn DomainTrackerFieldsID = new DataColumn("LinkTrackingDomainID", typeof(string));
            dt.Columns.Add(DomainTrackerFieldsID);

            DataColumn GroupDataFieldsName = new DataColumn("Domain", typeof(string));
            dt.Columns.Add(GroupDataFieldsName);

            DataColumn IsDeleted = new DataColumn("IsDeleted", typeof(bool));
            dt.Columns.Add(IsDeleted);
            foreach (ECN_Framework_Entities.Communicator.LinkTrackingDomain ltd in ltDomainList)
            {
                DataRow dr = dt.NewRow();
                dr["LinkTrackingDomainID"] = ltd.LinkTrackingDomainID.ToString();
                dr["Domain"] = ltd.Domain.ToString();
                dr["IsDeleted"] = false;
                dt.Rows.Add(dr);
                dt.AcceptChanges();
            }
            LinkTrackingDomain_DT = dt;
            if (ltDomainList.Count > 0)
            {
                rblLinkTracking.ClearSelection();
                rblLinkTracking.SelectedValue = "specific";
                gvLinkTrackingDomains.DataSource = ltDomainList;
                gvLinkTrackingDomains.DataBind();
                gvLinkTrackingDomains.Visible = true;
                pnlDomainList.Visible = true;
            }
            else
            {
                rblLinkTracking.ClearSelection();
                rblLinkTracking.SelectedValue = "all";
                gvLinkTrackingDomains.Visible = false;
                pnlDomainList.Visible = false;
            }
        }

        private void loadLinkTrackingDomainGrid()
        {
            var result = (from src in LinkTrackingDomain_DT.AsEnumerable()
                          where src.Field<bool>("IsDeleted") == false
                          select new
                          {
                              LinkTrackingDomainID = src.Field<string>("LinkTrackingDomainID"),
                              Domain = src.Field<string>("Domain"),
                              IsDeleted = src.Field<bool>("IsDeleted")
                          }).ToList();
            gvLinkTrackingDomains.DataSource = result;
            gvLinkTrackingDomains.DataBind();
            if (result.Count > 0)
                gvLinkTrackingDomains.Visible = true;
            else
                gvLinkTrackingDomains.Visible = false;
        }

        private void loadReportSchedule()
        {
            //If there was an existing reportschedule record created, i.e. the user is editing a scheduled report, the following line should return the same blast(s) with the same blast 
            //IDs as the reports were created with. So it should be safe to grab the report schedules by these BlastID's and update those reportSchedule records in SaveScheduleReport
            CampaignItemID = Convert.ToInt32(Request.QueryString["CampaignItemID"]);
            List<ECN_Framework_Entities.Communicator.BlastAbstract> newBlastList = new List<ECN_Framework_Entities.Communicator.BlastAbstract>();
            List<ECN_Framework_Entities.Communicator.ReportSchedule> reportsList = new List<ECN_Framework_Entities.Communicator.ReportSchedule>();
            if (CampaignItemID > 0)
            {
                newBlastList = ECN_Framework_BusinessLayer.Communicator.Blast.GetByCampaignItemID(CampaignItemID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
                if (newBlastList.Count > 0)
                {
                    reportsList = ECN_Framework_BusinessLayer.Communicator.ReportSchedule.GetByBlastId(newBlastList[0].BlastID);
                }
            }
            if (reportsList.Count > 0)
            {
                hasScheduledReport = true;
                chkScheduleReport.Checked = true;
                btnEditScheduleReport.Visible = true;
                btnEditScheduleReport.Enabled = true;

                foreach (ECN_Framework_Entities.Communicator.ReportSchedule report in reportsList)
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(report.ReportParameters);
                    if (report.ReportID == 10)//Report Schedule is an FTP Report, populate all the information
                    {
                        chkFtpExport.Checked = true;
                        scheduleFtpExport = true;
                        //Select and store the various configuration and credential information stored in the xml to be used for building the file and moveToFtp later
                        XmlNode exportFormatNode = xDoc.GetElementsByTagName("ExportFormat")[0];
                        string ftpUrlTemp = xDoc.GetElementsByTagName("FtpUrl")[0].InnerText;
                        string ftpUserNameTemp = xDoc.GetElementsByTagName("FtpUsername")[0].InnerText;
                        string ftpPasswordTemp = xDoc.GetElementsByTagName("FtpPassword")[0].InnerText;
                        //For each of the elements in the ftp exports section of the XML doc with a value of true, add an element to dataForExportList 
                        XmlNode exports = xDoc.GetElementsByTagName("Exports")[0];

                        //Store report details in page properties
                        ftpExportFormat = exportFormatNode.InnerText;
                        ftpUrl = ftpUrlTemp;
                        ftpUsername = ftpUserNameTemp;
                        ftpPassword = ftpPasswordTemp;
                        List<string> ftpExportsTemp = new List<string>();
                        foreach (XmlNode exportTypeNode in exports.ChildNodes)
                        {
                            if (exportTypeNode.InnerText.ToLower() == "true")
                            {
                                ftpExportsTemp.Add(exportTypeNode.Name);
                            }
                        }
                        ftpExports = ftpExportsTemp;

                        //Populate the modal fields with report values
                        txtFtpUrl.Text = ftpUrl;
                        ddlFormat.SelectedValue = ftpExportFormat;
                        txtFtpUsername.Text = ftpUsername;
                        txtFtpPassword.Text = ftpPassword;

                        foreach (ListItem ftpExport in lbFtpExports.Items)
                        {
                            if (ftpExports.Contains(ftpExport.Value))
                            {
                                ftpExport.Selected = true;
                            }
                        }
                    }
                    if (report.ReportID == 9)//Report is a Blast report, fill that specific information. Which is only the check box...
                    {
                        chkEmailBlastReport.Checked = true;
                        scheduleBlastReport = true;
                    }

                    //Populate the rest of the envelope information
                    fromEmail = report.FromEmail;
                    toEmail = report.ToEmail;
                    emailSubject = report.EmailSubject;
                    fromName = report.FromName;
                    XmlNode ccEmails = xDoc.GetElementsByTagName("ccEmails")[0];
                    List<string> cc = new List<string>();
                    foreach (XmlNode ccEmail in ccEmails.ChildNodes)
                    {
                        cc.Add(ccEmail.InnerText);
                    }
                    ccList = cc;
                    DateTime tempDate = new DateTime();
                    DateTime.TryParse(report.StartDate, out tempDate);
                    reportDate = tempDate;
                    reportTime = report.StartTime;

                    txtFromEmail.Text = fromEmail;
                    txtToEmail.Text = toEmail;
                    txtSubject.Text = emailSubject;
                    txtFromName.Text = fromName;
                    txtAddCc.Text = string.Join(", ", ccList.ToArray());
                    txtReportDate.Text = reportDate.ToShortDateString();
                    ddlReportTime.SelectedValue = reportTime;
                    divScheduleReportErrorMessage.Visible = false;


                }

            }
        }

        #endregion

        public void Initialize()
        {
            if (KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.Edit))
            {
                bool setupTest = CurrentCustomer.DefaultBlastAsTest.HasValue ? CurrentCustomer.DefaultBlastAsTest.Value : false;

                phMessage.Visible = false;
                //loadTestGroupDR(0);
                //testgroupExplorer1.LoadGroupFolder();
                LoadFoldersDR(0);
                LoadLinkTrackingParamOptions();
                SetupScheduler(setupTest);

                loadOptoutGroups();
                loadOptOutPreference();
                loadOptOutTemplate();
                loadReportSchedule();
                if (chkOptOutSpecificGroup.Checked)
                {
                    chkOptOutSpecificGroup.Enabled = true;
                    chkOptOutMasterSuppression.Enabled = false;
                }
                if (chkOptOutMasterSuppression.Checked)
                {
                    chkOptOutMasterSuppression.Enabled = true;
                    chkOptOutSpecificGroup.Enabled = false;
                }

                if (getCampaignItemType().ToLower().Equals("ab"))
                {
                    chkGoToChampion.Visible = true;

                }
                else
                    chkGoToChampion.Visible = false;

                if (getCampaignItemType().ToLower().Equals("champion"))
                {
                    if (SampleID > 0)
                    {
                        PrePopFromAB(SampleID);
                    }
                }
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }
        }

        private void loadOptOutPreference()
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemBlast> ciBlastList =
            ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(CampaignItemID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
            if (ciBlastList[0].AddOptOuts_to_MS != null && ciBlastList[0].AddOptOuts_to_MS.Value == true)
                chkOptOutMasterSuppression.Checked = true;
            else
                chkOptOutMasterSuppression.Checked = false;
        }

        private void loadOptOutTemplate()
        {
            ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(CampaignItemID, true);
            if ((ci.OptOutGroupList == null || ci.OptOutGroupList.Count == 0) && ci.CampaignItemTemplateID.HasValue && ci.CampaignItemTemplateID.Value > 0)
            {
                ECN_Framework_Entities.Communicator.CampaignItemTemplate cit = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.GetByCampaignItemTemplateID(ci.CampaignItemTemplateID.Value, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, true);
                if (cit.OptOutMasterSuppression.HasValue && cit.OptOutMasterSuppression.Value)
                {
                    chkOptOutMasterSuppression.Checked = true;
                    chkOptOutSpecificGroup.Enabled = false;
                    pnlOptOutSpecificGroups.Visible = false;
                }
                if (cit.OptOutSpecificGroup.HasValue && cit.OptOutSpecificGroup.Value)
                {
                    chkOptOutSpecificGroup.Checked = true;
                    pnlOptOutSpecificGroups.Visible = true;
                    chkOptOutMasterSuppression.Enabled = false;

                    if (cit.OptoutGroupList.Count > 0)
                    {
                        foreach (ECN_Framework_Entities.Communicator.CampaignItemTemplateOptoutGroup CampaignItemOptOutGroup in cit.OptoutGroupList)
                        {
                            ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(CampaignItemOptOutGroup.GroupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                            DataRow dr = OptoutGroups_DT.NewRow();
                            dr["CampaignItemOptOutID"] = Guid.NewGuid().ToString();
                            dr["GroupName"] = group.GroupName;
                            dr["GroupID"] = CampaignItemOptOutGroup.GroupID;
                            dr["IsDeleted"] = CampaignItemOptOutGroup.IsDeleted;
                            OptoutGroups_DT.Rows.Add(dr);
                        }

                        loadOptoutGroupsGrid();
                    }
                }
            }
        }

        private int getTestBlastCount()
        {
            int count = 0;
            try
            {
                count = Convert.ToInt32(ConfigurationManager.AppSettings["CU_" + CurrentCustomer.CustomerID + "_TestBlastEmails"].ToString());
            }
            catch
            {
                try
                {
                    count = Convert.ToInt32(ConfigurationManager.AppSettings["CH_" + ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID + "_TestBlastEmails"].ToString());
                }
                catch
                {
                    count = Convert.ToInt32(ConfigurationManager.AppSettings["BASE_TestBlastEmails"].ToString());
                }
            }

            return count;
        }

        public bool Save()
        {
            phMessage.Visible = false;
            return true;
        }

        void BlastScheduler1_checkedHandler(bool isTestBlast)
        {
            if (isTestBlast && getCampaignItemType().ToLower().Equals("regular"))
            {
                pnlTestOptions.Visible = true;
                pnlBlastOptions.Visible = false;
                testgroupExplorer1.LoadGroupFolder();

            }
            else
            {
                pnlTestOptions.Visible = false;
                pnlBlastOptions.Visible = true;
            }
        }

        #region LinkTracking Events
        protected void chkboxGoogleAnalytics_CheckedChanged(object sender, EventArgs e)
        {
            if (chkboxGoogleAnalytics.Checked)
            {
                pnlGoogleAnalytics.Visible = true;
            }
            else
                pnlGoogleAnalytics.Visible = false;
        }

        protected void chkboxOmnitureTracking_CheckedChanged(object sender, EventArgs e)
        {
            if (chkboxOmnitureTracking.Checked)
            {
                pnlOmniture.Visible = true;
            }
            else
                pnlOmniture.Visible = false;
        }

        protected void drpCampaignSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpCampaignSource.SelectedValue.Equals("6"))
            {
                txtCampaignSource.Visible = true;
            }
            else
            {
                txtCampaignSource.Visible = false;
            }
        }

        protected void drpCampaignMedium_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (drpCampaignMedium.SelectedValue.Equals("6"))
            {
                txtCampaignMedium.Visible = true;
            }
            else
            {
                txtCampaignMedium.Visible = false;
            }
        }

        protected void drpCampaignTerm_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (drpCampaignTerm.SelectedValue.Equals("6"))
            {
                txtCampaignTerm.Visible = true;
            }
            else
            {
                txtCampaignTerm.Visible = false;
            }
        }

        protected void drpCampaignContent_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (drpCampaignContent.SelectedValue.Equals("6"))
            {
                txtCampaignContent.Visible = true;
            }
            else
            {
                txtCampaignContent.Visible = false;
            }
        }

        protected void drpCampaignName_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (drpCampaignName.SelectedValue.Equals("6"))
            {
                txtCampaignName.Visible = true;
            }
            else
            {
                txtCampaignName.Visible = false;
            }
        }

        protected void ddlOmniture1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOmniture1.SelectedItem.Text.ToLower().Replace(" ", "").Equals("customvalue"))
            {
                txtOmniture1.Visible = true;
            }
            else
                txtOmniture1.Visible = false;
        }

        protected void ddlOmniture2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOmniture2.SelectedItem.Text.ToLower().Replace(" ", "").Equals("customvalue"))
            {
                txtOmniture2.Visible = true;
            }
            else
                txtOmniture2.Visible = false;
        }

        protected void ddlOmniture3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOmniture3.SelectedItem.Text.ToLower().Replace(" ", "").Equals("customvalue"))
            {
                txtOmniture3.Visible = true;
            }
            else
                txtOmniture3.Visible = false;
        }

        protected void ddlOmniture4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOmniture4.SelectedItem.Text.ToLower().Replace(" ", "").Equals("customvalue"))
            {
                txtOmniture4.Visible = true;
            }
            else
                txtOmniture4.Visible = false;
        }

        protected void ddlOmniture5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOmniture5.SelectedItem.Text.ToLower().Replace(" ", "").Equals("customvalue"))
            {
                txtOmniture5.Visible = true;
            }
            else
                txtOmniture5.Visible = false;
        }

        protected void ddlOmniture6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOmniture6.SelectedItem.Text.ToLower().Replace(" ", "").Equals("customvalue"))
            {
                txtOmniture6.Visible = true;
            }
            else
                txtOmniture6.Visible = false;
        }

        protected void ddlOmniture7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOmniture7.SelectedItem.Text.ToLower().Replace(" ", "").Equals("customvalue"))
            {
                txtOmniture7.Visible = true;
            }
            else
                txtOmniture7.Visible = false;
        }

        protected void ddlOmniture8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOmniture8.SelectedItem.Text.ToLower().Replace(" ", "").Equals("customvalue"))
            {
                txtOmniture8.Visible = true;
            }
            else
                txtOmniture8.Visible = false;
        }

        protected void ddlOmniture9_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOmniture9.SelectedItem.Text.ToLower().Replace(" ", "").Equals("customvalue"))
            {
                txtOmniture9.Visible = true;
            }
            else
                txtOmniture9.Visible = false;
        }

        protected void ddlOmniture10_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOmniture10.SelectedItem.Text.ToLower().Replace(" ", "").Equals("customvalue"))
            {
                txtOmniture10.Visible = true;
            }
            else
                txtOmniture10.Visible = false;
        }

        protected void imgGoogleAnalyticsSettings_Click(object sender, EventArgs e)
        {
            hfLinkTrackingDomain.Value = "1";
            modalPopupLinkTrackingDomain.Show();
            loadLinkTrackingDomainData();
        }

        protected void imgbtnOmnitureSettings_Click(object sender, ImageClickEventArgs e)
        {
            hfLinkTrackingDomain.Value = "3";
            modalPopupLinkTrackingDomain.Show();
            loadLinkTrackingDomainData();
        }

        protected void imgbtnConvTrackingSettings_Click(object sender, ImageClickEventArgs e)
        {
            hfLinkTrackingDomain.Value = "2";
            modalPopupLinkTrackingDomain.Show();
            loadLinkTrackingDomainData();
        }

        protected void rblLinkTracking_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (rblLinkTracking.SelectedValue.Equals("all"))
            {
                pnlDomainList.Visible = false;
            }
            else
            {
                pnlDomainList.Visible = true;
            }
        }

        protected void LinkTrackingDomain_Close(object sender, EventArgs e)
        {
            txtDomainName.Text = "";
            modalPopupLinkTrackingDomain.Hide();
            LinkTrackingDomain_DT = null;
        }

        protected void LinkTrackingDomain_Save(object sender, EventArgs e)
        {
            try
            {
                int LTID = Convert.ToInt32(hfLinkTrackingDomain.Value);
                if (rblLinkTracking.SelectedValue.Equals("specific"))
                {
                    foreach (DataRow dr in LinkTrackingDomain_DT.AsEnumerable())
                    {
                        string isDeleted = dr["IsDeleted"].ToString();
                        if (dr["LinkTrackingDomainID"].ToString().Contains("-") && isDeleted.Equals("False"))
                        {
                            ECN_Framework_Entities.Communicator.LinkTrackingDomain ltd = new ECN_Framework_Entities.Communicator.LinkTrackingDomain();
                            ltd.LTID = LTID;
                            ltd.CustomerID = CurrentCustomer.CustomerID;
                            ltd.Domain = dr["Domain"].ToString();
                            ltd.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                            ECN_Framework_BusinessLayer.Communicator.LinkTrackingDomain.Save(ltd, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                        }
                        if (isDeleted.Equals("True") && !dr["LinkTrackingDomainID"].ToString().Contains("-"))
                        {
                            ECN_Framework_BusinessLayer.Communicator.LinkTrackingDomain.Delete(Convert.ToInt32(dr["LinkTrackingDomainID"].ToString()), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                        }
                    }
                }
                else
                {
                    ECN_Framework_BusinessLayer.Communicator.LinkTrackingDomain.DeleteAll(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, LTID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                }
                modalPopupLinkTrackingDomain.Hide();
                txtDomainName.Text = "";
            }
            catch (ECN_Framework_Common.Objects.ECNException ecn)
            {
                setECNError(ecn);
            }
        }

        protected void btnLinkTrackingDomainAdd_Click(object sender, EventArgs e)
        {
            if (!txtDomainName.Text.Equals("") && LinkTrackingDomain_DT.Select("Domain LIKE '%" + txtDomainName.Text.Trim() + "%' AND IsDeleted = false").Length == 0)
            {
                DataTable dt = LinkTrackingDomain_DT;
                if (dt == null)
                {
                    dt = new DataTable();
                    DataColumn DomainTrackerFieldsID = new DataColumn("LinkTrackingDomainID", typeof(string));
                    dt.Columns.Add(DomainTrackerFieldsID);

                    DataColumn GroupDataFieldsName = new DataColumn("Domain", typeof(string));
                    dt.Columns.Add(GroupDataFieldsName);

                    DataColumn IsDeleted = new DataColumn("IsDeleted", typeof(bool));
                    dt.Columns.Add(IsDeleted);
                }
                DataRow dr = dt.NewRow();
                dr["LinkTrackingDomainID"] = Guid.NewGuid();
                dr["Domain"] = txtDomainName.Text;
                dr["IsDeleted"] = false;
                dt.Rows.Add(dr);
                txtDomainName.Text = "";
                LinkTrackingDomain_DT = dt;
                loadLinkTrackingDomainGrid();
                lblDomainError.Visible = false;
            }
            else
            {
                lblDomainError.Visible = true;
                lblDomainError.Text = "Domain name is invalid";
            }
        }

        protected void gvLinkTrackingDomains_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string LinkTrackingDomainID = e.CommandArgument.ToString();
            if (e.CommandName == "LinkTrackingDomain")
            {
                foreach (DataRow dr in LinkTrackingDomain_DT.AsEnumerable())
                {
                    if (dr["LinkTrackingDomainID"].Equals(LinkTrackingDomainID))
                    {
                        dr["IsDeleted"] = true;
                    }
                }
                loadLinkTrackingDomainGrid();
            }
        }
        #endregion

        protected void btnSchedule_Click_CheckMA(object sender, EventArgs e)
        {
            ECN_Framework_Entities.Communicator.BlastSetupInfo setupInfo = BlastScheduler1.SetupSchedule(getCampaignItemType());
            KMPlatform.Entity.User currentUser = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
            if (setupInfo != null)
            {
                if (!setupInfo.IsTestBlast.Value)
                {
                    //check if it exists in MA
                    List<ECN_Framework_Entities.Communicator.MarketingAutomation> checkList = new List<ECN_Framework_Entities.Communicator.MarketingAutomation>();
                    checkList = ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.CheckIfControlExists(CampaignItemID, ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.CampaignItem);
                    if(checkList.Count > 0)
                    {
                        string message = "This Campaign Item is tied to the following Marketing Automation(s) {0}. Making changes may require updates to the Automation(s). Are you sure you want to save your changes?";
                        StringBuilder sbMACheck = new StringBuilder();
                        foreach(ECN_Framework_Entities.Communicator.MarketingAutomation ma in checkList)
                        {
                            sbMACheck.Append(ma.Name + ",");
                        }

                        message = string.Format(message, sbMACheck.ToString().TrimEnd(','));

                        lblMAText.Text = message;
                        mpeMACheck.Show();
                    }
                    else
                    {
                        btnSchedule_Click(sender, e);
                    }
                }
                else
                {
                    btnSchedule_Click(sender, e);
                }
            }
            else
            {
                throwECNException("Error when setting up the schedule.");
                return;
            }
        }
        #region ErrorHandling
        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Blast, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }
        #endregion

        public int AddEmails()
        {
            ECN_Framework_Entities.Communicator.Group group = new ECN_Framework_Entities.Communicator.Group();
            if (ValidateEmailAddress())
            {
                try
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        string gname = StringFunctions.CleanString(txtGroupName.Text);
                        group.GroupName = gname;
                        group.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                        group.FolderID = Convert.ToInt32(drpFolder.SelectedValue);
                        group.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
                        group.PublicFolder = 1;
                        group.IsSeedList = false;
                        group.AllowUDFHistory = "N";
                        group.OwnerTypeCode = "customer";
                        ECN_Framework_BusinessLayer.Communicator.Group.Save(group, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

                        string emailAddressToAdd = txtEmailAddress.Text;
                        StringBuilder xmlInsert = new StringBuilder();
                        xmlInsert.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>");
                        DateTime startDateTime = DateTime.Now;

                        Hashtable hUpdatedRecords = new Hashtable();

                        if (emailAddressToAdd.Length > 0)
                        {
                            emailAddressToAdd = emailAddressToAdd.Replace("\r\n", ",");
                            emailAddressToAdd = emailAddressToAdd.Replace("\n", ",");
                            emailAddressToAdd = emailAddressToAdd.Replace("\r", ",");
                            StringTokenizer st = new StringTokenizer(emailAddressToAdd, ',');

                            while (st.HasMoreTokens())
                            {
                                xmlInsert.Append("<Emails><emailaddress>" + st.NextToken().Trim() + "</emailaddress></Emails>");

                            }

                            xmlInsert.Append("</XML>");
                            DataTable emailRecordsDT = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, group.GroupID, xmlInsert.ToString(), "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML></XML>", "HTML", "S", true, "", "Ecn.communicator.main.ecnwizard.controls.wizardschedule.AddEmails");
                        }
                        scope.Complete();
                    }
                }
                catch (ECN_Framework_Common.Objects.ECNException ex)
                {
                    setECNError(ex);
                }
            }
            return group.GroupID;
        }

        private bool ValidateEmailAddress()
        {
            bool isValidEA = true;
            string InvalidEmails = string.Empty;
            string emailaddress = string.Empty;
            Regex cr = new Regex("\n");
            int emailcount = 0;
            foreach (string s in cr.Split(txtEmailAddress.Text))
            {
                emailcount++;
                if (emailcount > getTestBlastCount())
                {
                    throw new Exception("Maximum of " + getTestBlastCount() + " emailaddresses are allowed for Test List.");
                }

                emailaddress = ECN_Framework_Common.Functions.StringFunctions.Remove(s, ECN_Framework_Common.Functions.StringFunctions.NonDomain());
                if (!CommonStringFunctions.IsEmail(emailaddress))
                {
                    isValidEA = false;
                    InvalidEmails += "<BR>" + emailaddress;
                }
            }

            if (!isValidEA)
            {
                ECN_Framework_Common.Objects.ECNError ecnError = new ECN_Framework_Common.Objects.ECNError(ECN_Framework_Common.Objects.Enums.Entity.Email, ECN_Framework_Common.Objects.Enums.Method.Save, "Invalid Email Address" + InvalidEmails);
                List<ECN_Framework_Common.Objects.ECNError> errorList = new List<ECN_Framework_Common.Objects.ECNError>();
                errorList.Add(ecnError);
                ECN_Framework_Common.Objects.ECNException ex = new ECN_Framework_Common.Objects.ECNException(errorList, ECN_Framework_Common.Objects.Enums.ExceptionLayer.WebSite);
                throw ex;
            }
            return isValidEA;
        }

        protected void rbTestNew_CheckedChanged(object sender, System.EventArgs e)
        {
            phMessage.Visible = false;
            phTestExisting.Visible = false;
            phTestNew.Visible = true;
        }

        protected void rbTestExisting_CheckedChanged(object sender, System.EventArgs e)
        {
            phMessage.Visible = false;
            phTestExisting.Visible = true;
            //testgroupExplorer1.LoadGroupFolder();
            phTestNew.Visible = false;
        }

        protected void chkOptOutMasterSuppression_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOptOutMasterSuppression.Checked)
            {
                chkOptOutSpecificGroup.Enabled = false;
                pnlOptOutSpecificGroups.Visible = false;
            }
            else
            {
                chkOptOutSpecificGroup.Enabled = true;
            }
        }

        #region OptOut Groups
        protected void chkOptOutSpecificGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOptOutSpecificGroup.Checked)
            {
                pnlOptOutSpecificGroups.Visible = true;
                chkOptOutMasterSuppression.Enabled = false;
            }
            else
            {
                pnlOptOutSpecificGroups.Visible = false;
                chkOptOutMasterSuppression.Enabled = true;
            }
        }

        protected void lnkSelectOptOutGroups_Click(object sender, EventArgs e)
        {
            ctrlgroupsLookup1.LoadControl();
            ctrlgroupsLookup1.Visible = true;
        }

        protected void gvOptOutGroups_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string CampaignItemOptOutID = e.CommandArgument.ToString();
            if (e.CommandName.Equals("CampaignItemOptOutGroupDelete"))
            {
                foreach (DataRow dr in OptoutGroups_DT.AsEnumerable())
                {
                    if (dr["CampaignItemOptOutID"].Equals(CampaignItemOptOutID))
                    {
                        dr["IsDeleted"] = true;
                    }
                }
                loadOptoutGroupsGrid();
            }
        }

        private void loadOptoutGroups()
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup> CampaignItemOptOutGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID(CampaignItemID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            if (CampaignItemOptOutGroupList.Count > 0)
            {
                foreach (ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup CampaignItemOptOutGroup in CampaignItemOptOutGroupList)
                {
                    ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(CampaignItemOptOutGroup.GroupID.Value, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    DataRow dr = OptoutGroups_DT.NewRow();
                    dr["CampaignItemOptOutID"] = CampaignItemOptOutGroup.CampaignItemOptOutID.ToString();
                    dr["GroupName"] = group.GroupName;
                    dr["GroupID"] = CampaignItemOptOutGroup.GroupID.Value;
                    dr["IsDeleted"] = CampaignItemOptOutGroup.IsDeleted;
                    OptoutGroups_DT.Rows.Add(dr);
                }
                chkOptOutSpecificGroup.Checked = true;
                pnlOptOutSpecificGroups.Visible = true;
                loadOptoutGroupsGrid();
            }
            else
            {
                chkOptOutSpecificGroup.Checked = false;
                pnlOptOutSpecificGroups.Visible = false;
            }
        }

        private void loadOptoutGroupsGrid()
        {
            if (OptoutGroups_DT != null)
            {
                var result = (from src in OptoutGroups_DT.AsEnumerable()
                              where src.Field<bool>("IsDeleted") == false
                              select new
                              {
                                  CampaignItemOptOutID = src.Field<string>("CampaignItemOptOutID"),
                                  GroupName = src.Field<string>("GroupName"),
                                  GroupID = src.Field<int>("GroupID"),
                                  IsDeleted = src.Field<bool>("IsDeleted")
                              }).ToList();
                gvOptOutGroups.Visible = true;
                gvOptOutGroups.DataSource = result;
                gvOptOutGroups.DataBind();

            }
            else
            {
                gvOptOutGroups.Visible = false;
            }
            pnlOptOutSpecificGroups.Update();
        }
        #endregion

        private bool CheckQueryParamLength()
        {
            StringBuilder sbParam = new StringBuilder();

            if (ddlOmniture1.SelectedIndex > 0)
                sbParam.Append(ddlOmniture1.SelectedValue.Equals("-1") ? txtOmniture1.Text : ddlOmniture1.SelectedItem.Text);
            if (ddlOmniture2.SelectedIndex > 0)
                sbParam.Append(ddlOmniture2.SelectedValue.Equals("-1") ? txtOmniture2.Text : ddlOmniture2.SelectedItem.Text);
            if (ddlOmniture3.SelectedIndex > 0)
                sbParam.Append(ddlOmniture3.SelectedValue.Equals("-1") ? txtOmniture3.Text : ddlOmniture3.SelectedItem.Text);
            if (ddlOmniture4.SelectedIndex > 0)
                sbParam.Append(ddlOmniture4.SelectedValue.Equals("-1") ? txtOmniture4.Text : ddlOmniture4.SelectedItem.Text);
            if (ddlOmniture5.SelectedIndex > 0)
                sbParam.Append(ddlOmniture5.SelectedValue.Equals("-1") ? txtOmniture5.Text : ddlOmniture5.SelectedItem.Text);
            if (ddlOmniture6.SelectedIndex > 0)
                sbParam.Append(ddlOmniture6.SelectedValue.Equals("-1") ? txtOmniture6.Text : ddlOmniture6.SelectedItem.Text);
            if (ddlOmniture7.SelectedIndex > 0)
                sbParam.Append(ddlOmniture7.SelectedValue.Equals("-1") ? txtOmniture7.Text : ddlOmniture7.SelectedItem.Text);
            if (ddlOmniture8.SelectedIndex > 0)
                sbParam.Append(ddlOmniture8.SelectedValue.Equals("-1") ? txtOmniture8.Text : ddlOmniture8.SelectedItem.Text);
            if (ddlOmniture9.SelectedIndex > 0)
                sbParam.Append(ddlOmniture9.SelectedValue.Equals("-1") ? txtOmniture9.Text : ddlOmniture9.SelectedItem.Text);
            if (ddlOmniture10.SelectedIndex > 0)
                sbParam.Append(ddlOmniture10.SelectedValue.Equals("-1") ? txtOmniture10.Text : ddlOmniture10.SelectedItem.Text);

            //adding 9 because of the delimiter
            if (sbParam.Length + 9 > 255)
            {

                return false;
            }
            else
                return true;
        }



        #endregion

        protected void chkScheduleReport_CheckedChanged(object sender, EventArgs e)
        {
            if (chkScheduleReport.Checked)
            {
                modalPopupScheduleReport.Show();
            }
            else
            {
                CancelScheduleReport();
                btnEditScheduleReport.Visible = false;
            }

        }
        protected void chkFtpExport_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFtpExport.Checked)
            {
                pnlFtp.Visible = true;
                modalPopupScheduleReport.Show();
            }
            else
            {
                pnlFtp.Visible = false;
                modalPopupScheduleReport.Show();
            }
        }
        protected void btnEditScheduleReport_Click(object sender, EventArgs e)
        {
            //Populate modal with view state values
            chkEmailBlastReport.Checked = scheduleBlastReport;
            chkFtpExport.Checked = scheduleFtpExport;
            if (chkFtpExport.Checked)
            {
                foreach (ListItem ftpExport in lbFtpExports.Items)
                {
                    if (ftpExports.Contains(ftpExport.Value))
                    {
                        ftpExport.Selected = true;
                    }
                }
                txtFtpUrl.Text = ftpUrl;
                txtFtpUsername.Text = ftpUsername;
                txtFtpPassword.Text = ftpPassword;
                ddlFormat.SelectedValue = ftpExportFormat;
                pnlFtp.Visible = true;
            }
            else
            {
                txtFtpPassword.Text = "";
                txtFtpUsername.Text = "";
                txtFtpUrl.Text = "";
                ddlFormat.SelectedIndex = 0;
                lbFtpExports.ClearSelection();
                pnlFtp.Visible = false;
            }

            txtFromEmail.Text = fromEmail;
            txtToEmail.Text = toEmail;
            txtSubject.Text = emailSubject;
            txtFromName.Text = fromName;
            txtAddCc.Text = string.Join(", ", ccList.ToArray());
            txtReportDate.Text = reportDate.ToShortDateString();
            ddlReportTime.SelectedValue = reportTime;
            divScheduleReportErrorMessage.Visible = false;
            modalPopupScheduleReport.Show();
        }
        protected void btnSaveReportDetails_Click(object sender, EventArgs e)
        {
            if (ValidateScheduleReportFields())
            {
                storeReportDetails();
                hasScheduledReport = true;
                btnEditScheduleReport.Visible = true;
                modalPopupScheduleReport.Hide();
                txtFtpUrl.Text = "";
                txtFtpUsername.Text = "";
                txtFtpPassword.Text = "";
                txtFromEmail.Text = "";
                txtToEmail.Text = "";
                txtSubject.Text = "";
                txtFromName.Text = "";
                txtAddCc.Text = "";
                txtReportDate.Text = "";
                ddlReportTime.SelectedIndex = 0;
                chkEmailBlastReport.Checked = false;
                chkFtpExport.Checked = false;
                pnlFtp.Visible = false;
                lbFtpExports.ClearSelection();
                divScheduleReportErrorMessage.Visible = false;
            }
            else
            {
                modalPopupScheduleReport.Show();
            }
        }
        protected void btnCancelDetailEdits_Click(object sender, EventArgs e)
        {
            txtFtpUrl.Text = "";
            txtFtpUsername.Text = "";
            txtFtpPassword.Text = "";
            txtFromEmail.Text = "";
            txtToEmail.Text = "";
            txtSubject.Text = "";
            txtFromName.Text = "";
            txtAddCc.Text = "";
            txtReportDate.Text = "";
            ddlReportTime.SelectedIndex = 0;
            chkEmailBlastReport.Checked = false;
            chkFtpExport.Checked = false;
            pnlFtp.Visible = false;
            lbFtpExports.ClearSelection();
            divScheduleReportErrorMessage.Visible = false;
            if (!hasScheduledReport)
            {
                chkScheduleReport.Checked = false;
            }
            modalPopupScheduleReport.Hide();
        }

        //method to validate the FTP credentials
        public bool CredentialsValid(string login, string password, string ftpURL )
        {
            revFTPURL.Validate();
            if (!revFTPURL.IsValid)
                return false;

            //try to post to ftp
            ECN_Framework_Common.Functions.FtpFunctions ftp = new ECN_Framework_Common.Functions.FtpFunctions(ftpURL, login, password);
            if ((!ftp.ValidateCredentials(login, password, ftpURL, "", "")) || (!ftp.ValidateFtpUrl(login, password, ftpURL, "", "")))
                return false;

            return true;
        }
        
        private void storeReportDetails()
        {
            reportTime = ddlReportTime.SelectedValue;
            DateTime tempDate = new DateTime();
            DateTime.TryParse(txtReportDate.Text, out tempDate);
            reportDate = tempDate;
            if (txtAddCc.Text != "")
            {
                ccList.Clear();
                List<string> ccListTemp = new List<string>();
                if (txtAddCc.Text.IndexOf(',') >= 0)
                {
                    foreach (string emailToAdd in txtAddCc.Text.Split(','))
                    {
                        string currentEmail = emailToAdd.Trim();
                        if (ECN_Framework_Common.Functions.RegexUtilities.IsValidEmail(currentEmail))
                        {
                            ccListTemp.Add(currentEmail);
                        }
                    }
                    ccList = ccListTemp;
                }
                else if (ECN_Framework_Common.Functions.RegexUtilities.IsValidEmail(txtAddCc.Text))
                {
                    ccListTemp.Add(txtAddCc.Text);
                    ccList = ccListTemp;
                }
            }
            scheduleBlastReport = chkEmailBlastReport.Checked;

            scheduleFtpExport = chkFtpExport.Checked;
            if (scheduleFtpExport)
            {
                List<string> ftpExportsTemp = new List<string>();
                foreach (ListItem ftpToExport in lbFtpExports.Items)
                {
                    if (ftpToExport.Selected)
                    {
                        ftpExportsTemp.Add(ftpToExport.Value);
                    }
                }
                ftpExports = ftpExportsTemp;
                ftpUsername = txtFtpUsername.Text;
                ftpUrl = txtFtpUrl.Text;
                ftpPassword = txtFtpPassword.Text;
                ftpExportFormat = ddlFormat.SelectedValue;
            }
            else
            {
                ftpExports = new List<string>();
                ftpUrl = "";
                ftpUsername = "";
                ftpPassword = "";
                ftpExportFormat = "";
            }
            fromEmail = txtFromEmail.Text;
            toEmail = txtToEmail.Text;

            fromName = txtFromName.Text;
            emailSubject = txtSubject.Text;
        }

        protected void txtFromName_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnMACheckCancel_Click(object sender, EventArgs e)
        {
            mpeMACheck.Hide();
        }

        private void SetupScheduler(bool isTestBlast = false)
        {
            SetAbCompainTypeVisibility();
            isTestBlast = isTestBlast && SetRegularCompainTypeVisibility();
            var campaignItem = BusinessCampaignItem.GetByCampaignItemID(CampaignItemID, ECNSession.CurrentSession().CurrentUser, false);
            var campaignItemBlasts = BusinessCampaignItemBlast.GetByCampaignItemID(
                CampaignItemID,
                ECNSession.CurrentSession().CurrentUser,
                false);
            SetupBlastScheduler(isTestBlast, campaignItemBlasts, campaignItem);
            SetupGoogleAnalytics();
            SetupEcnConversionTracking();
            SetupOmnitureTracking();
        }

        private void SetupOmnitureTracking()
        {
            if (Client.HasServiceFeature(ECNSession.CurrentSession().ClientID,
                KMPlatform.Enums.Services.EMAILMARKETING,
                KMPlatform.Enums.ServiceFeatures.Omniture))
            {
                var linkTrackingParamList = BusinessLinkTrackingParam.GetByLinkTrackingID(OmnitureLinkTrackingId);

                var campaignItemLinkTrackingList = BusinessCampaignItemLinkTracking.GetByCampaignItemID(
                    CampaignItemID,
                    ECNSession.CurrentSession().CurrentUser);
                if (campaignItemLinkTrackingList.Count > 0)
                {
                    var omniValue1 = OmnitureHelper.LoadOmnitureCompainItemSavedData(
                        linkTrackingParamList,
                        campaignItemLinkTrackingList,
                        "Omniture1",
                        ddlOmniture1,
                        txtOmniture1);

                    var omniValue2 = OmnitureHelper.LoadOmnitureCompainItemSavedData(
                        linkTrackingParamList,
                        campaignItemLinkTrackingList,
                        "Omniture2",
                        ddlOmniture2,
                        txtOmniture2);

                    var omniValue3 = OmnitureHelper.LoadOmnitureCompainItemSavedData(
                        linkTrackingParamList,
                        campaignItemLinkTrackingList,
                        "Omniture3",
                        ddlOmniture3,
                        txtOmniture3);

                    var omniValue4 = OmnitureHelper.LoadOmnitureCompainItemSavedData(
                        linkTrackingParamList,
                        campaignItemLinkTrackingList,
                        "Omniture4",
                        ddlOmniture4,
                        txtOmniture4);

                    var omniValue5 = OmnitureHelper.LoadOmnitureCompainItemSavedData(
                        linkTrackingParamList,
                        campaignItemLinkTrackingList,
                        "Omniture5",
                        ddlOmniture5,
                        txtOmniture5);

                    var omniValue6 = OmnitureHelper.LoadOmnitureCompainItemSavedData(
                        linkTrackingParamList,
                        campaignItemLinkTrackingList,
                        "Omniture6",
                        ddlOmniture6,
                        txtOmniture6);

                    var omniValue7 = OmnitureHelper.LoadOmnitureCompainItemSavedData(
                        linkTrackingParamList,
                        campaignItemLinkTrackingList,
                        "Omniture7",
                        ddlOmniture7,
                        txtOmniture7);

                    var omniValue8 = OmnitureHelper.LoadOmnitureCompainItemSavedData(
                        linkTrackingParamList,
                        campaignItemLinkTrackingList,
                        "Omniture8",
                        ddlOmniture8,
                        txtOmniture8);

                    var omniValue9 = OmnitureHelper.LoadOmnitureCompainItemSavedData(
                        linkTrackingParamList,
                        campaignItemLinkTrackingList,
                        "Omniture9",
                        ddlOmniture9,
                        txtOmniture9);

                    var omniValue10 = OmnitureHelper.LoadOmnitureCompainItemSavedData(
                        linkTrackingParamList,
                        campaignItemLinkTrackingList,
                        "Omniture10",
                        ddlOmniture10,
                        txtOmniture10);

                    if (omniValue1.Any() ||
                        omniValue2.Any() ||
                        omniValue3.Any() ||
                        omniValue4.Any() ||
                        omniValue5.Any() ||
                        omniValue6.Any() ||
                        omniValue7.Any() ||
                        omniValue8.Any() ||
                        omniValue9.Any() ||
                        omniValue10.Any())
                    {
                        chkboxOmnitureTracking.Checked = true;
                        pnlOmniture.Visible = true;
                    }
                }
            }
            else
            {
                chkboxOmnitureTracking.Enabled = false;
            }
        }

        private void SetupEcnConversionTracking()
        {
            if (Client.HasServiceFeature(ECNSession.CurrentSession().ClientID,
                KMPlatform.Enums.Services.EMAILMARKETING,
                KMPlatform.Enums.ServiceFeatures.ConversionTracking))
            {
                var campaignItemLinkTrackingList = BusinessCampaignItemLinkTracking.GetByCampaignItemID(
                    CampaignItemID,
                    ECNSession.CurrentSession().CurrentUser);

                if (campaignItemLinkTrackingList.Count > 0)
                {
                    var linkTrackingList = ECN_Framework_BusinessLayer.Communicator.LinkTracking.GetAll();

                    var conversionTrackingQuery = (from src in linkTrackingList
                                                   where src.DisplayName == "ECN Conversion Tracking"
                                                   select src).ToList();
                    var linkTrackingParamList = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParam.GetByLinkTrackingID(conversionTrackingQuery[0].LTID);
                    var eid = (from src in linkTrackingParamList
                               where src.DisplayName == "eid"
                               select src).ToList();
                    var eidValue = from src in campaignItemLinkTrackingList
                                   where src.LTPID == eid.FirstOrDefault()?.LTPID
                                   select src;

                    if (eidValue.Any())
                    {
                        chkboxConvTracking.Checked = true;
                    }
                }
            }
            else
            {
                chkboxConvTracking.Enabled = false;
            }
        }

        private void SetupGoogleAnalytics()
        {
            if (Client.HasServiceFeature(ECNSession.CurrentSession().ClientID,
                KMPlatform.Enums.Services.EMAILMARKETING,
                KMPlatform.Enums.ServiceFeatures.GoogleAnalytics))
            {
                var linkTrackingList = ECN_Framework_BusinessLayer.Communicator.LinkTracking.GetAll();
                var googleAnalyticsQuery = (from src in linkTrackingList
                                            where src.DisplayName == "Google"
                                            select src).ToList();
                var linkTrackingParamList = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParam.GetByLinkTrackingID(googleAnalyticsQuery[0].LTID);

                #region LoadCustomerLinkTrackingData

                var customerLinkTrackingList = ECN_Framework_BusinessLayer.Communicator.CustomerLinkTracking.GetByCustomerID(
                    ECNSession.CurrentSession().CurrentCustomer.CustomerID);

                SetLinkTrackingValueFor("utm_source", drpCampaignSource, linkTrackingParamList, customerLinkTrackingList);
                SetLinkTrackingValueFor("utm_medium", drpCampaignMedium, linkTrackingParamList, customerLinkTrackingList);
                SetLinkTrackingValueFor("utm_term", drpCampaignTerm, linkTrackingParamList, customerLinkTrackingList);
                SetLinkTrackingValueFor("utm_content", drpCampaignContent, linkTrackingParamList, customerLinkTrackingList);
                SetLinkTrackingValueFor("utm_campaign", drpCampaignName, linkTrackingParamList, customerLinkTrackingList);

                #endregion

                pnlGoogleAnalytics.Visible = true;
                chkboxGoogleAnalytics.Checked = true;

                var campaignItemLinkTrackingList = BusinessCampaignItemLinkTracking.GetByCampaignItemID(
                    CampaignItemID,
                    ECNSession.CurrentSession().CurrentUser);
                if (campaignItemLinkTrackingList.Count > 0)
                {
                    SetCompainItemLinkTrackingValueFor("utm_source", txtCampaignSource, linkTrackingParamList, campaignItemLinkTrackingList);
                    var utmMediumValue = SetCompainItemLinkTrackingValueFor("utm_medium", txtCampaignMedium, linkTrackingParamList, campaignItemLinkTrackingList);
                    var utmTermValue = SetCompainItemLinkTrackingValueFor("utm_term", txtCampaignTerm, linkTrackingParamList, campaignItemLinkTrackingList);
                    var utmContentValue = SetCompainItemLinkTrackingValueFor("utm_content", txtCampaignContent, linkTrackingParamList, campaignItemLinkTrackingList);
                    var utmCampaignValue = SetCompainItemLinkTrackingValueFor("utm_campaign", txtCampaignName, linkTrackingParamList, campaignItemLinkTrackingList);
                    if (utmMediumValue.Any() || utmTermValue.Any() || utmContentValue.Any() || utmCampaignValue.Any())
                    {
                        chkboxGoogleAnalytics.Checked = true;
                        pnlGoogleAnalytics.Visible = true;
                    }
                }
            }
            else
            {
                chkboxGoogleAnalytics.Enabled = false;
            }
        }

        private List<CampaignItemLinkTracking> SetCompainItemLinkTrackingValueFor(
            string tagName,
            TextBox destinationTextBox,
            IEnumerable<LinkTrackingParam> linkTrackingParamList,
            IEnumerable<CampaignItemLinkTracking> campaignItemLinkTrackingList)
        {
            var linkTrackingParams = linkTrackingParamList.Where(src => src.DisplayName == tagName).ToList();
            var itemLinkTrackings = campaignItemLinkTrackingList.Where(src =>
                src.LTPID == linkTrackingParams.FirstOrDefault()?.LTPID).ToList();
            if (itemLinkTrackings.Any())
            {
                var ltpoId = itemLinkTrackings.First().LTPOID.Value.ToString();
                drpCampaignSource.SelectedValue = ltpoId;
                if (ltpoId.Equals("6"))
                {
                    destinationTextBox.Text = itemLinkTrackings.First().CustomValue;
                    destinationTextBox.Visible = true;
                }
                else
                {
                    destinationTextBox.Visible = false;
                }
            }

            return itemLinkTrackings;
        }

        private void SetLinkTrackingValueFor(
            string tagName,
            DropDownList destinationDropDown,
            IEnumerable<LinkTrackingParam> linkTrackingParamList,
            IEnumerable<CustomerLinkTracking> customerLinkTrackingList)
        {
            var linkTrackingParams = linkTrackingParamList.Where(src => src.DisplayName == tagName).ToList();
            var valueList = customerLinkTrackingList.Where(src =>
                src.LTPID == linkTrackingParams.FirstOrDefault()?.LTPID).ToList();
            if (valueList.Any())
            {
                destinationDropDown.SelectedValue = valueList.First().LTPOID.Value.ToString();
            }
        }

        private void SetupBlastScheduler(bool isTestBlast, IReadOnlyList<CampaignItemBlast> campaignItemBlasts, CampaignItem campaignItem)
        {
            BlastScheduler1.CanScheduleBlast = true;
            BlastScheduler1.RequestBlastID = 0;
            if (campaignItemBlasts.Count > 0)
            {
                BlastScheduler1.SourceBlastID = campaignItemBlasts[0].BlastID ?? 0;
            }
            else
            {
                BlastScheduler1.SourceBlastID = 0;
            }

            BlastScheduler1.CampaignItemType = campaignItem.CampaignItemType.ToLower();
            if (BlastScheduler1.SourceBlastID == 0)
            {
                BlastScheduler1.SetupWizard(isTestBlast);
                pnlTestOptions.Visible = isTestBlast;
            }
            else
            {
                BlastScheduler1.SetupWizard();
                pnlTestOptions.Visible = false;
            }
        }

        private bool SetRegularCompainTypeVisibility()
        {
            if (getCampaignItemType().Equals("regular", StringComparison.InvariantCultureIgnoreCase))
            {
                BlastScheduler1.CanTestBlast = true;
                BlastScheduler1.CanEmailPreview = true;
                BlastScheduler1.CanScheduleRecurringBlast = true;
                return true;
            }

            BlastScheduler1.CanTestBlast = false;
            BlastScheduler1.CanEmailPreview = false;
            BlastScheduler1.CanScheduleRecurringBlast = false;
            return false;
        }

        private void SetAbCompainTypeVisibility()
        {
            if (getCampaignItemType().Equals("ab", StringComparison.InvariantCultureIgnoreCase))
            {
                BlastScheduler1.CanSendAll = false;
                chkGoToChampion.Visible = true;
            }
            else
            {
                BlastScheduler1.CanSendAll = true;
                chkGoToChampion.Visible = false;
            }
        }

        private bool ValidateScheduleReportFields()
        {
            var errorMessages = new List<string>();
            CheckReportTypeSelected(errorMessages);

            if (chkFtpExport.Checked)
            {
                CheckItemsToExportSelected(errorMessages);
                CheckFtpUrlEntered(errorMessages);
                CheckFtpUsernameEntered(errorMessages);
                CheckFtpPasswordEntered(errorMessages);
                CheckFtpCredentialsValid(errorMessages);
            }

            CheckFromEmailValid(errorMessages);
            CheckToEmailValid(errorMessages);
            CheckSubjectEntered(errorMessages);
            CheckFromNameEntered(errorMessages);
            CheckCcEmailsValid(errorMessages);
            CheckDatesPeriodValid(errorMessages);

            if (errorMessages.Any())
            {
                modalError = string.Join("<br \\>", errorMessages.ToArray());
                divScheduleReportErrorMessage.InnerHtml = $"<p align=\"center\" Style=\"color:red\">{ modalError }";
                divScheduleReportErrorMessage.Visible = true;
            }
            else
            {
                modalError = string.Empty;
                divScheduleReportErrorMessage.Visible = false;
            }
            return !errorMessages.Any();
        }

        private void CheckDatesPeriodValid(List<string> errorMessages)
        {
            DateTime tempDate;
            DateTime tempTime;

            DateTime.TryParse(txtReportDate.Text, out tempDate);
            DateTime.TryParse(ddlReportTime.SelectedValue, out tempTime);
            tempDate = new DateTime(tempDate.Year,
                tempDate.Month,
                tempDate.Day,
                tempTime.Hour,
                tempTime.Minute,
                tempTime.Second);

            if (tempDate == DateTime.MinValue || tempDate.CompareTo(DateTime.Now) <= 0)
            {
                errorMessages.Add("Please enter a valid future date.");
            }
        }

        private void CheckCcEmailsValid(List<string> errorMessages)
        {
            if (txtAddCc.Text == string.Empty)
            {
                return;
            }

            if (txtAddCc.Text.IndexOf(Comma) >= 0)
            {
                var emailsToAdd = txtAddCc.Text.Split(Comma);
                errorMessages.AddRange(emailsToAdd.Select(emailToAdd => emailToAdd.Trim())
                    .Where(currentEmail => !Email.IsValidEmailAddress(currentEmail))
                    .Select(currentEmail => $"The email address {currentEmail} is invalid."));

                if (emailsToAdd.Length > MaxCarbonCopyLimit)
                {
                    errorMessages.Add($"Invalid Cc: {txtAddCc.Text.Trim()}");
                }
            }
            else if (!Email.IsValidEmailAddress(txtAddCc.Text))
            {
                errorMessages.Add($"Invalid Cc: {txtAddCc.Text.Trim()}");
            }
        }

        private void CheckFromNameEntered(List<string> errorMessages)
        {
            if (string.IsNullOrWhiteSpace(txtFromName.Text))
            {
                errorMessages.Add("Please enter a from name.");
            }
        }

        private void CheckSubjectEntered(List<string> errorMessages)
        {
            if (string.IsNullOrWhiteSpace(txtSubject.Text))
            {
                errorMessages.Add("Please enter a subject line.");
            }
        }

        private void CheckToEmailValid(List<string> errorMessages)
        {
            if (!Email.IsValidEmailAddress(txtToEmail.Text))
            {
                errorMessages.Add($"Invalid to email: {txtToEmail.Text}.");
            }
        }

        private void CheckFromEmailValid(ICollection<string> errorMessages)
        {
            if (!Email.IsValidEmailAddress(txtFromEmail.Text))
            {
                errorMessages.Add($"Invalid from email: {txtFromEmail.Text}.");
            }
        }

        private void CheckFtpCredentialsValid(ICollection<string> errorMessages)
        {
            if (!errorMessages.Any() && !CredentialsValid(txtFtpUsername.Text, txtFtpPassword.Text, txtFtpUrl.Text))
            {
                errorMessages.Add("Entered FTP credentials are invalid");
            }
        }

        private void CheckFtpPasswordEntered(ICollection<string> errorMessages)
        {
            if (string.IsNullOrWhiteSpace(txtFtpPassword.Text))
            {
                errorMessages.Add("Please enter a Password for connecting to the FTP site.");
            }
        }

        private void CheckFtpUsernameEntered(List<string> errorMessages)
        {
            if (string.IsNullOrWhiteSpace(txtFtpUsername.Text))
            {
                errorMessages.Add("Please enter a Username for connecting to the FTP site.");
            }
        }

        private void CheckFtpUrlEntered(ICollection<string> errorMessages)
        {
            if (string.IsNullOrWhiteSpace(txtFtpUrl.Text))
            {
                errorMessages.Add("Please enter a valid FTP URL.");
            }
        }

        private void CheckItemsToExportSelected(ICollection<string> errorMessages)
        {
            var atLeastOneFtp = false;
            foreach (ListItem ftpToExport in lbFtpExports.Items)
            {
                if (ftpToExport.Selected)
                {
                    atLeastOneFtp = true;
                    break;
                }
            }

            if (!atLeastOneFtp)
            {
                errorMessages.Add("Please select the statistics you would like exported to FTP.");
            }
        }

        private void CheckReportTypeSelected(ICollection<string> errorMessages)
        {
            if (!chkFtpExport.Checked && !chkEmailBlastReport.Checked)
            {
                errorMessages.Add("Please select at least one type of report.");
            }
        }

        /// <summary>
        /// Populate the schedule report entity with the type of report requested and details from the user, updating existing reportSchedule items where necessary 
        /// </summary>
        /// <param name="newBlastList"></param>
        private void SaveScheduleReport(List<BlastAbstract> newBlastList)
        {
            try
            {
                var alreadyScheduledReports = GetReportsAlreadyScheduled(newBlastList);
                var alreadyScheduledBlastReport = alreadyScheduledReports.FindAll(blastScheduleReport => blastScheduleReport.ReportID == BlastReportId);
                var alreadyScheduledFtpReport = alreadyScheduledReports.FindAll(ftpScheduleReport => ftpScheduleReport.ReportID == FtpReportId);

                ScheduleReports(
                    newBlastList,
                    alreadyScheduledBlastReport,
                    FillEmailParamsInReportSchedule,
                    scheduleBlastReport);
                ScheduleReports(
                    newBlastList,
                    alreadyScheduledFtpReport,
                    FillFtpParamsInReportSchedule,
                    scheduleFtpExport);
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }

        private static void ScheduleReports(
            IEnumerable<BlastAbstract> newBlastList,
            IReadOnlyCollection<ReportSchedule> alreadyScheduledReports,
            Func<ReportSchedule, User> fillParamsFunc,
            bool scheduleReport)
        {
            if (alreadyScheduledReports.Count > 0)
            {
                if (scheduleReport)
                {
                    foreach (var reportSchedule in alreadyScheduledReports)
                    {
                        var currentUser = fillParamsFunc(reportSchedule);
                        if (reportSchedule.ReportScheduleID > 0)
                        {
                            BusinessReportQueue.Delete_ReportScheduleID(reportSchedule.ReportScheduleID);
                        }

                        BusinessReportSchedule.Save(reportSchedule, currentUser);
                    }
                }
                else
                {
                    foreach (var report in alreadyScheduledReports)
                    {
                        if (report.ReportScheduleID > 0)
                        {
                            BusinessReportQueue.Delete_ReportScheduleID(report.ReportScheduleID);
                        }

                        BusinessReportSchedule.Delete(report.ReportScheduleID, ECNSession.CurrentSession().CurrentUser);
                    }
                }
            }
            else if (scheduleReport)
            {
                foreach (var blast in newBlastList)
                {
                    var reportSchedule = new ReportSchedule { BlastID = blast.BlastID };

                    var currentUser = fillParamsFunc(reportSchedule);
                    BusinessReportSchedule.Save(reportSchedule, currentUser);
                }
            }
        }

        private static List<ReportSchedule> GetReportsAlreadyScheduled(IEnumerable<BlastAbstract> newBlastList)
        {
            var alreadyScheduledReports = new List<ReportSchedule>();
            foreach (var blast in newBlastList)
            {
                var listOfReportsForCurrentBlast = BusinessReportSchedule.GetByBlastId(blast.BlastID);
                foreach (var report in listOfReportsForCurrentBlast)
                {
                    alreadyScheduledReports.Add(report);
                }
            }

            return alreadyScheduledReports;
        }

        private User FillEmailParamsInReportSchedule(ReportSchedule reportSchedule)
        {
            var currentUser = ECNSession.CurrentSession().CurrentUser;
            new ReportScheduleBuilder(reportSchedule)
                .SetReportId(ECN_Framework_BusinessLayer.Communicator.Reports.GetByReportName("BlastDetailsReport", currentUser).ReportID)
                .SetCommonHeader(
                    currentUser.UserID,
                    ECNSession.CurrentSession().CurrentUser.CustomerID,
                    reportTime,
                    reportDate,
                    fromEmail,
                    toEmail,
                    fromName,
                    emailSubject)
                .AddCcList(ccList)
                .Build();
            return currentUser;
        }

        private User FillFtpParamsInReportSchedule(ReportSchedule reportSchedule)
        {
            var currentUser = ECNSession.CurrentSession().CurrentUser;
            new ReportScheduleBuilder(reportSchedule)
                .SetReportId(ECN_Framework_BusinessLayer.Communicator.Reports.GetByReportName("FtpExport", currentUser).ReportID)
                .SetCommonHeader(
                    currentUser.UserID,
                    ECNSession.CurrentSession().CurrentUser.CustomerID,
                    reportTime,
                    reportDate,
                    fromEmail,
                    toEmail,
                    fromName,
                    emailSubject)
                .AddExportsSection(ftpExports)
                .AddFtpCredentials(ftpUrl, ftpUsername, ftpPassword, ftpExportFormat)
                .AddCcList(ccList)
                .AddExportSettings()
                .Build();
            return currentUser;
        }

        private void CancelScheduleReport()
        {
            fromEmail = string.Empty;
            toEmail = string.Empty;
            emailSubject = string.Empty;
            fromName = string.Empty;
            ccList = new List<string>();
            reportDate = new DateTime();
            reportTime = string.Empty;
            scheduleBlastReport = false;
            scheduleFtpExport = false;
            ftpExports = new List<string>();
            ftpUrl = string.Empty;
            ftpUsername = string.Empty;
            ftpPassword = string.Empty;
            ftpExportFormat = string.Empty;

            txtFtpUrl.Text = string.Empty;
            txtFtpUsername.Text = string.Empty;
            txtFtpPassword.Text = string.Empty;
            txtFromEmail.Text = string.Empty;
            txtToEmail.Text = string.Empty;
            txtSubject.Text = string.Empty;
            txtFromName.Text = string.Empty;
            txtAddCc.Text = string.Empty;
            txtReportDate.Text = string.Empty;
            ddlReportTime.SelectedIndex = 0;
            chkEmailBlastReport.Checked = false;
            chkFtpExport.Checked = false;
            lbFtpExports.ClearSelection();
            divScheduleReportErrorMessage.Visible = false;
            pnlFtp.Visible = false;
            hasScheduledReport = false;
            modalPopupScheduleReport.Hide();
        }

        private void LoadLinkTrackingParamOptions()
        {
            var campaignItem = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(
                CampaignItemID,
                ECNSession.CurrentSession().CurrentUser,
                false);

            LoadGoogleLinkTrackingParamOptions();

            if (Client.HasServiceFeature(
                ECNSession.CurrentSession().ClientID,
                KMPlatform.Enums.Services.EMAILMARKETING,
                KMPlatform.Enums.ServiceFeatures.Omniture))
            {
                LoadOmnitureLinkTrackingParamOptions(campaignItem);
            }
        }

        private void LoadOmnitureLinkTrackingParamOptions(CampaignItem campaignItem)
        {
            Guard.NotNull(campaignItem, nameof(campaignItem));

            var ltpList = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParam.GetByLinkTrackingID(OmnitureLinkTrackingId);

            var lstCampaignItemLinkTracking = BusinessCampaignItemLinkTracking.GetByCampaignItemID(
                    CampaignItemID,
                    ECNSession.CurrentSession().CurrentUser);
            var ltsBase = LinkTrackingSettings.GetByBaseChannelID_LTID(
                    ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID,
                    OmnitureLinkTrackingId);
            var ltsCustomer = LinkTrackingSettings.GetByCustomerID_LTID(
                    CurrentCustomer.CustomerID,
                    OmnitureLinkTrackingId);
            var omnitureSettings = OmnitureHelper.GetOmnitureSettings(ltsBase, ltsCustomer);

            LoadOmnitureParamOptionsTemplate(lstCampaignItemLinkTracking, ltpList, omnitureSettings);

            if (campaignItem.CampaignItemTemplateID.GetValueOrDefault(0) > 0)
            {
                LoadOmnitureCompainContextParamOptions(campaignItem);
            }
        }

        private void LoadOmnitureCompainContextParamOptions(CampaignItem campaignItem)
        {
            Guard.NotNull(campaignItem.CampaignItemTemplateID, () =>
            {
                throw new ArgumentException("CampaignItemTemplateID should have a value");
            });

            ddlOmniture1.ClearSelection();
            ddlOmniture2.ClearSelection();
            ddlOmniture3.ClearSelection();
            ddlOmniture4.ClearSelection();
            ddlOmniture5.ClearSelection();
            ddlOmniture6.ClearSelection();
            ddlOmniture7.ClearSelection();
            ddlOmniture8.ClearSelection();
            ddlOmniture9.ClearSelection();
            ddlOmniture10.ClearSelection();

            var campaignItemTemplate = CampaignItemTemplate.GetByCampaignItemTemplateID(
                campaignItem.CampaignItemTemplateID.GetValueOrDefault(),
                ECNSession.CurrentSession().CurrentUser);

            SetOmnitureDropDownCampainValue(campaignItemTemplate.Omniture1, ddlOmniture1);
            SetOmnitureDropDownCampainValue(campaignItemTemplate.Omniture2, ddlOmniture2);
            SetOmnitureDropDownCampainValue(campaignItemTemplate.Omniture3, ddlOmniture3);
            SetOmnitureDropDownCampainValue(campaignItemTemplate.Omniture4, ddlOmniture4);
            SetOmnitureDropDownCampainValue(campaignItemTemplate.Omniture5, ddlOmniture5);
            SetOmnitureDropDownCampainValue(campaignItemTemplate.Omniture6, ddlOmniture6);
            SetOmnitureDropDownCampainValue(campaignItemTemplate.Omniture7, ddlOmniture7);
            SetOmnitureDropDownCampainValue(campaignItemTemplate.Omniture8, ddlOmniture8);
            SetOmnitureDropDownCampainValue(campaignItemTemplate.Omniture9, ddlOmniture9);
            SetOmnitureDropDownCampainValue(campaignItemTemplate.Omniture10, ddlOmniture10);

            if (campaignItemTemplate.Omniture1.Any() ||
                campaignItemTemplate.Omniture2.Any() ||
                campaignItemTemplate.Omniture3.Any() ||
                campaignItemTemplate.Omniture4.Any() ||
                campaignItemTemplate.Omniture5.Any() ||
                campaignItemTemplate.Omniture6.Any() ||
                campaignItemTemplate.Omniture7.Any() ||
                campaignItemTemplate.Omniture8.Any() ||
                campaignItemTemplate.Omniture9.Any() ||
                campaignItemTemplate.Omniture10.Any())
            {
                chkboxOmnitureTracking.Checked = true;
                pnlOmniture.Visible = true;
            }
        }

        private static void SetOmnitureDropDownCampainValue(string campaignItemTemplateName, DropDownList dropDown)
        {
            Guard.NotNullOrWhitespace(campaignItemTemplateName, nameof(campaignItemTemplateName));
            Guard.NotNull(dropDown, nameof(dropDown));

            var omnitureCompainItem = dropDown.Items.FindByText(campaignItemTemplateName);
            if (omnitureCompainItem != null)
            {
                dropDown.SelectedValue = omnitureCompainItem.Value;
            }
        }

        private void LoadOmnitureParamOptionsTemplate(
            List<CampaignItemLinkTracking> lstCampaignItemLinkTracking,
            List<LinkTrackingParam> linkTrackingParams,
            OmnitureSettings omnitureSettings)
        {
            Guard.NotNull(lstCampaignItemLinkTracking, nameof(lstCampaignItemLinkTracking));
            Guard.NotNull(linkTrackingParams, nameof(linkTrackingParams));
            Guard.NotNull(omnitureSettings, nameof(omnitureSettings));

            Func<string, bool> hasCampainItemLinkTrackingFirst = displayName =>
                !lstCampaignItemLinkTracking.Exists(x => x.LTPID == linkTrackingParams.First(y => y.DisplayName == displayName).LTPID);
            Func<string, bool> hasCampainItemLinkTracking = displayName => true;
            Func<string, List<EntitiesLinkTrackingParamOption>> ltpoListFunc = null;
            Func<string, EntitiesLinkTrackingParamSettings> ltpsFunc = null;
            if (omnitureSettings.UseOverride())
            {
                var customerId = CurrentCustomer.CustomerID;

                ltpoListFunc = displayName =>
                    BusinessLinkTrackingParamOption.Get_LTPID_CustomerID(
                        linkTrackingParams.First(x => x.DisplayName == displayName).LTPID,
                        customerId);
                ltpsFunc = displayName =>
                    BusinessLinkTrackingParamSettings.Get_LTPID_CustomerID(
                        linkTrackingParams.First(x => x.DisplayName == displayName).LTPID,
                        customerId);
            }
            else if (omnitureSettings.UseBaseChannel())
            {
                var baseChannelId = ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID;

                ltpoListFunc = displayName =>
                    BusinessLinkTrackingParamOption.Get_LTPID_BaseChannelID(
                        linkTrackingParams.First(x => x.DisplayName == displayName).LTPID,
                        baseChannelId);
                ltpsFunc = displayName =>
                    BusinessLinkTrackingParamSettings.Get_LTPID_BaseChannelID(
                        linkTrackingParams.First(x => x.DisplayName == displayName).LTPID,
                        baseChannelId);
                hasCampainItemLinkTracking = hasCampainItemLinkTrackingFirst;
            }

            if (omnitureSettings.UseOverride() || omnitureSettings.UseBaseChannel())
            {
                SetOmnitureDropDownContexts(
                    lstCampaignItemLinkTracking,
                    linkTrackingParams,
                    hasCampainItemLinkTrackingFirst,
                    ltpoListFunc,
                    ltpsFunc,
                    hasCampainItemLinkTracking);
            }
            else
            {
                chkboxOmnitureTracking.Enabled = false;
            }
        }

        private void SetOmnitureDropDownContexts(
            List<CampaignItemLinkTracking> lstCampaignItemLinkTracking,
            IList<LinkTrackingParam> ltpList,
            Func<string, bool> hasCampainItemLinkTrackingFirst,
            Func<string, List<EntitiesLinkTrackingParamOption>> ltpoListFunc,
            Func<string, EntitiesLinkTrackingParamSettings> ltpsFunc,
            Func<string, bool> hasCampainItemLinkTracking)
        {
            OmnitureHelper.InitializeOmnitureDropDown(
                ltpList,
                lstCampaignItemLinkTracking,
                "Omniture1",
                ddlOmniture1,
                lblOmniture1,
                pnlOmniture,
                chkboxOmnitureTracking,
                hasCampainItemLinkTrackingFirst,
                ltpoListFunc,
                ltpsFunc);

            OmnitureHelper.InitializeOmnitureDropDown(
                ltpList,
                lstCampaignItemLinkTracking,
                "Omniture2",
                ddlOmniture2,
                lblOmniture2,
                pnlOmniture,
                chkboxOmnitureTracking,
                hasCampainItemLinkTracking,
                ltpoListFunc,
                ltpsFunc);

            OmnitureHelper.InitializeOmnitureDropDown(
                ltpList,
                lstCampaignItemLinkTracking,
                "Omniture3",
                ddlOmniture3,
                lblOmniture3,
                pnlOmniture,
                chkboxOmnitureTracking,
                hasCampainItemLinkTracking,
                ltpoListFunc,
                ltpsFunc);

            OmnitureHelper.InitializeOmnitureDropDown(
                ltpList,
                lstCampaignItemLinkTracking,
                "Omniture4",
                ddlOmniture4,
                lblOmniture4,
                pnlOmniture,
                chkboxOmnitureTracking,
                hasCampainItemLinkTracking,
                ltpoListFunc,
                ltpsFunc);

            OmnitureHelper.InitializeOmnitureDropDown(
                ltpList,
                lstCampaignItemLinkTracking,
                "Omniture5",
                ddlOmniture5,
                lblOmniture5,
                pnlOmniture,
                chkboxOmnitureTracking,
                hasCampainItemLinkTracking,
                ltpoListFunc,
                ltpsFunc);

            OmnitureHelper.InitializeOmnitureDropDown(
                ltpList,
                lstCampaignItemLinkTracking,
                "Omniture6",
                ddlOmniture6,
                lblOmniture6,
                pnlOmniture,
                chkboxOmnitureTracking,
                hasCampainItemLinkTracking,
                ltpoListFunc,
                ltpsFunc);

            OmnitureHelper.InitializeOmnitureDropDown(
                ltpList,
                lstCampaignItemLinkTracking,
                "Omniture7",
                ddlOmniture7,
                lblOmniture7,
                pnlOmniture,
                chkboxOmnitureTracking,
                hasCampainItemLinkTracking,
                ltpoListFunc,
                ltpsFunc);

            OmnitureHelper.InitializeOmnitureDropDown(
                ltpList,
                lstCampaignItemLinkTracking,
                "Omniture8",
                ddlOmniture8,
                lblOmniture8,
                pnlOmniture,
                chkboxOmnitureTracking,
                hasCampainItemLinkTracking,
                ltpoListFunc,
                ltpsFunc);

            OmnitureHelper.InitializeOmnitureDropDown(
                ltpList,
                lstCampaignItemLinkTracking,
                "Omniture9",
                ddlOmniture9,
                lblOmniture9,
                pnlOmniture,
                chkboxOmnitureTracking,
                hasCampainItemLinkTracking,
                ltpoListFunc,
                ltpsFunc);

            OmnitureHelper.InitializeOmnitureDropDown(
                ltpList,
                lstCampaignItemLinkTracking,
                "Omniture10",
                ddlOmniture10,
                lblOmniture10,
                pnlOmniture,
                chkboxOmnitureTracking,
                hasCampainItemLinkTracking,
                ltpoListFunc,
                ltpsFunc);
        }

        private void LoadGoogleLinkTrackingParamOptions()
        {
            var linktrackingList =BusinessLinkTrackingParamOption.GetByLTPID(GoogleLinkTrackingId);

            var result = (from src in linktrackingList
                          where src.DisplayName != "Company"
                          select src).ToList();

            BindCampainControl(result, drpCampaignContent);
            BindCampainControl(result, drpCampaignMedium);
            BindCampainControl(result, drpCampaignName);
            BindCampainControl(result, drpCampaignTerm);
        }

        private static void BindCampainControl(IReadOnlyCollection<EntitiesLinkTrackingParamOption> result, DropDownList control)
        {
            control.DataSource = result;
            control.DataValueField = "LTPOID";
            control.DataTextField = "DisplayName";
            control.DataBind();
            control.Items.Insert(0, new ListItem("-Select-", "0"));
        }
    }
}
