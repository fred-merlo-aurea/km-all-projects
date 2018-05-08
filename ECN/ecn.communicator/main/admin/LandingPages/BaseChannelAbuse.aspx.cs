using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using Ecn.Communicator.Main.Admin.Helpers;
using Ecn.Communicator.Main.Admin.Interfaces;
using Ecn.Communicator.Main.Helpers;
using Ecn.Communicator.Main.Interfaces;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;

namespace ecn.communicator.main.admin.LandingPages
{
    public partial class BaseChannelAbuse : ECN_Framework_BusinessLayer.Application.WebPageHelper
    {
        private const int LandingPageID = 4;

        private static LandingPageAssign LPA = null;
        private static IList<LandingPageOption> _listOptions = null;

        private BaseChannelAbuseOperationsHandler _baseChannelOperations = null;
        private IMasterCommunicator MasterCommunicator;
        private ILandingPageAssign LandingPageAssign;
        private ILandingPageOption LandingPageOption;
        private ILandingPageAssignContent LandingPageAssignContent;
        private IApplicationLog ApplicationLog;
        private HttpResponseBase HttpResponse;

        public BaseChannelAbuse()
        {
            MasterCommunicator = new MasterCommunicatorAdapter(Master);
            LandingPageAssign = new LandingPageAssignAdapter();
            HttpResponse = new HttpResponseAdapter(Response);
            LandingPageOption = new LandingPageOptionAdapter();
            LandingPageAssignContent = new LandingPageAssignContentAdapter();
            ApplicationLog = new ApplicationLogAdapter();

            _baseChannelOperations = new BaseChannelAbuseOperationsHandler(
                LandingPageAssign, 
                LandingPageAssignContent,
                LandingPageOption);
        }

        public BaseChannelAbuse(
            IMasterCommunicator masterCommunicator, 
            ILandingPageAssign landingPageAssign, 
            ILandingPageAssignContent landingPageAssignContent, 
            HttpResponseBase response, 
            ILandingPageOption landingPageOption,
            IApplicationLog applicationLog)
        {
            MasterCommunicator = masterCommunicator;
            LandingPageAssign = landingPageAssign;
            LandingPageAssignContent = landingPageAssignContent;
            HttpResponse = response;
            LandingPageOption = landingPageOption;
            ApplicationLog = applicationLog;

            _baseChannelOperations = new BaseChannelAbuseOperationsHandler(
                LandingPageAssign,
                LandingPageAssignContent,
                LandingPageOption);
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
                "You do not have access to this page.",
                txtThankYou);

            _listOptions = _baseChannelOperations.LandingPageOptions;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var userID = MasterCommunicator.GetUserID();
                if (!_baseChannelOperations.CanSaveChannel(
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
                LandingPageAssignContent.Delete(LPA.LPAID, MasterCommunicator.GetCurrentUser());

                _baseChannelOperations.SaveLandingPageAssign(
                    txtThankYou,
                    MasterCommunicator.GetUserID(),
                    LPA.LPAID,
                    _listOptions,
                    LandingPageAssignContent,
                    MasterCommunicator.GetCurrentUser());

                _baseChannelOperations.SetButtonCssClass(btnPreview);

                HttpResponse.Redirect("BaseChannelMain.aspx");
            }
            catch (ECNException ex)
            {
                _baseChannelOperations.HandleCriticalError(ApplicationLog, ex, phError, lblErrorMessage);
            }
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
            _baseChannelOperations.HandleCustomerSelectedIndexChange(
                LPA.LPAID,
                ddlCustomer,
                btnHtmlPreviewShow,
                lblUrlWarning);
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
        protected void btnHtmlPreview_Hide(object sender, EventArgs e)
        {
            lblUrlWarning.Text = "";
            btnHtmlPreviewShow.Enabled = false;
            btnHtmlPreviewShow.Visible = false;
            lblUrlWarning.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
            this.modalPopupHtmlPreview.Hide();
        }
    }
}