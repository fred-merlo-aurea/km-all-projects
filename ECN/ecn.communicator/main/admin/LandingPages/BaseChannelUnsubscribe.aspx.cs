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
    using Role = KM.Platform.User;
    public partial class BaseChannelUnsubscribe : BaseChannelOperationsHandler
    {
        KMPlatform.Entity.User SessionCurrentUser { get { return Master.UserSession.CurrentUser; } }
        private static DataTable dtReason;

        private LandingPageAssign LPA;
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

        public BaseChannelUnsubscribe()
        {
            MasterCommunicator = new MasterCommunicatorAdapter(Master);
            LandingPageAssign = new LandingPageAssignAdapter();
            HttpResponse = new HttpResponseAdapter(Response);
        }

        public BaseChannelUnsubscribe(IMasterCommunicator masterCommunicator, ILandingPageAssign landingPageAssign, ILandingPageAssignContent landingPageAssignContent, HttpResponseBase response)
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
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BASECHANNEL;
            if (!IsPostBack)
            {
                dtReason = new DataTable();

                dtReason.Columns.Add("Reason", typeof(string));
                dtReason.Columns.Add("ID", typeof(string));
                dtReason.Columns.Add("SortOrder", typeof(int));
                dtReason.Columns.Add("IsDeleted", typeof(bool));

                btnPreview.Enabled = false;
                btnPreview.Visible = false;
                btnHtmlPreviewShow.Visible = false;
                btnHtmlPreviewShow.Enabled = false;


                if (Role.IsChannelAdministrator(SessionCurrentUser))
                {
                    pnlNoAccess.Visible = false;
                    pnlSettings.Visible = true;
                    LoadData();
                }
                else
                {
                    pnlNoAccess.Visible = true;
                    pnlSettings.Visible = false;
                    Label1.Text = "You do not have access to this page.";
                }
            }
        }


        private void LoadData()
        {
            if(Master?.UserSession?.CurrentCustomer?.BaseChannelID == null)
            {
                throw new InvalidOperationException("Unable to get BaseChannelID");
            }

            var landingPageAssign = BusinessLayerAccounts.LandingPageAssign.GetByBaseChannelID(
                                        Master.UserSession.CurrentCustomer.BaseChannelID.Value,
                                        LandingPageID);
            if (landingPageAssign != null)
            {
                if (landingPageAssign.BaseChannelDoesOverride.Value)
                {
                    rblOverrideDefaultSettings.SelectedValue = "Yes";
                }
                if (landingPageAssign.CustomerCanOverride.Value)
                {
                    rblAllowCustomerOverrideSettings.SelectedValue = "Yes";
                }

                LoadLandingPageAssignContentData(
                    landingPageAssign.LPAID,
                    landingPageAssign.Header,
                    landingPageAssign.Footer,
                    dtReason);

                btnPreview.Enabled = true;
                btnPreview.Visible = true;
            }
            else
            {
                try
                {
                    AddDefaultReasons(dtReason);
                }
                catch (Exception ex)
                {
                    Trace.Write(ex.ToString());
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //LandingPageAssign
            LPA = LandingPageAssign.GetByBaseChannelID(MasterCommunicator.GetBaseChannelID().Value, 1);

            var userID = MasterCommunicator.GetUserID();
            if (!CanSaveChannel(
                phError,
                ref LPA,
                LandingPageID,
                userID,
                MasterCommunicator.GetBaseChannelID(),
                rblOverrideDefaultSettings,
                rblAllowCustomerOverrideSettings,
                txtHeader,
                txtFooter,
                true,
                lblErrorMessage))
            {
                return;
            }

            LPA.UpdatedUserID = userID;
            LandingPageAssign.Save(LPA, MasterCommunicator.GetCurrentUser());

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
            
            HttpResponse.Redirect("BaseChannelMain.aspx");
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            lblBaseChannelOverride.Text = "";
            lblCustomerOverride.Text = "";
            ECN_Framework_Entities.Accounts.LandingPageAssign lpa = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByBaseChannelID(Master.UserSession.CurrentCustomer.BaseChannelID.Value, 1);
            if (lpa.BaseChannelDoesOverride == false)
            {
                lblBaseChannelOverride.Text = "Note: You must override the default landing page settings for your saved changes to take effect.";
            }
            if (lpa.CustomerCanOverride == true)
            {
                lblCustomerOverride.Text = "Note: If any Customer overrides the Basechannel settings they may see different results.";
            }
            loadDdlCustomer();
            this.modalPopupHtmlPreview.Show();
        }
        protected void btnHtmlPreview_Hide(object sender, EventArgs e)
        {
            lblUrlWarning.Text = "";
            btnHtmlPreviewShow.Enabled = false;
            btnHtmlPreviewShow.Visible = false;
            lblUrlWarning.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
            this.modalPopupHtmlPreview.Hide();
        }
        private void loadDdlCustomer()
        {
            List<ECN_Framework_Entities.Accounts.Customer> customerList = new List<ECN_Framework_Entities.Accounts.Customer>();
            customerList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(Master.UserSession.CurrentCustomer.BaseChannelID.Value);
            ddlCustomer.DataSource = customerList;
            ddlCustomer.DataTextField = "CustomerName";
            ddlCustomer.DataValueField = "CustomerID";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem(""));
            ddlCustomer.SelectedIndex = 0;
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCustomer.SelectedIndex != 0)
            {
                string url = getPreviewUrl();
                if (url != null)
                {
                    btnHtmlPreviewShow.Enabled = true;
                    btnHtmlPreviewShow.Visible = true;
                    //add the click event to open the specified url in a popup window
                    string s = "window.open('" + url + "', 'popup_window', 'width=1000,height=750,resizable=yes');";
                    lblUrlWarning.Text = "";
                    btnHtmlPreviewShow.Attributes.Add("onclick", s);
                    lblUrlWarning.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                }
                else
                {
                    //The selected customer did not generate a valid url (no blasts sent)
                    btnHtmlPreviewShow.Enabled = false;
                    btnHtmlPreviewShow.Visible = false;
                    lblUrlWarning.Text = "Something is wrong with the " + ddlCustomer.SelectedItem + " unsubscribe page. \n Please ensure that the customer has sent blasts to at least one group.";
                    lblUrlWarning.BorderColor = System.Drawing.Color.Red;
                    lblUrlWarning.BorderStyle = System.Web.UI.WebControls.BorderStyle.Dashed;
                }
            }
            else
            {
                lblUrlWarning.Text = "";
                btnHtmlPreviewShow.Enabled = false;
                btnHtmlPreviewShow.Visible = false;
                lblUrlWarning.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
            }
        }

        private string getPreviewUrl()
        {
            ECN_Framework_Entities.Accounts.LandingPageAssign lpa = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByBaseChannelID(Master.UserSession.CurrentCustomer.BaseChannelID.Value, 1);
            //returns BlastID, GroupID, CustomerID and EmailAddress
            DataTable dt = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetPreviewParameters(lpa.LPAID, Convert.ToInt32(ddlCustomer.SelectedValue));

            if (dt.Rows.Count > 0)
            {
                int blastId = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                int groupId = Convert.ToInt32(dt.Rows[0].ItemArray[1]);
                int customerId = Convert.ToInt32(dt.Rows[0].ItemArray[2]);
                string emailAddress = dt.Rows[0].ItemArray[3].ToString();
                string url = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/Unsubscribe.aspx?e=" + emailAddress + "&g=" + groupId + "&b=" + blastId + "&c=" + customerId + "&s=U&f=html&" + "preview=" + lpa.LPAID;
                return url.ToString();
            }
            return null;
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

        protected void rblReasonControlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(rblReasonControlType.SelectedValue.Equals("Text Box"))
            {
                pnlReasonDropDown.Visible = false;
                
            }
            else
            {
                if(dtReason.Rows.Count == 0)
                {
                    AddDefaultReasons(dtReason);
                }
                loadGrid();
            }
            txtNewReason.Text = string.Empty;
        }

        protected void gvReasonDropDown_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if(e.CommandName.Equals("EditReason"))
            {
                DataRow[] currentReason = dtReason.Select("ID = '" + e.CommandArgument.ToString() + "'");
                btnSaveReason.CommandArgument = e.CommandArgument.ToString();
                txtReasonLabelEdit.Text = currentReason[0]["Reason"].ToString();
                mpeEditReason.Show();
            }
            else if(e.CommandName.Equals("DeleteReason"))
            {
                dtReason.Select("ID = '" + e.CommandArgument.ToString() + "'")[0]["IsDeleted"] = true;
                rlReasonDropDown.DataSource = dtReason.Select("IsDeleted = 'false'");
                rlReasonDropDown.DataBind();
            }
        }

        protected void btnAddNewReason_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtNewReason.Text))
            {
                if (dtReason.Select("Reason = '" + txtNewReason.Text.Replace("'","''") + "' and IsDeleted = 'false'").Count() == 0)
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

        protected void gvReasonDropDown_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                string ID = rlReasonDropDown.DataKeys[e.Row.RowIndex].ToString();
                ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEditReason");
                ImageButton imgbtnDelete = (ImageButton)e.Row.FindControl("imgbtnDeleteReason");

                imgbtnEdit.CommandArgument = ID;
                imgbtnDelete.CommandArgument = ID;
            }
        }

        protected void btnSaveReason_Click(object sender, EventArgs e)
        {
            HandleSaveReason(
                dtReason,
                txtReasonLabelEdit.Text.Trim(),
                btnSaveReason.CommandArgument.ToString(),
                mpeEditReason,
                rlReasonDropDown,
                pnlReasonDropDown);
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
                e.CommandArgument.ToString(),
                txtReasonLabelEdit,
                btnSaveReason,
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
            Response.Redirect("BaseChannelMain.aspx");
        }
    }
}