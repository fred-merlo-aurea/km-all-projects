using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using Ecn.Communicator.Main.Admin.Helpers;
using Ecn.Communicator.Main.Admin.Interfaces;
using Ecn.Communicator.Main.Helpers;
using Ecn.Communicator.Main.Interfaces;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.admin.landingpages
{
    public partial class BaseChannelForwardToFriend : System.Web.UI.Page
    {
        private const int LandingPageID = 3;
        private static ECN_Framework_Entities.Accounts.LandingPageAssign LPA = null;

        private BaseChannelOperationsHandler _baseChannelOperations = null;
        private IMasterCommunicator MasterCommunicator;
        private ILandingPageAssign LandingPageAssign;
        private ILandingPageAssignContent LandingPageAssignContent;
        private HttpResponseBase HttpResponse;

        public BaseChannelForwardToFriend()
        {
            MasterCommunicator = new MasterCommunicatorAdapter(Master);
            LandingPageAssign = new LandingPageAssignAdapter();
            HttpResponse = new HttpResponseAdapter(Response);
            LandingPageAssignContent = new LandingPageAssignContentAdapter();

            _baseChannelOperations = new BaseChannelForwardToFriendOperationsHandler(LandingPageAssign);
        }

        public BaseChannelForwardToFriend(IMasterCommunicator masterCommunicator, ILandingPageAssign landingPageAssign, ILandingPageAssignContent landingPageAssignContent, HttpResponseBase response)
        {
            MasterCommunicator = masterCommunicator;
            LandingPageAssign = landingPageAssign;
            LandingPageAssignContent = landingPageAssignContent;
            HttpResponse = response;

            _baseChannelOperations = new BaseChannelForwardToFriendOperationsHandler(LandingPageAssign);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _baseChannelOperations.HandlePageLoad(
                ref LPA,
                phError,
                MasterCommunicator,
                IsPostBack,
                btnPreview,
                pnlNoAccess,
                pnlSettings,
                Label1,
                rblOverrideDefaultSettings,
                rblAllowCustomerOverrideSettings,
                txtHeader,
                txtFooter,
                "You do not have access to this page because you are not a Basechannel Administrator.");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!_baseChannelOperations.CanSaveChannel(
                phError,
                ref LPA,
                LandingPageID,
                MasterCommunicator.GetUserID(),
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

            LandingPageAssign.Save(LPA, MasterCommunicator.GetCurrentUser());

            btnPreview.CssClass = "ECN-Button-Medium";
            btnPreview.Enabled = true;
            btnPreview.Visible = true;

            HttpResponse.Redirect("BaseChannelMain.aspx");
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            lblBaseChannelOverride.Text = "";
            lblCustomerOverride.Text = "";
            
            if (LPA.BaseChannelDoesOverride == false)
            {
                lblBaseChannelOverride.Text = "Note: You must override the default landing page settings for your saved changes to take effect.";
            }
            if (LPA.CustomerCanOverride == true)
            {
                lblCustomerOverride.Text = "Note: If any Customer overrides the Basechannel settings they may see different results.";
            }
            loadDdlCustomer();
            this.modalPopupHtmlPreview.Show();

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

        protected void btnHtmlPreviewShow_Click(object sender, EventArgs e)
        {
            
            if (LPA.BaseChannelDoesOverride == false)
            {
                throwECNException("Note: You must override the default landing page settings for your saved changes to take effect.");
                return;
            }

            //Response.Write("<script language='javascript'>window.open('" + getPreviewUrl() + "');</script>");
        }

        protected void btnHtmlPreview_Hide(object sender, EventArgs e)
        {
            modalPopupHtmlPreview.Hide();
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            _baseChannelOperations.HandleCustomerSelectedIndexChange(
                LPA.LPAID,
                ddlCustomer,
                btnHtmlPreviewShow,
                lblUrlWarning);
        }
    }
}