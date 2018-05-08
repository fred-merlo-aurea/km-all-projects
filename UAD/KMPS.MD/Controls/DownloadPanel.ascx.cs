using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Text;
using KMPS.MD.Objects;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Xml.Linq;
using AjaxControlToolkit;
using FrameworkUAD.DataAccess;
using KMPS.MD.Helpers;
using CampaignFilter = KMPS.MD.Objects.CampaignFilter;
using DataFunctions = KMPS.MD.Objects.DataFunctions;
using DownloadTemplate = KMPS.MD.Objects.DownloadTemplate;
using DownloadTemplateDetails = KMPS.MD.Objects.DownloadTemplateDetails;
using Filter = KMPS.MD.Objects.Filter;
using MasterGroup = KMPS.MD.Objects.MasterGroup;
using ResponseGroup = KMPS.MD.Objects.ResponseGroup;
using Subscriber = KMPS.MD.Objects.Subscriber;
using SubscriptionsExtensionMapper = KMPS.MD.Objects.SubscriptionsExtensionMapper;
using UserDataMask = KMPS.MD.Objects.UserDataMask;
using Utilities = KMPS.MD.Objects.Utilities;

namespace KMPS.MD.Controls
{
    public partial class DownloadPanel : DownloadPanelBase
    {
        public Delegate DelMethod;
        public Delegate hideDownloadPopup;
        delegate void HidePanel();
        delegate void LoadEditCaseData(Dictionary<string, string> downloadfields);

        protected override PlaceHolder PhProfileFields => phProfileFields;
        protected override ListBox LstAvailableProfileFields => lstAvailableProfileFields;
        protected override ListBox LstSelectedFields => lstSelectedFields;
        protected override PlaceHolder PhDemoFields => phDemoFields;
        protected override ListBox LstAvailableDemoFields => lstAvailableDemoFields;
        protected override ListBox LstAvailableAdhocFields => lstAvailableAdhocFields;
        protected override DropDownList DrpIsBillable => drpIsBillable;
        protected override Button BtnExport => btnExport;
        protected override PlaceHolder PlNotes => plNotes;
        protected override PlaceHolder PhShowHeader => phShowHeader;
        protected override CheckBox CbShowHeader => cbShowHeader;
        protected override ModalPopupExtender MdlDownloads => mdlDownloads;
        protected override TextBox TxtDownloadCount => txtDownloadCount;
        protected override TextBox TxtPromocode => txtPromocode;
        protected override RadioButton RbDownload => rbDownload;
        protected override HtmlGenericControl DvDownloads => dvDownloads;
        protected override TextBox TxtNotes => txtNotes;
        protected override HiddenField HfDownloadTemplateID => hfDownloadTemplateID;
        protected override DataGrid DgDataCompareResult => dgDataCompareResult;
        protected override PlaceHolder PlDataCompareResult => plDataCompareResult;
        protected override Label LblDataCompareMessage => lblDataCompareMessage;
        protected override HtmlGenericControl DivError => divError;
        protected override Label LblErrorMessage => lblErrorMessage;

        public bool error
        {
            get;
            set;
        }

        public string errormsg
        {
            get;
            set;
        }

        public bool Showsavetocampaign
        {
            get
            {
                try
                {
                    return (bool)ViewState["Showsavetocampaign"];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                ViewState["Showsavetocampaign"] = value;
            }
        }

        public bool Showexporttoemailmarketing
        {
            get
            {
                try
                {
                    return (bool)ViewState["Showexporttoemailmarketing"];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                ViewState["Showexporttoemailmarketing"] = value;
            }
        }

        public bool Showexporttomarketo
        {
            get
            {
                try
                {
                    return (bool)ViewState["Showexporttomarketo"];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                ViewState["Showexporttomarketo"] = value;
            }
        }
        
        public bool IsPopupDimension
        {
            get
            {
                try
                {
                    return (bool)ViewState["IsPopupDimension"];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                ViewState["IsPopupDimension"] = value;
            }
        }

        public bool IsPopupCrossTab
        {
            get
            {
                try
                {
                    return (bool)ViewState["IsPopupCrossTab"];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                ViewState["IsPopupCrossTab"] = value;
            }
        }

        public bool IsPopupSVFilter
        {
            get
            {
                try
                {
                    return (bool)ViewState["IsPopupSVFilter"];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                ViewState["IsPopupSVFilter"] = value;
            }
        }
        
        public List<int> SubscriptionID
        {
            get
            { return (List<int>)Session["SubscriptionID"]; }
            set
            {
                Session["SubscriptionID"] = value;

                if (Session["SubscriptionID"] != null)
                {
                    List<int> Subscribers = (List<int>)Session["SubscriptionID"];
                    rngCount.MaximumValue = Subscribers.Count.ToString();
                    txtDownloadCount.Text = Subscribers.Count.ToString();
                    txtTotalCount.Text = Subscribers.Count.ToString();
                }
            }
        }

        public int CampaignID
        {
            get
            {
                try
                {
                    return (int)ViewState["CampaignID"];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState["CampaignID"] = value;
                if (ViewState["CampaignID"] != null)
                {
                    if (Convert.ToInt32(ViewState["CampaignID"]) != 0)
                        txtPromocode.Text = CampaignFilter.GetByCampaignID(clientconnections, Convert.ToInt32(ViewState["CampaignID"])).OrderByDescending(x => x.CampaignFilterID).FirstOrDefault().PromoCode;
                }
            }
        }

        public int CampaignFilterID
        {
            get
            {
                try
                {
                    return (int)ViewState["CampaignFilterID"];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState["CampaignFilterID"] = value;
                if (ViewState["CampaignFilterID"] != null)
                {
                    if (Convert.ToInt32(ViewState["CampaignFilterID"]) != 0)
                        txtPromocode.Text = CampaignFilter.GetByID(clientconnections, Convert.ToInt32(ViewState["CampaignFilterID"])).PromoCode;
                }
            }
        }
        
        public bool EnableCbIsRecentData
        {
            get
            {
                try
                {
                    return (bool)ViewState["EnableCbIsRecentData"];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                ViewState["EnableCbIsRecentData"] = value;
            }
        }
        
        private void DownloadCasePopupHide()
        {
            DownloadEditCase.Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            isError();
            if (rbDownload.Checked && ShowHeaderCheckBox && KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADExport, KMPlatform.Enums.Access.Download))
                phShowHeader.Visible = true;
            else
                phShowHeader.Visible = false;

            lblResults.Visible = false;
            lblResults.Text = string.Empty;
            lblErrSelectedFields.Visible = false;

            HidePanel delDownloadCase = new HidePanel(DownloadCasePopupHide);
            this.DownloadEditCase.hideDownloadCasePopup = delDownloadCase;

            LoadEditCaseData delNoParamDownloadFields = new LoadEditCaseData(LoadEditCase);
            this.DownloadEditCase.LoadEditCaseData = delNoParamDownloadFields;

            if (!IsPostBack)
            {
            }

            this.mdlDownloads.Show();

            if (IsPopupDimension)
            {
                AjaxControlToolkit.ModalPopupExtender mdlPopupDimensionReport = (AjaxControlToolkit.ModalPopupExtender)(this.Parent).FindControl("mdlPopupDimensionReport");
                mdlPopupDimensionReport.Show();
            }

            if (IsPopupCrossTab)
            {
                AjaxControlToolkit.ModalPopupExtender ModalPopupCrossTabReport = (AjaxControlToolkit.ModalPopupExtender)(this.Parent).FindControl("ModalPopupCrossTabReport");
                ModalPopupCrossTabReport.Show();
            }


            if (IsPopupSVFilter)
            {
                AjaxControlToolkit.ModalPopupExtender mdlPopupFilter = (AjaxControlToolkit.ModalPopupExtender)(this.Parent).FindControl("mdlPopupFilter");
                mdlPopupFilter.Show();
            }
        }

        public void LoadEditCase(Dictionary<string,string> downloadfields)
        {
            ControlsValidators.LoadEditCase(downloadfields, lstSelectedFields);
        }

        public void LoadControls()
        {
            if (ViewType == Enums.ViewType.ConsensusView || ViewType == Enums.ViewType.RecencyView)
            {
                cbIsRecentData.Checked = ViewType == Enums.ViewType.RecencyView ? true : false;
                cbIsRecentData.Enabled = EnableCbIsRecentData ? true : false;
                pnlIsRecentData.Visible = VisibleCbIsRecentData ? true : false;
            }
            else
            {
                pnlIsRecentData.Visible = false;
                cbIsRecentData.Checked = false;
                cbIsRecentData.Enabled = false;
            }

            rngCount.MaximumValue = downloadCount.ToString();
            txtDownloadCount.Text = downloadCount.ToString();
            txtTotalCount.Text = downloadCount.ToString();
        }

        public void LoadDownloadTemplate()
        {
            drpDownloadTemplate.Items.Clear();

            if (dcRunID > 0 && filterCombination.Equals("Matched NotIn Selected", StringComparison.OrdinalIgnoreCase) && ViewType == Enums.ViewType.ProductView)
                drpDownloadTemplate.DataSource = DownloadTemplate.GetByPubIDBrandID(clientconnections, 0, BrandID);
            else
                drpDownloadTemplate.DataSource = DownloadTemplate.GetByPubIDBrandID(clientconnections, PubIDs == null ? 0 : PubIDs.First(), BrandID);
            drpDownloadTemplate.DataBind();
            drpDownloadTemplate.Items.Insert(0, new ListItem("Select Download Template", "0"));
        }

        public void loadExportFields()
        {
            try
            {
                lstAvailableProfileFields.Items.Clear();
                lstAvailableDemoFields.Items.Clear();
                lstAvailableAdhocFields.Items.Clear();
                lstSelectedFields.Items.Clear();

                ExportFields();

                if (rbGroupExport.Checked)
                {
                    Dictionary<string, string> exportDemoFields = Utilities.GetExportFields(clientconnections, ViewType, BrandID, PubIDs, Enums.ExportType.ECN, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Demo);
                    Dictionary<string, string> exportAdhocFields = Utilities.GetExportFields(clientconnections, ViewType, BrandID, PubIDs, Enums.ExportType.ECN, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Adhoc);

                    int demoFieldsCount = 0;
                    int AdhocFieldsCount = 0;

                    for (int i = 0; i < lstSelectedFields.Items.Count; i++)
                    {
                        if (exportDemoFields.Any(x => x.Key.Split('|')[0] == lstSelectedFields.Items[i].Value.ToString().Split('|')[0]))
                        {
                            demoFieldsCount = demoFieldsCount + 1;
                        }
                        else if (exportAdhocFields.Any(x => x.Key.Split('|')[0] == lstSelectedFields.Items[i].Value.ToString().Split('|')[0]))
                        {
                            AdhocFieldsCount = AdhocFieldsCount + 1;
                        }
                    }

                    string strError = string.Empty;

                    if (demoFieldsCount > 5)
                    {
                        strError = "Only 5 Demographic fields are allowed in an Export to group";
                    }

                    if (AdhocFieldsCount > 5)
                    {
                        if (strError != string.Empty)
                            strError += " and ";
                        strError += "Only 5 Adhoc fields are allowed in an Export to group";
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
                Utilities.Log_Error(Request.RawUrl.ToString(), "loadExportFields", ex);
                DisplayError(ex.Message);
            }
        }

        private void ExportFields()
        {
            Enums.ExportType exportType = Enums.ExportType.FTP;
            if (rbDownload.Checked)
                exportType = Enums.ExportType.FTP;
            else if (rbGroupExport.Checked)
                exportType = Enums.ExportType.ECN;
            else if (rbCampaign.Checked)
                exportType = Enums.ExportType.Campaign;
            else if (rbMarketo.Checked)
                exportType = Enums.ExportType.Marketo;

            Dictionary<string, string> exportfields = new Dictionary<string, string>();
            Dictionary<string, string> exportProfileFields = new Dictionary<string, string>();
            Dictionary<string, string> exportDemoFields = new Dictionary<string, string>();
            Dictionary<string, string> exportAdhocFields = new Dictionary<string, string>();
            Dictionary<string, string> selectedfields = new Dictionary<string, string>();

            if (dcRunID > 0 && filterCombination.Equals("Matched NotIn Selected", StringComparison.OrdinalIgnoreCase) && ViewType == Enums.ViewType.ProductView)
            {
                exportProfileFields = Utilities.GetExportFields(clientconnections, Enums.ViewType.ConsensusView, 0, null, exportType, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Profile);
                exportDemoFields = Utilities.GetExportFields(clientconnections, Enums.ViewType.ConsensusView, 0, null, exportType, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Demo);
                exportAdhocFields = Utilities.GetExportFields(clientconnections, Enums.ViewType.ConsensusView, 0, null, exportType, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Adhoc);
            }
            else
            {
                exportProfileFields = Utilities.GetExportFields(clientconnections, ViewType, BrandID, PubIDs, exportType, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Profile);
                exportDemoFields = Utilities.GetExportFields(clientconnections, ViewType, BrandID, PubIDs, exportType, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Demo);
                exportAdhocFields = Utilities.GetExportFields(clientconnections, ViewType, BrandID, PubIDs, exportType, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Adhoc);
            }

            foreach (var e in exportProfileFields)
                exportfields.Add(e.Key, e.Value);

            foreach (var e in exportDemoFields)
                exportfields.Add(e.Key, e.Value);

            foreach (var e in exportAdhocFields)
                exportfields.Add(e.Key, e.Value);

            List<DownloadTemplateDetails> dtd = DownloadTemplateDetails.GetByDownloadTemplateID(clientconnections, Convert.ToInt32(hfDownloadTemplateID.Value));

            foreach (var item in exportProfileFields)
            {
                if (!dtd.Exists(x => x.ExportColumn.ToUpper() == item.Key.Split('|')[0].ToUpper()))
                    lstAvailableProfileFields.Items.Add(new ListItem(item.Value.ToUpper(), item.Key));
                else
                    selectedfields.Add(item.Key, item.Value);
            }

            foreach (var item in exportDemoFields)
            {
                if (!dtd.Exists(x => x.ExportColumn.ToUpper() == item.Key.Split('|')[0].ToUpper()))
                    lstAvailableDemoFields.Items.Add(new ListItem(item.Value.ToUpper(), item.Key));
                else
                    selectedfields.Add(item.Key, item.Value);
            }

            foreach (var item in exportAdhocFields)
            {
                if (!dtd.Exists(x => x.ExportColumn.ToUpper() == item.Key.Split('|')[0].ToUpper()))
                    lstAvailableAdhocFields.Items.Add(new ListItem(item.Value.ToUpper(), item.Key));
                else
                    selectedfields.Add(item.Key, item.Value);
            }

            foreach (DownloadTemplateDetails td in dtd)
            {
                string text = string.Empty;

                if (td.FieldCase != null && td.FieldCase != "")
                {
                    switch (td.FieldCase.ToUpper())
                    {
                        case "PROPERCASE":
                            text = "Proper Case";
                            break;
                        case "UPPERCASE":
                            text = "UPPER CASE";
                            break;
                        case "LOWERCASE":
                            text = "lower case";
                            break;
                        default:
                            text = "Default";
                            break;
                    }
                }

                if (exportfields.Any(x => x.Key.Split('|')[0].ToUpper() == td.ExportColumn.ToUpper()))
                {
                    var field = selectedfields.FirstOrDefault(x => x.Key.Split('|')[0].ToUpper() == td.ExportColumn.ToUpper());
                    lstSelectedFields.Items.Add(new ListItem(field.Value.ToUpper() +   (field.Key.Split('|')[1].ToUpper() == Enums.FieldType.Varchar.ToString().ToUpper() ?  "(" + (td.FieldCase == null ? Enums.FieldCase.Default.ToString() : text) + ")" : "") , field.Key + "|" + (td.FieldCase == null ? (field.Key.Split('|')[1].ToUpper() == KMPS.MD.Objects.Enums.FieldType.Varchar.ToString().ToUpper() ? KMPS.MD.Objects.Enums.FieldCase.Default.ToString() : KMPS.MD.Objects.Enums.FieldCase.None.ToString()) : td.FieldCase)));
                }
            }
        }

        public void reload()
        {
            if (SubscriptionID != null)
            {
                rngCount.MaximumValue = SubscriptionID.Count.ToString();
                txtDownloadCount.Text = SubscriptionID.Count.ToString();
                txtTotalCount.Text = SubscriptionID.Count.ToString();
                Session["SubscriptionID"] = SubscriptionID;
            }
            else
            {
                rngCount.MaximumValue = downloadCount.ToString();
                txtDownloadCount.Text = downloadCount.ToString();
                txtTotalCount.Text = downloadCount.ToString();
                Session["SubscribersQueries"] = SubscribersQueries;
            }

            if (CampaignFilterID != 0)
                txtPromocode.Text = CampaignFilter.GetByID(clientconnections, Convert.ToInt32(ViewState["CampaignFilterID"])).PromoCode;
        }

        public void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        public void isError()
        {
            if (!error)
            {
                lblErrorMessage.Text = string.Empty;
                divError.Visible = false;
            }
            else
            {
                DisplayError(errormsg);
            }
        }

        public void ValidateExportPermission()
        {
            if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADExport, KMPlatform.Enums.Access.Download) && !KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADExport, KMPlatform.Enums.Access.ExportToGroup) && !KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADExport, KMPlatform.Enums.Access.SaveToCampaign) && !KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Marketo, KMPlatform.Enums.Access.FullAccess))
            {
                lblNoDownloadMessage.Text = "You do not have permission to download/export the data.";
                lblNoDownloadMessage.Visible = true;
                pnlUADExport.Visible = false;
            }
            else
            {
                if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADExport, KMPlatform.Enums.Access.Download))
                {
                    phRBDownload.Visible = false;
                }
                else
                {
                    phRBDownload.Visible = true;
                    rbDownload.Checked = true;

                    if (rbDownload.Checked && ShowHeaderCheckBox)
                        phShowHeader.Visible = true;
                }

                if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.DataCompare, KMPlatform.Enums.Access.Yes) || dcRunID == 0)
                {
                    if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADExport, KMPlatform.Enums.Access.SaveToCampaign))
                    {
                        phRBCampaign.Visible = false;
                    }
                    else
                    {
                        if (Showsavetocampaign.Equals(false))
                            phRBCampaign.Visible = false;
                        else
                        {
                            phRBCampaign.Visible = true;
                        }
                    }

                    if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADExport, KMPlatform.Enums.Access.ExportToGroup))
                    {
                        phRBExport.Visible = false;
                    }
                    else
                    {
                        if (Showexporttoemailmarketing.Equals(true))
                        {
                            if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.ExternalImport))
                            {
                                phRBExport.Visible = false;
                            }
                            else
                            {
                                List<UserDataMask> udm = UserDataMask.GetByUserID(clientconnections, UserSession.UserID);

                                if (udm.Exists(u => u.MaskField.ToUpper() == "EMAIL") && !KM.Platform.User.IsAdministrator(UserSession.CurrentUser))
                                {
                                    phRBExport.Visible = false;
                                }
                                else
                                {
                                    phRBExport.Visible = true;

                                    if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.Edit))
                                        rbNewGroup.Visible = false;
                                    else
                                        rbNewGroup.Visible = true;

                                    if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.View))
                                        rbExistingGroup.Visible = false;
                                    else
                                        rbExistingGroup.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            phRBExport.Visible = false;
                        }
                    }

                    if (Showexporttoemailmarketing.Equals(false))
                    {
                        if (phRBDownload.Visible == false && phRBExport.Visible == false)
                        {
                            lblNoDownloadMessage.Text = "You do not have permission to download/export the data.";
                            lblNoDownloadMessage.Visible = true;
                            pnlUADExport.Visible = false;
                        }
                    }

                    if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Marketo, KMPlatform.Enums.Access.FullAccess))
                        phMarketo.Visible = false;
                    else
                    {
                        if (Showexporttomarketo.Equals(false))
                            phMarketo.Visible = false;
                        else
                        {
                            List<UserDataMask> udm = UserDataMask.GetByUserID(clientconnections, UserSession.UserID);

                            if (udm.Exists(u => u.MaskField.ToUpper() == "EMAIL") && !KM.Platform.User.IsAdministrator(UserSession.CurrentUser))
                            {
                                phMarketo.Visible = false;
                            }
                            else
                            {
                                phMarketo.Visible = true;
                                phDownloadCount.Visible = true;
                                Marketo.ViewType = ViewType;
                                if (ViewType == Enums.ViewType.ProductView)
                                    Marketo.PubID = PubIDs.First();
                                Marketo.BrandID = BrandID;
                                Marketo.loadMarketoExportFields();
                            }
                        }
                    }

                    if (phRBDownload.Visible == true || phRBExport.Visible == true)
                    {
                        phExportFields.Visible = true;
                        phDownloadCount.Visible = true;
                        phPromoCode.Visible = true;
                    }
                }
                else
                {
                    rcForDownload.Visible = true;

                    if (dcRunID > 0)
                    {
                        btnDownload.OnClientClick = "return confirmPurchase();";

                        if (UserSession.CurrentUser.IsKMStaff)
                        {
                            dvDownloads.Style.Add("height", "500px");
                            dvDownloads.Style.Add("overflow", "scroll");
                            plKmStaff.Visible = true;
                            plNotes.Visible = false;

                            List<FrameworkUAS.Entity.DataCompareView> datacv = new FrameworkUAS.BusinessLogic.DataCompareView().SelectForRun(dcRunID);

                            int? targetID = null;

                            if (ViewType == Enums.ViewType.ProductView)
                                targetID = PubIDs.First();
                            else
                                targetID = BrandID > 0 ? (int?)BrandID : null;

                            if (datacv.Exists(u => u.DcTargetCodeId == dcTargetCodeID && u.DcTargetIdUad == targetID && u.DcTypeCodeId == dcTypeCodeID))
                            {
                                FrameworkUAS.Entity.DataCompareView dc = datacv.Find(u => u.DcTargetCodeId == dcTargetCodeID && u.DcTargetIdUad == targetID && u.DcTypeCodeId == dcTypeCodeID);
                                if(dc.IsBillable)
                                {
                                    drpIsBillable.Enabled = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        dvDownloads.Style.Add("height", "");
                        dvDownloads.Style.Add("overflow", "hidden");
                        plKmStaff.Visible = false;
                        plNotes.Visible = false;
                        btnDownload.OnClientClick = null;
                    }
                }

                if (rbDownload.Checked)
                {
                    btnDownload.Visible = true;
                    btnExport.Visible = false;
                    btnPreECNExportResults.Visible = false;
                }
                else if (rbGroupExport.Checked || rbCampaign.Checked || rbMarketo.Checked)
                {
                    btnDownload.Visible = false;
                    btnExport.Visible = true;
                    if (rbGroupExport.Checked)
                        btnPreECNExportResults.Visible = true;
                    else
                        btnPreECNExportResults.Visible = false;
                }
            }
        }

        private int CampID
        {
            get
            {
                return Convert.ToInt32(ViewState["CampID"]);
            }
            set
            {
                ViewState["CampID"] = value;
            }
        }

        private int CampFilterID
        {
            get
            {
                return Convert.ToInt32(ViewState["CampFilterID"]);
            }
            set
            {
                ViewState["CampFilterID"] = value;
            }
        }

        protected void drpDownloadTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            hfDownloadTemplateID.Value = drpDownloadTemplate.SelectedValue;
            loadExportFields();
        }

        protected void rblFieldsType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblFieldsType.SelectedValue == Enums.ExportFieldType.Profile.ToString())
            {
                phProfileFields.Visible = true;
                phDemoFields.Visible = false;
                phAdhocFields.Visible = false;
            }
            else if (rblFieldsType.SelectedValue == Enums.ExportFieldType.Demo.ToString())
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
                        lstSelectedFields.Items.Add(new ListItem(lstAvailableProfileFields.Items[i].Value.Split('|')[1].ToUpper() == Enums.FieldType.Varchar.ToString().ToUpper() ?  lstAvailableProfileFields.Items[i].Text.ToUpper() + "(Default)" : lstAvailableProfileFields.Items[i].Text.ToUpper(), lstAvailableProfileFields.Items[i].Value.Split('|')[1].ToUpper() == Enums.FieldType.Varchar.ToString().ToUpper() ? lstAvailableProfileFields.Items[i].Value + "|Default"  : lstAvailableProfileFields.Items[i].Value + "|None"));
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
                        lstSelectedFields.Items.Add(new ListItem(lstAvailableDemoFields.Items[i].Value.Split('|')[1].ToUpper() == Enums.FieldType.Varchar.ToString().ToUpper() ? lstAvailableDemoFields.Items[i].Text.ToUpper() + "(Default)" : lstAvailableDemoFields.Items[i].Text.ToUpper(), lstAvailableDemoFields.Items[i].Value.Split('|')[1].ToUpper() == Enums.FieldType.Varchar.ToString().ToUpper() ? lstAvailableDemoFields.Items[i].Value + "|Default" : lstAvailableDemoFields.Items[i].Value + "|None"));
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
                        lstSelectedFields.Items.Add(new ListItem(lstAvailableAdhocFields.Items[i].Value.Split('|')[1].ToUpper() == Enums.FieldType.Varchar.ToString().ToUpper() ? lstAvailableAdhocFields.Items[i].Text.ToUpper() + "(Default)" : lstAvailableAdhocFields.Items[i].Text.ToUpper(), lstAvailableAdhocFields.Items[i].Value.Split('|')[1].ToUpper() == Enums.FieldType.Varchar.ToString().ToUpper() ? lstAvailableAdhocFields.Items[i].Value + "|Default" : lstAvailableAdhocFields.Items[i].Value + "|None"));
                        lstAvailableAdhocFields.Items.RemoveAt(i);
                        i--;
                    }
                }
            }

            if (rbGroupExport.Checked)
            {
                Dictionary<string, string> exportDemoFields = Utilities.GetExportFields(clientconnections, ViewType, BrandID, PubIDs, Enums.ExportType.ECN, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Demo);
                Dictionary<string, string> exportAdhocFields = Utilities.GetExportFields(clientconnections, ViewType, BrandID, PubIDs, Enums.ExportType.ECN, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Adhoc);

                int demoFieldsCount = 0;
                int AdhocFieldsCount = 0;

                for (int i = 0; i < lstSelectedFields.Items.Count; i++)
                {
                    if (exportDemoFields.Any(x=>x.Key.Split('|')[0] == lstSelectedFields.Items[i].Value.ToString().Split('|')[0]))
                    {
                        demoFieldsCount = demoFieldsCount + 1;
                    }
                    else if (exportAdhocFields.Any(x => x.Key.Split('|')[0] == lstSelectedFields.Items[i].Value.ToString().Split('|')[0]))
                    {
                        AdhocFieldsCount = AdhocFieldsCount + 1;
                    }
                }

                string strError = string.Empty;

                if (demoFieldsCount > 5)
                {
                    strError = "Only 5 Demographic fields are allowed in an Export to group";
                }

                if (AdhocFieldsCount > 5)
                {
                    if (strError != string.Empty)
                        strError += " and ";
                    strError += "Only 5 Adhoc fields are allowed in an Export to group";
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

            Enums.ExportType exportType = Enums.ExportType.FTP;
            if (rbDownload.Checked)
                exportType = Enums.ExportType.FTP;
            else if (rbGroupExport.Checked)
                exportType = Enums.ExportType.ECN;
            else if (rbCampaign.Checked)
                exportType = Enums.ExportType.Campaign;
            else if (rbMarketo.Checked)
                exportType = Enums.ExportType.Marketo;

            Dictionary<string, string> exportDemoFields = new Dictionary<string, string>();
            Dictionary<string, string> exportAdhocFields = new Dictionary<string, string>();

            if (dcRunID > 0 && filterCombination.Equals("Matched NotIn Selected", StringComparison.OrdinalIgnoreCase) && ViewType == Enums.ViewType.ProductView)
            {
                exportDemoFields = Utilities.GetExportFields(clientconnections, Enums.ViewType.ConsensusView, 0, null, exportType, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Demo);
                exportAdhocFields = Utilities.GetExportFields(clientconnections, Enums.ViewType.ConsensusView, 0, null, exportType, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Adhoc);
            }
            else
            {
                exportDemoFields = Utilities.GetExportFields(clientconnections, ViewType, BrandID, PubIDs, exportType, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Demo);
                exportAdhocFields = Utilities.GetExportFields(clientconnections, ViewType, BrandID, PubIDs, exportType, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Adhoc);
            }

            for (int i = 0; i < lstSelectedFields.Items.Count; i++)
            {
                if (lstSelectedFields.Items[i].Selected)
                {
                    if (exportDemoFields.Any(x => x.Key.Split('|')[0] == lstSelectedFields.Items[i].Value.ToString().Split('|')[0]))
                    {
                        lstAvailableDemoFields.Items.Add(new ListItem(lstSelectedFields.Items[i].Text.Split('(')[0].ToUpper(), lstSelectedFields.Items[i].Value.Split('|')[0] + "|" + lstSelectedFields.Items[i].Value.Split('|')[1]));
                        lstSelectedFields.Items.RemoveAt(i);
                    }
                    else if (exportAdhocFields.Any(x => x.Key.Split('|')[0] == lstSelectedFields.Items[i].Value.ToString().Split('|')[0]))
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

            if (rbGroupExport.Checked)
            {
                exportDemoFields = Utilities.GetExportFields(clientconnections, ViewType, BrandID, PubIDs, exportType, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Demo);
                exportAdhocFields = Utilities.GetExportFields(clientconnections, ViewType, BrandID, PubIDs, exportType, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Adhoc);

                int demoFieldsCount = 0;
                int AdhocFieldsCount = 0;

                for (int i = 0; i < lstSelectedFields.Items.Count; i++)
                {
                    if (exportDemoFields.Any(x => x.Key.Split('|')[0] == lstSelectedFields.Items[i].Value.ToString().Split('|')[0]))
                    {
                        demoFieldsCount = demoFieldsCount + 1;
                    }
                    else if (exportAdhocFields.Any(x => x.Key.Split('|')[0] == lstSelectedFields.Items[i].Value.ToString().Split('|')[0]))
                    {
                        AdhocFieldsCount = AdhocFieldsCount + 1;
                    }
                }

                string strError = string.Empty;

                if (demoFieldsCount > 5)
                {
                    strError = "Only 5 Demographic fields are allowed in an Export to group";
                }

                if (AdhocFieldsCount > 5)
                {
                    if (strError != string.Empty)
                        strError += " and ";
                    strError += "Only 5 Adhoc fields are allowed in an Export to group";
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

        protected void drpFolder1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(Convert.ToInt32(drpClient.SelectedItem.Value), false).CustomerID;
            drpExistingGroupName.DataSource = Groups.GetGroupsByCustomerIDFolderID(CustomerID, Convert.ToInt32(drpFolder1.SelectedItem.Value));
            drpExistingGroupName.DataBind();
            drpExistingGroupName.Items.Insert(0, new ListItem("", "0"));

            this.mdlDownloads.Show();
        }

        protected void rbExistingGroup_CheckedChanged(object sender, EventArgs e)
        {
            plNewList.Visible = false;
            plExistingList.Visible = true;

            int CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(Convert.ToInt32(drpClient.SelectedItem.Value), false).CustomerID;
            List<Folder> folders = Folder.GetByCustomerID(CustomerID);

            drpFolder1.DataSource = folders;
            drpFolder1.DataBind();
            drpFolder1.Items.Insert(0, new ListItem("", "0"));

            drpExistingGroupName.DataSource = Groups.GetGroupsByCustomerIDFolderID(CustomerID, Convert.ToInt32(drpFolder1.SelectedItem.Value));
            drpExistingGroupName.DataBind();
            drpExistingGroupName.Items.Insert(0, new ListItem("", "0"));

            this.mdlDownloads.Show();
        }

        protected void rbNewCampaign_CheckedChanged(object sender, EventArgs e)
        {
            phNewCampaign.Visible = true;
            phExistingCampaign.Visible = false;
            CampID = 0;
            CampFilterID = 0;

            this.mdlDownloads.Show();
        }

        protected void rbGroupExport_CheckedChanged(object sender, EventArgs e)
        {
            phExportFields.Visible = true;
            phDownloadCount.Visible = true;
            phPromoCode.Visible = true;
            phMarketoMapping.Visible = false;
            phGroupExport.Visible = true;
            btnExport.Text = "Export";
            btnExport.Visible = true;
            btnPreECNExportResults.Visible = true;
            btnDownload.Visible = false;
            phCampaign.Visible = false;
            phShowHeader.Visible = false;
            Reset();

            List<KMPlatform.Entity.Client> client = UserSession.CurrentUserClientGroupClients;

            client = (from c in client
                      where c.IsActive == true
                      select c).ToList();

            drpClient.DataSource = client;
            drpClient.DataBind();
            drpClient.Items.Insert(0, new ListItem("", "0"));

            this.mdlDownloads.Show();
        }

        protected void rbDownload_CheckedChanged(object sender, EventArgs e)
        {
            phExportFields.Visible = true;
            phDownloadCount.Visible = true;
            phPromoCode.Visible = true;
            phMarketoMapping.Visible = false;
            phGroupExport.Visible = false;
            btnExport.Visible = false;
            btnPreECNExportResults.Visible = false;
            btnDownload.Visible = true;
            phCampaign.Visible = false;
            if (ShowHeaderCheckBox)
            {
                phShowHeader.Visible = true;
            }

            Reset();

            this.mdlDownloads.Show();
        }

        protected void drpIsBillable_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(drpIsBillable.SelectedValue) == true)
            {
                btnDownload.OnClientClick = "return confirmPurchase();";
                plNotes.Visible = false;
            }
            else
            {
                btnDownload.OnClientClick = null;
                plNotes.Visible = true;
            }
        }

        protected void rbCampaign_CheckedChanged(object sender, EventArgs e)
        {
            phExportFields.Visible = false;
            phDownloadCount.Visible = true;
            phPromoCode.Visible = false;
            phMarketoMapping.Visible = false;
            phGroupExport.Visible = false;
            btnExport.Text = "Save";
            btnExport.Visible = true;
            btnPreECNExportResults.Visible = false;
            btnDownload.Visible = false;
            phCampaign.Visible = true;
            phShowHeader.Visible = false;
            Reset();

            if (BrandID != 0)
                drpExistingCampaign.DataSource = Campaigns.GetByBrandID(clientconnections, BrandID);
            else
                drpExistingCampaign.DataSource = Campaigns.GetNotInBrand(clientconnections);

            drpExistingCampaign.DataBind();
            drpExistingCampaign.Items.Insert(0, new ListItem("", "0"));

            this.mdlDownloads.Show();
        }

        protected void rbMarketo_CheckedChanged(object sender, EventArgs e)
        {
            phExportFields.Visible = false;
            phDownloadCount.Visible = true;
            phPromoCode.Visible = false;
            phMarketoMapping.Visible = true;
            phGroupExport.Visible = false;
            btnExport.Text = "Export";
            btnExport.Visible = true;
            btnPreECNExportResults.Visible = false;
            btnDownload.Visible = false;
            phCampaign.Visible = false;
            phShowHeader.Visible = false;
            Reset();
        }

        private void Reset()
        {
            drpClient.ClearSelection();
            plList.Visible = false;
            plMessage.Visible = false;
            rbNewGroup.Checked = false;
            plNewList.Visible = false;
            drpFolder.ClearSelection();
            txtGroupName.Text = "";
            plExistingList.Visible = false;
            rbExistingGroup.Checked = false;
            drpFolder1.ClearSelection();
            drpExistingGroupName.ClearSelection();
            cbShowHeader.Checked = false;

            drpDownloadTemplate.ClearSelection();
            hfDownloadTemplateID.Value = drpDownloadTemplate.SelectedValue;
            loadExportFields();
            txtDownloadCount.Text = txtTotalCount.Text;
            txtPromocode.Text = "";
            txtJob.Text = "";

            rbExistingCampaign.Checked = false;
            rbNewCampaign.Checked = false;
            txtFilterName.Text = "";
            phNewCampaign.Visible = false;
            phExistingCampaign.Visible = false;
            txtCampaignName.Text = "";
            rbExistingCampaign.Checked = false;

            TextBox txtMarketoBaseURL = (TextBox)Marketo.FindControl("txtMarketoBaseURL");
            TextBox txtMarketoClientID = (TextBox)Marketo.FindControl("txtMarketoClientID");
            TextBox txtMarketoClientSecret = (TextBox)Marketo.FindControl("txtMarketoClientSecret");
            TextBox txtMarketoPartition = (TextBox)Marketo.FindControl("txtMarketoPartition");
            TextBox txtMarketoNames = (TextBox)Marketo.FindControl("txtMarketoNames");
            txtMarketoBaseURL.Text = string.Empty;
            txtMarketoClientID.Text = string.Empty;
            txtMarketoClientSecret.Text = string.Empty;
            txtMarketoPartition.Text = string.Empty;
            txtMarketoNames.Text = string.Empty;

            DropDownList ddlMarketoList = (DropDownList)Marketo.FindControl("ddlMarketoList");
            ddlMarketoList.Items.Clear();
            ddlMarketoList.DataSource = null;
            ddlMarketoList.DataBind();

            TextBox txtQSName = (TextBox)Marketo.FindControl("txtQSName");
            DropDownList ddlQSValue = (DropDownList)Marketo.FindControl("ddlQSValue");
            PlaceHolder phCustomValue = (PlaceHolder)Marketo.FindControl("phCustomValue");
            txtQSName.Text = string.Empty;
            ddlQSValue.ClearSelection();
            phCustomValue.Visible = false;

            GridView gvHttpPost = (GridView)Marketo.FindControl("gvHttpPost");
            gvHttpPost.DataSource = null;
            gvHttpPost.DataBind();

            ResultsGrid.DataSource = null;
            ResultsGrid.DataBind();

            dgDataCompareResult.DataSource = null;
            dgDataCompareResult.DataBind();
            plDataCompareResult.Visible = false;
            lblDataCompareMessage.Text = string.Empty;
        }

        protected void rbNewGroup_CheckedChanged(object sender, EventArgs e)
        {
            plNewList.Visible = true;
            plExistingList.Visible = false;

            int CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(Convert.ToInt32(drpClient.SelectedItem.Value), false).CustomerID;
            List<Folder> folders = Folder.GetByCustomerID(CustomerID);

            drpFolder.DataSource = folders;
            drpFolder.DataBind();
            drpFolder.Items.Insert(0, new ListItem("", "0"));

            this.mdlDownloads.Show();
        }

        protected void rbExistingCampaign_CheckedChanged(object sender, EventArgs e)
        {
            phNewCampaign.Visible = false;
            phExistingCampaign.Visible = true;

            CampID = 0;
            CampFilterID = 0;

            this.mdlDownloads.Show();
        }

        //protected void btnViewPricing_click(object sender, EventArgs e)
        //{
        //    if (lstSelectedFields.Items.Count == 0)
        //    {
        //        DisplayError("Please select fields.");
        //        return;
        //    }

        //    List<string> StandardColumnsList = new List<string>();
        //    List<MasterGroup> MasterGroupColumnList = new List<MasterGroup>();
        //    List<MasterGroup> MasterGroupColumnDescList = new List<MasterGroup>();
        //    List<SubscriptionsExtensionMapper> SubscriptionsExtMapperValueList = new List<SubscriptionsExtensionMapper>();
        //    List<PubSubscriptionsExtensionMapper> PubSubscriptionsExtMapperValueList = new List<PubSubscriptionsExtensionMapper>();
        //    string CustomColumns = string.Empty;
        //    List<String> StandardColumns = new List<string>();
        //    DataTable dtSubscription = new DataTable();
        //    List<int> SubID = new List<int>();
        //    List<ResponseGroup> ResponseGroupIDList = new List<ResponseGroup>();
        //    List<ResponseGroup> ResponseGroupDescIDList = new List<ResponseGroup>();
        //    List<string> selectedItem = new List<string>();
        //    string strStandardColumns = string.Empty;

        //    foreach (ListItem item in lstSelectedFields.Items)
        //    {
        //        selectedItem.Add(item.Value);
        //    }

        //    if (ViewType == Enums.ViewType.ProductView)
        //    {
        //        //Get selected adhoc columns
        //        PubSubscriptionsExtMapperValueList = Utilities.GetSelectedPubSubExtMapperExportColumns(selectedItem, PubIDs.First());

        //        //Get selected responsegroup value and responsegroup desc columns
        //        Tuple<List<ResponseGroup>, List<ResponseGroup>, List<string>> rg = Utilities.GetSelectedResponseGroupStandardExportColumns(selectedItem, PubSubscriptionsExtMapperValueList, PubIDs.First(), rbGroupExport.Checked ? true : false);
        //        ResponseGroupIDList = rg.Item1;
        //        ResponseGroupDescIDList = rg.Item2;

        //        //Get selected standard columns
        //        StandardColumnsList = rg.Item3;
        //    }
        //    else
        //    {
        //        //Get selected adhoc columns
        //        StandardColumnsList = Utilities.GetSelectedStandardExportColumns(selectedItem, BrandID);

        //        //Get selected mastergroup value and mastergroup desc columns
        //        Tuple<List<MasterGroup>, List<MasterGroup>> mg = Utilities.GetSelectedMasterGroupExportColumns(selectedItem, BrandID);
        //        MasterGroupColumnList = mg.Item1;
        //        MasterGroupColumnDescList = mg.Item2;

        //        //Get selected standard columns
        //        SubscriptionsExtMapperValueList = Utilities.GetSelectedSubExtMapperExportColumns(selectedItem);
        //    }

        //    strStandardColumns = Utilities.GetStandardExportColumnFieldName(StandardColumnsList, ViewType, BrandID, rbGroupExport.Checked ? true : false);

        //    //Get custom selected columns
        //    List<string> customColumnList = Utilities.GetSelectedCustomExportColumns(selectedItem);
        //    CustomColumns = string.Join(",", customColumnList);

        //    List<int> LNth = Utilities.getNth(Int32.Parse(txtTotalCount.Text), Int32.Parse(txtDownloadCount.Text));
        //    foreach (int n in LNth)
        //    {
        //        SubID.Add(SubscriptionID.ElementAt(n));
        //    }

        //    if (dcViewID > 0)
        //    {
        //        string demoColumns = string.Empty;

        //        if (ViewType == Enums.ViewType.ProductView)
        //        {
        //            foreach (ResponseGroup r in ResponseGroupIDList)
        //            {
        //                demoColumns += demoColumns == string.Empty ? r.ResponseGroupName : "," + r.ResponseGroupName;
        //            }

        //            foreach (PubSubscriptionsExtensionMapper s in PubSubscriptionsExtMapperValueList)
        //            {
        //                demoColumns += demoColumns == string.Empty ? s.CustomField : "," + s.CustomField;
        //            }
        //        }
        //        else
        //        {
        //            foreach (MasterGroup m in MasterGroupColumnList)
        //            {
        //                demoColumns += demoColumns == string.Empty ? m.Name : "," + m.Name;
        //            }

        //            foreach (SubscriptionsExtensionMapper s in SubscriptionsExtMapperValueList)
        //            {
        //                demoColumns += demoColumns == string.Empty ? s.CustomField : "," + s.CustomField;
        //            }
        //        }

        //        List<FrameworkUAS.Entity.DataCompareDownloadCostDetail> cd = new FrameworkUAS.BusinessLogic.DataCompareDownloadCostDetail().CreateCostDetail(dcViewID, dcTypeCodeID, SubID.Count.ToString(), string.Join(",", StandardColumnsList), demoColumns, UserSession.CurrentUser.UserID);

        //        List<FrameworkUAD_Lookup.Entity.CodeType> ct = new FrameworkUAD_Lookup.BusinessLogic.CodeType().Select();

        //        var query = (from d in cd
        //                     join t in ct on d.CodeTypeId equals t.CodeTypeId
        //                     select new { d.ItemCount, d.CostPerItemClient, d.CostPerItemDetailClient, d.CostPerItemDetailThirdParty, d.CostPerItemThirdParty, d.ItemTotalCostClient, d.ItemTotalCostThirdParty, type = t.CodeTypeName}).ToList();

        //        plDataCompareResult.Visible = true;
        //        dgDataCompareResult.DataSource = query;
        //        dgDataCompareResult.DataBind();

        //        //btnViewPricing.Visible = false;
        //    }

        //    btnDownload.Visible = true;
        //}

        protected void btnExport_click(object sender, EventArgs e)
        {
            ResultsGrid.DataSource = null;
            ResultsGrid.DataBind();

            if (!Page.IsValid)
            {
                return;
            }

            if (rbDownload.Checked || rbGroupExport.Checked)
            {
                if (lstSelectedFields.Items.Count == 0)
                {
                    DisplayError("Please select fields.");
                    return;
                }

                if(rbGroupExport.Checked)
                {
                    Enums.ExportType exportType = Enums.ExportType.ECN;

                    Dictionary<string, string> exportDemoFields = new Dictionary<string, string>();
                    Dictionary<string, string> exportAdhocFields = new Dictionary<string, string>();

                    if (dcRunID > 0 && filterCombination.Equals("Matched NotIn Selected", StringComparison.OrdinalIgnoreCase) && ViewType == Enums.ViewType.ProductView)
                    {
                        exportDemoFields = Utilities.GetExportFields(clientconnections, Enums.ViewType.ConsensusView, 0, null, exportType, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Demo);
                        exportAdhocFields = Utilities.GetExportFields(clientconnections, Enums.ViewType.ConsensusView, 0, null, exportType, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Adhoc);
                    }
                    else
                    {
                        exportDemoFields = Utilities.GetExportFields(clientconnections, ViewType, BrandID, PubIDs, exportType, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Demo);
                        exportAdhocFields = Utilities.GetExportFields(clientconnections, ViewType, BrandID, PubIDs, exportType, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Adhoc);
                    }

                    int demoFieldsCount = 0;
                    int AdhocFieldsCount = 0;

                    for (int i = 0; i < lstSelectedFields.Items.Count; i++)
                    {
                        if (lstSelectedFields.Items[i].Selected)
                        {
                            if (exportDemoFields.Any(x => x.Key.Split('|')[0] == lstSelectedFields.Items[i].Value.ToString().Split('|')[0]))
                            {
                                demoFieldsCount = demoFieldsCount + 1;
                            }
                            else if (exportAdhocFields.Any(x => x.Key.Split('|')[0] == lstSelectedFields.Items[i].Value.ToString().Split('|')[0]))
                            {
                                AdhocFieldsCount = AdhocFieldsCount + 1;
                            }
                        }
                    }

                    if(demoFieldsCount > 5)
                    {
                        DisplayError("Only 5 Demographic fields are allowed in an Export to group.");
                        return;
                    }
                    else if (AdhocFieldsCount > 5)
                    {
                        DisplayError("Only 5 Adhoc fields are allowed in an Export to group.");
                        return;
                    }
                }
            }

            if (rbMarketo.Checked)
            {
                bool IsEmailExists = false;

                GridView gvHttpPost = (GridView)Marketo.FindControl("gvHttpPost");

                if (gvHttpPost.Rows.Count > 0)
                {
                    for (int i = 0; i < gvHttpPost.Rows.Count; i++)
                    {
                        Label lblParamValue = (Label)gvHttpPost.Rows[i].FindControl("lblParamValue");

                        if (string.Equals("Email", lblParamValue.Text.Split('|')[0], StringComparison.OrdinalIgnoreCase))
                        {
                            IsEmailExists = true;
                        }
                    }
                }

                if (!IsEmailExists)
                {
                    divError.Visible = true;
                    lblErrorMessage.Text = "Email required for Marketo export";
                    return;
                }
            }

            DetailsDownload();
        }

        protected void btnPreECNExportResults_click(object sender, EventArgs e)
        {
            ResultsGrid.DataSource = null;
            ResultsGrid.DataBind();

            int GroupID = 0;

            if (rbExistingGroup.Checked)
            {
                GroupID = Convert.ToInt32(drpExistingGroupName.SelectedValue);
            }
            else if (rbNewGroup.Checked)
            {
            }
            else
            {
                DisplayError("Select existing group or new group");
                mdlDownloads.Show();
                return;
            }

            DataTable dtSubscription = new DataTable();

            List<string> standardColumnList = new List<string>();
            standardColumnList.Add("s.SubscriptionID|Default");

            if (ViewType == Enums.ViewType.ProductView)
            {
                standardColumnList.Add("ps.email|Default");
                dtSubscription = Subscriber.GetProductDimensionSubscriberData(clientconnections, SubscribersQueries, standardColumnList, PubIDs, new List<string>(), new List<string>(), new List<string>(), new List<string>(), BrandID, Convert.ToInt32(txtDownloadCount.Text));
            }
            else
            {
                standardColumnList.Add("s.email|Default");

                if (SubscriptionID != null)
                {
                    List<int> SubID = new List<int>();
                    List<int> SubscriptionIDs = SubscriptionID;

                    List<int> LNth = Utilities.getNth(Int32.Parse(txtTotalCount.Text), Int32.Parse(txtDownloadCount.Text));
                    foreach (int n in LNth)
                    {
                        SubID.Add(SubscriptionIDs.ElementAt(n));
                    }

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < SubID.Count; i++)
                    {
                        sb.AppendLine("<Subscriptions SubscriptionID=\"" + SubID[i] + "\"/>");
                    }

                    dtSubscription = Subscriber.GetSubscriberData(clientconnections, new StringBuilder(), standardColumnList, new List<string>(), new List<string>(), new List<string>(), new List<string>(), BrandID, PubIDs, cbIsRecentData.Checked ? true : false, Convert.ToInt32(txtDownloadCount.Text), sb.ToString());
                }
                else
                    dtSubscription = Subscriber.GetSubscriberData(clientconnections, SubscribersQueries, standardColumnList, new List<string>(), new List<string>(), new List<string>(), new List<string>(), BrandID, PubIDs, cbIsRecentData.Checked ? true : false, Convert.ToInt32(txtDownloadCount.Text));
            }

            dtSubscription.DefaultView.Sort = "EMAIL Asc";
            dtSubscription = dtSubscription.DefaultView.ToTable();

            int CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(Convert.ToInt32(drpClient.SelectedItem.Value), false).CustomerID;

            StringBuilder xmlProfile = new StringBuilder("");
            int cnt = 0;
            Hashtable subscriptionIdList = new Hashtable(30000, (float)0.6);
            Hashtable hUpdatedRecords = new Hashtable();

            foreach (DataRow dr in dtSubscription.Rows)
            {
                if (subscriptionIdList.ContainsKey(dr["subscriptionID"].ToString()))
                {
                    cnt++;
                    continue;
                }

                subscriptionIdList.Add(dr["subscriptionID"].ToString(), 1);

                xmlProfile.Append("<Emails>");

                if (dr["EMAIL"].ToString().Trim().Length > 0)
                {
                    xmlProfile.Append("<emailaddress>" + Utilities.cleanXMLString(dr["EMAIL"].ToString()) + "</emailaddress>");
                }

                xmlProfile.Append("</Emails>");

                if ((cnt != 0) && (cnt % 1000 == 0) || (cnt == dtSubscription.Rows.Count - 1))
                {
                    DataTable dtImportedRecords = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails_PreImportResults(((KMPS.MD.MasterPages.Site)this.Page.Master).UserSession.CurrentUser, CustomerID, GroupID, "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile.ToString() + "</XML>");

                    if (dtImportedRecords.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtImportedRecords.Rows)
                        {
                            if (!hUpdatedRecords.Contains(row["Action"].ToString()))
                                hUpdatedRecords.Add(row["Action"].ToString().ToUpper(), Convert.ToInt32(row["Counts"]));
                            else
                            {
                                int eTotal = Convert.ToInt32(hUpdatedRecords[row["Action"].ToString().ToUpper()]);
                                hUpdatedRecords[row["Action"].ToString().ToUpper()] = eTotal + Convert.ToInt32(row["Counts"]);
                            }
                        }
                    }

                    xmlProfile = new StringBuilder("");
                }

                cnt++;
            }

            if (hUpdatedRecords.Count > 0)
            {
                DataTable dt = Utilities.getImportedResult(hUpdatedRecords, DateTime.Now);
                dt.Rows.RemoveAt(dt.Rows.Count - 1);

                DataView dv = dt.DefaultView;
                dv.Sort = "sortorder asc";

                ResultsGrid.DataSource = dv;
                ResultsGrid.DataBind();
                ResultsGrid.Visible = true;
                ResultsGrid.Columns[0].Visible = false;

                dvDownloads.Style.Add("height", "500px");
                dvDownloads.Style.Add("overflow", "scroll");
            }
        }
        protected void btnCloseExport_Click(object sender, EventArgs e)
        {
            ResetPopupControls();
            hideDownloadPopup.DynamicInvoke();
            this.mdlDownloads.Hide();

            IsPopupDimension = false;
            IsPopupCrossTab = false;
            IsPopupSVFilter = false;
        }

        private void ResetPopupControls()
        {
            txtDownloadCount.Text = "";
            txtTotalCount.Text = "";
            txtPromocode.Text = "";
            txtJob.Text = "";
            if (Showexporttoemailmarketing.Equals(false))
            {
                phRBExport.Visible = false;
            }
            rbGroupExport.Checked = false;
            phGroupExport.Visible = false;
            drpClient.ClearSelection();
            plList.Visible = false;
            plMessage.Visible = false;
            rbNewGroup.Checked = false;
            plNewList.Visible = false;
            drpFolder.ClearSelection();
            txtGroupName.Text = "";
            plExistingList.Visible = false;
            rbExistingGroup.Checked = false;
            drpFolder1.ClearSelection();
            drpExistingGroupName.ClearSelection();
            phRBDownload.Visible = false;
            rbDownload.Checked = false;
            phShowHeader.Visible = false;
            cbShowHeader.Checked = false;
            btnExport.Visible = false;
            btnPreECNExportResults.Visible = false;
            btnDownload.Visible = false;
            ResultsGrid.DataSource = null;
            ResultsGrid.DataBind();
            lblResults.Text = "";
            lblResults.Visible = false;
            dvDownloads.Style.Remove("height");
            dvDownloads.Style.Remove("overflow");
            phCampaign.Visible = false;
            txtFilterName.Text = "";
            phNewCampaign.Visible = false;
            phExistingCampaign.Visible = false;
            txtCampaignName.Text = "";
            rbExistingCampaign.Checked = false;
            rbNewCampaign.Checked = false;
            if (Showsavetocampaign.Equals(false))
            {
                phRBCampaign.Visible = false;
            }
            rbCampaign.Checked = false;
            drpExistingCampaign.ClearSelection();
            hfDownloadTemplateID.Value = "0";
            rbMarketo.Checked = false;
            phMarketoMapping.Visible = false;

            TextBox txtMarketoBaseURL = (TextBox)Marketo.FindControl("txtMarketoBaseURL");
            TextBox txtMarketoClientID = (TextBox)Marketo.FindControl("txtMarketoClientID");
            TextBox txtMarketoClientSecret = (TextBox)Marketo.FindControl("txtMarketoClientSecret");
            TextBox txtMarketoPartition = (TextBox)Marketo.FindControl("txtMarketoPartition");
            txtMarketoBaseURL.Text = string.Empty;
            txtMarketoClientID.Text = string.Empty;
            txtMarketoClientSecret.Text = string.Empty;
            txtMarketoPartition.Text = string.Empty;

            DropDownList ddlMarketoList = (DropDownList)Marketo.FindControl("ddlMarketoList");
            ddlMarketoList.DataSource = null;
            ddlMarketoList.DataBind();

            GridView gvHttpPost = (GridView)Marketo.FindControl("gvHttpPost");
            gvHttpPost.DataSource = null;
            gvHttpPost.DataBind();

            dgDataCompareResult.DataSource = null;
            dgDataCompareResult.DataBind();
            plDataCompareResult.Visible = false;
            lblDataCompareMessage.Text = string.Empty;
            drpIsBillable.SelectedIndex = -1;
            btnDownload.OnClientClick = null;
            plNotes.Visible = false;
            txtNotes.Text = string.Empty;
        }

        protected void drpClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            plList.Visible = false;
            plMessage.Visible = false;
            rbNewGroup.Checked = false;
            plNewList.Visible = false;
            drpFolder.ClearSelection();
            txtGroupName.Text = "";
            plExistingList.Visible = false;
            rbExistingGroup.Checked = false;
            drpFolder1.ClearSelection();
            drpExistingGroupName.ClearSelection();

            if (Convert.ToInt32(drpClient.SelectedItem.Value) > 0)
            {

                if (UserSession.ClientID != Convert.ToInt32(drpClient.SelectedItem.Value) && !KM.Platform.User.IsChannelAdministrator(UserSession.CurrentUser))
                {
                    if (!KMPlatform.BusinessLogic.ServiceFeature.HasAccess(UserSession.UserID, Convert.ToInt32(drpClient.SelectedItem.Value), KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.ExternalImport))
                    {
                        plList.Visible = false;
                        plMessage.Visible = true;
                        lblMessage.Text = "You do not have permission to export to customer selected.";
                    }
                    else
                    {
                        plList.Visible = true;

                        if (!KMPlatform.BusinessLogic.ServiceFeature.HasAccess(UserSession.UserID, Convert.ToInt32(drpClient.SelectedItem.Value), KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.Edit))
                            rbNewGroup.Visible = false;
                        else
                            rbNewGroup.Visible = true;



                        if (!KMPlatform.BusinessLogic.ServiceFeature.HasAccess(UserSession.UserID, Convert.ToInt32(drpClient.SelectedItem.Value), KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.View))
                            rbExistingGroup.Visible = false;
                        else
                            rbExistingGroup.Visible = true;
                    }
                }
                else
                {
                    plList.Visible = true;

                    if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.Edit))
                        rbNewGroup.Visible = false;
                    else
                        rbNewGroup.Visible = true;

                    if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.View))
                        rbExistingGroup.Visible = false;
                    else
                        rbExistingGroup.Visible = true;
                }
            }
            else
            {
                plList.Visible = false;
            }

            this.mdlDownloads.Show();
        }

        private void DetailsDownload()
        {
            if (Session["SubscribersQueries"] == null && Session["SubscriptionID"] == null)
            {
                DelMethod.DynamicInvoke();
            }

            List<string> StandardColumnsList = new List<string>();
            var stdColumnList = new List<string>();
            List<string> MasterGroupColumnList = new List<string>();
            List<string> MasterGroupColumnDescList = new List<string>();
            List<string> SubscriptionsExtMapperValueList = new List<string>();
            List<string> PubSubscriptionsExtMapperValueList = new List<string>();
            List<string> StandardColumns = new List<string>();
            DataTable dtSubscription = new DataTable();
            List<int> SubID = new List<int>();
            List<string> ResponseGroupIDList = new List<string>();
            List<string> ResponseGroupDescIDList = new List<string>();
            List<string> selectedItem = new List<string>();
            List<string> customColumnList = new List<string>();

            if (rbMarketo.Checked)
            {
                GridView gvHttpPost = (GridView)Marketo.FindControl("gvHttpPost");
                List<string> marketoExportFields = new List<string>();

                for (int i = 0; i < gvHttpPost.Rows.Count; i++)
                {
                    if (gvHttpPost.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        Label lblHttpPostParamsID = (Label)gvHttpPost.Rows[i].FindControl("lblHttpPostParamsID");
                        Label lblParamName = (Label)gvHttpPost.Rows[i].FindControl("lblParamName");
                        Label lblParamValue = (Label)gvHttpPost.Rows[i].FindControl("lblParamValue");
                        Label lblCustomValue = (Label)gvHttpPost.Rows[i].FindControl("lblCustomValue");
                        Label lblParamDisplayName = (Label)gvHttpPost.Rows[i].FindControl("lblParamDisplayName");

                        if (lblParamValue.Text.ToUpper() != "CUSTOMVALUE")
                            marketoExportFields.Add(lblParamValue.Text);
                    }
                }

                if (ViewType == Enums.ViewType.ProductView)
                {
                    //Get selected adhoc columns
                    PubSubscriptionsExtMapperValueList = Utilities.GetSelectedPubSubExtMapperExportColumns(clientconnections, marketoExportFields, PubIDs.First());

                    //Get selected responsegroup value and responsegroup desc columns
                    Tuple<List<string>, List<string>, List<string>> rg = Utilities.GetSelectedResponseGroupStandardExportColumns(clientconnections, marketoExportFields, PubIDs.First(), false);
                    ResponseGroupIDList = rg.Item1;
                    ResponseGroupDescIDList = rg.Item2;

                    //Get selected standard columns
                    StandardColumnsList = rg.Item3;
                }
                else
                {
                    //Get selected adhoc columns
                    SubscriptionsExtMapperValueList = Utilities.GetSelectedSubExtMapperExportColumns(clientconnections, marketoExportFields);

                    //Get selected mastergroup value and mastergroup desc columns
                    Tuple<List<string>, List<string>> mg = Utilities.GetSelectedMasterGroupExportColumns(clientconnections, marketoExportFields, BrandID);
                    MasterGroupColumnList = mg.Item1;
                    MasterGroupColumnDescList = mg.Item2;

                    //Get selected standard columns
                    StandardColumnsList = Utilities.GetSelectedStandardExportColumns(clientconnections, marketoExportFields, BrandID);
                }

                stdColumnList = Utilities.GetStandardExportColumnFieldName(StandardColumnsList, ViewType, BrandID, false).ToList();

                if(SubscriptionID != null)
                {
                    List<int> LNth = Utilities.getNth(Int32.Parse(txtTotalCount.Text), Int32.Parse(txtDownloadCount.Text));
                    List<int> SubscriptionIDs = SubscriptionID;

                    foreach (int n in LNth)
                    {
                        SubID.Add(SubscriptionIDs.ElementAt(n));
                    }
                }
            }
            else
            {
                if (rbDownload.Checked || rbGroupExport.Checked)
                {
                    foreach (ListItem item in lstSelectedFields.Items)
                    {
                        selectedItem.Add(item.Value);
                    }

                    if (ViewType == Enums.ViewType.ProductView)
                    {
                        if (dcRunID > 0 && filterCombination.Equals("Matched NotIn Selected", StringComparison.OrdinalIgnoreCase))
                        {
                            //Get selected adhoc columns
                            SubscriptionsExtMapperValueList = Utilities.GetSelectedSubExtMapperExportColumns(clientconnections, selectedItem);

                            //Get selected mastergroup value and mastergroup desc columns
                            Tuple<List<string>, List<string>> mg = Utilities.GetSelectedMasterGroupExportColumns(clientconnections, selectedItem, BrandID);
                            MasterGroupColumnList = mg.Item1;
                            MasterGroupColumnDescList = mg.Item2;

                            //Get selected standard columns
                            StandardColumnsList = Utilities.GetSelectedStandardExportColumns(clientconnections, selectedItem, BrandID);
                        }
                        else
                        {
                            //Get selected adhoc columns
                            PubSubscriptionsExtMapperValueList = Utilities.GetSelectedPubSubExtMapperExportColumns(clientconnections, selectedItem, PubIDs.First());

                            //Get selected responsegroup value and responsegroup desc columns
                            Tuple<List<string>, List<string>, List<string>> rg = Utilities.GetSelectedResponseGroupStandardExportColumns(clientconnections, selectedItem, PubIDs.First(), rbGroupExport.Checked ? true : false);
                            ResponseGroupIDList = rg.Item1;
                            ResponseGroupDescIDList = rg.Item2;

                            //Get selected standard columns
                            StandardColumnsList = rg.Item3;
                        }
                    }
                    else
                    {
                        //Get selected adhoc columns
                        SubscriptionsExtMapperValueList = Utilities.GetSelectedSubExtMapperExportColumns(clientconnections, selectedItem);

                        //Get selected mastergroup value and mastergroup desc columns
                        Tuple<List<string>, List<string>> mg = Utilities.GetSelectedMasterGroupExportColumns(clientconnections, selectedItem, BrandID);
                        MasterGroupColumnList = mg.Item1;
                        MasterGroupColumnDescList = mg.Item2;

                        //Get selected standard columns
                        StandardColumnsList = Utilities.GetSelectedStandardExportColumns(clientconnections, selectedItem, BrandID);
                    }

                    if (dcRunID > 0 && filterCombination.Equals("Matched NotIn Selected", StringComparison.OrdinalIgnoreCase) && ViewType == Enums.ViewType.ProductView)
                        stdColumnList = Utilities.GetStandardExportColumnFieldName(StandardColumnsList, Enums.ViewType.ConsensusView, BrandID, rbGroupExport.Checked ? true : false).ToList();
                    else
                        stdColumnList = Utilities.GetStandardExportColumnFieldName(StandardColumnsList, ViewType, BrandID, rbGroupExport.Checked ? true : false).ToList();

                    //Get custom selected columns
                    customColumnList = Utilities.GetSelectedCustomExportColumns(selectedItem);
                }

                if (SubscriptionID != null)
                {
                    List<int> LNth = Utilities.getNth(Int32.Parse(txtTotalCount.Text), Int32.Parse(txtDownloadCount.Text));
                    List<int> SubscriptionIDs = SubscriptionID;

                    foreach (int n in LNth)
                    {
                        SubID.Add(SubscriptionIDs.ElementAt(n));
                    }
                }
            }

            if (rbDownload.Checked || rbGroupExport.Checked || rbMarketo.Checked)
            {
                if (ViewType == Enums.ViewType.ProductView)
                {
                    if (dcRunID > 0 && filterCombination.Equals("Matched NotIn Selected", StringComparison.OrdinalIgnoreCase))
                        dtSubscription = Subscriber.GetSubscriberData(clientconnections, SubscribersQueries, stdColumnList, MasterGroupColumnList, MasterGroupColumnDescList, SubscriptionsExtMapperValueList, customColumnList, BrandID, PubIDs, cbIsRecentData.Checked ? true : false, Convert.ToInt32(txtDownloadCount.Text));
                    else
                        dtSubscription = Subscriber.GetProductDimensionSubscriberData(clientconnections, SubscribersQueries, stdColumnList, PubIDs, ResponseGroupIDList, ResponseGroupDescIDList, PubSubscriptionsExtMapperValueList, customColumnList, BrandID, Convert.ToInt32(txtDownloadCount.Text));
                }
                else
                {
                    if (SubscriptionID != null)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < SubID.Count; i++)
                        {
                            sb.AppendLine("<Subscriptions SubscriptionID=\"" + SubID[i] + "\"/>");
                        }

                        dtSubscription = Subscriber.GetSubscriberData(clientconnections, new StringBuilder(), stdColumnList, MasterGroupColumnList, MasterGroupColumnDescList, SubscriptionsExtMapperValueList, customColumnList, BrandID, PubIDs, cbIsRecentData.Checked ? true : false, Convert.ToInt32(txtDownloadCount.Text), sb.ToString());
                    }
                    else
                        dtSubscription = Subscriber.GetSubscriberData(clientconnections, SubscribersQueries, stdColumnList, MasterGroupColumnList, MasterGroupColumnDescList, SubscriptionsExtMapperValueList, customColumnList, BrandID, PubIDs, cbIsRecentData.Checked ? true : false, Convert.ToInt32(txtDownloadCount.Text));
                }
            }

            if (rbGroupExport.Checked)
            {
                if (KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADExport, KMPlatform.Enums.Access.ExportToGroup))
                {
                    #region Export to Group Logic

                    int GroupID = 0;
                    bool mastersupression = false;
                    int folderID = 0;
                    int CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(Convert.ToInt32(drpClient.SelectedItem.Value), false).CustomerID;

                    if (rbExistingGroup.Checked)
                    {
                        GroupID = Convert.ToInt32(drpExistingGroupName.SelectedValue);
                        mastersupression = Groups.GetGroupByID(GroupID).MasterSupression;
                        folderID = Convert.ToInt32(drpFolder1.SelectedValue);
                    }
                    else if (rbNewGroup.Checked)
                    {
                        folderID = Convert.ToInt32(drpFolder.SelectedValue);

                        string gname = DataFunctions.CleanString(txtGroupName.Text);

                        if (Groups.ExistsByGroupNameByCustomerID(gname, CustomerID))
                        {
                            DisplayError("<font color='#000000'>\"" + gname + "\"</font> already exists. Please enter a different name.");
                            mdlDownloads.Show();
                            return;
                        }
                        else
                        {
                            GroupID = Utilities.InsertGroup(gname, CustomerID, folderID);
                        }
                    }
                    else
                    {
                        DisplayError("Select existing group or new group");
                        mdlDownloads.Show();
                        return;
                    }

                    try
                    {
                        //if group is mastersupression then only profile field exported
                        if (mastersupression)
                        {
                            string[] cols = { "FIRSTNAME", "LASTNAME", "COMPANY", "TITLE", "ADDRESS", "ADDRESS2", "ADDRESS3", "CITY", "STATE", "ZIP", "COUNTRY", "PHONE", "FAX", "MOBILE" };

                            List<DataColumn> columnsToDelete = new List<DataColumn>();
                            foreach (DataColumn column in dtSubscription.Columns)
                            {
                                if (!cols.Contains(column.ColumnName.ToUpper()))
                                    columnsToDelete.Add(column);
                            }

                            foreach (DataColumn col in columnsToDelete)

                                dtSubscription.Columns.Remove(col);
                        }

                        // Non UDF fields 
                        string[] NotUDFcols = { "SUBSCRIPTIONID", "EMAIL", "FIRSTNAME", "LASTNAME", "COMPANY", "TITLE", "ADDRESS", "ADDRESS2", "ADDRESS3", "CITY", "STATE", "ZIP", "COUNTRY", "PHONE", "FAX", "MOBILE" };

                        List<ExportFields> ExportFields = new List<ExportFields>();

                        foreach (DataColumn column in dtSubscription.Columns)
                        {
                            if (NotUDFcols.Contains(column.ColumnName.ToUpper()))
                                ExportFields.Add(new ExportFields(column.ColumnName.ToUpper(), "", false, 0));
                            else
                                ExportFields.Add(new ExportFields(column.ColumnName.ToUpper(), "", true, 0));
                        }

                        Hashtable hUpdatedRecords = new Hashtable();
                        DateTime startDateTime = DateTime.Now;

                        hUpdatedRecords = Utilities.ExportToECN(GroupID, txtGroupName.Text, CustomerID, folderID, txtPromocode.Text, txtJob.Text, ExportFields, dtSubscription, ((KMPS.MD.MasterPages.Site)this.Page.Master).LoggedInUser, Enums.GroupExportSource.UADManualExport);

                        if (hUpdatedRecords.Count > 0)
                        {
                            DataTable dt = Utilities.getImportedResult(hUpdatedRecords, startDateTime);
                            DataView dv = dt.DefaultView;
                            dv.Sort = "sortorder asc";

                            ResultsGrid.DataSource = dv;
                            ResultsGrid.DataBind();
                            ResultsGrid.Visible = true;
                            ResultsGrid.Columns[0].Visible = false;

                            dvDownloads.Style.Add("height", "500px");
                            dvDownloads.Style.Add("overflow", "scroll");
                        }
                    }
                    catch (Exception ex)
                    {
                        Utilities.Log_Error(Request.RawUrl.ToString(), "DetailsDownload - group export", ex);
                        DisplayError("ERROR - " + ex.Message);
                    }

                    mdlDownloads.Show();
                    return;
                    #endregion
                }
            }
            else if (rbDownload.Checked)
            {
                #region Download to file Logic

                if (KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADExport, KMPlatform.Enums.Access.Download))
                {
                    List<dynamic> fefName = Utilities.getListboxSelectedExportFields(lstSelectedFields);

                    string[] columnsOrder = new string[fefName.Count + 1];
                    int i = 0;

                    columnsOrder[0] = "SubscriptionID";

                    foreach (dynamic e in fefName)
                    {
                        i++;

                        if (ViewType == Enums.ViewType.ProductView)
                        {
                            var displayName = (string)e.GetType().GetProperty("text").GetValue(e, null);
                            if (dcRunID > 0 && filterCombination.Equals("Matched NotIn Selected", StringComparison.OrdinalIgnoreCase))
                            {
                                columnsOrder[i] =
                                    FieldMapper.GetColumnOrderByDefaultExportFieldDisplayName(displayName);
                            }
                            else
                            {
                                columnsOrder[i] =
                                    FieldMapper.GetColumnOrderByProductExportFieldDisplayName(displayName);
                            }
                        }
                        else
                        {
                            switch ((string)e.GetType().GetProperty("text").GetValue(e, null))
                            {
                                case "FNAME":
                                    columnsOrder[i] = "FirstName";
                                    break;
                                case "LNAME":
                                    columnsOrder[i] = "LastName";
                                    break;
                                case "ISLATLONVALID":
                                    columnsOrder[i] = "GeoLocated";
                                    break;
                                default:
                                    columnsOrder[i] = (string)e.GetType().GetProperty("text").GetValue(e, null);
                                    break;
                            }
                        }
                    }

                    for (int j = 0; j < columnsOrder.Length; j++)
                    {
                        dtSubscription.Columns[columnsOrder[j].Split('(')[0]].SetOrdinal(j);
                    }

                    dtSubscription = (DataTable)ProfileFieldMask.MaskData(clientconnections, dtSubscription, UserSession.CurrentUser);

                    //Save DataCompare view details

                    int dcViewID = 0;

                    FrameworkUAD_Lookup.Enums.DataCompareType typeCodeName = FrameworkUAD_Lookup.Enums.DataCompareType.Match;

                    if (dcTypeCodeID > 0)
                        typeCodeName = (FrameworkUAD_Lookup.Enums.DataCompareType)Enum.Parse(typeof(FrameworkUAD_Lookup.Enums.DataCompareType), new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeId(dcTypeCodeID).CodeName);

                    if (dcRunID > 0)
                    {
                        int targetCodeID = dcTargetCodeID;
                        int? targetID = null;
                        Filter filter = new Filter();

                        if (ViewType == Enums.ViewType.ProductView)
                            targetID = PubIDs.First();
                        else
                            targetID = BrandID > 0 ? (int?)BrandID : null;

                        //Calculating Total UAD Records count

                        filter.ViewType = ViewType;

                        if (BrandID > 0)
                        {
                            filter.BrandID = BrandID;
                            filter.Fields.Add(new Field("Brand", BrandID.ToString(), "", "", Enums.FiltersType.Brand, "BRAND"));
                        }

                        if (ViewType == Enums.ViewType.ProductView)
                        {
                            filter.PubID = PubIDs.First();
                            filter.Fields.Add(new Field("Product", PubIDs.First().ToString(), "", "", Enums.FiltersType.Product, "PRODUCT"));
                        }

                        filter.Execute(clientconnections, "");

                        List<FrameworkUAS.Entity.DataCompareView> datacv = new FrameworkUAS.BusinessLogic.DataCompareView().SelectForRun(dcRunID);

                        int tcID = Code.GetDataCompareTarget().Find(x => x.CodeName == Enums.DataCompareViewType.Consensus.ToString()).CodeID;

                        int paymentStatusPendingID = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status, FrameworkUAD_Lookup.Enums.PaymentStatus.Pending.ToString()).CodeId;
                        int paymentStatusNon_BilledID = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status, FrameworkUAD_Lookup.Enums.PaymentStatus.Non_Billed.ToString()).CodeId;

                        if (datacv.Exists(y => y.DcTargetCodeId == tcID && (y.PaymentStatusId == paymentStatusPendingID || y.PaymentStatusId == paymentStatusNon_BilledID)))
                        {
                            int id = datacv.Find(y => y.DcTargetCodeId == tcID && (y.PaymentStatusId == paymentStatusPendingID || y.PaymentStatusId == paymentStatusNon_BilledID)).DcViewId;
                            new FrameworkUAS.BusinessLogic.DataCompareView().Delete(id);
                            dcViewID = saveDataCompareView(targetCodeID, targetID, dcTypeCodeID, typeCodeName, filter.Count);
                        }
                        else
                        {
                            if (!datacv.Exists(u => u.DcTargetCodeId == targetCodeID && u.DcTargetIdUad == targetID && u.DcTypeCodeId == dcTypeCodeID))
                            {
                                dcViewID = saveDataCompareView(targetCodeID, targetID, dcTypeCodeID, typeCodeName, filter.Count);
                            }
                            else
                            {
                                if (plKmStaff.Visible)
                                {
                                    FrameworkUAS.Entity.DataCompareView dcv = datacv.Find(u => u.DcTargetCodeId == targetCodeID && u.DcTargetIdUad == targetID && u.DcTypeCodeId == dcTypeCodeID);

                                    if (!dcv.IsBillable && (plKmStaff.Visible && Convert.ToBoolean(drpIsBillable.SelectedValue)) || !Convert.ToBoolean(drpIsBillable.SelectedValue))
                                    {
                                        dcViewID = dcv.DcViewId;
                                        dcv.UadNetCount = filter.Count;
                                        dcv.MatchedCount = matchedRecordsCount;
                                        dcv.NoDataCount = nonMatchedRecordsCount;
                                        dcv.Cost = new FrameworkUAS.BusinessLogic.DataCompareView().GetDataCompareCost(UserSession.CurrentUser.UserID, UserSession.ClientID, (filter.Count + TotalFileRecords), typeCodeName, FrameworkUAD_Lookup.Enums.DataCompareCost.MergePurge);

                                        if (plNotes.Visible)
                                            dcv.Notes = txtNotes.Text;

                                        dcv.IsBillable = plKmStaff.Visible ? (Convert.ToBoolean(drpIsBillable.SelectedValue)) : true;

                                        if (plKmStaff.Visible && !Convert.ToBoolean(drpIsBillable.SelectedValue))
                                            dcv.PaymentStatusId = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status, FrameworkUAD_Lookup.Enums.PaymentStatus.Non_Billed.ToString()).CodeId;
                                        else
                                            dcv.PaymentStatusId = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status, FrameworkUAD_Lookup.Enums.PaymentStatus.Unpaid.ToString()).CodeId;

                                        dcv.DateUpdated = DateTime.Now;
                                        dcv.UpdatedByUserID = UserSession.UserID;

                                        new FrameworkUAS.BusinessLogic.DataCompareView().Save(dcv);
                                    }
                                    else
                                    {
                                        dcViewID = dcv.DcViewId;
                                    }
                                }
                            }
                        }
                    }

                    //Save DataCompare Details

                    List<MasterGroup> masterGroupList = new List<MasterGroup>();
                    string outfilepath = string.Empty;

                    if (dcViewID > 0)
                    {
                        string demoColumns = string.Empty;

                        if (ViewType == Enums.ViewType.ProductView)
                        {
                            List<ResponseGroup> responseGroupList = ResponseGroup.GetActiveByPubID(clientconnections, PubIDs.First());
                            foreach (string s in ResponseGroupIDList)
                            {
                                demoColumns += demoColumns == string.Empty ? responseGroupList.Find(x => x.ResponseGroupID == Convert.ToInt32(s.Split('|')[0])).ResponseGroupName : "," + responseGroupList.Find(x => x.ResponseGroupID == Convert.ToInt32(s.Split('|')[0])).ResponseGroupName;
                            }

                            foreach (string s in PubSubscriptionsExtMapperValueList)
                            {
                                demoColumns += demoColumns == string.Empty ? s.Split('|')[0] : "," + s.Split('|')[0];
                            }
                        }
                        else
                        {
                            if (BrandID > 0)
                                masterGroupList = MasterGroup.GetActiveByBrandID(clientconnections, BrandID);
                            else
                                masterGroupList = MasterGroup.GetActiveMasterGroupsSorted(clientconnections);

                            foreach (string s in MasterGroupColumnList)
                            {
                                demoColumns += demoColumns == string.Empty ? masterGroupList.Find(x => x.ColumnReference == s.Split('|')[0]).Name : "," + masterGroupList.Find(x => x.ColumnReference == s.Split('|')[0]).Name;
                            }

                            foreach (string s in SubscriptionsExtMapperValueList)
                            {
                                demoColumns += demoColumns == string.Empty ? s.Split('|')[0] : "," + s.Split('|')[0];
                            }
                        }

                        string filename = string.Empty;
                        Guid g = System.Guid.NewGuid();
                        filename = "filter_report_" + g.ToString() + ".tsv";
                        outfilepath = Server.MapPath("../downloads/datacompare/") + UserSession.ClientID + "/" + filename;
                        string path = Server.MapPath("../downloads/datacompare/") + UserSession.ClientID + "/";

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        List<string> sColumnsList = new List<string>();
                        sColumnsList.AddRange(StandardColumnsList.Select(x=>x.Split('|')[0]));

                        List<FrameworkUAS.Entity.DataCompareDownloadCostDetail> cd = new FrameworkUAS.BusinessLogic.DataCompareDownloadCostDetail().CreateCostDetail(dcViewID, dcTypeCodeID, txtTotalCount.Text, string.Join(",", sColumnsList), demoColumns, UserSession.CurrentUser.UserID);

                        FrameworkUAS.Entity.DataCompareDownload dcd = new FrameworkUAS.Entity.DataCompareDownload();

                        dcd.DcViewId = dcViewID;
                        dcd.WhereClause = filterCombination;
                        dcd.DcTypeCodeId = dcTypeCodeID;
                        dcd.ProfileCount = dtSubscription.Rows.Count;
                        dcd.TotalBilledCost = new FrameworkUAS.BusinessLogic.DataCompareView().GetDataCompareCost(UserSession.CurrentUser.UserID, UserSession.ClientID, dtSubscription.Rows.Count, typeCodeName, FrameworkUAD_Lookup.Enums.DataCompareCost.Download);
                        dcd.IsPurchased = plKmStaff.Visible ? (Convert.ToBoolean(drpIsBillable.SelectedValue)) : true;
                        dcd.PurchasedByUserId = UserSession.UserID;
                        dcd.PurchasedDate = DateTime.Now;
                        dcd.DownloadFileName = filename;

                        int dcDownloadID = new FrameworkUAS.BusinessLogic.DataCompareDownload().Save(dcd);

                        XDocument xDoc = new XDocument(
                             new XElement("SubcriptionIDs", from sub in dtSubscription.AsEnumerable()
                                                            select
                                                                  new XElement("SubcriptionID", sub["SubscriptionID"])
                            ));

                        new FrameworkUAS.BusinessLogic.DataCompareDownloadDetail().Save(dcDownloadID, xDoc.ToString());

                        //save filters
                        if (!(filterCombination.Equals("Matched", StringComparison.OrdinalIgnoreCase) || filterCombination.Equals("Matched NotIn Selected", StringComparison.OrdinalIgnoreCase)))
                        {
                            Filters filters = (this.Page as dynamic).FilterCollection;

                            foreach (Filter f in filters)
                            {
                                FrameworkUAS.Entity.DataCompareDownloadFilterGroup fg = new FrameworkUAS.Entity.DataCompareDownloadFilterGroup();
                                fg.DcDownloadId = dcDownloadID;
                                int dcFilterGroupID = new FrameworkUAS.BusinessLogic.DataCompareDownloadFilterGroup().Save(fg);

                                foreach (Field field in f.Fields)
                                {
                                    if (!field.Name.Equals("DataCompare", StringComparison.OrdinalIgnoreCase))
                                    {
                                        FrameworkUAS.Entity.DataCompareDownloadFilterDetail fd = new FrameworkUAS.Entity.DataCompareDownloadFilterDetail();

                                        fd.DcFilterGroupId = dcFilterGroupID;
                                        fd.FilterType = (int)field.FilterType;
                                        fd.Group = field.Group;
                                        fd.Name = field.Name;
                                        fd.Values = field.Values;
                                        fd.SearchCondition = field.SearchCondition;

                                        new FrameworkUAS.BusinessLogic.DataCompareDownloadFilterDetail().Save(fd);
                                    }
                                }
                            }
                        }

                        //Save download fields

                        List<Code> c = Code.GetUADFieldType();

                        foreach (string s in StandardColumnsList)
                        {
                            FrameworkUAS.Entity.DataCompareDownloadField f = new FrameworkUAS.Entity.DataCompareDownloadField();
                            f.DcDownloadId = dcDownloadID;
                            f.DcDownloadFieldCodeId = c.Find(x => x.CodeName == Enums.UADFieldType.Profile.ToString()).CodeID;
                            f.ColumnName = s.Split('|')[0];
                            f.IsDescription = false;
                            new FrameworkUAS.BusinessLogic.DataCompareDownloadField().Save(f);
                        }

                        foreach (string s in customColumnList)
                        {
                            FrameworkUAS.Entity.DataCompareDownloadField f = new FrameworkUAS.Entity.DataCompareDownloadField();
                            f.DcDownloadId = dcDownloadID;
                            f.DcDownloadFieldCodeId = c.Find(x => x.CodeName == Enums.UADFieldType.Custom.ToString()).CodeID;
                            f.ColumnName = s.Split('|')[0];
                            f.IsDescription = false;
                            new FrameworkUAS.BusinessLogic.DataCompareDownloadField().Save(f);
                        }

                        if (ViewType == Enums.ViewType.ProductView)
                        {
                            foreach (string s in ResponseGroupIDList)
                            {
                                FrameworkUAS.Entity.DataCompareDownloadField f = new FrameworkUAS.Entity.DataCompareDownloadField();
                                f.DcDownloadId = dcDownloadID;
                                f.DcDownloadFieldCodeId = c.Find(x => x.CodeName == Enums.UADFieldType.Dimension.ToString()).CodeID;
                                f.ColumnID = Convert.ToInt32(s.Split('|')[0]);
                                f.IsDescription = false;
                                new FrameworkUAS.BusinessLogic.DataCompareDownloadField().Save(f);
                            }

                            foreach (string s in ResponseGroupDescIDList)
                            {
                                FrameworkUAS.Entity.DataCompareDownloadField f = new FrameworkUAS.Entity.DataCompareDownloadField();
                                f.DcDownloadId = dcDownloadID;
                                f.DcDownloadFieldCodeId = c.Find(x => x.CodeName == Enums.UADFieldType.Dimension.ToString()).CodeID;
                                f.ColumnID = Convert.ToInt32(s.Split('|')[0]);
                                f.IsDescription = true;
                                new FrameworkUAS.BusinessLogic.DataCompareDownloadField().Save(f);
                            }

                            foreach (string s in PubSubscriptionsExtMapperValueList)
                            {
                                List<PubSubscriptionsExtensionMapper> PubSubExtensionMapperList = PubSubscriptionsExtensionMapper.GetActiveByPubID(clientconnections, PubIDs.First());

                                FrameworkUAS.Entity.DataCompareDownloadField f = new FrameworkUAS.Entity.DataCompareDownloadField();
                                f.DcDownloadId = dcDownloadID;
                                f.DcDownloadFieldCodeId = c.Find(x => x.CodeName == Enums.UADFieldType.Adhoc.ToString()).CodeID;
                                f.ColumnID = PubSubExtensionMapperList.Find(x => x.CustomField.ToString() == s.Split('|')[0]).PubSubscriptionsExtensionMapperId;
                                f.IsDescription = false;
                                new FrameworkUAS.BusinessLogic.DataCompareDownloadField().Save(f);
                            }
                        }
                        else
                        {
                            foreach (string s in MasterGroupColumnList)
                            {
                                FrameworkUAS.Entity.DataCompareDownloadField f = new FrameworkUAS.Entity.DataCompareDownloadField();
                                f.DcDownloadId = dcDownloadID;
                                f.DcDownloadFieldCodeId = c.Find(x => x.CodeName == Enums.UADFieldType.Dimension.ToString()).CodeID;
                                f.ColumnID = masterGroupList.Find(x => x.ColumnReference == s.Split('|')[0]).MasterGroupID;
                                f.IsDescription = false;
                                new FrameworkUAS.BusinessLogic.DataCompareDownloadField().Save(f);
                            }

                            foreach (string s in MasterGroupColumnDescList)
                            {
                                FrameworkUAS.Entity.DataCompareDownloadField f = new FrameworkUAS.Entity.DataCompareDownloadField();
                                f.DcDownloadId = dcDownloadID;
                                f.DcDownloadFieldCodeId = c.Find(x => x.CodeName == Enums.UADFieldType.Dimension.ToString()).CodeID;
                                f.ColumnID = masterGroupList.Find(x => x.ColumnReference == s.Split('|')[0]).MasterGroupID;
                                f.IsDescription = true;
                                new FrameworkUAS.BusinessLogic.DataCompareDownloadField().Save(f);
                            }

                            foreach (string s in SubscriptionsExtMapperValueList)
                            {
                                List<SubscriptionsExtensionMapper> SubExtensionMapperList = SubscriptionsExtensionMapper.GetActive(clientconnections);

                                FrameworkUAS.Entity.DataCompareDownloadField f = new FrameworkUAS.Entity.DataCompareDownloadField();
                                f.DcDownloadId = dcDownloadID;
                                f.DcDownloadFieldCodeId = c.Find(x => x.CodeName == Enums.UADFieldType.Adhoc.ToString()).CodeID;
                                f.ColumnID = SubExtensionMapperList.Find(x => x.CustomField.ToString() == s.Split('|')[0]).SubscriptionsExtensionMapperId;
                                f.IsDescription = false;
                                new FrameworkUAS.BusinessLogic.DataCompareDownloadField().Save(f);
                            }
                        }
                    }
                    else
                    {
                        outfilepath = Server.MapPath("../main/temp/") + System.Guid.NewGuid().ToString().Substring(0, 5) + ".tsv";
                    }

                    if (UserSession.CurrentUser.IsKMStaff)
                    {
                        if (Convert.ToBoolean(drpIsBillable.SelectedValue))
                        {
                            drpIsBillable.Enabled = false;
                        }
                    }

                    DataColumn newColumn = new DataColumn("PromoCode", typeof(System.String));
                    newColumn.DefaultValue = txtPromocode.Text;
                    dtSubscription.Columns.Add(newColumn);

                    if (!cbShowHeader.Checked)
                    {
                        HeaderText = string.Empty;
                    }

                    Utilities.Download(dtSubscription, outfilepath, HeaderText, Convert.ToInt32(txtTotalCount.Text), Convert.ToInt32(txtDownloadCount.Text));
                }

                #endregion
            }
            else if (rbCampaign.Checked)
            {
                if (KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADExport, KMPlatform.Enums.Access.SaveToCampaign))
                {
                    #region Save to campaign Logic

                    List<string> columnList = new List<string>();
                    columnList.Add("s.SubscriptionID|Default");
                    columnList.Add("s.CGRP_NO|None");

                    if (SubscriptionID != null)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < SubID.Count; i++)
                        {
                            sb.AppendLine("<Subscriptions SubscriptionID=\"" + SubID[i] + "\"/>");
                        }

                        dtSubscription = Subscriber.GetSubscriberData(clientconnections, new StringBuilder(), columnList, MasterGroupColumnList, MasterGroupColumnDescList, SubscriptionsExtMapperValueList, customColumnList, BrandID, PubIDs, false, Convert.ToInt32(txtDownloadCount.Text), sb.ToString());
                    }
                    else
                    {
                        dtSubscription = Subscriber.GetSubscriberData(clientconnections, SubscribersQueries, columnList, MasterGroupColumnList, MasterGroupColumnDescList, SubscriptionsExtMapperValueList, customColumnList, BrandID, PubIDs, false, Convert.ToInt32(txtDownloadCount.Text));
                    }

                    if (rbExistingCampaign.Checked)
                    {
                        CampID = Convert.ToInt32(drpExistingCampaign.SelectedValue);
                    }
                    else if (rbNewCampaign.Checked)
                    {
                        if (CampID == 0)
                        {
                            if (Campaigns.CampaignExists(clientconnections, txtCampaignName.Text) == 0)
                            {
                                CampID = Campaigns.Insert(clientconnections, txtCampaignName.Text, ((KMPS.MD.MasterPages.Site)this.Page.Master).LoggedInUser, BrandID);
                            }
                            else
                            {
                                DisplayError("ERROR - <font color='#000000'>\"" + txtCampaignName.Text + "\"</font> already exists. Please enter a different name.");
                                mdlDownloads.Show();
                                return;
                            }
                        }
                    }
                    else
                    {
                        DisplayError("Select existing Campaign or new Campaign");
                        mdlDownloads.Show();
                        return;
                    }

                    if (CampFilterID == 0)
                    {
                        if (CampaignFilter.CampaignFilterExists(clientconnections, txtFilterName.Text, CampID) == 0)
                        {
                            CampFilterID = CampaignFilter.Insert(clientconnections, txtFilterName.Text, ((KMPS.MD.MasterPages.Site)this.Page.Master).LoggedInUser, CampID, txtPromocode.Text);
                        }
                        else
                        {
                            DisplayError("ERROR - <font color='#000000'>\"" + txtFilterName.Text + "\"</font> already exists. Please enter a different name.");
                            mdlDownloads.Show();
                            return;
                        }
                    }

                    StringBuilder xmlSubscriber = new StringBuilder("");
                    int cnt = 0;

                    try
                    {
                        foreach (DataRow dr in dtSubscription.Rows)
                        {
                            xmlSubscriber.Append("<sID>" + Utilities.cleanXMLString(dr["SubscriptionID"].ToString()) + "</sID>");

                            if ((cnt != 0) && (cnt % 10000 == 0) || (cnt == dtSubscription.Rows.Count - 1))
                            {
                                CampaignFilterDetails.saveCampaignDetails(clientconnections, CampFilterID, "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlSubscriber.ToString() + "</XML>");
                                xmlSubscriber = new StringBuilder("");
                            }

                            cnt++;
                        }

                        lblResults.Visible = true;
                        lblResults.Text = "Total subscriber in the campaign : " + Campaigns.GetCountByCampaignID(clientconnections, CampID);
                    }
                    catch (Exception ex)
                    {
                        Utilities.Log_Error(Request.RawUrl.ToString(), "DetailsDownload - campaign save", ex);
                        DisplayError("ERROR - " + ex.Message);
                    }

                    CampID = 0;
                    CampFilterID = 0;

                    mdlDownloads.Show();
                    #endregion
                }
            }
            else if (rbMarketo.Checked)
            {
                #region Export to Marketo Logic

                    GridView gvHttpPost = (GridView)Marketo.FindControl("gvHttpPost");

                    List<Dictionary<string, string>> Leads = new List<Dictionary<string, string>>();

                    foreach (DataRow dr in dtSubscription.Rows)
                    {
                        Dictionary<string, string> dleads = new Dictionary<string, string>();

                        for (int i = 0; i < gvHttpPost.Rows.Count; i++)
                        {
                            if (gvHttpPost.Rows[i].RowType == DataControlRowType.DataRow)
                            {
                                Label lblHttpPostParamsID = (Label)gvHttpPost.Rows[i].FindControl("lblHttpPostParamsID");
                                Label lblParamName = (Label)gvHttpPost.Rows[i].FindControl("lblParamName");
                                Label lblParamValue = (Label)gvHttpPost.Rows[i].FindControl("lblParamValue");
                                Label lblCustomValue = (Label)gvHttpPost.Rows[i].FindControl("lblCustomValue");
                                Label lblParamDisplayName = (Label)gvHttpPost.Rows[i].FindControl("lblParamDisplayName");

                                string ExportColumn = string.Empty;

                                switch (lblParamValue.Text.Split('|')[0].ToUpper())
                                {
                                    case "ADDRESS1":
                                        ExportColumn = "Address";
                                        break;
                                    case "REGIONCODE":
                                        ExportColumn = "State";
                                        break;
                                    case "ZIPCODE":
                                        ExportColumn = "Zip";
                                        break;
                                    case "PUBTRANSACTIONDATE":
                                        ExportColumn = "TransactionDate";
                                        break;
                                    case "QUALIFICATIONDATE":
                                        ExportColumn = "QDate";
                                        break;
                                    case "FNAME":
                                        ExportColumn = "FirstName";
                                        break;
                                    case "LNAME":
                                        ExportColumn = "LastName";
                                        break;
                                    case "ISLATLONVALID":
                                        ExportColumn = "GeoLocated";
                                        break;
                                    default:
                                        {
                                            ExportColumn = lblParamDisplayName.Text;
                                            break;
                                        }
                                }

                                dleads.Add(lblParamName.Text, lblCustomValue.Text != string.Empty ? lblCustomValue.Text : dr[ExportColumn].ToString());
                            }
                        }

                        Leads.Add(dleads);
                    }

                    if (Leads.Count > 0)
                    {
                        TextBox txtMarketoBaseURL = (TextBox)Marketo.FindControl("txtMarketoBaseURL");
                        TextBox txtMarketoClientID = (TextBox)Marketo.FindControl("txtMarketoClientID");
                        TextBox txtMarketoClientSecret = (TextBox)Marketo.FindControl("txtMarketoClientSecret");
                        TextBox txtMarketoPartition = (TextBox)Marketo.FindControl("txtMarketoPartition");
                        DropDownList ddlMarketoList = (DropDownList)Marketo.FindControl("ddlMarketoList");

                        string MarketoServer = txtMarketoBaseURL.Text;

                        if (!MarketoServer.Contains("https://"))
                            MarketoServer = "https://" + MarketoServer;

                        if (MarketoServer[MarketoServer.Length - 1].ToString() == "/")
                            MarketoServer = MarketoServer.Remove(MarketoServer.Length - 1);

                        KM.Integration.Marketo.Process.MarketoRestAPIProcess mMarketoRestApiProcess = new KM.Integration.Marketo.Process.MarketoRestAPIProcess(MarketoServer, txtMarketoClientID.Text, txtMarketoClientSecret.Text);

                        List<KM.Integration.Marketo.Result> results = mMarketoRestApiProcess.CreateUpdateLeads(Leads, "email", txtMarketoPartition.Text, ddlMarketoList.SelectedValue != string.Empty ? Convert.ToInt32(ddlMarketoList.SelectedValue) : (int?)null);

                        DataTable dtResults = new DataTable();
                        dtResults.Columns.Add("Type");
                        dtResults.Columns.Add("Action");
                        dtResults.Columns.Add("Totals");
                        DataRow row;

                        var query = results
                                    .GroupBy(n => new { n.type, n.status })
                                    .Select(n => new
                                    {
                                        Type = n.Key.type,
                                        Status = n.Key.status,
                                        Count = n.Count()
                                    }
                                    )
                                    .OrderBy(n => n.Type).ThenBy(n => n.Status);


                        foreach (var item in query)
                        {
                            row = dtResults.NewRow();
                            row["Type"] = item.Type;
                            row["Action"] = item.Status;
                            row["Totals"] = item.Count;
                            dtResults.Rows.Add(row);
                        }

                        ResultsGrid.DataSource = dtResults;
                        ResultsGrid.DataBind();
                        ResultsGrid.Visible = true;
                        ResultsGrid.Columns[0].Visible = true;
                    }

                #endregion
            }
            else
            {
                DisplayError("Select Export to Group or Download or Save to campaign");
                mdlDownloads.Show();
            }
        }

        private int saveDataCompareView(int targetCodeID, int? targetID, int typeCodeID, FrameworkUAD_Lookup.Enums.DataCompareType typeCodeName,  int UadNetCount)
        {
            FrameworkUAS.Entity.DataCompareView dcv = new FrameworkUAS.Entity.DataCompareView();

            dcv.DcTargetCodeId = dcTargetCodeID;
            dcv.DcTargetIdUad = targetID;
            dcv.DcTypeCodeId = typeCodeID;
            dcv.DcRunId = dcRunID;
            dcv.UadNetCount = UadNetCount;
            dcv.MatchedCount = matchedRecordsCount;
            dcv.NoDataCount = nonMatchedRecordsCount;
            dcv.Cost = new FrameworkUAS.BusinessLogic.DataCompareView().GetDataCompareCost(UserSession.CurrentUser.UserID, UserSession.ClientID, (UadNetCount + TotalFileRecords), typeCodeName, FrameworkUAD_Lookup.Enums.DataCompareCost.MergePurge);

            if (plNotes.Visible)
                dcv.Notes = txtNotes.Text;
            
            dcv.IsBillable = plKmStaff.Visible ? (Convert.ToBoolean(drpIsBillable.SelectedValue)) : true;

            if (plKmStaff.Visible && !Convert.ToBoolean(drpIsBillable.SelectedValue))
                dcv.PaymentStatusId = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status, FrameworkUAD_Lookup.Enums.PaymentStatus.Non_Billed.ToString()).CodeId;
            else
                dcv.PaymentStatusId = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status, FrameworkUAD_Lookup.Enums.PaymentStatus.Unpaid.ToString()).CodeId;

            dcv.DateCreated = DateTime.Now;
            dcv.CreatedByUserID = UserSession.UserID;

            int dcViewID = new FrameworkUAS.BusinessLogic.DataCompareView().Save(dcv);

            return dcViewID;
        }

        protected void btnEditCase_Click(object sender, EventArgs e)
        {
            if (lstSelectedFields.Items.Count >= 1)
            {
                DownloadEditCase.Visible = true;
                Dictionary<string, string> downloadFields = new Dictionary<string, string>();

                for (int i = 0; i < lstSelectedFields.Items.Count; i++)
                {
                    if (lstSelectedFields.Items[i].Value.Split('|')[1].ToUpper() == Enums.FieldType.Varchar.ToString().ToUpper())
                        downloadFields.Add(lstSelectedFields.Items[i].Value, lstSelectedFields.Items[i].Text.Split('(')[0]);
                }

                DownloadEditCase.DownloadFields = downloadFields;
                DownloadEditCase.loadControls();
            }
            else
            {
                if(rbGroupExport.Checked)
                {
                    dvDownloads.Style.Add("height", "500px");
                    dvDownloads.Style.Add("overflow", "scroll");
                }

                divError.Visible = true;
                lblErrorMessage.Text = "Please select field to edit case.";
            }
        }
    }
}