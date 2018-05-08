using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using Ecn.Communicator.Main.Admin.Interfaces;
using Ecn.Communicator.Main.Interfaces;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using KM.Common;
using static KM.Platform.User;
using KMPlatformEntity = KMPlatform.Entity;
using static ECN_Framework_Common.Objects.Enums;
using BusinessLayerAccounts = ECN_Framework_BusinessLayer.Accounts;

namespace Ecn.Communicator.Main.Admin.Helpers
{
    public class BaseChannelOperationsHandler : WebPageHelper
    {
        private const string OnClickAttribute = "onclick";
        private const string YesText = "Yes";
        private const string NoText = "No";
        private const string SelectedValueNo = "no";
        private const string SelectedValueDropDown = "drop down";
        private const string SelectedValueTextBox = "Text Box";
        private const string SelectedValueDropDownCapitalInitials = "Drop Down";
        private const string CodeSnippetPattern = "%%";
        private const string InvalidCodeSnippetErrorForCustomerName =
            "Invalid codesnippet, only %%customername%%, %%groupname%% and %%groupdescription%% are allowed";
        private const string EcnMediumClass = "ECN-Button-Medium";
        private const string KMCommonApplicationKey = "KMCommon_Application";
        private const string ReasonSortOrderKey = "SortOrder";
        private const string ReasonIDKey = "ID";
        private const string ReasonIsDeletedKey = "IsDeleted";
        private const string ReasonKey = "Reason";
        private const string SelectedValueThankYou = "thankyou";
        private const string SelectedValueRedirect = "redirect";
        private const string SelectedValueBoth = "both";
        private const string SelectedValueNeither = "neither";
        private const int MasterSuppressionLabelSettings = 1;
        private const int ReasonTextSettings = 2;
        private const int ReasonDropDownSettings = 3;
        private const int UnsubscribeTextSettings = 4;
        private const int ThankYouTextSettings = 5;
        private const int PageLabelSettings = 6;
        private const int MainLabelSettings = 7;
        private const int RedirectUrlTextSettings = 12;
        private const int RedirectDelayTextSettings = 13;
        private const int ExistingReasonsSetting = 14;

        private string[] DefaultSnippets = new string[]
        {
            "%%customername%%",
            "%%groupname%%",
            "%%groupdescription%%"
        };

        protected virtual TextBox TxtHeader { get; }
        protected virtual TextBox TxtFooter { get; }
        protected virtual RadioButtonList RblVisibilityPageLabel { get; }
        protected virtual TextBox TxtPageLabel { get; }
        protected virtual RadioButtonList RblVisibilityMainLabel { get; }
        protected virtual TextBox TxtMainLabel { get; }
        protected virtual RadioButtonList RblVisibilityMasterSuppression { get; }
        protected virtual TextBox TxtMasterSuppressionLabel { get; }
        protected virtual TextBox TxtUnsubscribeText { get; }
        protected virtual RadioButtonList RblRedirectThankYou { get; }
        protected virtual HtmlTable TblThankYou { get; }
        protected virtual HtmlTable TblRedirect { get; }
        protected virtual HtmlTable TblDelay { get; }
        protected virtual TextBox TxtThankYouMessage { get; }
        protected virtual TextBox TxtRedirectURL { get; }
        protected virtual DropDownList DdlRedirectDelay { get; }
        protected virtual RadioButtonList RblVisibilityReason { get; }
        protected virtual RadioButtonList RblReasonControlType { get; }
        protected virtual TextBox TxtReasonLabel { get; }
        protected virtual Panel PnlReasonDropDown { get; }
        protected virtual Label LblReasonLabel { get; }
        protected virtual HtmlTable TblReasonResponseType { get; }
        protected virtual ReorderList RlReasonDropDown { get; }
        protected virtual ILandingPageAssign LandingPageAssignAdapter { get; }
        protected virtual PlaceHolder PhError { get; }
        protected virtual Label LblErrorMessage { get; }

        public BaseChannelOperationsHandler()
        {

        }

        public BaseChannelOperationsHandler(ILandingPageAssign landingPageAssign)
        {
            LandingPageAssignAdapter = landingPageAssign;
        }

        internal void HandleCustomerSelectedIndexChange(
            int landingPageAssignId,
            DropDownList ddlCustomer,
            Button btnHtmlPreviewShow,
            Label lblUrlWarning)
        {
            Guard.NotNull(ddlCustomer, nameof(ddlCustomer));

            if (ddlCustomer.SelectedIndex != 0)
            {
                var customerId = Guard.ParseStringToInt(ddlCustomer.SelectedValue);
                var datatable = LandingPageAssignAdapter.GetPreviewParameters(landingPageAssignId, customerId);
                var url = string.Empty;
                if (datatable.Rows.Count > 0)
                {
                    url = GetPreviewUrl(datatable, landingPageAssignId);
                }

                if (!string.IsNullOrWhiteSpace(url))
                {
                    SetButtonVisibility(
                        btnHtmlPreviewShow,
                        true,
                        $"window.open('{url}', 'popup_window', 'width=1000,height=750,resizable=yes');");
                    SetLabelTextAndBorderStyle(lblUrlWarning, string.Empty, BorderStyle.None);
                }
                else
                {
                    //The selected customer did not generate a valid url (no blasts sent)
                    SetButtonVisibility(btnHtmlPreviewShow, false);
                    SetLabelTextAndBorderStyle(
                        lblUrlWarning,
                        $"Something is wrong with the {ddlCustomer.SelectedItem} {PageName} page. \n Please ensure that the customer has sent blasts to at least one group.",
                        BorderStyle.Dashed,
                        Color.Red);
                }
            }
            else
            {
                SetButtonVisibility(btnHtmlPreviewShow, false);
                SetLabelTextAndBorderStyle(lblUrlWarning, string.Empty, BorderStyle.None);
            }
        }

        internal void HandlePageLoad(
            ref LandingPageAssign LPA,
            PlaceHolder phError,
            IMasterCommunicator MasterCommunicator,
            bool isPostBack,
            Button btnPreview,
            Panel pnlNoAccess,
            Panel pnlSettings,
            Label label1,
            RadioButtonList rblOverrideDefaultSettings,
            RadioButtonList rblAllowCustomerOverrideSettings,
            TextBox txtHeader,
            TextBox txtFooter,
            string errorMessage,
            TextBox txtThankYou = null)
        {
            phError.Visible = false;
            MasterCommunicator.SubMenu = string.Empty;
            MasterCommunicator.Heading = string.Empty;
            MasterCommunicator.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BASECHANNEL;
            if (!isPostBack)
            {
                SetButtonVisibility(btnPreview, false);

                if (IsChannelAdministrator(MasterCommunicator.GetCurrentUser()))
                {
                    pnlNoAccess.Visible = false;
                    pnlSettings.Visible = true;
                    LoadData(
                        ref LPA,
                        MasterCommunicator.GetBaseChannelID().Value,
                        rblOverrideDefaultSettings,
                        rblAllowCustomerOverrideSettings,
                        txtHeader,
                        txtFooter,
                        btnPreview,
                        txtThankYou);
                }
                else
                {
                    pnlNoAccess.Visible = true;
                    pnlSettings.Visible = false;
                    label1.Text = errorMessage;
                }
            }
        }

        internal bool CanSaveCustomer(
            IMasterCommunicator master,
            PlaceHolder phError,
            ref LandingPageAssign landingPageAssign,
            int landingPageID,
            int customerID,
            RadioButtonList rblBasechannelOverride,
            TextBox txtHeader,
            TextBox txtFooter,
            bool shouldFailonFooterCSError,
            Label lblErrorMessage)
        {
            Guard.NotNull(master, nameof(master));
            var userID = master.GetUserID();

            Guard.NotNull(phError, nameof(phError));
            phError.Visible = false;

            InitializeCustomerLandingPageAssign(ref landingPageAssign, landingPageID, userID, customerID);
            SaveOverrideBaseChannelSelection(landingPageAssign, rblBasechannelOverride);

            if (!ValidateHeaderOrFooter(txtHeader, txtFooter, shouldFailonFooterCSError, phError, lblErrorMessage))
            {
                return false;
            }
            SetHeaderAndFooterAndUserID(landingPageAssign, userID, txtHeader, txtFooter);

            landingPageAssign.UpdatedUserID = userID;
            LandingPageAssignAdapter.Save(landingPageAssign, master.GetCurrentUser());
            return true;
        }

        internal void SaveLandingPageAssign(
            TextBox txtThankYou,
            int? userId,
            int? landingPageAssignId,
            IList<LandingPageOption> listOptions,
            ILandingPageAssignContent landingPageAssignContent,
            KMPlatformEntity.User currentUser)
        {
            Guard.NotNull(txtThankYou, nameof(txtThankYou));

            var landingPageAssignContentObject = new LandingPageAssignContent();
            if (txtThankYou.Text.Length > 0)
            {
                landingPageAssignContentObject.Display = txtThankYou.Text;
                landingPageAssignContentObject.CreatedUserID = userId;
                landingPageAssignContentObject.LPAID = landingPageAssignId;

                Guard.NotNull(listOptions, nameof(listOptions));
                landingPageAssignContentObject.LPOID = listOptions
                                                        .First(x => x.Name.ToLower()
                                                        .Replace(" ", string.Empty)
                                                        .Contains(SelectedValueThankYou))
                                                        .LPOID;

                Guard.NotNull(landingPageAssignContent, nameof(landingPageAssignContent));
                landingPageAssignContent.Save(landingPageAssignContentObject, currentUser);
            }
        }

        internal void HandleCriticalError(
            IApplicationLog applicationLog,
            ECNException ecnException,
            PlaceHolder phError,
            Label lblErrorMessage)
        {
            Guard.NotNull(applicationLog, nameof(applicationLog));

            applicationLog.LogCriticalError(
                ecnException,
                "BaseChannelAbuse.btnSave_Click",
                Guard.ParseStringToInt(ConfigurationManager.AppSettings[KMCommonApplicationKey]?.ToString()));
            SetECNError(ecnException, phError, lblErrorMessage);
        }

        internal bool CanSaveChannel(
            PlaceHolder phError,
            ref LandingPageAssign landingPageAssign,
            int landingPageID,
            int userID,
            int? baseChannelID,
            RadioButtonList rblOverrideDefaultSettings,
            RadioButtonList rblAllowCustomerOverrideSettings,
            TextBox txtHeader,
            TextBox txtFooter,
            bool shouldFailonFooterCSError,
            Label lblErrorMessage)
        {
            Guard.NotNull(phError, nameof(phError));

            phError.Visible = false;
            InitializeChannelLandingPageAssign(ref landingPageAssign, landingPageID, userID, baseChannelID);
            SaveOverridDefaultSelection(landingPageAssign, rblOverrideDefaultSettings);
            SaveAllowCustomerOverrideSelection(landingPageAssign, rblAllowCustomerOverrideSettings);

            if (!ValidateHeaderOrFooter(txtHeader, txtFooter, shouldFailonFooterCSError, phError, lblErrorMessage))
            {
                return false;
            }

            SetHeaderAndFooterAndUserID(landingPageAssign, userID, txtHeader, txtFooter);
            return true;
        }

        internal CodeSnippetError ValidCodeSnippets(
            string text,
            List<string> snippetsToMatch = null,
            string errorMessage = InvalidCodeSnippetErrorForCustomerName)
        {
            snippetsToMatch = snippetsToMatch ?? new List<string>(DefaultSnippets);

            var codeSnippetError = new CodeSnippetError();
            var count = Regex.Matches(text, CodeSnippetPattern).Count;
            if (count > 0)
            {
                if (count % 2 == 0)
                {
                    var matches = Regex.Matches(text, "%%[A-Za-z0-9]*%%", RegexOptions.IgnoreCase);
                    foreach (Match match in matches)
                    {
                        foreach (Capture capture in match.Captures)
                        {
                            if (!(snippetsToMatch.Contains(capture.Value.ToString())))
                            {
                                codeSnippetError.message = errorMessage;
                                codeSnippetError.valid = false;
                                return codeSnippetError;
                            }
                        }
                    }

                    codeSnippetError.valid = true;
                    return codeSnippetError;
                }
                else
                {
                    codeSnippetError.valid = false;
                    codeSnippetError.message = "There is a badly formed codesnippet";
                    return codeSnippetError;
                }
            }
            else
            {
                codeSnippetError.valid = true;
                return codeSnippetError;
            }
        }

        internal void ThrowECNException(string message, PlaceHolder phError, Label lblErrorMessage)
        {
            var ecnError = new ECNError(Entity.LandingPage, Method.Save, message);
            var errorList = new List<ECNError>();
            errorList.Add(ecnError);
            SetECNError(
                new ECNException(errorList, ExceptionLayer.WebSite),
                phError,
                lblErrorMessage);
        }

        internal void SetECNError(ECNException ecnException, PlaceHolder phError, Label lblErrorMessage)
        {
            Guard.NotNull(phError, nameof(phError));
            Guard.NotNull(lblErrorMessage, nameof(lblErrorMessage));
            Guard.NotNull(ecnException, nameof(ecnException));

            phError.Visible = true;
            lblErrorMessage.Text = string.Empty;
            foreach (ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = string.Format("{0}<br/>{1}: {2}", lblErrorMessage.Text, ecnError.Entity, ecnError.ErrorMessage);
            }
        }

        internal void SetButtonVisibility(Button htmlButton, bool state)
        {
            Guard.NotNull(htmlButton, nameof(htmlButton));

            htmlButton.Enabled = state;
            htmlButton.Visible = state;
        }

        internal void LoadReasonData(DataTable dtReason, ReorderList rlReasonDropDown, Control pnlReasonDropDown)
        {
            Guard.NotNull(rlReasonDropDown, nameof(rlReasonDropDown));

            if (dtReason != null)
            {
                var result = (from src in dtReason.AsEnumerable()
                              orderby src.Field<int>(ReasonSortOrderKey)
                              where src.Field<bool>(ReasonIsDeletedKey) == false
                              select new
                              {
                                  ID = src.Field<string>(ReasonIDKey),
                                  Reason = src.Field<string>(ReasonKey),
                                  SortOrder = src.Field<int>(ReasonSortOrderKey),
                                  IsDeleted = src.Field<bool>(ReasonIsDeletedKey),
                              }).ToList();
                rlReasonDropDown.DataSource = result;
                rlReasonDropDown.DataBind();

                SetControlVisibility(pnlReasonDropDown, true);
            }
        }

        internal void HandleRedirectThankYouSelectionChanged(
            RadioButtonList rblRedirectThankYou,
            HtmlTable tblDelay,
            HtmlTable tblRedirect,
            HtmlTable tblThankYou,
            TextBox txtRedirectURL,
            TextBox txtThankYouMessage,
            DropDownList ddlRedirectDelay)
        {
            Guard.NotNull(rblRedirectThankYou, nameof(rblRedirectThankYou));

            switch (rblRedirectThankYou.SelectedValue.ToLower())
            {
                case SelectedValueThankYou:
                    SetControlVisibility(tblDelay, false);
                    SetControlVisibility(tblRedirect, false);
                    SetControlVisibility(tblThankYou, true);
                    InitializeRedirectControls(txtRedirectURL, txtThankYouMessage, ddlRedirectDelay);
                    break;
                case SelectedValueRedirect:
                    SetControlVisibility(tblDelay, false);
                    SetControlVisibility(tblRedirect, true);
                    SetControlVisibility(tblThankYou, false);
                    InitializeRedirectControls(txtRedirectURL, txtThankYouMessage, ddlRedirectDelay);
                    break;
                case SelectedValueBoth:
                    SetControlVisibility(tblDelay, true);
                    SetControlVisibility(tblRedirect, true);
                    SetControlVisibility(tblThankYou, true);
                    InitializeRedirectControls(txtRedirectURL, txtThankYouMessage, ddlRedirectDelay);
                    break;
                case SelectedValueNeither:
                    SetControlVisibility(tblDelay, false);
                    SetControlVisibility(tblRedirect, false);
                    SetControlVisibility(tblThankYou, false);
                    InitializeRedirectControls(txtRedirectURL, txtThankYouMessage, ddlRedirectDelay);
                    break;
                default:
                    break;
            }
        }

        internal void AddDefaultReasons(DataTable dtReason)
        {
            var defaultReasonNames = new string[]
            {
                "Email Frequency",
                "Email Volume",
                "Content not relevant",
                "Signed up for one-time email",
                "Circumstances changed(moved, married, changed jobs, etc.)",
                "Prefer to get information another way"
            };
            for (var i = 0; i < defaultReasonNames.Length; i++)
            {
                AddReason(dtReason, defaultReasonNames[i], i + 1);
            }
        }

        internal void SetButtonCssClass(Button htmlButton)
        {
            Guard.NotNull(htmlButton, nameof(htmlButton));

            SetButtonVisibility(htmlButton, true);
            htmlButton.CssClass = EcnMediumClass;
        }

        internal void HandleSelectedIndexChanged(RadioButtonList radioButton, TextBox txtBox)
        {
            Guard.NotNull(radioButton, nameof(radioButton));

            if (string.Compare(radioButton.SelectedValue, SelectedValueNo, StringComparison.OrdinalIgnoreCase) == 0)
            {
                InitializeTextBox(txtBox, false);
            }
            else
            {
                InitializeTextBox(txtBox);
            }
        }

        internal void HandleSelectedIndexChanged(
            RadioButtonList rblVisibilityReason,
            TextBox txtReasonLabel,
            RadioButtonList rblReasonControlType,
            Panel pnlReasonDropDown,
            HtmlTable tblReasonResponseType,
            TextBox txtNewReason)
        {
            Guard.NotNull(rblVisibilityReason, nameof(rblVisibilityReason));

            if (string.Compare(rblVisibilityReason.SelectedValue, SelectedValueNo, StringComparison.OrdinalIgnoreCase) == 0)
            {
                InitializeTextBox(txtReasonLabel, false);
                SetControlVisibility(tblReasonResponseType, false);
                SetControlVisibility(rblReasonControlType, false);
                SetControlVisibility(pnlReasonDropDown, false);
                InitializeTextBox(txtNewReason);
            }
            else
            {
                InitializeTextBox(txtReasonLabel);
                SetControlVisibility(tblReasonResponseType, true);
                SetControlVisibility(rblReasonControlType, true);
                if (string.Compare(rblReasonControlType.SelectedValue, SelectedValueDropDown, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    SetControlVisibility(pnlReasonDropDown, true);
                }
                else
                {
                    SetControlVisibility(pnlReasonDropDown, false);
                }
                InitializeTextBox(txtNewReason);
            }
        }

        internal void HandleSaveReason(
            DataTable dtReason,
            string reasonText,
            string reasonCommand,
            ModalPopupExtender mpeEditReason,
            ReorderList rlReasonDropDown,
            Control pnlReasonDropDown)
        {
            if (!string.IsNullOrWhiteSpace(reasonText))
            {
                Guard.NotNull(dtReason, nameof(dtReason));
                var dataRow = dtReason.Select($"ID = '{reasonCommand}'");
                if (dataRow.Any())
                {
                    if (dataRow[0] != null)
                    {
                        dataRow[0][ReasonKey] = reasonText;
                        LoadReasonData(dtReason, rlReasonDropDown, pnlReasonDropDown);

                        Guard.NotNull(mpeEditReason, nameof(mpeEditReason));
                        mpeEditReason.Hide();
                    }
                }
            }
        }

        internal void HandleReasonCommand(
            DataTable dtReason,
            string commandName,
            string commandArgument,
            TextBox txtReasonLabelEdit,
            Button btnSaveReason,
            ModalPopupExtender mpeEditReason,
            ReorderList rlReasonDropDown,
            Control pnlReasonDropDown)
        {
            Guard.NotNull(dtReason, nameof(dtReason));

            if (string.Compare(commandName, "EditReason", StringComparison.OrdinalIgnoreCase) == 0)
            {
                var currentReason = dtReason.Select($"ID = '{commandArgument}'");
                if (currentReason.Any())
                {
                    if (currentReason[0] != null)
                    {
                        Guard.NotNull(txtReasonLabelEdit, nameof(txtReasonLabelEdit));
                        Guard.NotNull(btnSaveReason, nameof(btnSaveReason));
                        Guard.NotNull(mpeEditReason, nameof(mpeEditReason));

                        txtReasonLabelEdit.Text = currentReason[0][ReasonKey]?.ToString();
                        btnSaveReason.CommandArgument = commandArgument;
                        mpeEditReason.Show();
                    }
                }
            }
            else if (string.Compare(commandName, "DeleteReason", StringComparison.OrdinalIgnoreCase) == 0)
            {
                var currentPriority = 0;
                foreach (var dataRow in dtReason.AsEnumerable())
                {
                    if (string.Compare(
                        dataRow[ReasonIDKey] as string,
                        commandArgument,
                        StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        dataRow[ReasonIsDeletedKey] = true;
                        currentPriority = Guard.ParseStringToInt(dataRow[ReasonSortOrderKey]?.ToString());
                        break;
                    }
                }

                foreach (var dataRow in dtReason.AsEnumerable())
                {
                    var sortOrder = Guard.ParseStringToInt(dataRow[ReasonSortOrderKey]?.ToString());
                    if (sortOrder > currentPriority)
                    {
                        dataRow[ReasonSortOrderKey] = sortOrder - 1;
                    }
                }

                LoadReasonData(dtReason, rlReasonDropDown, pnlReasonDropDown);
            }
        }

        internal void HandleReasonReorder(
            DataTable dtReason,
            int newIndex,
            int oldIndex,
            ReorderList rlReasonDropDown,
            Control pnlReasonDropDown)
        {
            Guard.NotNull(dtReason, nameof(dtReason));

            newIndex++;
            oldIndex++;

            var movedReasonIDs = (from src in dtReason.AsEnumerable()
                                  where src.Field<int>(ReasonSortOrderKey) == oldIndex
                                  select new
                                  {
                                      ReasonID = src.Field<string>(ReasonIDKey)
                                  }).ToList();

            if (newIndex < oldIndex)
            {
                foreach (var dataRow in dtReason.AsEnumerable())
                {
                    var sortOrder = Guard.ParseStringToInt(dataRow[ReasonSortOrderKey]?.ToString());
                    if (sortOrder >= newIndex && sortOrder <= oldIndex)
                    {
                        dataRow[ReasonSortOrderKey] = sortOrder + 1;
                    }
                }
            }
            else if (newIndex > oldIndex)
            {
                foreach (var dataRow in dtReason.AsEnumerable())
                {
                    var sortOrder = Guard.ParseStringToInt(dataRow[ReasonSortOrderKey]?.ToString());
                    if (sortOrder <= newIndex && sortOrder >= oldIndex)
                    {
                        dataRow[ReasonSortOrderKey] = sortOrder - 1;
                    }
                }
            }

            foreach (var dataRow in dtReason.AsEnumerable())
            {
                if (movedReasonIDs.Any()
                    && string.Compare(
                        dataRow[ReasonIDKey] as string,
                        movedReasonIDs[0]?.ReasonID,
                        StringComparison.OrdinalIgnoreCase) == 0)
                {
                    dataRow[ReasonSortOrderKey] = newIndex;
                }
            }

            LoadReasonData(dtReason, rlReasonDropDown, pnlReasonDropDown);
        }

        internal void LoadLandingPageAssignContentData(
            int landingPageAssignId,
            string header,
            string footer,
            DataTable dtReason)
        {
            InitializeTextBox(TxtHeader, header);
            InitializeTextBox(TxtFooter, footer);
            var landingPageAssignContents = BusinessLayerAccounts.LandingPageAssignContent.GetByLPAID(landingPageAssignId);

            UpdateSettingsControls(landingPageAssignContents, PageLabelSettings, RblVisibilityPageLabel, TxtPageLabel);
            UpdateSettingsControls(landingPageAssignContents, MainLabelSettings, RblVisibilityMainLabel, TxtMainLabel);
            UpdateSettingsControls(landingPageAssignContents, MasterSuppressionLabelSettings, RblVisibilityMasterSuppression, TxtMasterSuppressionLabel);

            UpdateUnsubscribeTextControls(landingPageAssignContents);
            UpdateRedirectControls(landingPageAssignContents);
            UpdateReasonTextSettings(landingPageAssignContents, landingPageAssignId, dtReason);
        }

        internal bool SaveLandingPageAssignContents(
            int landingPageAssignId,
            KMPlatformEntity.User currentUser,
            string pageLabelText,
            string mainLabelText,
            string masterSuppressionLabelText,
            string unsubscribeText,
            string thankYouMessageText,
            string redirectURLText,
            string reasonLabelText,
            DataTable dtReason)
        {
            BusinessLayerAccounts.LandingPageAssignContent.Delete(landingPageAssignId, currentUser);

            if (!SaveIfVisibleAndValidCodeSnippets(
                landingPageAssignId,
                currentUser,
                RblVisibilityPageLabel,
                pageLabelText,
                "Page Label",
                PageLabelSettings))
            {
                return false;
            }
            if (!SaveIfVisibleAndValidCodeSnippets(
                landingPageAssignId,
                currentUser,
                RblVisibilityMainLabel,
                mainLabelText,
                "Main Label",
                MainLabelSettings))
            {
                return false;
            }
            if (!SaveIfVisibleAndValidCodeSnippets(
                landingPageAssignId,
                currentUser,
                RblVisibilityMasterSuppression,
                masterSuppressionLabelText,
                "Master Suppression Label",
                MasterSuppressionLabelSettings))
            {
                return false;
            }
            if (!SaveIfValidCodeSnippet(
                landingPageAssignId,
                currentUser,
                unsubscribeText,
                "Unsubscribe Text",
                UnsubscribeTextSettings))
            {
                return false;
            }
            if (!SaveRedirectThankYouMessage(landingPageAssignId, currentUser, thankYouMessageText, redirectURLText))
            {
                return false;
            }

            return SaveReasonIfVisibleAndValidCodeSnippets(landingPageAssignId, currentUser, reasonLabelText, dtReason);
        }

        private bool SaveReasonIfVisibleAndValidCodeSnippets(
            int landingPageAssignId,
            KMPlatformEntity.User currentUser,
            string reasonLabelText,
            DataTable dtReason)
        {
            if (RblVisibilityReason.SelectedValue == YesText)
            {
                var codeSnippetError = ValidCodeSnippets(reasonLabelText);
                if (codeSnippetError.valid)
                {
                    if (RblReasonControlType.SelectedValue.Equals(SelectedValueTextBox))
                    {
                        SaveLandingPageAssignContent(landingPageAssignId, currentUser, reasonLabelText, ReasonTextSettings);
                    }
                    else
                    {
                        SaveLandingPageAssignContent(landingPageAssignId, currentUser, reasonLabelText, ReasonDropDownSettings);
                        if (dtReason.Select($"{ReasonIsDeletedKey} = 'false'").Any())
                        {
                            foreach (DataRow dataRow in dtReason.Rows)
                            {
                                if (dataRow[ReasonIDKey].ToString().Contains("-"))
                                {
                                    if (dataRow[ReasonIsDeletedKey].ToString().Equals("false", StringComparison.OrdinalIgnoreCase))
                                    {
                                        SaveLandingPageAssignContent(
                                            landingPageAssignId,
                                            currentUser,
                                            dataRow[ReasonKey]?.ToString(),
                                            ExistingReasonsSetting,
                                            Guard.ParseStringToInt(dataRow[ReasonSortOrderKey]?.ToString()));
                                    }
                                }
                                else
                                {
                                    //Dont need to delete here because we did it already
                                    if (!dataRow[ReasonIsDeletedKey].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                                    {
                                        SaveLandingPageAssignContent(
                                            landingPageAssignId,
                                            currentUser,
                                            dataRow[ReasonKey]?.ToString(),
                                            ExistingReasonsSetting,
                                            Guard.ParseStringToInt(dataRow[ReasonSortOrderKey]?.ToString()));
                                    }
                                }
                            }
                        }
                        else
                        {
                            ThrowECNException("Please enter at least one Reason", PhError, LblErrorMessage);
                            return false;
                        }
                    }
                }
                else
                {
                    ThrowECNException($"{codeSnippetError.message} in Reason label", PhError, LblErrorMessage);
                    return false;
                }
            }

            return true;
        }

        private bool SaveRedirectThankYouMessage(
            int landingPageAssignId,
            KMPlatformEntity.User currentUser,
            string thankYouMessageText,
            string redirectURLText)
        {
            if (RblRedirectThankYou.SelectedValue == SelectedValueThankYou)
            {
                if (!SaveIfValidCodeSnippet(landingPageAssignId, currentUser, thankYouMessageText, "Thank You Message", ThankYouTextSettings))
                {
                    return false;
                }
            }
            else if (RblRedirectThankYou.SelectedValue == SelectedValueRedirect)
            {
                if (!ValidUrl(redirectURLText))
                {
                    return false;
                }

                SaveLandingPageAssignContent(landingPageAssignId, currentUser, redirectURLText, RedirectUrlTextSettings);
            }
            else if (RblRedirectThankYou.SelectedValue == SelectedValueBoth)
            {
                if (!SaveIfValidCodeSnippet(landingPageAssignId, currentUser, thankYouMessageText, "Thank You Message", ThankYouTextSettings))
                {
                    return false;
                }
                if (!ValidUrl(redirectURLText))
                {
                    return false;
                }

                SaveLandingPageAssignContent(landingPageAssignId, currentUser, redirectURLText, RedirectUrlTextSettings);
                Guard.NotNull(DdlRedirectDelay, nameof(DdlRedirectDelay));
                SaveLandingPageAssignContent(landingPageAssignId, currentUser, DdlRedirectDelay.SelectedValue?.ToString(), RedirectDelayTextSettings);
            }

            return true;
        }

        private bool ValidUrl(string urlText)
        {
            var urlRegex = new Regex(@"^((https|http):\/\/([a-zA-Z0-9]+\.)([a-zA-Z0-9]+\.).{2,3}.*)");
            if (!urlRegex.IsMatch(urlText))
            {
                ThrowECNException("Invalid URL for Redirect", PhError, LblErrorMessage);
                return false;
            }

            return true;
        }

        private bool SaveIfVisibleAndValidCodeSnippets(
            int landingPageAssignId,
            KMPlatformEntity.User currentUser,
            RadioButtonList rblVisibilityControl,
            string pageLabelText,
            string errorLocation,
            int contentId)
        {
            if (rblVisibilityControl.SelectedValue == "Yes")
            {
                return SaveIfValidCodeSnippet(landingPageAssignId, currentUser, pageLabelText, errorLocation, contentId);
            }

            return true;
        }

        private bool SaveIfValidCodeSnippet(
            int landingPageAssignId,
            KMPlatformEntity.User currentUser,
            string pageLabelText,
            string errorLocation,
            int contentId)
        {
            var codeSnippetError = ValidCodeSnippets(pageLabelText);
            if (codeSnippetError.valid)
            {
                SaveLandingPageAssignContent(landingPageAssignId, currentUser, pageLabelText, contentId);
            }
            else
            {
                ThrowECNException(string.Format("{0} in {1}", codeSnippetError.message, errorLocation), PhError, LblErrorMessage);
                return false;
            }

            return true;
        }

        private static void SaveLandingPageAssignContent(
            int landingPageAssignId,
            KMPlatformEntity.User currentUser,
            string pageLabelText,
            int contentId,
            int? sortOrder = null)
        {
            var landingPageAssignContent = new LandingPageAssignContent();
            landingPageAssignContent.LPAID = landingPageAssignId;
            landingPageAssignContent.CreatedUserID = currentUser.UserID;
            landingPageAssignContent.LPOID = contentId;
            landingPageAssignContent.Display = pageLabelText;
            if (sortOrder != null)
            {
                landingPageAssignContent.SortOrder = sortOrder;
            }

            BusinessLayerAccounts.LandingPageAssignContent.Save(landingPageAssignContent, currentUser);
        }

        protected virtual string GetPreviewUrl(DataTable datatable, int landingPageAssignId)
        {
            throw new NotImplementedException();
        }

        // This method is left empty as a default implementation.
        // Child classes can implement it if needed.
        protected virtual void DisplayThankYouText(TextBox txtThankYou, int landingPageAssignId)
        {

        }

        // This method is left empty as a default implementation.
        // Child classes can implement it if needed.
        protected virtual void InitializeLandingPageOptions()
        {

        }

        protected virtual string PageName { get; }

        protected virtual int LandingPageID { get; }

        private void UpdateUnsubscribeTextControls(List<LandingPageAssignContent> landingPageAssignContents)
        {
            Guard.NotNull(landingPageAssignContents, nameof(landingPageAssignContents));
            var result = landingPageAssignContents.Where(x => x.LPOID == UnsubscribeTextSettings).ToList();
            if (result.Any())
            {
                Guard.NotNull(TxtUnsubscribeText, nameof(TxtUnsubscribeText));
                TxtUnsubscribeText.Text = result[0].Display;
            }
            else
            {
                Guard.NotNull(TxtMasterSuppressionLabel, nameof(TxtMasterSuppressionLabel));
                TxtMasterSuppressionLabel.Text = string.Empty;
            }
        }

        private void UpdateReasonTextSettings(
            List<LandingPageAssignContent> landingPageAssignContents,
            int landingPageAssignId,
            DataTable dtReason)
        {
            Guard.NotNull(landingPageAssignContents, nameof(landingPageAssignContents));
            Guard.NotNull(RblReasonControlType, nameof(RblReasonControlType));
            Guard.NotNull(PnlReasonDropDown, nameof(PnlReasonDropDown));

            var result = landingPageAssignContents.Where(x => x.LPOID == ReasonTextSettings).ToList();
            if (result.Any())
            {
                InitializeReasonControls(
                    RblVisibilityReason,
                    TxtReasonLabel,
                    TblReasonResponseType,
                    YesText,
                    result[0].Display,
                    true,
                    true);

                RblReasonControlType.SelectedValue = SelectedValueTextBox;
                PnlReasonDropDown.Visible = false;

                Guard.NotNull(LblReasonLabel, nameof(LblReasonLabel));
                LblReasonLabel.Visible = true;
            }
            else
            {
                result = landingPageAssignContents.Where(x => x.LPOID == ReasonDropDownSettings).ToList();
                if (result.Any())
                {
                    InitializeReasonControls(
                        RblVisibilityReason,
                        TxtReasonLabel,
                        TblReasonResponseType,
                        YesText,
                        result[0].Display,
                        true,
                        true);

                    RblReasonControlType.SelectedValue = SelectedValueDropDownCapitalInitials;

                    try
                    {
                        var contents = BusinessLayerAccounts.LandingPageAssignContent.GetByLPOID(ExistingReasonsSetting, landingPageAssignId);
                        if (contents.Any())
                        {
                            foreach (var content in contents)
                            {
                                var dataRow = dtReason.NewRow();
                                dataRow[ReasonKey] = content.Display;
                                dataRow[ReasonIDKey] = content.LPACID.ToString();
                                dataRow[ReasonSortOrderKey] = content.SortOrder.Value;
                                dataRow[ReasonIsDeletedKey] = content.IsDeleted;
                                dtReason.Rows.Add(dataRow);
                            }
                        }
                        else
                        {
                            AddDefaultReasons(dtReason);
                        }

                        LoadReasonData(dtReason, RlReasonDropDown, PnlReasonDropDown);
                    }
                    catch (Exception ex)
                    {
                        Trace.Write(ex.ToString());
                    }
                }
                else
                {
                    InitializeReasonControls(
                        RblVisibilityReason,
                        TxtReasonLabel,
                        TblReasonResponseType,
                        NoText,
                        string.Empty,
                        false,
                        false);

                    PnlReasonDropDown.Visible = false;
                }
            }
        }

        private void InitializeReasonControls(
            RadioButtonList rblVisibilityReason,
            TextBox txtReasonLabel,
            HtmlTable tblReasonResponseType,
            string visibilityReasonSelectedValue,
            string reasonDisplayText,
            bool isReasonLabelVisible,
            bool isReasonResponseTypeVisible)
        {
            Guard.NotNull(rblVisibilityReason, nameof(rblVisibilityReason));
            rblVisibilityReason.SelectedValue = visibilityReasonSelectedValue;

            Guard.NotNull(tblReasonResponseType, nameof(tblReasonResponseType));
            tblReasonResponseType.Visible = isReasonResponseTypeVisible;

            InitializeTextBox(txtReasonLabel, isReasonResponseTypeVisible, reasonDisplayText);
        }

        private void UpdateRedirectControls(List<LandingPageAssignContent> landingPageAssignContents)
        {
            var thankYouMessage = string.Empty;
            var hasThankYou = CheckContentsAndGetMessage(landingPageAssignContents, ThankYouTextSettings, out thankYouMessage);

            var redirectURL = string.Empty;
            var hasRedirectURL = CheckContentsAndGetMessage(landingPageAssignContents, RedirectUrlTextSettings, out redirectURL);

            var redirectDelay = string.Empty;
            var hasRedirectDelay = CheckContentsAndGetMessage(landingPageAssignContents, RedirectDelayTextSettings, out redirectDelay);

            if (hasThankYou && !hasRedirectURL && !hasRedirectDelay)
            {
                UpdateRedirectControls(SelectedValueThankYou, true, false, false);
                InitializeTextBox(TxtThankYouMessage, thankYouMessage.Trim());
            }
            else if (!hasThankYou && hasRedirectURL && !hasRedirectDelay)
            {
                UpdateRedirectControls(SelectedValueRedirect, false, true, false);
                InitializeTextBox(TxtRedirectURL, redirectURL.Trim());
            }
            else if (hasThankYou && hasRedirectURL && hasRedirectDelay)
            {
                UpdateRedirectControls(SelectedValueBoth, true, true, true);
                InitializeTextBox(TxtRedirectURL, redirectURL.Trim());
                InitializeTextBox(TxtThankYouMessage, thankYouMessage.Trim());

                Guard.NotNull(DdlRedirectDelay, nameof(DdlRedirectDelay));
                DdlRedirectDelay.SelectedValue = redirectDelay;
            }
            else
            {
                UpdateRedirectControls(SelectedValueNeither, false, false, false);
            }
        }

        private void UpdateRedirectControls(
            string selectedValue,
            bool hasThankYou,
            bool hasRedirectUrl,
            bool hasRedirectDelay)
        {
            Guard.NotNull(RblRedirectThankYou, nameof(RblRedirectThankYou));
            RblRedirectThankYou.SelectedValue = selectedValue;

            SetControlVisibility(TblThankYou, hasThankYou);
            SetControlVisibility(TblRedirect, hasRedirectUrl);
            SetControlVisibility(TblDelay, hasRedirectDelay);
        }

        private bool CheckContentsAndGetMessage(List<LandingPageAssignContent> landingPageAssignContents, int contentId, out string message)
        {
            Guard.NotNull(landingPageAssignContents, nameof(landingPageAssignContents));

            message = string.Empty;
            var hasMessage = false;
            var result = landingPageAssignContents.Where(x => x.LPOID == contentId).ToList();
            if (result.Any())
            {
                hasMessage = true;
                message = result[0].Display;
            }

            return hasMessage;
        }

        private void UpdateSettingsControls(
            List<LandingPageAssignContent> landingPageAssignContents,
            int contentId,
            RadioButtonList rblVisibilityPageLabel,
            TextBox txtPageLabel)
        {
            Guard.NotNull(landingPageAssignContents, nameof(landingPageAssignContents));
            Guard.NotNull(rblVisibilityPageLabel, nameof(rblVisibilityPageLabel));
            Guard.NotNull(txtPageLabel, nameof(txtPageLabel));

            var result = landingPageAssignContents.Where(x => x.LPOID == contentId).ToList();
            if (result.Any())
            {
                rblVisibilityPageLabel.SelectedValue = YesText;
                txtPageLabel.Text = result[0].Display;
                txtPageLabel.Visible = true;
            }
            else
            {
                rblVisibilityPageLabel.SelectedValue = NoText;
                txtPageLabel.Text = string.Empty;
                txtPageLabel.Visible = false;
            }
        }

        private void SaveOverrideBaseChannelSelection(LandingPageAssign landingPageAssign, RadioButtonList rblBasechannelOverride)
        {
            Guard.NotNull(rblBasechannelOverride, nameof(rblBasechannelOverride));

            if (rblBasechannelOverride.SelectedValue.Equals(YesText))
            {
                landingPageAssign.CustomerDoesOverride = true;
            }
            else
            {
                landingPageAssign.CustomerDoesOverride = false;
            }
        }

        private void SaveAllowCustomerOverrideSelection(LandingPageAssign landingPageAssign, RadioButtonList rblAllowCustomerOverrideSettings)
        {
            Guard.NotNull(rblAllowCustomerOverrideSettings, nameof(rblAllowCustomerOverrideSettings));

            if (rblAllowCustomerOverrideSettings.SelectedValue.Equals(YesText))
            {
                landingPageAssign.CustomerCanOverride = true;
            }
            else
            {
                landingPageAssign.CustomerCanOverride = false;
            }
        }

        private void SaveOverridDefaultSelection(LandingPageAssign landingPageAssign, RadioButtonList rblOverrideDefaultSettings)
        {
            Guard.NotNull(rblOverrideDefaultSettings, nameof(rblOverrideDefaultSettings));

            if (rblOverrideDefaultSettings.SelectedValue.Equals(YesText))
            {
                landingPageAssign.BaseChannelDoesOverride = true;
            }
            else
            {
                landingPageAssign.BaseChannelDoesOverride = false;
            }
        }

        private void InitializeCustomerLandingPageAssign(
            ref LandingPageAssign landingPageAssign,
            int landingPageID,
            int userID,
            int customerID)
        {
            InitializeLandingPageAssign(ref landingPageAssign, landingPageID, userID);
            landingPageAssign.CustomerID = customerID;
        }

        private void InitializeChannelLandingPageAssign(
            ref LandingPageAssign landingPageAssign,
            int landingPageID,
            int userID,
            int? baseChannelID)
        {
            InitializeLandingPageAssign(ref landingPageAssign, landingPageID, userID);
            landingPageAssign.BaseChannelID = baseChannelID;
        }

        private void InitializeLandingPageAssign(
            ref LandingPageAssign landingPageAssign,
            int landingPageID,
            int userID)
        {
            if (landingPageAssign == null)
            {
                landingPageAssign = new LandingPageAssign();
            }

            landingPageAssign.LPID = landingPageID;
            landingPageAssign.CreatedUserID = userID;
        }

        private void SetButtonVisibility(Button htmlButton, bool state, string onClickAttributeValue)
        {
            Guard.NotNull(htmlButton, nameof(htmlButton));

            SetButtonVisibility(htmlButton, state);
            htmlButton.Attributes.Add(OnClickAttribute, onClickAttributeValue);
        }

        private void SetHeaderAndFooterAndUserID(LandingPageAssign landingPageAssign, int userID, TextBox txtHeader, TextBox txtFooter)
        {
            landingPageAssign.Header = txtHeader.Text;
            landingPageAssign.Footer = txtFooter.Text;
        }

        private bool ValidateHeaderOrFooter(TextBox txtHeader, TextBox txtFooter, bool shouldFailonFooterCSError, PlaceHolder phError, Label lblErrorMessage)
        {
            Guard.NotNull(txtHeader, nameof(txtHeader));
            Guard.NotNull(txtFooter, nameof(txtFooter));

            var codeSnippetError = new CodeSnippetError();
            codeSnippetError = ValidCodeSnippets(txtHeader.Text);
            if (!codeSnippetError.valid)
            {
                ThrowECNException(string.Format("{0} in Header", codeSnippetError.message), phError, lblErrorMessage);
                return false;
            }
            codeSnippetError = new CodeSnippetError();
            codeSnippetError = ValidCodeSnippets(txtFooter.Text);
            if (!codeSnippetError.valid)
            {
                ThrowECNException(string.Format("{0} in Footer", codeSnippetError.message), phError, lblErrorMessage);
                if (shouldFailonFooterCSError)
                {
                    return false;
                }
            }

            return true;
        }

        private void LoadData(
            ref LandingPageAssign LPA,
            int baseChannelId,
            RadioButtonList rblOverrideDefaultSettings,
            RadioButtonList rblAllowCustomerOverrideSettings,
            TextBox txtHeader,
            TextBox txtFooter,
            Button btnPreview,
            TextBox txtThankYou = null)
        {
            LPA = LandingPageAssignAdapter.GetByBaseChannelID(baseChannelId, LandingPageID);

            InitializeLandingPageOptions();

            if (LPA != null)
            {
                if (LPA.BaseChannelDoesOverride.Value)
                {
                    rblOverrideDefaultSettings.SelectedValue = YesText;
                }
                if (LPA.CustomerCanOverride.Value)
                {
                    rblAllowCustomerOverrideSettings.SelectedValue = YesText;
                }

                DisplayThankYouText(txtThankYou, LPA.LPAID);

                txtHeader.Text = LPA.Header;
                txtFooter.Text = LPA.Footer;
                SetButtonVisibility(btnPreview, true);
            }
        }

        private void SetLabelTextAndBorderStyle(Label label, string text, BorderStyle borderStyle)
        {
            Guard.NotNull(label, nameof(label));
            label.Text = text;
            label.BorderStyle = borderStyle;
        }

        private void SetLabelTextAndBorderStyle(Label label, string text, BorderStyle borderStyle, Color borderColor)
        {
            Guard.NotNull(label, nameof(label));
            SetLabelTextAndBorderStyle(label, text, borderStyle);
            label.BorderColor = borderColor;
        }

        private void InitializeRedirectControls(TextBox txtRedirectURL, TextBox txtThankYouMessage, DropDownList ddlRedirectDelay)
        {
            InitializeTextBox(txtRedirectURL);
            InitializeTextBox(txtThankYouMessage);
            InitializeDropDownListSelectedIndex(ddlRedirectDelay);
        }

        private void InitializeDropDownListSelectedIndex(DropDownList dropDownList)
        {
            Guard.NotNull(dropDownList, nameof(dropDownList));
            dropDownList.SelectedIndex = 0;
        }

        private void InitializeTextBox(TextBox textBox, string text = "")
        {
            Guard.NotNull(textBox, nameof(textBox));
            textBox.Text = text;
        }

        private void InitializeTextBox(TextBox textBox, bool isVisible, string text = "")
        {
            InitializeTextBox(textBox, text);
            textBox.Visible = isVisible;
        }

        private void SetControlVisibility(Control htmlTable, bool isVisible)
        {
            Guard.NotNull(htmlTable, nameof(htmlTable));
            htmlTable.Visible = isVisible;
        }

        private void AddReason(DataTable dtReason, string reasonName, int sortOrder)
        {
            Guard.NotNull(dtReason, nameof(dtReason));

            var dataRow = dtReason.NewRow();
            dataRow[ReasonKey] = reasonName;
            dataRow[ReasonIDKey] = Guid.NewGuid().ToString();
            dataRow[ReasonSortOrderKey] = sortOrder;
            dataRow[ReasonIsDeletedKey] = false;
            dtReason.Rows.Add(dataRow);
        }
    }
}
