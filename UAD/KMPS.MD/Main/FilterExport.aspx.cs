using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Communicator;
using FrameworkUAD.BusinessLogic;
using KM.Integration.Marketo.Process;
using KM.Integration.OAuth2;
using KMPS.MD.Helpers;
using KMPS.MD.Objects;
using static KMPlatform.Enums;
using static KMPS.MD.Objects.Enums;
using DownloadTemplate = KMPS.MD.Objects.DownloadTemplate;
using DownloadTemplateDetails = KMPS.MD.Objects.DownloadTemplateDetails;
using Enums = KMPS.MD.Objects.Enums;
using Filter = KMPS.MD.Objects.Filter;
using FilterSchedule = KMPS.MD.Objects.FilterSchedule;
using UserDataMask = KMPS.MD.Objects.UserDataMask;

namespace KMPS.MD.Main
{
    public partial class FilterExport : System.Web.UI.Page
    {
        private const string FieldCasePropercase = "PROPERCASE";
        private const string FieldCaseUppercase = "UPPERCASE";
        private const string FieldCaseLowercase = "LOWERCASE";
        private const string TextProperCase = "Proper Case";
        private const string TextUpperCase = "UPPER CASE";
        private const string TextLowerCase = "lower case";
        private const string TextDefault = "Default";
        private const char PipeSeparator = '|';
        private const string FileNameAlreadyExists = "File Name already exists";
        private const string PleaseSelectExportFields = "Please select Export Fields";
        private const string OnlyDemographicFieldsAreAllowedInAnExportToGroup = "Only 5 Demographic fields are allowed in an Export to group";
        private const string OnlyAdhocFieldsAreAllowedInAnExportToGroup = "Only 5 Adhoc fields are allowed in an Export to group";
        private const string EmailRequiredForMarketoExport = "Email required for Marketo export";
        private const string PleaseSelectMoreThanOneDataSegments = "Please select more than one Data Segments";
        private const string CanBeAddedEitherInInOrNotinList = " can be added either in In or NotIn list.";
        private const string PleaseSelectAFilterSegmentForThisScheduledFilterSegmentationExport = "Please select a filter segment for this scheduled filter segmentation export.";
        private const string PleaseEnterWhatDayOfEachMonthOrSelectLastDay = "Please enter What day of each month or select Last Day";
        private const string PleaseEnterEitherWhatDayOfEachMonthOrSelectLastDay = "Please enter either What day of each month or select Last Day";
        private const string PleaseSelectDays = "Please select Days";
        private const string ScheduleTypeRecurring = "Recurring";
        private const string FtpProtocol = "ftp://";
        private const string FtpsProtocol = "ftps://";
        private const string SftpProtocol = "sftp://";
        private const string ParameterValueDescription = "_Description";
        private const string HttpsProtocol = "https://";
        private const char CommaSeparator = ',';
        private const string Email = "Email";
        private const string GridViewHttpPostId = "gvHttpPost";
        private const string LabelParamValueId = "lblParamValue";

        Filters fc;
        delegate void HidePanel();
        delegate void LoadEditCaseData(Dictionary<string, string> downloadfields);

        private int FilterID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["FilterID"].ToString());
                }
                catch
                {
                    return 0;

                }
            }
        }

        private int FilterSegmentationID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["FilterSegmentationID"].ToString());
                }
                catch
                {
                    return 0;

                }
            }
        }

        private int FilterScheduleID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["FilterScheduleID"].ToString());
                }
                catch
                {
                    return 0;
                }
            }
        }
         
        private static string GetTextFromFieldCase(string fieldCase)
        {
            var fieldCaseText = string.Empty;

            if (string.IsNullOrEmpty(fieldCase))
            {
                return fieldCaseText;
            }

            switch (fieldCase.ToUpper())
            {
                case FieldCasePropercase:
                    fieldCaseText = TextProperCase;
                    break;
                case FieldCaseUppercase:
                    fieldCaseText = TextUpperCase;
                    break;
                case FieldCaseLowercase:
                    fieldCaseText = TextLowerCase;
                    break;
                default:
                    fieldCaseText = TextDefault;
                    break;
            }

            return fieldCaseText;
        }

 
        private static void AddFilterExportFieldToListControl(
            IDictionary<string, string> exportProfileFields,
            ListControl listBox,
            List<FilterExportField> filterExportFields,
            IDictionary<string, string> selectedfields)
        {
            foreach (var item in exportProfileFields)
            {
                if (!filterExportFields.Exists(
                        x => string.Equals(x.ExportColumn, item.Key.Split('|')[0], StringComparison.OrdinalIgnoreCase)))
                {
                    listBox.Items.Add(new ListItem(item.Value.ToUpper(), item.Key));
                }
                else
                {
                    selectedfields.Add(item.Key, item.Value);
                }
            }
        }

        private static ListItem GetDownloadTemplateListItem(KeyValuePair<string, string> field, string fieldCase, string text)
        {
            var item = new ListItem(
                GetListItemText(field, fieldCase, text),
                GetListItemValue(field, fieldCase));

            return item;
        }

        private static string GetListItemText(KeyValuePair<string, string> field, string fieldCase, string text)
        {
            var fieldCaseValue = string.Empty;

            if (string.Equals(
                field.Key.Split(PipeSeparator)[1],
                FieldType.Varchar.ToString(),
                StringComparison.OrdinalIgnoreCase))
            {
                var fieldCaseText = fieldCase == null
                                         ? FieldCase.Default.ToString()
                                         : text;
                fieldCaseValue = $"({fieldCaseText})";
            }
                        
            return $"{field.Value.ToUpper()}{fieldCaseValue}";
        }

        private static string GetListItemValue(KeyValuePair<string, string> field, string fieldCase)
        {
            return field.Key + PipeSeparator + (fieldCase 
                                                ?? (string.Equals(field.Key.Split(PipeSeparator)[1], FieldType.Varchar.ToString(), StringComparison.OrdinalIgnoreCase)
                                                        ? FieldCase.Default.ToString()
                                                        : FieldCase.None.ToString()));
        }

        private static void AddDownloadTemplateDetailsToListControl(
            IDictionary<string, string> exportProfileFields,
            ListControl listBox,
            List<DownloadTemplateDetails> downloadTemplateDetails,
            IDictionary<string, string> selectedfields)
        {
            foreach (var item in exportProfileFields)
            {
                if (downloadTemplateDetails.Exists(
                    templateDetails => templateDetails.ExportColumn.Equals(item.Key.Split(PipeSeparator)[0], StringComparison.OrdinalIgnoreCase)))
                {
                    selectedfields.Add(item.Key, item.Value);
                }
                else
                {
                    listBox.Items.Add(new ListItem(item.Value.ToUpper(), item.Key));
                }
            }
        }

        private void GroupsLookupPopupHide()
        {
            GroupsLookup.Visible = false;
        }

        private void DownloadCasePopupHide()
        {
            DownloadEditCase.Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Filters";
            Master.SubMenu = "Schedule Filter Export";
            lblErrorMessage.Text = string.Empty;
            divError.Visible = false;
            divErrMsg.Visible = false;
            lblErrMsg.Text = string.Empty;
            lblErrSelectedFields.Visible = false;

            fc = new Filters(Master.clientconnections, Master.LoggedInUser);

            HidePanel delGroupsLookup = new HidePanel(GroupsLookupPopupHide);
            this.GroupsLookup.hideGroupsLookupPopup = delGroupsLookup;

            HidePanel delDownloadCase = new HidePanel(DownloadCasePopupHide);
            this.DownloadEditCase.hideDownloadCasePopup = delDownloadCase;

            LoadEditCaseData delNoParamDownloadFields = new LoadEditCaseData(LoadEditCase);
            this.DownloadEditCase.LoadEditCaseData = delNoParamDownloadFields;

            if (!IsPostBack)
            {
                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, Services.UAD, ServiceFeatures.ScheduledExport, Access.Edit))
                {
                    Response.Redirect("../SecurityAccessError.aspx");
                }

                int fID = 0;

                if (FilterSegmentationID > 0)
                {
                    FrameworkUAD.Entity.FilterSegmentation fs = new FrameworkUAD.BusinessLogic.FilterSegmentation().SelectByID(FilterSegmentationID, Master.clientconnections);
                    fID = fs.FilterID;
                    lblpnlHeader.Text = "Filter Segmentation Export";
                    PlFiltersSelect.Visible = false;
                    plFilterSegmentationsSelect.Visible = true;
                    phFSName.Visible = true;
                    lblFSName.Text = fs.FilterSegmentationName;
                }
                else
                {
                    fID = FilterID;
                    lblpnlHeader.Text = "Filter Export";
                    PlFiltersSelect.Visible = true;
                }

                MDFilter mdf = MDFilter.GetByID(Master.clientconnections, fID);
                lblFilterName.Text = mdf.Name;
                hdFilterID.Value = mdf.FilterId.ToString();
                hfViewType.Value = mdf.ViewType.ToString();
                hfPubID.Value = mdf.PubID.ToString();
                hfBrandID.Value = mdf.BrandID.ToString() == "" ? "0" : mdf.BrandID.ToString();

                ddlExport.Items.Add(new ListItem("Export to FTP", "FTP"));

                List<UserDataMask> udm = UserDataMask.GetByUserID(Master.clientconnections, Master.UserSession.UserID);

                if (!udm.Exists(u => u.MaskField.ToUpper() == "EMAIL") || KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, Services.UAD, ServiceFeatures.UADExport, Access.ExportToGroup) && KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, Services.EMAILMARKETING, ServiceFeatures.Email, Access.ExternalImport))
                    {
                        ddlExport.Items.Add(new ListItem("Export to Email Marketing", "ECN"));
                    }

                    if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, Services.UAD, ServiceFeatures.Marketo, Access.FullAccess))
                    {
                        ddlExport.Items.Add(new ListItem("Export to Marketo", "Marketo"));
                        Marketo.PubID = mdf.PubID;
                        Marketo.BrandID = mdf.BrandID ?? 0;
                        Marketo.ViewType = mdf.ViewType;
                        Marketo.loadMarketoExportFields();
                    }
                }

                cmpRecStartDate.ValueToCompare = DateTime.Today.ToShortDateString();
                cmpStartDate.ValueToCompare = DateTime.Today.ToShortDateString();

                fc = MDFilter.LoadFilters(Master.clientconnections, fID, Master.LoggedInUser);

                if (fc.Count > 1)
                {
                    fc.Execute();
                    plFilters.Visible = true;
                    grdFilters.DataSource = fc;
                    grdFilters.DataBind();

                    foreach (Filter f in fc)
                    {
                        lstSelectedFilters.Items.Add(new ListItem(f.FilterGroupName.ToString(), f.FilterGroupID.ToString()));
                        lstSuppressedFilters.Items.Add(new ListItem(f.FilterGroupName.ToString(), f.FilterGroupID.ToString()));
                    }
                }

                if (FilterSegmentationID > 0)
                {
                    LoadFilterSegmenationData();
                }

                hdFilterScheduleID.Value = FilterScheduleID.ToString();

                ddlRecurrence.DataSource = RecurrenceType.GetAll(Master.clientconnections);
                ddlRecurrence.DataBind();

                drpDownloadTemplate.DataSource = DownloadTemplate.GetByPubIDBrandID(Master.clientconnections, mdf.PubID, mdf.BrandID ?? 0);
                drpDownloadTemplate.DataBind();
                drpDownloadTemplate.Items.Insert(0, new ListItem("Select Download Template", "0"));

                List<int> PubIDs = new List<int>();
                PubIDs.Add(mdf.PubID);

                if (FilterScheduleID > 0)
                    LoadFilterScheduleDetails(mdf, PubIDs);

                loadExportFields();
            }
        }

        public void LoadEditCase(Dictionary<string, string> downloadfields)
        {
            ControlsValidators.LoadEditCase(downloadfields, lstSelectedFields);
        }

        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadExportFields();

            if (ddlExport.SelectedValue == MD.Objects.Enums.ExportType.FTP.ToString())
            {
                plFTP.Visible = true;
                plECN.Visible = false;
                plMarketo.Visible = false;
                pnlExportFields.Visible = true;
            }
            else if (ddlExport.SelectedValue == MD.Objects.Enums.ExportType.ECN.ToString())
            {
                plFTP.Visible = false;
                plECN.Visible = true;
                plMarketo.Visible = false;
                pnlExportFields.Visible = true;
            }
            else
            {
                plFTP.Visible = false;
                plECN.Visible = false;
                plMarketo.Visible = true;
                pnlExportFields.Visible = false;
            }
        }

        private void LoadFilterScheduleDetails(MDFilter mdf, List<int> PubIDs)
        {
            FilterSchedule fs = FilterSchedule.GetByID(Master.clientconnections, FilterScheduleID);
            txtExportName.Text = fs.ExportName;
            txtExportNotes.Text = fs.ExportNotes;

            if (fs.ExportTypeID.ToString() == MD.Objects.Enums.ExportType.Marketo.ToString())
            {
                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, Services.UAD, ServiceFeatures.Marketo, Access.FullAccess))
                {
                    DisplayError("You cannot edit this filterSchedule because you do not have access to Marketo");
                    return;
                }
            }

            ddlExport.SelectedValue = fs.ExportTypeID.ToString();

            if (ddlExport.SelectedValue == MD.Objects.Enums.ExportType.FTP.ToString())
            {
                if (fs.Server.Contains("sftp://"))
                    ddlSiteType.SelectedValue = MD.Objects.Enums.SiteType.SFTP.ToString();
                else if (fs.Server.Contains("ftps://"))
                    ddlSiteType.SelectedValue = MD.Objects.Enums.SiteType.FTPS.ToString();
                else if (fs.Server.Contains("ftp://"))
                    ddlSiteType.SelectedValue = MD.Objects.Enums.SiteType.FTP.ToString();


                txtServer.Text = fs.Server;
                txtUserName.Text = fs.UserName;
                txtPassword.Text = fs.Password;
                txtFolder.Text = fs.Folder;
                hdFilterID.Value = fs.FilterID.ToString();
                plFTP.Visible = true;
                plECN.Visible = false;
                plMarketo.Visible = false;
                pnlExportFields.Visible = true;
            }
            else if (ddlExport.SelectedValue == MD.Objects.Enums.ExportType.ECN.ToString())
            {
                try
                {
                    if (fs.GroupID != null || fs.GroupID > 0)
                    {
                        hfGroupID.Value = fs.GroupID.ToString();
                        txtGroupName.Text = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID((int)fs.GroupID, Master.UserSession.CurrentUser).GroupName;
                    }
                }
                catch { }

                plECN.Visible = true;
                plFTP.Visible = false;
                plMarketo.Visible = false;
                pnlExportFields.Visible = true;
            }
            else
            {
                List<FilterScheduleIntegration> lfsi = FilterScheduleIntegration.getByFilterScheduleID(Master.clientconnections, FilterScheduleID);

                TextBox txtMarketoBaseURL = (TextBox)Marketo.FindControl("txtMarketoBaseURL");
                TextBox txtMarketoClientID = (TextBox)Marketo.FindControl("txtMarketoClientID");
                TextBox txtMarketoClientSecret = (TextBox)Marketo.FindControl("txtMarketoClientSecret");
                TextBox txtMarketoPartition = (TextBox)Marketo.FindControl("txtMarketoPartition");

                foreach (FilterScheduleIntegration fsi in lfsi)
                {
                    if (fsi.IntegrationParamName == MD.Objects.Enums.Marketo.BaseURL.ToString())
                        txtMarketoBaseURL.Text = fsi.IntegrationParamValue;
                    else if (fsi.IntegrationParamName == MD.Objects.Enums.Marketo.ClientID.ToString())
                        txtMarketoClientID.Text = fsi.IntegrationParamValue;
                    else if (fsi.IntegrationParamName == MD.Objects.Enums.Marketo.ClientSecret.ToString())
                        txtMarketoClientSecret.Text = fsi.IntegrationParamValue;
                    else if (fsi.IntegrationParamName == MD.Objects.Enums.Marketo.Partition.ToString())
                        txtMarketoPartition.Text = fsi.IntegrationParamValue;
                }

                try
                {
                    MarketoRestAPIProcess ra = new MarketoRestAPIProcess(txtMarketoBaseURL.Text, txtMarketoClientID.Text, txtMarketoClientSecret.Text);

                    var ids = new List<string>();
                    ids.Add(fs.GroupID.ToString());

                    Authentication auth = new Authentication(txtMarketoBaseURL.Text, txtMarketoClientID.Text, txtMarketoClientSecret.Text);

                    var token = auth.getToken();

                    DropDownList ddlMarketoList = (DropDownList)Marketo.FindControl("ddlMarketoList");
                    ddlMarketoList.DataSource = ra.GetMarketoLists(token, ids.ToArray(), null, 100, "")[0].result;
                    ddlMarketoList.DataBind();
                    ddlMarketoList.Items.Insert(0, new ListItem("", "0"));

                    ddlMarketoList.SelectedValue = fs.GroupID.ToString();
                }
                catch
                {
                }

                Dictionary<string, string> fields = Utilities.GetExportFields(Master.clientconnections, mdf.ViewType, mdf.BrandID ?? 0, PubIDs, KMPS.MD.Objects.Enums.ExportType.Marketo, Master.UserSession.UserID);

                List<FilterExportField> lfef = FilterExportField.getByFilterScheduleID(Master.clientconnections, FilterScheduleID);

                var query = (from a in lfef
                             join b in fields on a.ExportColumn equals b.Key into ab
                             from f in ab.DefaultIfEmpty()
                             select new
                             {
                                 HttpPostParamsID = Guid.NewGuid(),
                                 ParamName = a.MappingField,
                                 ParamValue = a.ExportColumn,
                                 CustomValue = a.CustomValue,
                                 ParamDisplayName = a.ExportColumn == "CustomValue" ? "CustomValue" : f.Value
                             }).ToList();

                GridView gvHttpPost = (GridView)Marketo.FindControl("gvHttpPost");
                gvHttpPost.DataSource = query;
                gvHttpPost.DataBind();

                plECN.Visible = false;
                plFTP.Visible = false;
                plMarketo.Visible = true;
                pnlExportFields.Visible = false;
            }

            rblSelectedFiltersOperation.SelectedValue = fs.SelectedOperation;
            rblSuppressedFiltersOperation.SelectedValue = fs.SuppressedOperation == null ? "Union" : fs.SuppressedOperation;

            if (fc.Count > 1)
            {
                if (fs.FilterGroupID_Selected != null & fs.FilterGroupID_Selected.Count > 0)
                {
                    foreach (ListItem item in lstSelectedFilters.Items)
                    {
                        item.Selected = fs.FilterGroupID_Selected.Where(c => c.Equals(Convert.ToInt32(item.Value))).FirstOrDefault() != 0;
                    }
                }

                if (fs.FilterGroupID_Suppressed != null & fs.FilterGroupID_Suppressed.Count > 0)
                {
                    foreach (ListItem item in lstSuppressedFilters.Items)
                    {
                        item.Selected = fs.FilterGroupID_Suppressed.Where(c => c.Equals(Convert.ToInt32(item.Value))).FirstOrDefault() != 0;
                    }
                }
            }
            else
            {
                PlFiltersSelect.Visible = false;
            }

            if (FilterSegmentationID > 0)
            {
                List<FrameworkUAD.Entity.FilterSegmentationGroup> lfsg = new FrameworkUAD.BusinessLogic.FilterSegmentationGroup().SelectByFilterSegmentationID(FilterSegmentationID, Master.clientconnections);

                if (lfsg.Exists(x => string.Join(",", x.FilterGroupID_Selected) == string.Join(",", fs.FilterGroupID_Selected) && string.Join(",", x.FilterGroupID_Suppressed) == string.Join(",", fs.FilterGroupID_Suppressed) && x.SelectedOperation == fs.SelectedOperation && x.SuppressedOperation == (fs.SuppressedOperation == null ? "" : fs.SuppressedOperation)))
                {
                    plExistingSegment.Visible = true;
                    PlFiltersSelect.Visible = false;
                    rbNewExisitngSegment.SelectedValue = "No";

                    foreach (GridViewRow r in grdFilterSegmentationCounts.Rows)
                    {
                        RadioButton rb = r.FindControl("rbFSSelect") as RadioButton;
                        HiddenField hfSelectedFilterNo = (HiddenField)r.FindControl("hfSelectedFilterNo");
                        HiddenField hfSuppressedFilterNo = (HiddenField)r.FindControl("hfSuppressedFilterNo");
                        HiddenField hfSelectedFilterOperation = (HiddenField)r.FindControl("hfSelectedFilterOperation");
                        HiddenField hfSuppressedFilterOperation = (HiddenField)r.FindControl("hfSuppressedFilterOperation");

                        string selectedfilterNos = string.Empty;
                        string suppressedfilterNos = string.Empty;

                        foreach (int s in fs.FilterGroupID_Selected)
                        {
                            string fNo = fc.FirstOrDefault(x => x.FilterGroupID == s).FilterNo.ToString();
                            selectedfilterNos += (selectedfilterNos == string.Empty ? "" : ",") + fNo;
                        }

                        foreach (int s in fs.FilterGroupID_Suppressed)
                        {
                            string fNo = fc.FirstOrDefault(x => x.FilterGroupID == s).FilterNo.ToString();
                            suppressedfilterNos += (suppressedfilterNos == string.Empty ? "" : ",") + fNo;
                        }

                        if (hfSelectedFilterNo.Value == selectedfilterNos & hfSuppressedFilterNo.Value == suppressedfilterNos & hfSelectedFilterOperation.Value == fs.SelectedOperation & hfSuppressedFilterOperation.Value == (fs.SuppressedOperation == null ? "" : fs.SuppressedOperation))
                        {
                            rb.Checked = true;
                        }
                    }
                }
                else
                {
                    if (fc.Count > 1)
                    {
                        rbNewExisitngSegment.SelectedValue = "Yes";
                        PlFiltersSelect.Visible = true;
                        plExistingSegment.Visible = false;
                    }
                }
            }

            if (fs.IsRecurring)
            {
                pnlRecurrence.Visible = true;
                pnlRecurring.Visible = true;
                pnlOneTime.Visible = false;
                ddlScheduleType.SelectedValue = "Recurring";
                ddlRecurrence.SelectedValue = fs.RecurrenceTypeID.ToString();
                txtRecurringStartDate.Text = fs.StartDate.ToShortDateString();
                ddlRecurringStartTime.SelectedValue = fs.StartTime;
                txtRecurringEndDate.Text = fs.EndDate == null ? "" : fs.EndDate.GetValueOrDefault().ToShortDateString();

                switch (ddlRecurrence.SelectedValue)
                {
                    case "2":
                        pnlDays.Visible = true;
                        break;
                    case "3":
                        pnlMonth.Visible = true;
                        break;
                    default:
                        break;
                }

                foreach (ListItem item in cbDays.Items)
                {
                    switch (item.Value)
                    {
                        case "Sunday":
                            item.Selected = fs.RunSunday;
                            break;
                        case "Monday":
                            item.Selected = fs.RunMonday;
                            break;
                        case "Tuesday":
                            item.Selected = fs.RunTuesday;
                            break;
                        case "Wednesday":
                            item.Selected = fs.RunWednesday;
                            break;
                        case "Thursday":
                            item.Selected = fs.RunThursday;
                            break;
                        case "Friday":
                            item.Selected = fs.RunFriday;
                            break;
                        case "Saturday":
                            item.Selected = fs.RunSaturday;
                            break;
                    }
                }

                cbDays.SelectedValue = fs.RunSunday.ToString();
                cbDays.SelectedValue = fs.RunMonday.ToString();
                cbDays.SelectedValue = fs.RunTuesday.ToString();
                cbDays.SelectedValue = fs.RunWednesday.ToString();
                cbDays.SelectedValue = fs.RunThursday.ToString();
                cbDays.SelectedValue = fs.RunFriday.ToString();
                cbDays.SelectedValue = fs.RunSaturday.ToString();

            }
            else
            {
                pnlOneTime.Visible = true;
                ddlScheduleType.SelectedValue = "One-Time";
                txtStartDate.Text = fs.StartDate.ToShortDateString();
                ddlStartTime.SelectedValue = fs.StartTime;
            }

            txtMonth.Text = fs.MonthScheduleDay.ToString();
            cbLastDay.Checked = fs.MonthLastDay;
            txtEmailAddress.Text = fs.EmailNotification;
            ddlExportFormat.SelectedValue = fs.ExportFormat;
            cbShowHeader.Checked = fs.ShowHeader;
            rblFileNameFormat.SelectedValue = fs.FileNameFormat;
            txtFileName.Text = fs.FileName;
        }

        private void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        protected void rbECNExport_CheckedChanged(object sender, EventArgs e)
        {
            plECN.Visible = true;
            plFTP.Visible = false;
        }

        protected void rbFTPExport_CheckedChanged(object sender, EventArgs e)
        {
            plFTP.Visible = true;
            plECN.Visible = false;
        }

        protected void ImgGroupList_Click(object sender, EventArgs e)
        {
            GroupsLookup.LoadControl();
            GroupsLookup.Visible = true;
        }

        protected void drpDownloadTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadExportFields();
        }

        protected void rbNewExisitngSegment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbNewExisitngSegment.SelectedValue.ToUpper() == "YES")
            {
                plExistingSegment.Visible = false;
                PlFiltersSelect.Visible = true;
            }
            else
            {
                plExistingSegment.Visible = true;
                PlFiltersSelect.Visible = false;
            }
        }

        protected void rblFieldsType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblFieldsType.SelectedValue == KMPS.MD.Objects.Enums.ExportFieldType.Profile.ToString())
            {
                phProfileFields.Visible = true;
                phDemoFields.Visible = false;
                phAdhocFields.Visible = false;
            }
            else if (rblFieldsType.SelectedValue == KMPS.MD.Objects.Enums.ExportFieldType.Demo.ToString())
            {
                phProfileFields.Visible = false;
                phDemoFields.Visible = true;
                phAdhocFields.Visible = false;
            }
            else
            {
                phProfileFields.Visible = false;
                phDemoFields.Visible = false;
                phAdhocFields.Visible = true;
            }
        }

        protected void btnAdd_Click(Object sender, EventArgs e)
        {
            lblErrSelectedFields.Visible = false;
            lblErrSelectedFields.Text = string.Empty;

            if (phProfileFields.Visible == true)
            {
                for (int i = 0; i < lstAvailableProfileFields.Items.Count; i++)
                {
                    if (lstAvailableProfileFields.Items[i].Selected)
                    {
                        lstSelectedFields.Items.Add(new ListItem(lstAvailableProfileFields.Items[i].Value.Split('|')[1] == "varchar" ? lstAvailableProfileFields.Items[i].Text.ToUpper() + "(Default)" : lstAvailableProfileFields.Items[i].Text.ToUpper(), lstAvailableProfileFields.Items[i].Value.Split('|')[1] == "varchar" ? lstAvailableProfileFields.Items[i].Value + "|Default" : lstAvailableProfileFields.Items[i].Value + "|None"));
                        lstAvailableProfileFields.Items.RemoveAt(i);
                        i--;
                    }
                }
            }
            else if (phDemoFields.Visible == true)
            {
                for (int i = 0; i < lstAvailableDemoFields.Items.Count; i++)
                {
                    if (lstAvailableDemoFields.Items[i].Selected)
                    {
                        lstSelectedFields.Items.Add(new ListItem(lstAvailableDemoFields.Items[i].Value.Split('|')[1] == "varchar" ? lstAvailableDemoFields.Items[i].Text.ToUpper() + "(Default)" : lstAvailableDemoFields.Items[i].Text.ToUpper(), lstAvailableDemoFields.Items[i].Value.Split('|')[1] == "varchar" ? lstAvailableDemoFields.Items[i].Value + "|Default" : lstAvailableDemoFields.Items[i].Value + "|None"));
                        lstAvailableDemoFields.Items.RemoveAt(i);
                        i--;
                    }
                }
            }
            else
            {
                for (int i = 0; i < lstAvailableAdhocFields.Items.Count; i++)
                {
                    if (lstAvailableAdhocFields.Items[i].Selected)
                    {
                        lstSelectedFields.Items.Add(new ListItem(lstAvailableAdhocFields.Items[i].Value.Split('|')[1] == "varchar" ? lstAvailableAdhocFields.Items[i].Text.ToUpper() + "(Default)" : lstAvailableAdhocFields.Items[i].Text.ToUpper(), lstAvailableAdhocFields.Items[i].Value.Split('|')[1] == "varchar" ? lstAvailableAdhocFields.Items[i].Value + "|Default" : lstAvailableAdhocFields.Items[i].Value + "|None"));
                        lstAvailableAdhocFields.Items.RemoveAt(i);
                        i--;
                    }
                }
            }

           if (ddlExport.SelectedValue == MD.Objects.Enums.ExportType.ECN.ToString())
            {
                List<int> PubIDs = new List<int>();
                PubIDs.Add(Convert.ToInt32(hfPubID.Value));

                Dictionary<string, string> exportDemoFields = Utilities.GetExportFields(Master.clientconnections, (KMPS.MD.Objects.Enums.ViewType)Enum.Parse(typeof(KMPS.MD.Objects.Enums.ViewType), hfViewType.Value), (int?)(Convert.ToInt32(hfBrandID.Value)) ?? 0, PubIDs, (KMPS.MD.Objects.Enums.ExportType)Enum.Parse(typeof(KMPS.MD.Objects.Enums.ExportType), ddlExport.SelectedValue), Master.UserSession.UserID, KMPS.MD.Objects.Enums.ExportFieldType.Demo);
                Dictionary<string, string> exportAdhocFields = Utilities.GetExportFields(Master.clientconnections, (KMPS.MD.Objects.Enums.ViewType)Enum.Parse(typeof(KMPS.MD.Objects.Enums.ViewType), hfViewType.Value), (int?)(Convert.ToInt32(hfBrandID.Value)) ?? 0, PubIDs, (KMPS.MD.Objects.Enums.ExportType)Enum.Parse(typeof(KMPS.MD.Objects.Enums.ExportType), ddlExport.SelectedValue), Master.UserSession.UserID, KMPS.MD.Objects.Enums.ExportFieldType.Adhoc);

                int demoFieldsCount = 0;
                int AdhocFieldsCount = 0;

                for (int i = 0; i < lstSelectedFields.Items.Count; i++)
                {
                    if (exportDemoFields.ContainsKey(lstSelectedFields.Items[i].Value.ToString().Split('|')[0]))
                    {
                        demoFieldsCount = demoFieldsCount + 1;
                    }
                    else if (exportAdhocFields.ContainsKey(lstSelectedFields.Items[i].Value.ToString().Split('|')[0]))
                    {
                        AdhocFieldsCount = AdhocFieldsCount + 1;
                    }
                }

                string strError = string.Empty;

                if (demoFieldsCount > 5)
                {
                    strError = OnlyDemographicFieldsAreAllowedInAnExportToGroup;
                }

                if (AdhocFieldsCount > 5)
                {
                    if (strError != string.Empty)
                        strError += " and ";
                    strError += OnlyAdhocFieldsAreAllowedInAnExportToGroup;
                }

                if (strError != string.Empty)
                {
                    lblErrSelectedFields.Text = strError;
                    lblErrSelectedFields.Visible = true;
                }
            }
        }

        protected void btnRemove_Click(Object sender, EventArgs e)
        {
            lblErrSelectedFields.Visible = false;

            Dictionary<string, string> exportDemoFields = new Dictionary<string, string>();
            Dictionary<string, string> exportAdhocFields = new Dictionary<string, string>();

            List<int> PubIDs = new List<int>();
            PubIDs.Add(Convert.ToInt32(hfPubID.Value));

            exportDemoFields = Utilities.GetExportFields(Master.clientconnections, (KMPS.MD.Objects.Enums.ViewType)Enum.Parse(typeof(KMPS.MD.Objects.Enums.ViewType), hfViewType.Value), (int?)(Convert.ToInt32(hfBrandID.Value)) ?? 0, PubIDs, (KMPS.MD.Objects.Enums.ExportType)Enum.Parse(typeof(KMPS.MD.Objects.Enums.ExportType), ddlExport.SelectedValue), Master.UserSession.UserID, KMPS.MD.Objects.Enums.ExportFieldType.Demo);
            exportAdhocFields = Utilities.GetExportFields(Master.clientconnections, (KMPS.MD.Objects.Enums.ViewType)Enum.Parse(typeof(KMPS.MD.Objects.Enums.ViewType), hfViewType.Value), (int?)(Convert.ToInt32(hfBrandID.Value)) ?? 0, PubIDs, (KMPS.MD.Objects.Enums.ExportType)Enum.Parse(typeof(KMPS.MD.Objects.Enums.ExportType), ddlExport.SelectedValue), Master.UserSession.UserID, KMPS.MD.Objects.Enums.ExportFieldType.Adhoc);

            for (int i = 0; i < lstSelectedFields.Items.Count; i++)
            {
                if (lstSelectedFields.Items[i].Selected)
                {
                    if (exportDemoFields.ContainsKey(lstSelectedFields.Items[i].Value.ToString()))
                    {
                        lstAvailableDemoFields.Items.Add(new ListItem(lstSelectedFields.Items[i].Text.Split('(')[0].ToUpper(), lstSelectedFields.Items[i].Value.Split('|')[0] + "|" + lstSelectedFields.Items[i].Value.Split('|')[1]));
                        lstSelectedFields.Items.RemoveAt(i);
                    }
                    else if (exportAdhocFields.ContainsKey(lstSelectedFields.Items[i].Value.ToString()))
                    {
                        lstAvailableAdhocFields.Items.Add(new ListItem(lstSelectedFields.Items[i].Text.Split('(')[0].ToUpper(), lstSelectedFields.Items[i].Value.Split('|')[0] + "|" + lstSelectedFields.Items[i].Value.Split('|')[1]));
                        lstSelectedFields.Items.RemoveAt(i);
                    }
                    else
                    {
                        lstAvailableProfileFields.Items.Add(new ListItem(lstSelectedFields.Items[i].Text.Split('(')[0].ToUpper(), lstSelectedFields.Items[i].Value.Split('|')[0] + "|" + lstSelectedFields.Items[i].Value.Split('|')[1]));
                        lstSelectedFields.Items.RemoveAt(i);
                    }

                    i--;
                }
            }

            if (ddlExport.SelectedValue == MD.Objects.Enums.ExportType.ECN.ToString())
            {
                int demoFieldsCount = 0;
                int AdhocFieldsCount = 0;

                for (int i = 0; i < lstSelectedFields.Items.Count; i++)
                {
                    if (exportDemoFields.ContainsKey(lstSelectedFields.Items[i].Value.ToString().Split('|')[0]))
                    {
                        demoFieldsCount = demoFieldsCount + 1;
                    }
                    else if (exportAdhocFields.ContainsKey(lstSelectedFields.Items[i].Value.ToString().Split('|')[0]))
                    {
                        AdhocFieldsCount = AdhocFieldsCount + 1;
                    }
                }

                string strError = string.Empty;

                if (demoFieldsCount > 5)
                {
                    strError = OnlyDemographicFieldsAreAllowedInAnExportToGroup;
                }

                if (AdhocFieldsCount > 5)
                {
                    if (strError != string.Empty)
                        strError += " and ";
                    strError += OnlyAdhocFieldsAreAllowedInAnExportToGroup;
                }

                if (strError != string.Empty)
                {
                    lblErrSelectedFields.Text = strError;
                    lblErrSelectedFields.Visible = true;
                }
            }
        }

        protected void btnUp_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstSelectedFields.Items.Count; i++)
            {
                if (lstSelectedFields.Items[i].Selected)
                {
                    if (i > 0 && !lstSelectedFields.Items[i - 1].Selected)
                    {
                        ListItem bottom = lstSelectedFields.Items[i];
                        lstSelectedFields.Items.Remove(bottom);
                        lstSelectedFields.Items.Insert(i - 1, bottom);
                        lstSelectedFields.Items[i - 1].Selected = true;
                    }
                }
            }
        }

        protected void btndown_Click(object sender, EventArgs e)
        {
            int startindex = lstSelectedFields.Items.Count - 1;

            for (int i = startindex; i > -1; i--)
            {
                if (lstSelectedFields.Items[i].Selected)
                {
                    if (i < startindex && !lstSelectedFields.Items[i + 1].Selected)
                    {
                        ListItem bottom = lstSelectedFields.Items[i];
                        lstSelectedFields.Items.Remove(bottom);
                        lstSelectedFields.Items.Insert(i + 1, bottom);
                        lstSelectedFields.Items[i + 1].Selected = true;
                    }
                }
            }
        }

        protected void ddlRecurrence_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlRecurrence.SelectedValue)
            {
                case "1":
                    pnlDays.Visible = false;
                    pnlMonth.Visible = false;
                    cbDays.ClearSelection();
                    txtMonth.Text = "";
                    cbLastDay.Checked = false;
                    break;
                case "2":
                    pnlDays.Visible = true;
                    pnlMonth.Visible = false;
                    txtMonth.Text = "";
                    cbLastDay.Checked = false;
                    break;
                case "3":
                    pnlDays.Visible = false;
                    pnlMonth.Visible = true;
                    cbDays.ClearSelection();
                    break;
                default:
                    break;
            }
        }

        protected void ddlScheduleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlScheduleType.SelectedValue)
            {
                case "One-Time":
                    pnlOneTime.Visible = true;
                    pnlRecurrence.Visible = false;
                    pnlRecurring.Visible = false;
                    pnlMonth.Visible = false;
                    pnlDays.Visible = false;
                    resetRecurringControls();
                    break;
                case "Recurring":
                    pnlOneTime.Visible = false;
                    pnlRecurrence.Visible = true;
                    pnlRecurring.Visible = true;
                    resetOneTimeControls();
                    break;
                default:
                    break;
            }
        }
       
        private void resetOneTimeControls()
        {
            txtStartDate.Text = "";
            ddlStartTime.ClearSelection();
        }

        private void resetRecurringControls()
        {
            ddlRecurrence.ClearSelection();
            txtRecurringStartDate.Text = "";
            ddlRecurringStartTime.ClearSelection();
            txtRecurringEndDate.Text = "";
            cbDays.ClearSelection();
            txtMonth.Text = "";
            cbLastDay.Checked = false;
        }

        private void loadExportFields()
        {
            lstAvailableProfileFields.Items.Clear();
            lstAvailableDemoFields.Items.Clear();
            lstAvailableAdhocFields.Items.Clear();
            lstSelectedFields.Items.Clear();

            try
            {
                ExportFields();

                if (ddlExport.SelectedValue == MD.Objects.Enums.ExportType.ECN.ToString())
                {
                    List<int> PubIDs = new List<int>();
                    PubIDs.Add(Convert.ToInt32(hfPubID.Value));

                    Dictionary<string, string> exportDemoFields = Utilities.GetExportFields(Master.clientconnections, (KMPS.MD.Objects.Enums.ViewType)Enum.Parse(typeof(KMPS.MD.Objects.Enums.ViewType), hfViewType.Value), (int?)(Convert.ToInt32(hfBrandID.Value)) ?? 0, PubIDs, (KMPS.MD.Objects.Enums.ExportType)Enum.Parse(typeof(KMPS.MD.Objects.Enums.ExportType), ddlExport.SelectedValue), Master.UserSession.UserID, KMPS.MD.Objects.Enums.ExportFieldType.Demo);
                    Dictionary<string, string> exportAdhocFields = Utilities.GetExportFields(Master.clientconnections, (KMPS.MD.Objects.Enums.ViewType)Enum.Parse(typeof(KMPS.MD.Objects.Enums.ViewType), hfViewType.Value), (int?)(Convert.ToInt32(hfBrandID.Value)) ?? 0, PubIDs, (KMPS.MD.Objects.Enums.ExportType)Enum.Parse(typeof(KMPS.MD.Objects.Enums.ExportType), ddlExport.SelectedValue), Master.UserSession.UserID, KMPS.MD.Objects.Enums.ExportFieldType.Adhoc);

                    int demoFieldsCount = 0;
                    int AdhocFieldsCount = 0;

                    for (int i = 0; i < lstSelectedFields.Items.Count; i++)
                    {
                        if (exportDemoFields.ContainsKey(lstSelectedFields.Items[i].Value.ToString()))
                        {
                            demoFieldsCount = demoFieldsCount + 1;
                        }
                        else if (exportAdhocFields.ContainsKey(lstSelectedFields.Items[i].Value.ToString()))
                        {
                            AdhocFieldsCount = AdhocFieldsCount + 1;
                        }
                    }

                    string strError = string.Empty;

                    if (demoFieldsCount > 5)
                    {
                        strError = OnlyDemographicFieldsAreAllowedInAnExportToGroup;
                    }

                    if (AdhocFieldsCount > 5)
                    {
                        if (strError != string.Empty)
                            strError += " and ";
                        strError += OnlyAdhocFieldsAreAllowedInAnExportToGroup;
                    }

                    if (strError != string.Empty)
                    {
                        lblErrSelectedFields.Text = strError;
                        lblErrSelectedFields.Visible = true;
                    }
                }

            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        private IDictionary<string, string> GetExportFieldsFromUtilities(IList<int> productIds, ExportFieldType exportFieldType)
        {
            var viewType = (ViewType)Enum.Parse(typeof(ViewType), hfViewType.Value);
            var brandId = Convert.ToInt32(hfBrandID.Value);
            var exportType = (Enums.ExportType)Enum.Parse(typeof(Enums.ExportType), ddlExport.SelectedValue);

            return Utilities.GetExportFields(
                Master.clientconnections,
                viewType,
                brandId,
                productIds,
                exportType,
                Master.UserSession.UserID,
                exportFieldType,
                true);
        }

        private void ExportFields()
        {
            var pubIds = new List<int> { Convert.ToInt32(hfPubID.Value) };

            var exportProfileFields = GetExportFieldsFromUtilities(pubIds, ExportFieldType.Profile);
            var exportDemoFields = GetExportFieldsFromUtilities(pubIds, ExportFieldType.Demo);
            var exportAdhocFields = GetExportFieldsFromUtilities(pubIds, ExportFieldType.Adhoc); 

            var exportfields = exportProfileFields.ToDictionary(e => e.Key, e => e.Value);

            foreach (var fieldPair in exportDemoFields)
            {
                exportfields.Add(fieldPair.Key, fieldPair.Value);
            }

            foreach (var fieldPair in exportAdhocFields)
            {
                exportfields.Add(fieldPair.Key, fieldPair.Value);
            }

            var selectedfields = new Dictionary<string, string>();
            if (FilterScheduleID > 0 && Convert.ToInt32(drpDownloadTemplate.SelectedValue) <= 0)
            {
                var filterExportFields = FilterExportField.getByFilterScheduleID(Master.clientconnections, FilterScheduleID);

                AddFilterExportFieldToListControl(exportProfileFields, lstAvailableProfileFields, filterExportFields, selectedfields);
                AddFilterExportFieldToListControl(exportDemoFields, lstAvailableDemoFields, filterExportFields, selectedfields);
                AddFilterExportFieldToListControl(exportAdhocFields, lstAvailableAdhocFields, filterExportFields, selectedfields);
                AddFilterExportFields(filterExportFields, exportfields, selectedfields);
            }
            else
            {
                var downloadTemplate = DownloadTemplateDetails.GetByDownloadTemplateID(Master.clientconnections, Convert.ToInt32(drpDownloadTemplate.SelectedValue));

                AddDownloadTemplateDetailsToListControl(exportProfileFields, lstAvailableProfileFields, downloadTemplate, selectedfields);
                AddDownloadTemplateDetailsToListControl(exportDemoFields, lstAvailableDemoFields, downloadTemplate, selectedfields);
                AddDownloadTemplateDetailsToListControl(exportAdhocFields, lstAvailableAdhocFields, downloadTemplate, selectedfields);
                AddTemplateDetailFields(downloadTemplate, exportfields, selectedfields);
            }
        }

        private void AddTemplateDetailFields(IEnumerable<DownloadTemplateDetails> downloadTemplate, Dictionary<string, string> exportfields, Dictionary<string, string> selectedfields)
        {
            foreach (var templateDetails in downloadTemplate)
            {
                var text = GetTextFromFieldCase(templateDetails.FieldCase);

                if (exportfields.Any(x => string.Equals(x.Key.Split(PipeSeparator)[0], templateDetails.ExportColumn, StringComparison.OrdinalIgnoreCase)))
                {
                    var field = selectedfields.FirstOrDefault(x => string.Equals(x.Key.Split(PipeSeparator)[0], templateDetails.ExportColumn, StringComparison.OrdinalIgnoreCase));
                    lstSelectedFields.Items.Add(GetDownloadTemplateListItem(field, templateDetails.FieldCase, text));
                }
            }
        }

        private void AddFilterExportFields(List<FilterExportField> filterExportFields, Dictionary<string, string> exportfields, Dictionary<string, string> selectedfields)
        {
            foreach (var exportField in filterExportFields)
            {
                var text = GetTextFromFieldCase(exportField.FieldCase);

                if (exportfields.Any(x => string.Equals(x.Key.Split(PipeSeparator)[0], exportField.ExportColumn, StringComparison.OrdinalIgnoreCase)))
                {
                    var field = selectedfields.FirstOrDefault(x => string.Equals(x.Key.Split(PipeSeparator)[0], exportField.ExportColumn, StringComparison.OrdinalIgnoreCase));
                    lstSelectedFields.Items.Add(GetDownloadTemplateListItem(field, exportField.FieldCase, text));
                }
            }
        }

        //protected void btnAddHttpPostURL_Click(object sender, EventArgs e)
        //{
        //    DataTable dt = ExtURLQS;
        //    if (dt == null)
        //    {
        //        dt = new DataTable();
        //        DataColumn HttpPostParamsID = new DataColumn("HttpPostParamsID", typeof(string));
        //        dt.Columns.Add(HttpPostParamsID);

        //        DataColumn ParamName = new DataColumn("ParamName", typeof(string));
        //        dt.Columns.Add(ParamName);

        //        DataColumn ParamValue = new DataColumn("ParamValue", typeof(string));
        //        dt.Columns.Add(ParamValue);

        //        DataColumn CustomValue = new DataColumn("CustomValue", typeof(string));
        //        dt.Columns.Add(CustomValue);

        //        DataColumn ParamDisplayName = new DataColumn("ParamDisplayName", typeof(string));
        //        dt.Columns.Add(ParamDisplayName);
        //    }
        //    else
        //    {
        //        foreach (DataRow r in dt.Rows)
        //        {
        //            if (string.Equals(r["ParamName"].ToString(), txtQSName.Text, StringComparison.OrdinalIgnoreCase))
        //            {
        //                txtErrMsg.Visible = true;
        //                txtErrMsg.Text = "Name entered has been already mapped. Please enter different Name.";
        //                return;
        //            }
        //        }
        //    }

        //    DataRow dr = dt.NewRow();
        //    dr["HttpPostParamsID"] = Guid.NewGuid();
        //    dr["ParamName"] = txtQSName.Text;
        //    dr["ParamValue"] = ddlQSValue.SelectedValue;
        //    dr["CustomValue"] = txtQSValue.Text;
        //    dr["ParamDisplayName"] = ddlQSValue.SelectedItem.Text;
        //    dt.Rows.Add(dr);
        //    txtQSName.Text = "";
        //    txtQSValue.Text = "";
        //    ExtURLQS = dt;
        //    gvHttpPost.DataSource = dt;
        //    gvHttpPost.DataBind();
        //}

        //protected void gvHttpPost_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    try
        //    {
        //        string HttpPostParamsID = e.CommandArgument.ToString();
        //        if (e.CommandName == "ParamDelete")
        //        {
        //            DataTable dt = ExtURLQS;
        //            var result = (from src in dt.AsEnumerable()
        //                          where src.Field<string>("HttpPostParamsID") == HttpPostParamsID
        //                          select src).ToList();
        //            dt.Rows.Remove(result[0]);
        //            ExtURLQS = dt;
        //            gvHttpPost.DataSource = ExtURLQS;
        //            gvHttpPost.DataBind();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = ex.Message;
        //    }
        //}

        //protected void btnTestConnection_Click(Object sender, EventArgs e)
        //{
        //    if (TestMarketoConnection(lblTestConnErrorMessage))
        //    {
        //        lblTestConnErrorMessage.Text = "Marketo Connection successful.";
        //    }
        //}

        //private bool TestMarketoConnection(Label lblErrorMsg)
        //{
        //    bool ValidConnection = false;
        //    lblErrorMsg.Visible = true;
        //    if (txtMarketoBaseURL.Text == string.Empty || txtMarketoClientID.Text == string.Empty || txtMarketoClientSecret.Text == string.Empty)
        //        lblErrorMsg.Text = "Please enter Marketo information for testing connection.";
        //    else
        //    {
        //        try
        //        {
        //            KM.Integration.OAuth2.Authentication auth = new KM.Integration.OAuth2.Authentication(txtMarketoBaseURL.Text, txtMarketoClientID.Text, txtMarketoClientSecret.Text);
        //            Token token = auth.getToken();

        //            if (token == null || token.expires_in == 0)
        //                lblErrorMsg.Text = "Marketo Connection failed. Please verify the credentials.";
        //            else
        //            {

        //                ValidConnection = true;
        //            }

        //        }
        //        catch
        //        {
        //            lblErrorMsg.Text = "Marketo Connection failed. Please verify the credentials.";
        //        }
        //    }

        //    return ValidConnection;

        //}

        //protected void btnSearchMarketoList_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (TestMarketoConnection(lblTestErrMsg))
        //        {
        //            MarketoRestAPIProcess ra = new MarketoRestAPIProcess(txtMarketoBaseURL.Text, txtMarketoClientID.Text, txtMarketoClientSecret.Text);

        //            Authentication auth = new Authentication(txtMarketoBaseURL.Text, txtMarketoClientID.Text, txtMarketoClientSecret.Text);

        //            var token = auth.getToken();


        //            ddlMarketoList.DataSource = ra.GetMarketoLists(token, null, txtMarketoNames.Text.Split(','), 100, "")[0].result;
        //            ddlMarketoList.DataBind();
        //            ddlMarketoList.Items.Insert(0, new ListItem("", "0"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //     }
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!KM.Platform.User.HasAccess(
                    Master.UserSession.CurrentUser,
                    Services.UAD,
                    ServiceFeatures.ScheduledExport,
                    Access.Edit))
            {
                return;
            }

            try
            {
                if (FilterSchedule.ExistsByFileName(Master.clientconnections, Convert.ToInt32(hdFilterScheduleID.Value), txtFileName.Text))
                {
                    divErrMsg.Visible = true;
                    lblErrMsg.Text = FileNameAlreadyExists;
                    return;
                }

                if (ddlExport.SelectedValue == Enums.ExportType.FTP.ToString() 
                    || ddlExport.SelectedValue == Enums.ExportType.ECN.ToString())
                {
                    if (lstSelectedFields.Items.Count == 0)
                    {
                        divErrMsg.Visible = true;
                        lblErrMsg.Text = PleaseSelectExportFields;
                        return;
                    }

                    if (ddlExport.SelectedValue == Enums.ExportType.ECN.ToString())
                    {
                        if (!CanExportToEcn())
                        {
                            return;
                        }
                    }
                }
                else
                {
                    if (EmailExists())
                    {
                        return;
                    }
                }

                if (PlFiltersSelect.Visible)
                {
                    if (DataSegmentsInvalid())
                    {
                        return;
                    }
                }
                else
                {
                    if (FilterSegmentationID > 0)
                    {
                        var ischecked = false;
                        foreach (GridViewRow gridViewRow in grdFilterSegmentationCounts.Rows)
                        {
                            var radioButton = gridViewRow.FindControl("rbFSSelect") as RadioButton;

                            if (radioButton != null && radioButton.Checked)
                            {
                                ischecked = true;
                            }
                        }

                        if (!ischecked)
                        {
                            divErrMsg.Visible = true;
                            lblErrMsg.Text = PleaseSelectAFilterSegmentForThisScheduledFilterSegmentationExport;
                            return;
                        }
                    }
                }

                if (ProcessReccurrence())
                {
                    return;
                }

                ProcessFilterSchedules();

                Response.Redirect($"FilterSchedules.aspx?{(FilterSegmentationID > 0 ? "Mode=FilterSegmentation" : "Mode=Filters")}");
            }
            catch (Exception exception)
            {
                DisplayError(exception.Message);
            }
        }

        private void ProcessFilterSchedules()
        {
            var filterSchedule = SetFilterSchedule();

            if (PlFiltersSelect.Visible)
            {
                SetSuppressedFilter(filterSchedule);
            }
            else
            {
                if (FilterSegmentationID > 0)
                {
                    ProcessFilterSegmentation(filterSchedule);
                }
            }

            var filterScheduleId = FilterSchedule.Save(Master.clientconnections, filterSchedule);

            if (filterScheduleId > 0)
            {
                FilterExportField.Delete(Master.clientconnections, Convert.ToInt32(hdFilterScheduleID.Value));
            }

            if (ddlExport.SelectedValue == Enums.ExportType.Marketo.ToString())
            {
                ExportMarketo(filterScheduleId);
            }
            else
            {
                if (lstSelectedFields.Items.Count > 0)
                {
                    SaveFilterExportFields(filterScheduleId);
                }
            }
        }

        private bool DataSegmentsInvalid()
        {
            if (lstSuppressedFilters.GetSelectedIndices().Length < 1 
                && lstSelectedFilters.GetSelectedIndices().Length < 2)
            {
                divErrMsg.Visible = true;
                lblErrMsg.Text = PleaseSelectMoreThanOneDataSegments;
                return true;
            }

            var dataSegments = new StringBuilder();
            var selectedFilters = lstSelectedFilters.GetSelectedIndices()
                .Select(index => lstSelectedFilters.Items[index].Text)
                .ToList();

            var suppressedFilters = lstSuppressedFilters.GetSelectedIndices()
                .Select(index => lstSuppressedFilters.Items[index].Text)
                .ToList();

            var difference = selectedFilters.Intersect(suppressedFilters);
            foreach (var item in difference)
            {
                dataSegments.Append(dataSegments.Length == 0 
                                        ? item 
                                        : ", " + item);
            }

            var suppressedSelected = rblSelectedFiltersOperation.SelectedValue == rblSuppressedFiltersOperation.SelectedValue
                                     && selectedFilters.SequenceEqual(suppressedFilters);

            if (dataSegments.Length > 0 && ((selectedFilters.Count == 1 && suppressedFilters.Count == 1) 
                                            || suppressedSelected))
            {
                divErrMsg.Visible = true;
                lblErrMsg.Text = dataSegments + CanBeAddedEitherInInOrNotinList;
                return true;
            }

            return false;
        }

        private bool EmailExists()
        {
            var isEmailExists = false;

            var gridViewHttpPost = (GridView)Marketo.FindControl(GridViewHttpPostId);

            if (gridViewHttpPost.Rows.Count > 0)
            {
                for (var i = 0; i < gridViewHttpPost.Rows.Count; i++)
                {
                    var lblParamValue = (Label)gridViewHttpPost.Rows[i].FindControl(LabelParamValueId);

                    if (string.Equals(Email, lblParamValue.Text.Split(PipeSeparator)[0], StringComparison.OrdinalIgnoreCase))
                    {
                        isEmailExists = true;
                    }
                }
            }

            if (!isEmailExists)
            {
                divErrMsg.Visible = true;
                lblErrMsg.Text = EmailRequiredForMarketoExport;
                return true;
            }

            return false;
        }

        private bool ProcessReccurrence()
        {
            if (Convert.ToInt32(ddlRecurrence.SelectedValue) == 3)
            {
                if (string.IsNullOrWhiteSpace(txtMonth.Text) && cbLastDay.Checked == false)
                {
                    divErrMsg.Visible = true;
                    lblErrMsg.Text = PleaseEnterWhatDayOfEachMonthOrSelectLastDay;
                    return true;
                }

                if (!string.IsNullOrWhiteSpace(txtMonth.Text) && cbLastDay.Checked)
                {
                    divErrMsg.Visible = true;
                    lblErrMsg.Text = PleaseEnterEitherWhatDayOfEachMonthOrSelectLastDay;
                    return true;
                }
            }
            else if (Convert.ToInt32(ddlRecurrence.SelectedValue) == 2)
            {
                if (cbDays.SelectedIndex == -1)
                {
                    divErrMsg.Visible = true;
                    lblErrMsg.Text = PleaseSelectDays;
                    return true;
                }
            }

            return false;
        }

        private void SetSuppressedFilter(FilterSchedule filterSchedule)
        {
            filterSchedule.SelectedOperation = rblSelectedFiltersOperation.SelectedValue;

            var selected = new List<int>();
            var suppressed = new List<int>();

            foreach (ListItem selectedFilter in lstSelectedFilters.Items)
            {
                if (selectedFilter.Selected)
                {
                    selected.Add(Convert.ToInt32(selectedFilter.Value));
                }
            }

            filterSchedule.FilterGroupID_Selected = selected;

            foreach (ListItem suppressedFilter in lstSuppressedFilters.Items)
            {
                if (suppressedFilter.Selected)
                {
                    suppressed.Add(Convert.ToInt32(suppressedFilter.Value));
                }
            }

            filterSchedule.FilterGroupID_Suppressed = suppressed;

            if (suppressed.Count > 0)
            {
                filterSchedule.SuppressedOperation = rblSuppressedFiltersOperation.SelectedValue;
            }
        }

        private bool CanExportToEcn()
        {
            var productIds = new List<int> { Convert.ToInt32(hfPubID.Value) };

            var exportDemoFields = Utilities.GetExportFields(
                Master.clientconnections,
                (ViewType)Enum.Parse(typeof(ViewType), hfViewType.Value),
                Convert.ToInt32(hfBrandID.Value),
                productIds,
                (Enums.ExportType)Enum.Parse(typeof(Enums.ExportType), ddlExport.SelectedValue),
                Master.UserSession.UserID,
                ExportFieldType.Demo);

            var exportAdhocFields = Utilities.GetExportFields(
                Master.clientconnections,
                (ViewType)Enum.Parse(typeof(ViewType), hfViewType.Value),
                Convert.ToInt32(hfBrandID.Value),
                productIds,
                (Enums.ExportType)Enum.Parse(typeof(Enums.ExportType), ddlExport.SelectedValue),
                Master.UserSession.UserID,
                ExportFieldType.Adhoc);

            var demoFieldsCount = 0;
            var adhocFieldsCount = 0;

            for (var index = 0; index < lstSelectedFields.Items.Count; index++)
            {
                if (exportDemoFields.ContainsKey(lstSelectedFields.Items[index].Value))
                {
                    demoFieldsCount = demoFieldsCount + 1;
                }
                else if (exportAdhocFields.ContainsKey(lstSelectedFields.Items[index].Value))
                {
                    adhocFieldsCount = adhocFieldsCount + 1;
                }
            }

            var strError = string.Empty;

            if (demoFieldsCount > 5)
            {
                strError = OnlyDemographicFieldsAreAllowedInAnExportToGroup;
            }

            if (adhocFieldsCount > 5)
            {
                if (!string.IsNullOrWhiteSpace(strError))
                {
                    strError += " and ";
                }

                strError += OnlyAdhocFieldsAreAllowedInAnExportToGroup;
            }

            if (!string.IsNullOrWhiteSpace(strError))
            {
                DisplayError(strError);
                return false;
            }

            return true;
        }

        private FilterSchedule SetFilterSchedule()
        {
            var filterSchedule = new FilterSchedule
            {
                FilterScheduleID = Convert.ToInt32(hdFilterScheduleID.Value),
                ExportName = txtExportName.Text,
                ExportNotes = txtExportNotes.Text
            };

            if (FilterSegmentationID > 0)
            {
                filterSchedule.FilterSegmentationID = FilterSegmentationID;
            }

            if (ddlExport.SelectedValue == Enums.ExportType.FTP.ToString())
            {
                ExportFtp(filterSchedule);
            }
            else if (ddlExport.SelectedValue == Enums.ExportType.ECN.ToString())
            {
                ExportEcn(filterSchedule);
            }
            else
            {
                var ddlMarketoList = (DropDownList)Marketo.FindControl("ddlMarketoList");
                filterSchedule.GroupID = !string.IsNullOrEmpty(ddlMarketoList.SelectedValue)
                                             ? Convert.ToInt32(ddlMarketoList.SelectedValue)
                                             : (int?)null;

                filterSchedule.ExportTypeID = Enums.ExportType.Marketo;
            }

            filterSchedule.FilterID = Convert.ToInt32(hdFilterID.Value);

            if (ddlScheduleType.SelectedValue == ScheduleTypeRecurring)
            {
                filterSchedule.IsRecurring = true;
                filterSchedule.RecurrenceTypeID = Convert.ToInt32(ddlRecurrence.SelectedItem.Value);
                filterSchedule.StartDate = Convert.ToDateTime(txtRecurringStartDate.Text);
                filterSchedule.StartTime = ddlRecurringStartTime.SelectedValue;
                filterSchedule.EndDate = string.IsNullOrEmpty(txtRecurringEndDate.Text)
                                             ? null
                                             : (DateTime?)DateTime.Parse(txtRecurringEndDate.Text);
            }
            else
            {
                filterSchedule.IsRecurring = false;
                filterSchedule.StartDate = Convert.ToDateTime(txtStartDate.Text);
                filterSchedule.StartTime = ddlStartTime.SelectedValue;
            }

            if (Convert.ToInt32(hdFilterScheduleID.Value) > 0)
            {
                filterSchedule.UpdatedBy = Master.LoggedInUser;
            }
            else
            {
                filterSchedule.CreatedBy = Master.LoggedInUser;
            }

            SetRunDay(filterSchedule);

            filterSchedule.MonthScheduleDay = string.IsNullOrEmpty(txtMonth.Text)
                                                  ? null
                                                  : (int?)int.Parse(txtMonth.Text);
            filterSchedule.MonthLastDay = cbLastDay.Checked;
            filterSchedule.EmailNotification = txtEmailAddress.Text;
            return filterSchedule;
        }

        private void SetRunDay(FilterSchedule filterSchedule)
        {
            foreach (ListItem item in cbDays.Items)
            {
                if (!item.Selected)
                {
                    continue;
                }

                if (item.Value == DayOfWeek.Sunday.ToString())
                {
                    filterSchedule.RunSunday = true;
                }
                else if (item.Value == DayOfWeek.Monday.ToString())
                {
                    filterSchedule.RunMonday = true;
                }
                else if (item.Value == DayOfWeek.Tuesday.ToString())
                {
                    filterSchedule.RunTuesday = true;
                }
                else if (item.Value == DayOfWeek.Wednesday.ToString())
                {
                    filterSchedule.RunWednesday = true;
                }
                else if (item.Value == DayOfWeek.Thursday.ToString())
                {
                    filterSchedule.RunThursday = true;
                }
                else if (item.Value == DayOfWeek.Friday.ToString())
                {
                    filterSchedule.RunFriday = true;
                }
                else if (item.Value == DayOfWeek.Saturday.ToString())
                {
                    filterSchedule.RunSaturday = true;
                }
            }
        }

        private void ExportMarketo(int filterScheduleId)
        {
            FilterScheduleIntegration.Delete(Master.clientconnections, Convert.ToInt32(hdFilterScheduleID.Value));
            AddFilterScheduleIntegrations(filterScheduleId);

            var gridViewHttpPost = (GridView)Marketo.FindControl("gvHttpPost");

            if (gridViewHttpPost.Rows.Count <= 0)
            {
                return;
            }

            for (var index = 0; index < gridViewHttpPost.Rows.Count; index++)
            {
                if (gridViewHttpPost.Rows[index].RowType != DataControlRowType.DataRow)
                {
                    continue;
                }

                var lblParamName = (Label)gridViewHttpPost.Rows[index].FindControl("lblParamName");
                var lblParamValue = (Label)gridViewHttpPost.Rows[index].FindControl("lblParamValue");
                var lblCustomValue = (Label)gridViewHttpPost.Rows[index].FindControl("lblCustomValue");
                var lblParamDisplayName = (Label)gridViewHttpPost.Rows[index].FindControl("lblParamDisplayName");

                var fef = new FilterExportField
                {
                    FilterScheduleID = filterScheduleId,
                    MappingField = lblParamName.Text,
                    IsCustomValue = lblCustomValue.Text != string.Empty,
                    CustomValue = lblCustomValue.Text,
                    DisplayName = lblParamDisplayName.Text
                };

                if (lblParamValue.Text.Contains(ParameterValueDescription))
                {
                    fef.ExportColumn = lblParamValue.Text.Split(PipeSeparator)[0]
                        .Replace(ParameterValueDescription, string.Empty);
                    fef.IsDescription = true;
                }
                else
                {
                    fef.ExportColumn = lblParamValue.Text.Split(PipeSeparator)[0];
                    fef.IsDescription = false;
                }

                FilterExportField.Save(Master.clientconnections, fef);
            }
        }

        private void AddFilterScheduleIntegrations(
            int filterScheduleId)
        {
            var txtMarketoClientId = (TextBox)Marketo.FindControl("txtMarketoClientID");
            var txtMarketoClientSecret = (TextBox)Marketo.FindControl("txtMarketoClientSecret");
            var txtMarketoPartition = (TextBox)Marketo.FindControl("txtMarketoPartition");
            var txtMarketoBaseUrl = (TextBox)Marketo.FindControl("txtMarketoBaseURL");

            var marketoServer = txtMarketoBaseUrl.Text;

            if (!marketoServer.Contains(HttpsProtocol))
            {
                marketoServer = HttpsProtocol + marketoServer;
            }

            if (marketoServer.Last() == '/') 
            {
                marketoServer = marketoServer.Remove(marketoServer.Length - 1);
            }

            var filterScheduleIntegrationBaseUrl = new FilterScheduleIntegration
            {
                FilterScheduleID = filterScheduleId,
                IntegrationParamName =
                    Enums.Marketo.BaseURL.ToString(),
                IntegrationParamValue = marketoServer
            };

            FilterScheduleIntegration.Save(Master.clientconnections, filterScheduleIntegrationBaseUrl);

            var filterScheduleIntegration = new FilterScheduleIntegration
            {
                FilterScheduleID = filterScheduleId,
                IntegrationParamName =
                    Enums.Marketo.ClientID.ToString(),
                IntegrationParamValue = txtMarketoClientId.Text
            };

            FilterScheduleIntegration.Save(Master.clientconnections, filterScheduleIntegration);

            var scheduleIntegrationClientSecret = new FilterScheduleIntegration
            {
                FilterScheduleID = filterScheduleId,
                IntegrationParamName = Enums.Marketo.ClientSecret.ToString(),
                IntegrationParamValue = txtMarketoClientSecret.Text
            };

            FilterScheduleIntegration.Save(Master.clientconnections, scheduleIntegrationClientSecret);

            var scheduleIntegrationPartition = new FilterScheduleIntegration
            {
                FilterScheduleID = filterScheduleId,
                IntegrationParamName = Enums.Marketo.Partition.ToString(),
                IntegrationParamValue = txtMarketoPartition.Text
            };

            FilterScheduleIntegration.Save(Master.clientconnections, scheduleIntegrationPartition);
        }

        private void SaveFilterExportFields(int filterScheduleId)
        {
            foreach (ListItem item in lstSelectedFields.Items)
            {
                var filterExportField = new FilterExportField
                {
                    FilterScheduleID = filterScheduleId,
                    IsCustomValue = false,
                    MappingField = string.Empty,
                    CustomValue = string.Empty
                };

                if (item.Value.Split(PipeSeparator)[0].Contains(ParameterValueDescription))
                {
                    filterExportField.ExportColumn = item.Value.Split(PipeSeparator)[0].Replace(ParameterValueDescription, string.Empty);
                    filterExportField.IsDescription = true;
                    var equalToFieldCaseNone = item.Value.Split(PipeSeparator)[2].Equals(
                        FieldCase.None.ToString(), 
                        StringComparison.OrdinalIgnoreCase);

                    filterExportField.FieldCase = equalToFieldCaseNone
                                                      ? null
                                                      : item.Value.Split(PipeSeparator)[2];
                }
                else
                {
                    filterExportField.ExportColumn = item.Value.Split(PipeSeparator)[0];
                    filterExportField.IsDescription = false;
                    var equalToFieldCaseNone = item.Value.Split(PipeSeparator)[2].Equals(
                        FieldCase.None.ToString(),
                        StringComparison.OrdinalIgnoreCase);

                    filterExportField.FieldCase = equalToFieldCaseNone
                                                      ? null
                                                      : item.Value.Split(PipeSeparator)[2];
                }

                FilterExportField.Save(Master.clientconnections, filterExportField);
            }
        }

        private void ExportEcn(FilterSchedule filterSchedule)
        {
            var filterGroup = Group.GetByGroupID(int.Parse(hfGroupID.Value), Master.UserSession.CurrentUser);
            filterSchedule.GroupID = int.Parse(hfGroupID.Value);
            filterSchedule.CustomerID = filterGroup.CustomerID;
            filterSchedule.FolderID = filterGroup.FolderID;
            filterSchedule.ExportTypeID = Enums.ExportType.ECN;
        }

        private void ExportFtp(FilterSchedule filterSchedule)
        {
            if (ddlSiteType.SelectedValue == SiteType.FTP.ToString())
            {
                filterSchedule.Server = txtServer.Text.Contains(FtpProtocol)
                                            ? txtServer.Text
                                            : FtpProtocol + txtServer.Text;
            }
            else if (ddlSiteType.SelectedValue == SiteType.FTPS.ToString())
            {
                filterSchedule.Server = txtServer.Text.Contains(FtpsProtocol)
                                            ? txtServer.Text
                                            : FtpsProtocol + txtServer.Text;
            }
            else if (ddlSiteType.SelectedValue == SiteType.SFTP.ToString())
            {
                filterSchedule.Server = txtServer.Text.Contains(SftpProtocol)
                                            ? txtServer.Text
                                            : SftpProtocol + txtServer.Text;
            }

            filterSchedule.UserName = txtUserName.Text;
            filterSchedule.Password = txtPassword.Text;
            filterSchedule.Folder = txtFolder.Text;
            filterSchedule.ExportFormat = ddlExportFormat.SelectedValue;
            filterSchedule.ShowHeader = Convert.ToBoolean(cbShowHeader.Checked);
            filterSchedule.FileNameFormat = rblFileNameFormat.SelectedValue;
            filterSchedule.FileName = txtFileName.Text;
            filterSchedule.ExportTypeID = Enums.ExportType.FTP;
        }

        private void ProcessFilterSegmentation(FilterSchedule filterSchedule)
        {
            var filterSegmentation = new FilterSegmentation().SelectByID(FilterSegmentationID, Master.clientconnections);
            fc = MDFilter.LoadFilters(Master.clientconnections, filterSegmentation.FilterID, Master.LoggedInUser);

            foreach (GridViewRow gridViewRow in grdFilterSegmentationCounts.Rows)
            {
                var radioButton = gridViewRow.FindControl("rbFSSelect") as RadioButton;

                if (radioButton == null || !radioButton.Checked)
                {
                    continue;
                }

                var selectedFilterNo = (HiddenField)gridViewRow.FindControl("hfSelectedFilterNo");
                var suppressedFilterNo = (HiddenField)gridViewRow.FindControl("hfSuppressedFilterNo");
                var selectedFilterOperation = (HiddenField)gridViewRow.FindControl("hfSelectedFilterOperation");
                var suppressedFilterOperation = (HiddenField)gridViewRow.FindControl("hfSuppressedFilterOperation");

                var groupIds = new List<int>();
                var selected = selectedFilterNo.Value.Split(
                    new[]
                    {
                        CommaSeparator
                    },
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (var selectedFilterId in selected)
                {
                    groupIds.Add(
                        fc.First(x => x.FilterNo == Convert.ToInt32(selectedFilterId))
                            .FilterGroupID);
                }

                filterSchedule.FilterGroupID_Selected = groupIds;

                groupIds = new List<int>();
                var suppressed = suppressedFilterNo.Value.Split(
                    new[]
                    {
                        CommaSeparator
                    },
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (var suppressedFilterId in suppressed)
                {
                    groupIds.Add(
                        fc.First(filter => filter.FilterNo == Convert.ToInt32(suppressedFilterId))
                            .FilterGroupID);
                }

                filterSchedule.FilterGroupID_Suppressed = groupIds;

                filterSchedule.SelectedOperation = selectedFilterOperation.Value;
                filterSchedule.SuppressedOperation = string.IsNullOrEmpty(suppressedFilterOperation.Value)
                                                         ? null
                                                         : suppressedFilterOperation.Value;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("FilterSchedules.aspx?" + (FilterSegmentationID > 0 ? "Mode=FilterSegmentation" : "Mode=Filters"));
        }

        #region loadfilters
        protected void grdFilters_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView grdFilterValues = (GridView)e.Row.FindControl("grdFilterValues");
                List<Field> grdFilterList = LoadGridFilterValues(Convert.ToInt32(grdFilters.DataKeys[e.Row.RowIndex].Value.ToString()));
                grdFilterValues.DataSource = grdFilterList.Distinct().ToList();
                grdFilterValues.DataBind();
            }
        }

        private List<Field> LoadGridFilterValues(int filterNo)
        {
            Filter filter = fc.SingleOrDefault(f => f.FilterNo == filterNo);
            return filter.Fields;
        }
        #endregion

        protected void ddlOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lstSelectedFilters.ClearSelection();
            //lstSuppressedFilters.ClearSelection();

            //if (ddlOperation.SelectedValue.Equals("Intersect", StringComparison.OrdinalIgnoreCase) || ddlOperation.SelectedValue.Equals("Union", StringComparison.OrdinalIgnoreCase))
            //{
            //    plOperation.Visible = true;
            //    plOperationNotIn.Visible = false;
            //}
            //else if (ddlOperation.SelectedValue.Equals("NotIn", StringComparison.OrdinalIgnoreCase))
            //{
            //    plOperationNotIn.Visible = true;
            //    plOperation.Visible = false;
            //}
            //else
            //{
            //    plOperationNotIn.Visible = false;
            //    plOperation.Visible = false;
            //}
        }

        protected override bool OnBubbleEvent(object sender, EventArgs e)
        {
            try
            {
                string source = sender.ToString();
                if (source.Equals("GroupSelected"))
                {
                    int groupID = GroupsLookup.selectedGroupID;
                    ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(groupID);
                    txtGroupName.Text = group.GroupName;
                    hfGroupID.Value = groupID.ToString();
                    GroupsLookup.Visible = false;
                }
            }
            catch { }
            return true;
        }

        protected void btnEditCase_Click(object sender, EventArgs e)
        {
            if (lstSelectedFields.Items.Count >= 1)
            {
                DownloadEditCase.Visible = true;
                Dictionary<string, string> downloadFields = new Dictionary<string, string>();

                for (int i = 0; i < lstSelectedFields.Items.Count; i++)
                {
                    if (lstSelectedFields.Items[i].Value.Split('|')[1].ToUpper() == "VARCHAR")
                        downloadFields.Add(lstSelectedFields.Items[i].Value, lstSelectedFields.Items[i].Text.Split('(')[0]);
                }

                DownloadEditCase.DownloadFields = downloadFields;
                DownloadEditCase.loadControls();
            }
            else
            {
                DisplayError("Please select field to edit case.");
            }
        }

        public void LoadFilterSegmenationData()
        {
            FrameworkUAD.Entity.FilterSegmentation fs = new FrameworkUAD.BusinessLogic.FilterSegmentation().SelectByID(FilterSegmentationID, Master.clientconnections, true);

            FilterViews fv = new FilterViews(Master.clientconnections, Master.UserSession.UserID);

            foreach (FrameworkUAD.Entity.FilterSegmentationGroup f in fs.FilterSegmentationGroupList)
            {
                filterView fv1 = new filterView();
                fv1.FilterViewNo = fv.Count + 1;
                fv1.FilterViewName = fc.FirstOrDefault().FilterName + " - " + fs.FilterSegmentationName;

                string selectedfilterNos = string.Empty;
                string suppressedfilterNos = string.Empty;

                foreach (int s in f.FilterGroupID_Selected)
                {
                    string fNo = fc.FirstOrDefault(x => x.FilterGroupID == s).FilterNo.ToString();
                    selectedfilterNos += (selectedfilterNos == string.Empty ? "" : ",") + fNo;
                }

                fv1.SelectedFilterNo = selectedfilterNos;

                foreach (int s in f.FilterGroupID_Suppressed)
                {
                    string fNo = fc.FirstOrDefault(x => x.FilterGroupID == s).FilterNo.ToString();
                    suppressedfilterNos += (suppressedfilterNos == string.Empty ? "" : ",") + fNo;
                }

                fv1.SuppressedFilterNo = suppressedfilterNos;
                fv1.SelectedFilterOperation = f.SelectedOperation;
                fv1.SuppressedFilterOperation = f.SuppressedOperation;

                if (fv1.SuppressedFilterNo == "")
                {
                    fv1.FilterDescription = selectedfilterNos + "(" + fv1.SelectedFilterOperation + ")";
                }
                else
                {
                    string Selected_Segments_Label = fv1.SelectedFilterNo.Contains(",").ToString() == "" ? "(" + fv1.SelectedFilterOperation + ")" : "";
                    string Suppressed_Segments_Label = fv1.SuppressedFilterNo.Contains(",").ToString() == "" ? "(" + fv1.SuppressedFilterOperation + ")" : "";
                    fv1.FilterDescription = selectedfilterNos + Selected_Segments_Label + " Not In " + suppressedfilterNos + Suppressed_Segments_Label;
                }

                fv.Add(fv1);
            }

            grdFilterSegmentationCounts.DataSource = null;
            grdFilterSegmentationCounts.DataBind();

            fv.Execute(fc);

            grdFilterSegmentationCounts.DataSource = fv;
            grdFilterSegmentationCounts.DataBind();



        }
    }
}