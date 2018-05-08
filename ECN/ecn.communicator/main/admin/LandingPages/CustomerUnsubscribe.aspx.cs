using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using Ecn.Communicator.Main.Admin.Helpers;
using Ecn.Communicator.Main.Admin.Interfaces;
using Ecn.Communicator.Main.Helpers;
using Ecn.Communicator.Main.Interfaces;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using BusinessLayerAccounts = ECN_Framework_BusinessLayer.Accounts;

namespace ecn.communicator.main.admin.landingpages
{
    public partial class CustomerUnsubscribe : BaseChannelOperationsHandler
    {
        #region ViewStateDT
        private DataTable ViewStateDT
        {
            get
            {
                if (ViewState["ViewStateDT"] != null)
                {
                    return (DataTable)ViewState["ViewStateDT"];
                }
                else
                {
                    DataTable dt = new DataTable();
                    DataColumn LPACID = new DataColumn("LPACID", typeof(string));
                    dt.Columns.Add(LPACID);
                    DataColumn Display = new DataColumn("Display", typeof(string));
                    dt.Columns.Add(Display);
                    DataColumn IsDeleted = new DataColumn("IsDeleted", typeof(bool));
                    dt.Columns.Add(IsDeleted);
                    dt.AcceptChanges();
                    ViewState["ViewStateDT"] = dt;
                    return dt;
                }
            }
            set
            {
                ViewState["ViewStateDT"] = value;
            }
        }

        private void AddRow_ViewStateDT(string LPACID, string Display)
        {
            DataRow dr = ViewStateDT.NewRow();
            dr["LPACID"] = LPACID;
            dr["Display"] = Display;
            dr["IsDeleted"] = false;
            ViewStateDT.Rows.Add(dr);
            ViewStateDT.AcceptChanges();
        }

        private void DeleteRow_ViewStateDT(string LPACID)
        {
            foreach (DataRow dr in ViewStateDT.AsEnumerable())
            {
                if (dr["LPACID"].Equals(LPACID))
                {
                    dr["IsDeleted"] = true;
                }
            }
        }
        #endregion

        private static DataTable dtReason;
        private static LandingPageAssign LPA = null;

        private IMasterCommunicator MasterCommunicator;
        private ILandingPageAssign LandingPageAssign;
        private ILandingPageAssignContent LandingPageAssignContent;
        private HttpResponseBase HttpResponse;

        protected override int LandingPageID => 1;
        protected override TextBox TxtHeader => txtHeader;
        protected override TextBox TxtFooter => txtFooter;
        protected override RadioButtonList RblVisibilityPageLabel => rblVisibilityPageLabel;
        protected override TextBox TxtPageLabel => txtPageLabel;
        protected override RadioButtonList RblVisibilityMainLabel => rblVisibilityMainLabel;
        protected override TextBox TxtMainLabel => txtMainLabel;
        protected override RadioButtonList RblVisibilityMasterSuppression => rblVisibilityMasterSuppression;
        protected override TextBox TxtMasterSuppressionLabel => txtMasterSuppressionLabel;
        protected override TextBox TxtUnsubscribeText => txtUnsubscribeText;
        protected override RadioButtonList RblRedirectThankYou => rblRedirectThankYou;
        protected override HtmlTable TblThankYou => tblThankYou;
        protected override HtmlTable TblRedirect => tblRedirect;
        protected override HtmlTable TblDelay => tblDelay;
        protected override TextBox TxtThankYouMessage => txtThankYouMessage;
        protected override TextBox TxtRedirectURL => txtRedirectURL;
        protected override DropDownList DdlRedirectDelay => ddlRedirectDelay;
        protected override RadioButtonList RblVisibilityReason => rblVisibilityReason;
        protected override RadioButtonList RblReasonControlType => rblReasonControlType;
        protected override TextBox TxtReasonLabel => txtReasonLabel;
        protected override Panel PnlReasonDropDown => pnlReasonDropDown;
        protected override Label LblReasonLabel => lblReasonLabel;
        protected override HtmlTable TblReasonResponseType => tblReasonResponseType;
        protected override ReorderList RlReasonDropDown => rlReasonDropDown;
        protected override PlaceHolder PhError => phError;
        protected override Label LblErrorMessage => lblErrorMessage;
        protected override ILandingPageAssign LandingPageAssignAdapter => LandingPageAssign;

        public CustomerUnsubscribe()
        {
            MasterCommunicator = new MasterCommunicatorAdapter(Master);
            LandingPageAssign = new LandingPageAssignAdapter();
            HttpResponse = new HttpResponseAdapter(Response);
        }

        public CustomerUnsubscribe(IMasterCommunicator masterCommunicator, ILandingPageAssign landingPageAssign, ILandingPageAssignContent landingPageAssignContent, HttpResponseBase response)
        {
            MasterCommunicator = masterCommunicator;
            LandingPageAssign = landingPageAssign;
            LandingPageAssignContent = landingPageAssignContent;
            HttpResponse = response;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            Master.SubMenu = "";
            Master.Heading = "";
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.CUSTOMER;
            if (!IsPostBack)
            {
                if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    dtReason = new DataTable();

                    dtReason.Columns.Add("Reason", typeof(string));
                    dtReason.Columns.Add("ID", typeof(string));
                    dtReason.Columns.Add("SortOrder", typeof(int));
                    dtReason.Columns.Add("IsDeleted", typeof(bool));

                    btnPreview.Enabled = false;
                    btnPreview.Visible = false;
                    List<ECN_Framework_Entities.Accounts.LandingPageAssign> lpAssignBaseChannelList = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByBaseChannelID(Master.UserSession.CurrentBaseChannel.BaseChannelID);
                    var BaseChannelUnsubscribeSettings = (from src in lpAssignBaseChannelList
                                                          where src.LPID == 1
                                                          select src).ToList();


                    if (BaseChannelUnsubscribeSettings.Count > 0 && BaseChannelUnsubscribeSettings[0].CustomerCanOverride.Value == true)
                    {
                        pnlNoAccess.Visible = false;
                        pnlSettings.Visible = true;
                        LoadData();
                        loadPreview();
                    }
                    else
                    {
                        pnlNoAccess.Visible = true;
                        pnlSettings.Visible = false;
                    }

                }
                else
                {
                    throw new ECN_Framework_Common.Objects.SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
                }
            }
        }

        private void LoadData()
        {
            if (Master?.UserSession?.CurrentCustomer == null)
            {
                throw new InvalidOperationException("Unable to get CustomerID");
            }

            LPA = BusinessLayerAccounts.LandingPageAssign.GetByCustomerID(Master.UserSession.CurrentCustomer.CustomerID, LandingPageID);
            if (LPA != null && LPA.LPAID > 0)
            {
                if (LPA.CustomerDoesOverride.Value)
                    rblBasechannelOverride.SelectedValue = "Yes";

                LoadLandingPageAssignContentData(
                    LPA.LPAID,
                    LPA.Header,
                    LPA.Footer,
                    dtReason);
            }
            else
            {
                AddDefaultReasons(dtReason);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CanSaveCustomer(
                MasterCommunicator,
                phError,
                ref LPA,
                LandingPageID,
                MasterCommunicator.GetCustomerID(),
                rblBasechannelOverride,
                txtHeader,
                txtFooter,
                false,
                lblErrorMessage))
            {
                return;
            }

            if (!SaveLandingPageAssignContents(
                LPA.LPAID,
                MasterCommunicator?.GetCurrentUser(),
                txtPageLabel?.Text,
                txtMainLabel?.Text,
                txtMasterSuppressionLabel?.Text,
                txtUnsubscribeText?.Text,
                txtThankYouMessage?.Text,
                txtRedirectURL?.Text,
                txtReasonLabel?.Text,
                dtReason))
            {
                return;
            }

            HttpResponse.Redirect("CustomerMain.aspx");
        }

        protected void rblReasonControlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblReasonControlType.SelectedValue.Equals("Text Box"))
            {
                pnlReasonDropDown.Visible = false;
            }
            else
            {
                pnlReasonDropDown.Visible = true;
                if (dtReason.Rows.Count == 0)
                {
                    AddDefaultReasons(dtReason);
                }
                loadGrid();
            }
            txtNewReason.Text = string.Empty;
        }

        private void loadPreview()
        {

            lblCustomerOverrideWarning.Text = "";
            lblCustomerOverrideWarning.Text = "";
            if (LPA != null)
            {
                //returns BlastID, GroupID, CustomerID and EmailAddress
                DataTable dt = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetPreviewParameters(LPA.LPAID, Convert.ToInt32(Master.UserSession.CurrentCustomer.CustomerID));
                string url;
                if (dt.Rows.Count > 0)
                {
                    btnPreview.Visible = true;
                    btnPreview.Enabled = true;
                    int blastId = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                    int groupId = Convert.ToInt32(dt.Rows[0].ItemArray[1]);
                    int customerId = Convert.ToInt32(dt.Rows[0].ItemArray[2]);
                    string emailAddress = dt.Rows[0].ItemArray[3].ToString();
                    url = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/Unsubscribe.aspx?e=" + emailAddress + "&g=" + groupId + "&b=" + blastId + "&c=" + customerId + "&s=U&f=html&" + "preview=" + LPA.LPAID;
                    string s = "window.open('" + url + "', 'popup_window', 'width=1000,height=750,resizable=yes');";
                    btnPreview.Attributes.Add("onclick", s);
                }
                else
                {
                    btnPreview.Enabled = false;
                    btnPreview.Visible = false;
                    lblSentBlastsWarning.Text = "Preview functionality will not be available until after you have sent at least one blast from this customer account.";
                }

                if (LPA.CustomerDoesOverride.Value == false)
                {
                    lblCustomerOverrideWarning.Text = "Note: The above settings will not be visible to customers until you override the Basechannel settings.";
                }
            }

        }

        private CodeSnippetError validCodeSnippets(string text)
        {
            return ValidCodeSnippets(text);
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.LandingPage, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }

        }

        protected void btnAddNewReason_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNewReason.Text))
            {
                if (dtReason.Select("Reason = '" + txtNewReason.Text.Replace("'", "''") + "' and IsDeleted = 'false'").Count() == 0)
                {
                    DataRow dr = dtReason.NewRow();
                    dr["Reason"] = txtNewReason.Text.Trim();
                    dr["ID"] = Guid.NewGuid().ToString();
                    dr["SortOrder"] = dtReason.Select("IsDeleted = 'false'").Count() + 1;
                    dr["IsDeleted"] = false;

                    dtReason.Rows.Add(dr);
                    loadGrid();

                    txtNewReason.Text = string.Empty;
                }
                else
                {
                    throwECNException("Reason already exists");
                    return;
                }
            }
            else
            {
                throwECNException("Please enter a value for the new reason");
                return;
            }
        }

        private void loadGrid()
        {
            LoadReasonData(dtReason, rlReasonDropDown, pnlReasonDropDown);
        }

        protected void rblRedirectThankYou_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleRedirectThankYouSelectionChanged(
                rblRedirectThankYou,
                tblDelay,
                tblRedirect,
                tblThankYou,
                txtRedirectURL,
                txtThankYouMessage,
                ddlRedirectDelay);
        }


        protected void rlReasonDropDown_ItemReorder(object sender, ReorderListItemReorderEventArgs e)
        {
            HandleReasonReorder(
                dtReason,
                e.NewIndex,
                e.OldIndex,
                rlReasonDropDown,
                pnlReasonDropDown);
        }

        protected void rlReasonDropDown_ItemCommand(object sender, ReorderListCommandEventArgs e)
        {
            HandleReasonCommand(
                dtReason,
                e.CommandName,
                e.CommandArgument?.ToString(),
                txtReasonLabelEdit,
                btnSaveReason,
                mpeEditReason,
                rlReasonDropDown,
                pnlReasonDropDown);
        }

        protected void btnSaveReason_Click(object sender, EventArgs e)
        {
            HandleSaveReason(
                dtReason,
                txtReasonLabelEdit.Text.Trim(),
                btnSaveReason.CommandArgument?.ToString(),
                mpeEditReason,
                rlReasonDropDown, 
                pnlReasonDropDown);
        }

        protected void rblVisibilityReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleSelectedIndexChanged(
                rblVisibilityReason,
                txtReasonLabel,
                rblReasonControlType,
                pnlReasonDropDown,
                tblReasonResponseType,
                txtNewReason);
        }

        protected void rblVisibilityPageLabel_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleSelectedIndexChanged(rblVisibilityPageLabel, txtPageLabel);
        }

        protected void rblVisibilityMasterSuppression_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleSelectedIndexChanged(rblVisibilityMasterSuppression, txtMasterSuppressionLabel);
        }

        protected void rblVisibilityMainLabel_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleSelectedIndexChanged(rblVisibilityMainLabel, txtMainLabel);
        }

        protected void btnCancelEditPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomerMain.aspx");
        }
    }

}