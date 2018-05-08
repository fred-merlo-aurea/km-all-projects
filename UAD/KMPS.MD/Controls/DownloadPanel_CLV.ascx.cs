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
using System.IO;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using AjaxControlToolkit;
using FrameworkUAD.DataAccess;
using KMPS.MD.Helpers;
using DownloadTemplate = KMPS.MD.Objects.DownloadTemplate;
using DownloadTemplateDetails = KMPS.MD.Objects.DownloadTemplateDetails;
using Filter = KMPS.MD.Objects.Filter;
using MasterGroup = KMPS.MD.Objects.MasterGroup;
using ResponseGroup = KMPS.MD.Objects.ResponseGroup;
using Subscriber = KMPS.MD.Objects.Subscriber;
using SubscriptionsExtensionMapper = KMPS.MD.Objects.SubscriptionsExtensionMapper;
using Utilities = KMPS.MD.Objects.Utilities;

namespace KMPS.MD.Controls
{
    public partial class DownloadPanel_CLV : DownloadPanelV
    {
        public Delegate DelMethod;

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
        protected override TextBox TxtDownloadUniqueCount => txtDownloadUniqueCount;
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
                    txtDownloadCount.Text = Subscribers.Count.ToString();

                    if (Subscribers.Any())
                    {
                        txtDownloadUniqueCount.Text = Subscriber.GetUniqueLocationsCount(clientconnections, SubscribersQueries).ToString();
                    }
                }
            }
        }

        private void DownloadCasePopupHide()
        {
            DownloadEditCase.Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            isError();
            if (ShowHeaderCheckBox && KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADExport, KMPlatform.Enums.Access.Download))
                phShowHeader.Visible = true;
            else
                phShowHeader.Visible = false;

            HidePanel delDownloadCase = new HidePanel(DownloadCasePopupHide);
            this.DownloadEditCase.hideDownloadCasePopup = delDownloadCase;

            LoadEditCaseData delNoParamDownloadFields = new LoadEditCaseData(LoadEditCase);
            this.DownloadEditCase.LoadEditCaseData = delNoParamDownloadFields;

            if (!IsPostBack)
            {
            }

            this.mdlDownloads.Show();
        }

        public void LoadEditCase(Dictionary<string, string> downloadfields)
        {
            ControlsValidators.LoadEditCase(downloadfields, lstSelectedFields);
        }

        public void LoadControls()
        {
            if (ViewType == Enums.ViewType.ConsensusView || ViewType == Enums.ViewType.RecencyView)
            {
                pnlIsRecentData.Visible = VisibleCbIsRecentData ? true : false;;
                cbIsRecentData.Checked = ViewType == Enums.ViewType.RecencyView ? true : false;
            }
            else
            {
                pnlIsRecentData.Visible = false;
                cbIsRecentData.Checked = false;
            }

            txtDownloadCount.Text = downloadCount.ToString();
            if(downloadCount > 0)
                txtDownloadUniqueCount.Text = Subscriber.GetUniqueLocationsCount(clientconnections, SubscribersQueries).ToString();
        }

        public void LoadDownloadTemplate()
        {
            drpDownloadTemplate.Items.Clear();

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
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        private void ExportFields()
        {
            Dictionary<string, string> exportfields = new Dictionary<string, string>();
            Dictionary<string, string> selectedfields = new Dictionary<string, string>();
            Dictionary<string, string> exportProfileFields = Utilities.GetExportFields(clientconnections, ViewType, BrandID, PubIDs, Enums.ExportType.FTP, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Profile);
            Dictionary<string, string> exportDemoFields = Utilities.GetExportFields(clientconnections, ViewType, BrandID, PubIDs, Enums.ExportType.FTP, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Demo);
            Dictionary<string, string> exportAdhocFields = Utilities.GetExportFields(clientconnections, ViewType, BrandID, PubIDs, Enums.ExportType.FTP, UserSession.CurrentUser.UserID, Enums.ExportFieldType.Adhoc);

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
                    lstSelectedFields.Items.Add(new ListItem(field.Value.ToUpper() + (field.Key.Split('|')[1].ToUpper() == Enums.FieldType.Varchar.ToString().ToUpper() ? "(" + (td.FieldCase == null ? Enums.FieldCase.Default.ToString() : text) + ")" : ""), field.Key + "|" + (td.FieldCase == null ? (field.Key.Split('|')[1].ToUpper() == KMPS.MD.Objects.Enums.FieldType.Varchar.ToString().ToUpper() ? KMPS.MD.Objects.Enums.FieldCase.Default.ToString() : KMPS.MD.Objects.Enums.FieldCase.None.ToString()) : td.FieldCase)));
                }
            }
        }

        public void reload()
        {
            txtDownloadCount.Text = downloadCount.ToString();
            if(downloadCount > 0)
                txtDownloadUniqueCount.Text = Subscriber.GetUniqueLocationsCount(clientconnections, SubscribersQueries).ToString();

            Session["SubscribersQueries"] = SubscribersQueries;
        }
        public void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        protected void isError()
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

        public void ValidateDownload()
        {
            if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADExport, KMPlatform.Enums.Access.Download))
            {
                lblNoDownloadMessage.Text = "You do not have permission to download/export the data.";
                lblNoDownloadMessage.Visible = true;
                pnlUADExport.Visible = false;
                btnExport.Visible = false;
            }
            else
            {
                if (KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.DataCompare, KMPlatform.Enums.Access.Yes) && dcRunID > 0)
                {
                    rcForDownload.Visible = true;

                    if (dcRunID > 0)
                    {
                        btnExport.OnClientClick = "return confirmPopupPurchase();";

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
                                if (dc.IsBillable)
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
                        btnExport.OnClientClick = null;
                    }
                } 
            }
        }

        public void Show(
            string headerText,
            StringBuilder subscriberQueries,
            int brandId,
            Enums.ViewType viewType,
            string filterCombination,
            int downloadCount,
            bool showCompareData,
            int dcRunID,
            int dcTypeCodeID,
            int dcTargetCodeID,
            int matchedRecordsCount,
            int nonMatchedRecordsCount,
            int totalFileRecords)
        {
            this.SubscribersQueries = subscriberQueries;
            this.Visible = true;
            this.HeaderText = headerText;
            this.ShowHeaderCheckBox = true;
            this.BrandID = brandId;
            this.PubIDs = null;
            this.ViewType = viewType;
            this.VisibleCbIsRecentData = ViewType == Enums.ViewType.ConsensusView ? false : true;
            this.filterCombination = filterCombination;
            this.downloadCount = downloadCount;
            if (showCompareData)
            {
                this.dcRunID = dcRunID;
                this.dcTypeCodeID = dcTypeCodeID;
                this.dcTargetCodeID = dcTargetCodeID;
                this.matchedRecordsCount = matchedRecordsCount;
                this.nonMatchedRecordsCount = nonMatchedRecordsCount;
                this.TotalFileRecords = totalFileRecords;
            }
            
            LoadControls();
            LoadDownloadTemplate();
            loadExportFields();
            ValidateDownload();
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

        protected void drpIsBillable_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(drpIsBillable.SelectedValue) == true)
            {
                btnExport.OnClientClick = "return confirmPopupPurchase();";
                plNotes.Visible = false;
            }
            else
            {
                btnExport.OnClientClick = null;
                plNotes.Visible = true;
            }
        }

        protected override void DetailsDownload()
        {
            if (Session["SubscribersQueries"] == null)
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
            List<string> ResponseGroupIDList = new List<string>();
            List<string> ResponseGroupDescIDList = new List<string>();
            List<string> selectedItem = new List<string>();
            List<string> customColumnList = new List<string>();

            foreach (ListItem item in lstSelectedFields.Items)
            {
                selectedItem.Add(item.Value);
            }

            if (ViewType == Enums.ViewType.ProductView)
            {
                PubSubscriptionsExtMapperValueList = Utilities.GetSelectedPubSubExtMapperExportColumns(clientconnections, selectedItem, PubIDs.First());
                Tuple<List<string>, List<string>, List<string>> rg = Utilities.GetSelectedResponseGroupStandardExportColumns(clientconnections, selectedItem, PubIDs.First(), false);
                ResponseGroupIDList = rg.Item1;
                ResponseGroupDescIDList = rg.Item2;
                StandardColumnsList = rg.Item3;
            }
            else
            {
                StandardColumnsList = Utilities.GetSelectedStandardExportColumns(clientconnections, selectedItem, BrandID);
                Tuple<List<string>, List<string>> mg = Utilities.GetSelectedMasterGroupExportColumns(clientconnections, selectedItem, BrandID);
                MasterGroupColumnList = mg.Item1;
                MasterGroupColumnDescList = mg.Item2;
                SubscriptionsExtMapperValueList = Utilities.GetSelectedSubExtMapperExportColumns(clientconnections, selectedItem);
            }

            if (!StandardColumnsList.Any(x => x.Split('|')[0] == "CGRP_NO"))
                StandardColumnsList.Add("CGRP_NO|Default");

            stdColumnList = Utilities.GetStandardExportColumnFieldName(StandardColumnsList, ViewType, BrandID, false).ToList();
            customColumnList = Utilities.GetSelectedCustomExportColumns(selectedItem);

            if (rbDownloadAll.Checked || rbDownload.Checked)
            {
                 if (KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADExport, KMPlatform.Enums.Access.Download))
                {
                    if (rbDownloadAll.Checked)
                    {
                        if (ViewType == Enums.ViewType.ProductView)
                        {
                            dtSubscription = Subscriber.GetProductDimensionSubscriberData_CLV(clientconnections, SubscribersQueries, stdColumnList, PubIDs, ResponseGroupIDList, ResponseGroupDescIDList,  PubSubscriptionsExtMapperValueList, customColumnList, BrandID, false);
                        }
                        else
                        {
                            dtSubscription = Subscriber.GetSubscriberData_CLV(clientconnections, SubscribersQueries, stdColumnList, MasterGroupColumnList, MasterGroupColumnDescList, SubscriptionsExtMapperValueList, customColumnList, BrandID, PubIDs, false, ViewType == Enums.ViewType.RecencyView ? true : false);
                        }

                        dtSubscription.DefaultView.Sort = "CGRP_NO";
                        dtSubscription = dtSubscription.DefaultView.ToTable();
                    }
                    else
                    {
                        //get unique record
                        if (ViewType == Enums.ViewType.ProductView)
                        {
                            dtSubscription = Subscriber.GetProductDimensionSubscriberData_CLV(clientconnections, SubscribersQueries, stdColumnList, PubIDs, ResponseGroupIDList, ResponseGroupDescIDList, PubSubscriptionsExtMapperValueList, customColumnList, BrandID, true);
                        }
                        else
                        {
                            dtSubscription = Subscriber.GetSubscriberData_CLV(clientconnections, SubscribersQueries, stdColumnList, MasterGroupColumnList, MasterGroupColumnDescList, SubscriptionsExtMapperValueList, customColumnList, BrandID, PubIDs, true, ViewType == Enums.ViewType.RecencyView ? true : false);
                        }
                    }

                    List<dynamic> fefName = Utilities.getListboxSelectedExportFields(lstSelectedFields);

                    string[] columnsOrder = new string[fefName.Count + 1];
                    int i = 0;

                    columnsOrder[0] = "SubscriptionID";

                    foreach (dynamic e in fefName)
                    {
                        i++;
                        var displayName = (string)e.GetType()
                                                .GetProperty("text")
                                                .GetValue(e, null);
                        if (ViewType == Enums.ViewType.ProductView)
                        {
                            columnsOrder[i] =
                                FieldMapper.GetColumnOrderByProductExportFieldDisplayName(displayName);
                        }
                        else
                        {
                            columnsOrder[i] =
                                FieldMapper.GetColumnOrderByDefaultExportFieldDisplayName(displayName);
                        }
                    }

                    for (int j = 0; j < columnsOrder.Length; j++)
                    {
                        dtSubscription.Columns[columnsOrder[j].Split('(')[0]].SetOrdinal(j);
                    }

                    dtSubscription = (DataTable) ProfileFieldMask.MaskData(clientconnections, dtSubscription, UserSession.CurrentUser);

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
                            if (!datacv.Exists(u => u.DcTargetCodeId == targetCodeID && u.DcTypeCodeId == dcTypeCodeID && u.DcTargetIdUad == targetID))
                            {
                                dcViewID = saveDataCompareView(targetCodeID, targetID, dcTypeCodeID, typeCodeName, filter.Count);
                            }
                            else
                            {
                                if (plKmStaff.Visible)
                                {
                                    FrameworkUAS.Entity.DataCompareView dcv = datacv.Find(u => u.DcTargetCodeId == targetCodeID && u.DcTypeCodeId == dcTypeCodeID && u.DcTargetIdUad == targetID && u.DcTypeCodeId == dcTypeCodeID);

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

                                        new FrameworkUAS.BusinessLogic.DataCompareView().Save(dcv).ToString();
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
                        sColumnsList.AddRange(StandardColumnsList.Select(x => x.Split('|')[0]));

                        List<FrameworkUAS.Entity.DataCompareDownloadCostDetail> cd = new FrameworkUAS.BusinessLogic.DataCompareDownloadCostDetail().CreateCostDetail(dcViewID, dcTypeCodeID, rbDownloadAll.Checked ? txtDownloadCount.Text : txtDownloadUniqueCount.Text, string.Join(",", sColumnsList), demoColumns, UserSession.CurrentUser.UserID);

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

                    DataColumn newColumn = new DataColumn("PromoCode", typeof(System.String));
                    newColumn.DefaultValue = txtPromocode.Text;
                    dtSubscription.Columns.Add(newColumn);

                    if (!cbShowHeader.Checked)
                    {
                        HeaderText = string.Empty;
                    }

                    if (rbDownloadAll.Checked)
                        Utilities.Download(dtSubscription, outfilepath, HeaderText, Convert.ToInt32(txtDownloadCount.Text), Convert.ToInt32(txtDownloadCount.Text));
                    else
                        Utilities.Download(dtSubscription, outfilepath, HeaderText, Convert.ToInt32(txtDownloadUniqueCount.Text), Convert.ToInt32(txtDownloadUniqueCount.Text));
                }
            }
            else
            {
                DisplayError("Select Download All or Download one record per location");
                mdlDownloads.Show();
            }
        }

        private int saveDataCompareView(int targetCodeID, int? targetID, int typeCodeID, FrameworkUAD_Lookup.Enums.DataCompareType typeCodeName, int UadNetCount)
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
                divError.Visible = true;
                lblErrorMessage.Text = "Please select field to edit case.";
            }
        }
    }
}